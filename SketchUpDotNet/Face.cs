using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Face : DrawingElement<SUFaceRef>
{
    public static unsafe Face CreateSimple(params Point3D[] points)
    {
        SUFaceRef face;
        var vertices = points.Select(p => p.ToSU()).ToArray();
        fixed (SUPoint3D* pointsPtr = &vertices[0])
        {
            SUFaceCreateSimple(&face, pointsPtr, (nuint)vertices.Length).CheckError();
        }
        return new Face(face);
    }

    public static unsafe Face Create(Point3D[] points, LoopInput input)
    {
        SUFaceRef face;
        var pointsSU = points.Select(p => p.ToSU()).ToArray();
        fixed (SUPoint3D* pointsPtr = &pointsSU[0])
        fixed (SULoopInputRef* inputPtr = &input.Reference)
        {
            SUFaceCreate(&face, pointsPtr, inputPtr).CheckError();
        }
        return new(face);
    }

    public Material? FrontMaterial
    {
        get => GetFrontMaterial();
        set => SetFrontMaterial(value);
    }

    public Material? BackMaterial
    {
        get => GetBackMaterial();
        set => SetBackMaterial(value);
    }

    public Loop OuterLoop => GetOuterLoop();

    public IEnumerable<Loop> InnerLoops => GetInnerLoops();

    public IEnumerable<Edge> Edges => GetEdges();
    public int EdgeCount => GetEdgeCount();

    public IEnumerable<Vertex> Vertices => GetVertices();
    public int VertexCount => GetVertexCount();

    private unsafe int GetVertexCount() => GetInt(&SUFaceGetNumVertices);

    private unsafe int GetEdgeCount() => GetInt(&SUFaceGetNumEdges);

    private unsafe Material? GetFrontMaterial() =>
        GetOptionalOne<SUMaterialRef, Material>(&SUFaceGetFrontMaterial, f => new(f));

    private unsafe void SetFrontMaterial(Material? material) =>
        SUFaceSetFrontMaterial(Reference, material?.Reference ?? default).CheckError();

    private unsafe Material? GetBackMaterial() =>
        GetOptionalOne<SUMaterialRef, Material>(&SUFaceGetBackMaterial, f => new(f));

    private unsafe void SetBackMaterial(Material? material) =>
        SUFaceSetBackMaterial(Reference, material?.Reference ?? default).CheckError();

    private unsafe Edge[] GetEdges() =>
        GetMany(&SUFaceGetNumEdges, &SUFaceGetEdges, static (SUEdgeRef e) => new Edge(e));

    private unsafe Vertex[] GetVertices() =>
        GetMany(&SUFaceGetNumVertices, &SUFaceGetVertices, static (SUVertexRef v) => new Vertex(v));

    private unsafe Loop GetOuterLoop() => GetOne(&SUFaceGetOuterLoop, (SULoopRef l) => new Loop(l));

    private unsafe Loop[] GetInnerLoops() =>
        GetMany(&SUFaceGetNumInnerLoops, &SUFaceGetInnerLoops, static (SULoopRef l) => new Loop(l));

    internal unsafe Face(SUFaceRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUFaceRef*, SUResult> Release => &SUFaceRelease;

    protected sealed override unsafe delegate* <SUFaceRef, SUEntityRef> ToEntity => &SUFaceToEntity;

    protected sealed override unsafe delegate* <SUFaceRef, SUDrawingElementRef> ToDrawingElement =>
        &SUFaceToDrawingElement;
}
