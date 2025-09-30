using System;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using FFS.Libraries.StaticEcs.Unity;
using UnityEngine.Scripting;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif
using static FFS.Libraries.StaticEcs.World<WT>;

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
[Serializable]
[StaticEcsEditorColor(1f, 1f, 1f)]
[StaticEcsIgnoreEvent]
public struct UiEntityCountUpdate : IEvent {
    public int EntityCount;

    public UiEntityCountUpdate(int entityCount) {
        EntityCount = entityCount;
    }

    [Preserve]
    [StaticEcsAutoRegistration(typeof(WT))]
    public static void RegisterForWT() {
        Events.RegisterEventType(new Config<WT>());
    }

    public class Config<WorldType> : DefaultEventConfig<UiEntityCountUpdate, WorldType> where WorldType : struct, IWorldType {
        public override Guid Id() => new("92a5af153999efb4d85dbffe9c3864c2");

        public override BinaryWriter<UiEntityCountUpdate> Writer() => (ref BinaryPackWriter writer, in UiEntityCountUpdate value) => writer.WriteInt(value.EntityCount);

        public override BinaryReader<UiEntityCountUpdate> Reader() => (ref BinaryPackReader reader) => new UiEntityCountUpdate(reader.ReadInt());

        public override IPackArrayStrategy<UiEntityCountUpdate> ReadWriteStrategy() => new UnmanagedPackArrayStrategy<UiEntityCountUpdate>();

    }

}
