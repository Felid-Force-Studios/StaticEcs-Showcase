using System;
using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public struct UpdateEntitiesSystem : IUpdateSystem, W.IQueryFunction<Position, Direction, Speed, EntityColor, GpuInstance> {
    private W.ContextValue<RenderData> renderData;
    private W.ContextValue<SceneConfig> cfg;
    private NativeArray<InstanceGpuData> gpuData;
    private float _dtTemp;
    private float _colorChangeSpeed;
    private float _sphereRadius;
    private bool _parallel;

    [ThreadStatic] private static System.Random _random;

    public UpdateEntitiesSystem(bool parallel) {
        _parallel = parallel;
        gpuData = default;
        _dtTemp = 0;
        _colorChangeSpeed = 0;
        _sphereRadius = 0;
    }

    [MethodImpl(AggressiveInlining)]
    public void Update() {
        gpuData = W.Context<RenderData>.Get().GpuInstancesBuffer.LockBufferForWrite<InstanceGpuData>(0, W.Context<SceneConfig>.Get().MaxEntities);
        _dtTemp = Time.deltaTime;
        _sphereRadius = W.Context<RenderData>.Get().SphereRadius;
        _colorChangeSpeed = W.Context<SceneConfig>.Get().ColorChangeSpeed;

        if (_parallel) {
            var entitiesPerThread = (uint) Math.Max(renderData.Get().InstanceCount / Environment.ProcessorCount, 1024 * 16);
            W.Query.Parallel.For<Position, Direction, Speed, EntityColor, GpuInstance, UpdateEntitiesSystem>(entitiesPerThread, ref this);
        } else {
            W.Query.For<Position, Direction, Speed, EntityColor, GpuInstance, UpdateEntitiesSystem>(ref this);
        }

        W.Context<RenderData>.Get().GpuInstancesBuffer.UnlockBufferAfterWrite<InstanceGpuData>(W.Context<SceneConfig>.Get().MaxEntities);
        renderData.Get().Draw();
        gpuData = default;
    }

    [MethodImpl(AggressiveInlining)]
    public void Invoke(World<WT>.Entity entity, ref Position posC, ref Direction dirC, ref Speed speed, ref EntityColor colorC, ref GpuInstance instance) {
        _random ??= new System.Random();
        
        ref var pos = ref posC.Value;
        ref var dir = ref dirC.Value;
        ref var color = ref colorC.Value;
        
        // DIRECTION
        dir = math.normalize(dir);
        
        var yaw = math.radians(NextFloat(-1.5f, 1.5f));
        dir = math.mul(quaternion.RotateY(yaw), dir);

        var pitch = math.radians(NextFloat(-0.5f, 0.5f));
        var pitchAxis = math.normalize(math.cross(new float3(0, 1, 0), dir));

        dir = math.mul(quaternion.AxisAngle(pitchAxis, pitch), dir);
        dir = math.normalize(dir);
        pos += dir * (speed.Value * _dtTemp);

        // BOUNDS
        if (math.length(pos) > _sphereRadius) {
            var normal = math.normalize(pos);
            dir = math.reflect(dir, normal);
            pos = normal * _sphereRadius;
        }

        // COLOR
        MathUtils.RGBToHSV(ref color, out var h, out var s, out var v);
        h = math.frac(h + _colorChangeSpeed * _dtTemp);
        MathUtils.HSVToRGB(h, s, v, out color);
        
        gpuData[instance.Index] = new InstanceGpuData(pos, color);
    }

    [MethodImpl(AggressiveInlining)]
    private static float NextFloat(float min, float max) => (float) (_random.NextDouble() * (max - min) + min);
}