using FFS.Libraries.StaticEcs;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public struct WorldSave {
    public byte[] Data;
}

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public class InitSaveLoadSystem : IInitSystem {
    public void Init() {
        W.Context<WorldSave>.Set(default);

        var ui = W.Context<Ui>.Get();

        ui.SaveButton.onClick.AddListener(() => {
            ref var data = ref W.Context<WorldSave>.Get().Data;
            data ??= new byte[8 * 1024 * 1024];
            W.Serializer.CreateWorldSnapshot(ref data);
        });

        ui.LoadButton.onClick.AddListener(() => {
            var data = W.Context<WorldSave>.Get().Data;
            if (data != null) {
                W.Serializer.LoadWorldSnapshot(data);
            }
        });
    }
}