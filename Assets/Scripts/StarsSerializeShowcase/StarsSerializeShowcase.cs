using System;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticEcs.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

[StaticEcsEditorName("World")]
public struct WT : IWorldType { }
public abstract class W : World<WT> { }
public struct UpdateSystemsType : ISystemsType { }
public abstract class UpdateSystems : W.Systems<UpdateSystemsType> { }


#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
[Serializable]
public class WContext {
    public SceneConfig sceneConfig;
    public Ui ui;
}

public class StarsSerializeShowcase : MonoBehaviour {
    public WContext context;

    public void Create(int entitiesCount) {
        context.sceneConfig.MaxEntities = entitiesCount;
        
        var config = WorldConfig.Default();
        config.ParallelQueryType = ParallelQueryType.MaxThreadsCount;
        
        W.Create(config);

        EcsDebug<WT>.AddWorld();
        AutoRegister<WT>.Apply();

        W.Initialize((uint) context.sceneConfig.MaxEntities);

        W.Context<WContext>.Set(context);
        W.Context<SceneConfig>.Set(context.sceneConfig);
        W.Context<Ui>.Set(context.ui);

        UpdateSystems.Create();
        
        UpdateSystems.AddCallOnce(new InitRenderSystem());
        UpdateSystems.AddCallOnce(new InitSaveLoadSystem());
        UpdateSystems.AddUpdate(
            new UpdateCameraSystem(),
            new UpdateRadiusSystem(),
            new SpawnSystem(),
            new UpdateEntitiesSystem(),
            new UpdateUiSystem()
        );

        UpdateSystems.Initialize();
        EcsDebug<WT>.AddSystem<UpdateSystemsType>();
        
        context.ui.Canvas.SetActive(true);
        gameObject.SetActive(true);
    }

    private void Update() {
        UpdateSystems.Update();
    }

    public void OnDestroy() {
        if (W.IsWorldInitialized()) {
            UpdateSystems.Destroy();
            W.Destroy();
            context.ui.Canvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}


[Serializable]
public class SceneConfig {
    [Header("Spawner")]
    public Material Material;
    public int MaxEntities = 100_000;
    public float SpawnTimeSeconds = 30f;

    [Header("Bounds")]
    public float StartSphereRadius = 5f;
    public float MinSphereRadius = 3f;
    public float MaxSphereRadius = 8f;
    public float RadiusChangeSpeed = 4f;
    public float RadiusChangeTimeSeconds = 2f;

    [Header("Color")]
    public Color StartColor = Color.yellow;
    public float ColorChangeSpeed = 0.02f;

    [Header("Camera")]
    public float CameraMaxDistance = 11f;
    public float CameraAmplitude = 5f;
    public float CameraSpeed = 0.4f;
}

[Serializable]
public class Ui {
    public GameObject Canvas;
    public Button SaveButton;
    public Button LoadButton;
    public TextMeshProUGUI CurrentEntitiesCount;
}