using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class GeometryInput : SUBase<SUGeometryInputRef>
{
    public unsafe void SetVertices(params Point3D[] points)
    {
        var vertices = points.Select(p => p.ToSU()).ToArray();
        fixed (SUPoint3D* verticesPtr = &vertices[0])
        {
            SUGeometryInputSetVertices(Reference, (nuint)vertices.Length, verticesPtr).CheckError();
        }
    }

    public unsafe void AddVertices(params Point3D[] points)
    {
        foreach (var point in points)
        {
            var vertex = point.ToSU();
            SUGeometryInputAddVertex(Reference, &vertex).CheckError();
        }
    }

    public unsafe LoopInput CreateLoopInput(params Point3D[] points)
    {
        nuint verticesCount,
            faceCount,
            edgeCount,
            curveCount,
            arcCount;
        SUGeometryInputGetCounts(
                Reference,
                &verticesCount,
                &faceCount,
                &edgeCount,
                &curveCount,
                &arcCount
            )
            .CheckError();
        AddVertices(points);
        var indices = Enumerable.Range((int)verticesCount, points.Length).ToArray();
        return new LoopInput(indices);
    }

    private readonly HashSet<Material> materials = [];
    private readonly HashSet<Layer> layers = [];

    public unsafe void AddFace(
        LoopInput outerLoop,
        Material? material = null,
        Layer? layer = null,
        LoopInput[]? innerLoops = null
    )
    {
        nuint faceIndex;
        if (layer != null)
        {
            outerLoop.SetLayer(layer);
        }
        fixed (SULoopInputRef* inputRef = &outerLoop.Reference)
        {
            SUGeometryInputAddFace(Reference, inputRef, &faceIndex).CheckError();
        }
        if (innerLoops != null)
        {
            foreach (var loop in innerLoops)
            {
                if (layer != null)
                {
                    loop.SetLayer(layer);
                }
                fixed (SULoopInputRef* loopRef = &loop.Reference)
                {
                    SUGeometryInputFaceAddInnerLoop(Reference, faceIndex, loopRef).CheckError();
                }
            }
        }
        if (material != null)
        {
            SUMaterialInput materialInput = new() { material = material.Reference };
            SUGeometryInputFaceSetFrontMaterial(Reference, faceIndex, &materialInput).CheckError();
            SUGeometryInputFaceSetBackMaterial(Reference, faceIndex, &materialInput).CheckError();
            materials.Add(material);
        }
        if (layer != null)
        {
            SUGeometryInputFaceSetLayer(Reference, faceIndex, layer.Reference).CheckError();
            layers.Add(layer);
        }
    }

    public unsafe GeometryInput()
        : base(&SUGeometryInputCreate) { }

    internal override void SetAttachedToModel(bool attached)
    {
        foreach (var material in materials)
        {
            material.SetAttachedToModel(attached);
        }
        foreach (var layer in layers)
        {
            layer.SetAttachedToModel(attached);
        }
    }

    internal unsafe GeometryInput(SUGeometryInputRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUGeometryInputRef*, SUResult> Release =>
        &SUGeometryInputRelease;
}
