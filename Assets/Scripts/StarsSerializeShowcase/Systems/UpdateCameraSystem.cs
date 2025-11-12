using System;
using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using UnityEngine;

public readonly struct UpdateCameraSystem : IUpdateSystem, IInitSystem {
    public void Init() {
        SetSnapshotHandler();
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

    private static void SetSnapshotHandler() {
        W.Serializer.SetSnapshotHandler(
            new Guid("bc2da32558fd5ad422c48143852ff61e"), 0,
            (ref BinaryPackWriter writer, SnapshotWriteParams param) => {
                var cam = W.Context<RenderData>.Get().Camera.transform;
                writer.WriteVector3(cam.position);
                writer.WriteQuaternion(cam.rotation);
            }, (ref BinaryPackReader reader, ushort version, SnapshotReadParams param) => {
                var cam = W.Context<RenderData>.Get().Camera.transform;
                cam.position = reader.ReadVector3();
                cam.rotation = reader.ReadQuaternion();
            });
    }
}