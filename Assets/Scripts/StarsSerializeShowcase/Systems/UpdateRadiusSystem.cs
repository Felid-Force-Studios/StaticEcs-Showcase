using System.Runtime.CompilerServices;
using FFS.Libraries.StaticEcs;
using UnityEngine;

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