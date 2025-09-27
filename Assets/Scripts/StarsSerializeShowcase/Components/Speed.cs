using System;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using FFS.Libraries.StaticEcs.Unity;
using UnityEngine.Scripting;
using System.Runtime.CompilerServices;
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
public struct Speed : IComponent {
    public float Value;

    public Speed(float value) {
        Value = value;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        RegisterComponentType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultComponentConfig<Speed, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("40f39c8e81bb91646ac1e157532beae2");

        public override BinaryWriter<Speed> Writer() => (ref BinaryPackWriter writer, in Speed value) => writer.WriteFloat(value.Value);

        public override BinaryReader<Speed> Reader() =>
            (ref BinaryPackReader reader) => new Speed {
                Value = reader.ReadFloat()
            };

        public override IPackArrayStrategy<Speed> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<Speed>();

        public override bool IsCopyable() => true;

        public override bool IsClearable() => false;
    }
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public static class SpeedExtensionsForWT {
    [MethodImpl(AggressiveInlining)]
    public static ref Speed Speed(this Entity entity) => ref Components<Speed>.Value.Ref(entity);

    [MethodImpl(AggressiveInlining)]
    public static ref Speed AddSpeed(this Entity entity) => ref Components<Speed>.Value.Add(entity);

    [MethodImpl(AggressiveInlining)]
    public static void AddSpeed(this Entity entity, Speed value) => Components<Speed>.Value.Add(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static ref Speed TryAddSpeed(this Entity entity) => ref Components<Speed>.Value.TryAdd(entity);

    [MethodImpl(AggressiveInlining)]
    public static void TryAddSpeed(this Entity entity, Speed value) => Components<Speed>.Value.TryAdd(entity) = value;

    [MethodImpl(AggressiveInlining)]
    public static void PutSpeed(this Entity entity, Speed value) => Components<Speed>.Value.Put(entity, value);

    [MethodImpl(AggressiveInlining)]
    public static bool HasSpeed(this Entity entity) => Components<Speed>.Value.Has(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasDisabledSpeed(this Entity entity) => Components<Speed>.Value.HasDisabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool HasEnabledSpeed(this Entity entity) => Components<Speed>.Value.HasEnabled(entity);

    [MethodImpl(AggressiveInlining)]
    public static void EnableSpeed(this Entity entity) => Components<Speed>.Value.Enable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DisableSpeed(this Entity entity) => Components<Speed>.Value.Disable(entity);

    [MethodImpl(AggressiveInlining)]
    public static void DeleteSpeed(this Entity entity) => Components<Speed>.Value.Delete(entity);

    [MethodImpl(AggressiveInlining)]
    public static bool TryDeleteSpeed(this Entity entity) => Components<Speed>.Value.TryDelete(entity);

    [MethodImpl(AggressiveInlining)]
    public static void CopySpeedTo(this Entity entity, Entity dst) => Components<Speed>.Value.Copy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryCopySpeedTo(this Entity entity, Entity dst) => Components<Speed>.Value.TryCopy(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static void MoveSpeedTo(this Entity entity, Entity dst) => Components<Speed>.Value.Move(entity, dst);

    [MethodImpl(AggressiveInlining)]
    public static bool TryMoveSpeedTo(this Entity entity, Entity dst) => Components<Speed>.Value.TryMove(entity, dst);
}