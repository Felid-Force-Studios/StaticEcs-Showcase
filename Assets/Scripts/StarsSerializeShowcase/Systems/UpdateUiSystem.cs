using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;

public readonly struct UpdateUiSystem : IUpdateSystem {

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        ref var data = ref W.Context<RenderData>.Get();
        ref var ui = ref W.Context<Ui>.Get();

        ui.CurrentEntitiesCount.SetText(data.InstanceCount.ToString());
    }
}