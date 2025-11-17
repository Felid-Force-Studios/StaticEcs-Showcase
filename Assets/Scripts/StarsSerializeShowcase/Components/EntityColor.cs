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
public struct EntityColor : IComponent {
    public float4 Value;

    public EntityColor(float4 value) {
        Value = value;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        RegisterComponentType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultComponentConfig<EntityColor, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("bc4da30558fd5ad459c48543852ff65e");

        public override BinaryWriter<EntityColor> Writer() =>
            (ref BinaryPackWriter writer, in EntityColor value) => {
                writer.WriteFloat(value.Value.x);
                writer.WriteFloat(value.Value.y);
                writer.WriteFloat(value.Value.z);
                writer.WriteFloat(value.Value.w);
            };

        public override BinaryReader<EntityColor> Reader() =>
            (ref BinaryPackReader reader) => new EntityColor {
                Value = new float4(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat())
            };

        public override IPackArrayStrategy<EntityColor> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<EntityColor>();

        public override bool IsCopyable() => true;

        public override bool IsClearable() => true;
    }
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public static class EntityColorExtensionsForWT {
    [MethodImpl(AggressiveInlining)]
    public static ref EntityColor EntityColor(this Entity entity) => ref Components<EntityColor>.Value.Ref(entity);

    [MethodImpl(AggressiveInlining)]
    public static ref EntityColor AddEntityColor(this Entity entity) => ref Components<EntityColor>.Value.Add(entity);

    [MethodImpl(AggressiveInlining)]
    public static void AddEntityColor(this Entity entity, EntityColor value) => Components<EntityColor>.Value.Add(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static ref EntityColor TryAddEntityColor(this Entity entity) => ref Components<EntityColor>.Value.TryAdd(entity);

    [MethodImpl(AggressiveInlining)]
    public static void TryAddEntityColor(this Entity entity, EntityColor value) => Components<EntityColor>.Value.TryAdd(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static void PutEntityColor(this Entity entity, EntityColor value) => Components<EntityColor>.Value.Put(entity, value);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEntityColor(this Entity entity) => Components<EntityColor>.Value.Has(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDisabledEntityColor(this Entity entity) => Components<EntityColor>.Value.HasDisabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEnabledEntityColor(this Entity entity) => Components<EntityColor>.Value.HasEnabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static void EnableEntityColor(this Entity entity) => Components<EntityColor>.Value.Enable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DisableEntityColor(this Entity entity) => Components<EntityColor>.Value.Disable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DeleteEntityColor(this Entity entity) => Components<EntityColor>.Value.Delete(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool TryDeleteEntityColor(this Entity entity) => Components<EntityColor>.Value.TryDelete(entity);

    [MethodImpl(AggressiveInlining)]
    public static void CopyEntityColorTo(this Entity entity, Entity dst) => Components<EntityColor>.Value.Copy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryCopyEntityColorTo(this Entity entity, Entity dst) => Components<EntityColor>.Value.TryCopy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static void MoveEntityColorTo(this Entity entity, Entity dst) => Components<EntityColor>.Value.Move(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryMoveEntityColorTo(this Entity entity, Entity dst) => Components<EntityColor>.Value.TryMove(entity, dst);
}