using System;
using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using Unity.Collections;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public struct UpdateEntitiesSystem : IUpdateSystem, W.IQueryFunction<Position, Direction, Speed, EntityColor, GpuInstance> {
    private W.ContextValue<RenderData> renderData;
    private W.ContextValue<SceneConfig> cfg;
    private NativeArray<InstanceGpuData> tempGpuData;
    private float _dtTemp;

    [ThreadStatic] private static System.Random _random;

    [MethodImpl(AggressiveInlining)]
    public void Update() {
        tempGpuData = new NativeArray<InstanceGpuData>(cfg.Get().MaxEntities, Allocator.Temp);
        _dtTemp = Time.deltaTime;

        var entitiesPerThread = (uint) Math.Max(renderData.Get().InstanceCount / Environment.ProcessorCount, 1024 * 16);

        W.Query.Parallel.For<Position, Direction, Speed, EntityColor, GpuInstance, UpdateEntitiesSystem>(entitiesPerThread, ref this);

        renderData.Get().Draw(tempGpuData);
        tempGpuData = default;
    }

    [MethodImpl(AggressiveInlining)]
    public void Run(World<WT>.Entity entity, ref Position pos, ref Direction dir, ref Speed speed, ref EntityColor color, ref GpuInstance instance) {
        _random ??= new System.Random();

        UpdateDirection(ref dir.Value, 1.5f, 0.5f);
        pos.Value += dir.Value * (speed.Value * _dtTemp);

        CheckBounds(ref pos.Value, ref dir.Value, renderData.Get().SphereRadius);
        UpdateColor(ref color.Value, _dtTemp, cfg.Get().ColorChangeSpeed);
        tempGpuData[instance.Index] = new InstanceGpuData(pos.Value, color.Value);
    }

    [MethodImpl(AggressiveInlining)]
    private static void CheckBounds(ref Vector3 pos, ref Vector3 dir, float sphereRadius) {
        if (pos.magnitude > sphereRadius) {
            var normal = pos.normalized;
            dir = Vector3.Reflect(dir, normal);
            pos = normal * sphereRadius;
        }
    }

    [MethodImpl(AggressiveInlining)]
    private static void UpdateColor(ref Color color, float dt, float colorChangeSpeed) {
        Color.RGBToHSV(color, out var h, out var s, out var v);
        h = Mathf.Repeat(h + colorChangeSpeed * dt, 1f);
        color = Color.HSVToRGB(h, s, v);
    }

    [MethodImpl(AggressiveInlining)]
    private static void UpdateDirection(ref Vector3 dir, float maxYawDeg, float maxPitchDeg) {
        dir.Normalize();

        var yaw = NextFloat(-maxYawDeg, maxYawDeg);
        dir = Quaternion.Euler(0f, yaw, 0f) * dir;

        var pitch = NextFloat(-maxPitchDeg, maxPitchDeg);
        var pitchAxis = Vector3.Cross(Vector3.up, dir).normalized;
        dir = Quaternion.AngleAxis(pitch, pitchAxis) * dir;

        dir.Normalize();
    }

    [MethodImpl(AggressiveInlining)]
    private static float NextFloat(float min, float max) => (float) (_random.NextDouble() * (max - min) + min);
}