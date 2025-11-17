using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using UnityEngine;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
public struct UpdateRadiusSystem : IUpdateSystem {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() {
        ref var data = ref W.Context<RenderData>.Get();
        ref var cfg = ref W.Context<SceneConfig>.Get();

        var dt = Time.deltaTime;
        var timeFromStart = data.TimeFromStart;

        ref var radius = ref data.SphereRadius;
        if (timeFromStart > cfg.SpawnTimeSeconds && radius <= cfg.MaxSphereRadius) {
            radius = Mathf.Lerp(radius, timeFromStart < cfg.SpawnTimeSeconds + cfg.RadiusChangeTimeSeconds ? cfg.MinSphereRadius : cfg.MaxSphereRadius, cfg.RadiusChangeSpeed * dt);
        }
    }
}