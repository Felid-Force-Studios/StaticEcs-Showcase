using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FFS.Libraries.StaticEcs;
using FFS.Libraries.StaticPack;
using Unity.Collections;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public struct InitRenderSystem : IInitSystem, IDestroySystem {
    static readonly int BufferID = Shader.PropertyToID("_Buffer");
    static readonly int ScaleID = Shader.PropertyToID("_Scale");

    public void Init() {
        var cfg = W.Context<SceneConfig>.Get();
        var mesh = CreateMesh();
        var gpuBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, cfg.MaxEntities, Marshal.SizeOf(typeof(InstanceGpuData)));
        var material = CreateMaterial(cfg, gpuBuffer);

        W.Context<RenderData>.Set(new RenderData {
            Material = material,
            Camera = Camera.main,
            Mesh = mesh,
            GpuInstancesBuffer = gpuBuffer,
            SphereRadius = cfg.StartSphereRadius,
        });


        W.Serializer.SetSnapshotHandler(
            new Guid("bc1da30558fd5ad459c48143852ff61e"), 0,
            (ref BinaryPackWriter writer) => {
                ref var renderData = ref W.Context<RenderData>.Get();
                writer.WriteInt(renderData.InstanceCount);
                writer.WriteFloat(renderData.SphereRadius);
                writer.WriteFloat(renderData.TimeFromStart);
            }, (ref BinaryPackReader reader, ushort version) => {
                ref var renderData = ref W.Context<RenderData>.Get();
                renderData.InstanceCount = reader.ReadInt();
                renderData.SphereRadius = reader.ReadFloat();
                renderData.TimeFromStart = reader.ReadFloat();
            });
    }

    private static Material CreateMaterial(SceneConfig settings, GraphicsBuffer buffer) {
        var material = new Material(settings.Material) {
            enableInstancing = true,
        };
        material.SetBuffer(BufferID, buffer);
        material.SetFloat(ScaleID, 0.01f);
        return material;
    }

    public void Destroy() {
        W.Context<RenderData>.Get().GpuInstancesBuffer?.Dispose();
    }

    [MethodImpl(AggressiveInlining)]
    private static Mesh CreateMesh() {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var mesh = cube.GetComponent<MeshFilter>().mesh;
        GameObject.Destroy(cube);
        return mesh;
    }
}

[StructLayout(LayoutKind.Explicit)]
public struct InstanceGpuData {
    [FieldOffset(0)] public Vector3 position;
    [FieldOffset(0)] public float positionX;
    [FieldOffset(4)] public float positionY;
    [FieldOffset(8)] public float positionZ;
    [FieldOffset(12)] public float pad;
    [FieldOffset(16)] public Color color;

    [MethodImpl(AggressiveInlining)]
    public InstanceGpuData(Vector3 position, Color color) {
        positionX = 0;
        positionY = 0;
        positionZ = 0;
        pad = 0;
        this.position = position;
        this.color = color;
    }
}

public struct RenderData {
    public Camera Camera;
    public Material Material;
    public Mesh Mesh;
    public GraphicsBuffer GpuInstancesBuffer;
    public float SphereRadius;
    public int InstanceCount;
    public float TimeFromStart;

    [MethodImpl(AggressiveInlining)]
    public void Draw(NativeArray<InstanceGpuData> data) {
        GpuInstancesBuffer.SetData(data);

        var renderParams = new RenderParams(Material) {
            camera = Camera,
            worldBounds = new Bounds(Vector3.zero, SphereRadius * Vector3.one)
        };

        Graphics.RenderMeshPrimitives(renderParams, Mesh, 0, InstanceCount);
    }
}