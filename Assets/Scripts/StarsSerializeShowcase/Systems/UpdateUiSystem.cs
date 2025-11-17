using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public struct UpdateUiSystem : IUpdateSystem, IInitSystem, IDestroySystem {
    private EventReceiver<WT, UiEntityCountUpdate> _receiver;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        foreach (var ev in _receiver) {
            W.Context<Ui>.Get().CurrentEntitiesCount.SetText(ev.Value.EntityCount.ToString());
        } 
    }

    public void Init() {
        _receiver = W.Events.RegisterEventReceiver<UiEntityCountUpdate>();
    }
    
    public void Destroy() {
        W.Events.DeleteEventReceiver(ref _receiver);
    }
}