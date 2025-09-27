using System;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using FFS.Libraries.StaticEcs.Unity;
using UnityEngine.Scripting;
using System.Runtime.CompilerServices;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif
using static System.Runtime.CompilerServices.MethodImplOptions;
using static FFS.Libraries.StaticEcs.World<WT>;


#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif

[Serializable]
[StaticEcsEditorColor(1f, 1f, 1f)]
public struct GpuInstance : IComponent {
    public int Index;

    public GpuInstance(int index) {
        Index = index;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        RegisterComponentType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultComponentConfig<GpuInstance, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("261e1e8d06454624fb61e74dec181338");

        public override BinaryWriter<GpuInstance> Writer() => (ref BinaryPackWriter writer, in GpuInstance value) => writer.WriteInt(value.Index);

        public override BinaryReader<GpuInstance> Reader() => (ref BinaryPackReader reader) => new GpuInstance(reader.ReadInt());

        public override IPackArrayStrategy<GpuInstance> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<GpuInstance>();

        public override bool IsCopyable() => true;

        public override bool IsClearable() => true;
    }
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public static class GpuInstanceExtensionsForWT {
    [MethodImpl(AggressiveInlining)]
    public static ref GpuInstance GpuInstance(this Entity entity) => ref Components<GpuInstance>.Value.Ref(entity);

    [MethodImpl(AggressiveInlining)]
    public static ref GpuInstance AddGpuInstance(this Entity entity) => ref Components<GpuInstance>.Value.Add(entity);

    [MethodImpl(AggressiveInlining)]
    public static void AddGpuInstance(this Entity entity, GpuInstance value) => Components<GpuInstance>.Value.Add(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static ref GpuInstance TryAddGpuInstance(this Entity entity) => ref Components<GpuInstance>.Value.TryAdd(entity);

    [MethodImpl(AggressiveInlining)]
    public static void TryAddGpuInstance(this Entity entity, GpuInstance value) => Components<GpuInstance>.Value.TryAdd(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static void PutGpuInstance(this Entity entity, GpuInstance value) => Components<GpuInstance>.Value.Put(entity, value);

    [MethodImpl(AggressiveInlining)]
    public static bool HasGpuInstance(this Entity entity) => Components<GpuInstance>.Value.Has(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDisabledGpuInstance(this Entity entity) => Components<GpuInstance>.Value.HasDisabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEnabledGpuInstance(this Entity entity) => Components<GpuInstance>.Value.HasEnabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static void EnableGpuInstance(this Entity entity) => Components<GpuInstance>.Value.Enable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DisableGpuInstance(this Entity entity) => Components<GpuInstance>.Value.Disable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DeleteGpuInstance(this Entity entity) => Components<GpuInstance>.Value.Delete(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool TryDeleteGpuInstance(this Entity entity) => Components<GpuInstance>.Value.TryDelete(entity);

    [MethodImpl(AggressiveInlining)]
    public static void CopyGpuInstanceTo(this Entity entity, Entity dst) => Components<GpuInstance>.Value.Copy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryCopyGpuInstanceTo(this Entity entity, Entity dst) => Components<GpuInstance>.Value.TryCopy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static void MoveGpuInstanceTo(this Entity entity, Entity dst) => Components<GpuInstance>.Value.Move(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryMoveGpuInstanceTo(this Entity entity, Entity dst) => Components<GpuInstance>.Value.TryMove(entity, dst);
}