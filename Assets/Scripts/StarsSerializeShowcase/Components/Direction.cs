using System;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using FFS.Libraries.StaticEcs.Unity;
using UnityEngine.Scripting;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;
using static FFS.Libraries.StaticEcs.World<WT>;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
[Serializable]
[StaticEcsEditorColor(1f, 1f, 1f)]
public struct Direction : IComponent {
    public float3 Value;

    public Direction(float3 value) {
        Value = value;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        RegisterComponentType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultComponentConfig<Direction, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("1801f985db249a94d94f922f027d0eee");

        public override BinaryWriter<Direction> Writer() =>
            (ref BinaryPackWriter writer, in Direction value) => {
                writer.WriteFloat(value.Value.x);
                writer.WriteFloat(value.Value.y);
                writer.WriteFloat(value.Value.z);
            };

        public override BinaryReader<Direction> Reader() =>
            (ref BinaryPackReader reader) => new Direction {
                Value = new float3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat())
            };

        public override IPackArrayStrategy<Direction> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<Direction>();

        public override bool IsCopyable() => true;

        public override bool IsClearable() => true;
    }
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public static class DirectionExtensionsForWT {
    [MethodImpl(AggressiveInlining)]
    public static ref Direction Direction(this Entity entity) => ref Components<Direction>.Value.Ref(entity);

    [MethodImpl(AggressiveInlining)]
    public static ref Direction AddDirection(this Entity entity) => ref Components<Direction>.Value.Add(entity);

    [MethodImpl(AggressiveInlining)]
    public static void AddDirection(this Entity entity, Direction value) => Components<Direction>.Value.Add(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static ref Direction TryAddDirection(this Entity entity) => ref Components<Direction>.Value.TryAdd(entity);

    [MethodImpl(AggressiveInlining)]
    public static void TryAddDirection(this Entity entity, Direction value) => Components<Direction>.Value.TryAdd(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static void PutDirection(this Entity entity, Direction value) => Components<Direction>.Value.Put(entity, value);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDirection(this Entity entity) => Components<Direction>.Value.Has(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDisabledDirection(this Entity entity) => Components<Direction>.Value.HasDisabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEnabledDirection(this Entity entity) => Components<Direction>.Value.HasEnabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static void EnableDirection(this Entity entity) => Components<Direction>.Value.Enable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DisableDirection(this Entity entity) => Components<Direction>.Value.Disable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DeleteDirection(this Entity entity) => Components<Direction>.Value.Delete(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool TryDeleteDirection(this Entity entity) => Components<Direction>.Value.TryDelete(entity);

    [MethodImpl(AggressiveInlining)]
    public static void CopyDirectionTo(this Entity entity, Entity dst) => Components<Direction>.Value.Copy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryCopyDirectionTo(this Entity entity, Entity dst) => Components<Direction>.Value.TryCopy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static void MoveDirectionTo(this Entity entity, Entity dst) => Components<Direction>.Value.Move(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryMoveDirectionTo(this Entity entity, Entity dst) => Components<Direction>.Value.TryMove(entity, dst);
}