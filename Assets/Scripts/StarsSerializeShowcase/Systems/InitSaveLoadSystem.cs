using FFS.Libraries.StaticEcs;

public struct WorldSave {
    public byte[] Data;
}

public class InitSaveLoadSystem : IInitSystem, IDestroySystem {
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

    public void Destroy() { }
}