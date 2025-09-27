using System;
using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using UnityEngine;

public readonly struct UpdateCameraSystem : IUpdateSystem, IInitSystem {
    public void Init() {
        W.Serializer.SetSnapshotHandler(
            new Guid("bc2da32558fd5ad422c48143852ff61e"), 0,
            (ref BinaryPackWriter writer) => {
                var cam = W.Context<RenderData>.Get().Camera.transform;
                writer.WriteFloat(cam.position.x);
                writer.WriteFloat(cam.position.y);
                writer.WriteFloat(cam.position.z);
                writer.WriteFloat(cam.rotation.x);
                writer.WriteFloat(cam.rotation.y);
                writer.WriteFloat(cam.rotation.z);
                writer.WriteFloat(cam.rotation.w);
            }, (ref BinaryPackReader reader, ushort version) => {
                var cam = W.Context<RenderData>.Get().Camera.transform;
                cam.position = new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
                cam.rotation = new Quaternion(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
            });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        ref var cfg = ref W.Context<SceneConfig>.Get();
        ref var camera = ref W.Context<RenderData>.Get().Camera;
        ref var renderData = ref W.Context<RenderData>.Get();
        renderData.TimeFromStart += Time.deltaTime;

        var direction = camera.transform.position.normalized;
        direction = Quaternion.Euler(0f, 20f * Time.deltaTime, 0f) * direction;

        camera.transform.position = direction * (cfg.CameraMaxDistance + Mathf.Sin(renderData.TimeFromStart * cfg.CameraSpeed) * cfg.CameraAmplitude);
        camera.transform.LookAt(Vector3.zero);
    }
}