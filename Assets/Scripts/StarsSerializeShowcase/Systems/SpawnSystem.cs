using System;
using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct Spawner {
    public float Timer;
}

public struct SpawnSystem : IUpdateSystem, IInitSystem {
    public void Init() {
        W.Context<Spawner>.Set(new Spawner());

        SetSnapshotHandler();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        ref var data = ref W.Context<RenderData>.Get();
        ref var cfg = ref W.Context<SceneConfig>.Get();
        ref var spawner = ref W.Context<Spawner>.Get();

        var dt = Time.deltaTime;

        var max = cfg.MaxEntities;
        var spawnTime = cfg.SpawnTimeSeconds;
        if (data.InstanceCount < max) {
            spawner.Timer += dt;
            var target = (uint) Mathf.Min(max, Mathf.FloorToInt((spawner.Timer / spawnTime) * max));
            for (var i = data.InstanceCount; i < target; i++) {
                W.Entity.New(
                    new Position(),
                    new Direction(new Vector3(0.1f, 0, 0.1f)),
                    new Speed(Random.Range(0.5f, 1f)),
                    new EntityColor(cfg.StartColor),
                    new GpuInstance(data.InstanceCount++)
                );
            }

            W.Events.Send(new UiEntityCountUpdate(data.InstanceCount));
        }
    }

    private static void SetSnapshotHandler() {
        W.Serializer.SetSnapshotHandler(
            new Guid("bc1da30558fd5ad422c48143852ff61e"), 0,
            (ref BinaryPackWriter writer, SnapshotWriteParams param) => {
                ref var spawner = ref W.Context<Spawner>.Get();
                writer.WriteFloat(spawner.Timer);
            }, (ref BinaryPackReader reader, ushort version, SnapshotReadParams param) => {
                ref var spawner = ref W.Context<Spawner>.Get();
                spawner.Timer = reader.ReadFloat();
            });
    }
}