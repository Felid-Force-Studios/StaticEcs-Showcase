using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FFS.Libraries.StaticEcs;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

using QueryBlocks = FFS.Libraries.StaticEcs.QueryBlocks<Position, Direction, EntityColor, Speed, GpuInstance>;
using Random = Unity.Mathematics.Random;

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
[StructLayout(LayoutKind.Explicit, Size = 64)]
public struct PaddedRandom {
    [FieldOffset(0)] public Random Random;
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public unsafe struct UpdateEntitiesBurstSystem : IInitSystem, IUpdateSystem {
    private const EntityStatusType entityStatusType = EntityStatusType.Enabled;
    private const ComponentStatus componentStatus = ComponentStatus.Enabled;
    private static readonly ushort[] clusters = Array.Empty<ushort>(); // Empty == all clusters
    private static readonly WithNothing with = default;

    private static bool parallel = true;
    private static uint minEntitiesPerThread; // default - 64
    private static uint workersLimit;         // default - max threads
    
    private NativeArray<InstanceGpuData> gpuData;
    private float _dtTemp;
    private float _colorChangeSpeed;
    private float _sphereRadius;

    private PaddedRandom* _randomsPerThreads;

    public UpdateEntitiesBurstSystem(bool parallel) {
        UpdateEntitiesBurstSystem.parallel = parallel;
        gpuData = default;
        _dtTemp = 0;
        _colorChangeSpeed = 0;
        _sphereRadius = 0;
        _randomsPerThreads = null;
        
    }

    public void Init() {
        _randomsPerThreads = (PaddedRandom*) UnsafeUtility.MallocTracked(sizeof(PaddedRandom) * Environment.ProcessorCount, UnsafeUtility.AlignOf<PaddedRandom>(), Allocator.Persistent, 0);
        for (var i = 0; i < Environment.ProcessorCount; ++i) {
            _randomsPerThreads[i] = new PaddedRandom {
                Random = new Random((uint) UnityEngine.Random.Range(uint.MinValue, uint.MaxValue)),
            };
        }
    }

    [MethodImpl(AggressiveInlining)]
    public void BeforeUpdate() {
        gpuData = W.Context<RenderData>.Get().GpuInstancesBuffer.LockBufferForWrite<InstanceGpuData>(0, W.Context<SceneConfig>.Get().MaxEntities);
        _dtTemp = Time.deltaTime;
        minEntitiesPerThread = (uint) math.max(W.Context<RenderData>.Get().InstanceCount / System.Environment.ProcessorCount, 1024 * 16);
        _sphereRadius = W.Context<RenderData>.Get().SphereRadius;
        _colorChangeSpeed = W.Context<SceneConfig>.Get().ColorChangeSpeed;
    }

    [MethodImpl(AggressiveInlining)]
    public void AfterUpdate() {
        W.Context<RenderData>.Get().GpuInstancesBuffer.UnlockBufferAfterWrite<InstanceGpuData>(W.Context<SceneConfig>.Get().MaxEntities);
        W.Context<RenderData>.Get().Draw();
        gpuData = default;
    }

    [MethodImpl(AggressiveInlining)]
    private void InvokeOne(ref Position position, ref Direction direction, ref EntityColor entityColor, ref Speed speed, ref GpuInstance gpuInstance, int worker) {
        ref var pos = ref position.Value;
        ref var dir = ref direction.Value;
        ref var color = ref entityColor.Value;
        ref var random = ref _randomsPerThreads[worker].Random;
        
        // DIRECTION
        dir = math.normalize(dir);
        
        var yaw = math.radians(random.NextFloat(-1.5f, 1.5f));
        dir = math.mul(quaternion.RotateY(yaw), dir);

        var pitch = math.radians(random.NextFloat(-0.5f, 0.5f));
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
        
        gpuData[gpuInstance.Index] = new InstanceGpuData(pos, color);
    }

    [MethodImpl(AggressiveInlining)]
    private void InvokeBlock(Position* positions, Direction* directions, EntityColor* entityColors, Speed* speeds, GpuInstance* gpuInstances, int worker, uint dataOffest) {
        // Here, custom optimization of the entire entity block is possible. (SIMD, unroll, etc.)
        for (var i = 0; i < Const.ENTITIES_IN_BLOCK; i++) {
            var dIdx = i + dataOffest;
            InvokeOne(ref positions[dIdx], ref directions[dIdx], ref entityColors[dIdx], ref speeds[dIdx], ref gpuInstances[dIdx], worker);
        }
    }


    #region GENERATED
    [MethodImpl(AggressiveInlining)]
    public void Update() {
        BeforeUpdate();
        QueryBurstFunctionRunner<WT>.Prepare(W.HandleClustersRange(clusters), with, entityStatusType, componentStatus, out QueryBlocks* blocks, out var blocksCount);
        if (parallel) {
            var runner = W.Context.Value.GetOrCreate<ParallelRunner>();
            runner.Blocks = blocks;
            runner.System = this;
            ParallelRunner<WT>.Run(runner, (uint) blocksCount, math.max(minEntitiesPerThread / Const.BLOCK_IN_CHUNK, 2), 0);
            this = runner.System;
            runner.Blocks = default;
        } else {
            Runner.Run(ref this, blocks, 0, (uint) blocksCount, 0);
        }
        AfterUpdate();
    }

    #if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif
    private class ParallelRunner : AbstractParallelTask {
        internal QueryBlocks* Blocks;
        internal UpdateEntitiesBurstSystem System;
        public override void Run(uint from, uint to, int worker) => Runner.Run(ref System, Blocks, from, to, worker);
    }

    #if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    #endif
    [BurstCompile]
    private static class Runner {
        [BurstCompile]
        [MethodImpl(AggressiveInlining)]
        internal static void Run(ref UpdateEntitiesBurstSystem system, QueryBlocks* blocks, uint from, uint to, int worker) {
            blocks += from;
            for (var i = from; i < to; i++, blocks++) {
                var positions = blocks->d1;
                var directions = blocks->d2;
                var entityColors = blocks->d3;
                var speeds = blocks->d4;
                var gpuInstances = blocks->d5;
                ref var entities = ref blocks->EntitiesMask;
                var dataOffset = (blocks->BlockIdx << Const.BLOCK_IN_CHUNK_SHIFT) & Const.DATA_ENTITY_MASK;
                if (entities == ulong.MaxValue) {
                    system.InvokeBlock(positions, directions, entityColors, speeds, gpuInstances, worker, dataOffset);
                    continue;
                }
                var idx = math.tzcnt(entities);
                var end = Const.BITS_PER_LONG - math.lzcnt(entities);
                var total = math.countbits(entities);
                if (total >= (end - idx) >> 1) {
                    for (; idx < end; idx++) {
                        if ((entities & (1UL << idx)) != 0UL) {
                            var dIdx = idx + dataOffset;
                            system.InvokeOne(ref positions[dIdx], ref directions[dIdx], ref entityColors[dIdx], ref speeds[dIdx], ref gpuInstances[dIdx], worker);
                        }
                    }
                } else {
                    do {
                        var dIdx = idx + dataOffset;
                        system.InvokeOne(ref positions[dIdx], ref directions[dIdx], ref entityColors[dIdx], ref speeds[dIdx], ref gpuInstances[dIdx], worker);
                        entities &= entities - 1UL;
                        idx = math.tzcnt(entities);
                    } while (entities != 0UL);
                }
            }
        }
    }
    #endregion
}

