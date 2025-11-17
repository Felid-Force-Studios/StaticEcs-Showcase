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
public struct Position : IComponent {
    public float3 Value;

    public Position(float3 value) {
        Value = value;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        RegisterComponentType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultComponentConfig<Position, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("9955cb540182c2747b6a9d96496d6234");

        public override BinaryWriter<Position> Writer() =>
            (ref BinaryPackWriter writer, in Position value) => {
                writer.WriteFloat(value.Value.x);
                writer.WriteFloat(value.Value.y);
                writer.WriteFloat(value.Value.z);
            };

        public override BinaryReader<Position> Reader() =>
            (ref BinaryPackReader reader) => new Position {
                Value = new float3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat())
            };

        public override IPackArrayStrategy<Position> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<Position>();

        public override bool IsCopyable() => true;

        public override bool IsClearable() => true;
    }
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public static class PositionExtensionsForWT {
    [MethodImpl(AggressiveInlining)]
    public static ref Position Position(this Entity entity) => ref Components<Position>.Value.Ref(entity);

    [MethodImpl(AggressiveInlining)]
    public static ref Position AddPosition(this Entity entity) => ref Components<Position>.Value.Add(entity);

    [MethodImpl(AggressiveInlining)]
    public static void AddPosition(this Entity entity, Position value) => Components<Position>.Value.Add(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static ref Position TryAddPosition(this Entity entity) => ref Components<Position>.Value.TryAdd(entity);

    [MethodImpl(AggressiveInlining)]
    public static void TryAddPosition(this Entity entity, Position value) => Components<Position>.Value.TryAdd(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static void PutPosition(this Entity entity, Position value) => Components<Position>.Value.Put(entity, value);

    [MethodImpl(AggressiveInlining)]
    public static bool HasPosition(this Entity entity) => Components<Position>.Value.Has(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDisabledPosition(this Entity entity) => Components<Position>.Value.HasDisabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEnabledPosition(this Entity entity) => Components<Position>.Value.HasEnabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static void EnablePosition(this Entity entity) => Components<Position>.Value.Enable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DisablePosition(this Entity entity) => Components<Position>.Value.Disable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DeletePosition(this Entity entity) => Components<Position>.Value.Delete(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool TryDeletePosition(this Entity entity) => Components<Position>.Value.TryDelete(entity);

    [MethodImpl(AggressiveInlining)]
    public static void CopyPositionTo(this Entity entity, Entity dst) => Components<Position>.Value.Copy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryCopyPositionTo(this Entity entity, Entity dst) => Components<Position>.Value.TryCopy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static void MovePositionTo(this Entity entity, Entity dst) => Components<Position>.Value.Move(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryMovePositionTo(this Entity entity, Entity dst) => Components<Position>.Value.TryMove(entity, dst);
}