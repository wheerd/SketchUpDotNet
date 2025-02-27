using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Vertex : Entity<SUVertexRef>
{
    public Point3D Position
    {
        get => GetPosition();
        set => SetPosition(value);
    }

    public unsafe Point3D GetPositionTransformed(SUTransformation t)
    {
        SUPoint3D su = Get<SUPoint3D>(&SUVertexGetPosition);
        SUPoint3DTransform(&t, &su).CheckError();
        return Point3D.FromSU(su);
    }

    public IEnumerable<Edge> Edges => GetEdges();

    public IEnumerable<Face> Faces => GetFaces();

    public IEnumerable<Loop> Loops => GetLoops();

    private unsafe Edge[] GetEdges() =>
        GetMany(&SUVertexGetNumEdges, &SUVertexGetEdges, (SUEdgeRef e) => new Edge(e));

    private unsafe Face[] GetFaces() =>
        GetMany(&SUVertexGetNumFaces, &SUVertexGetFaces, (SUFaceRef f) => new Face(f));

    private unsafe Loop[] GetLoops() =>
        GetMany(&SUVertexGetNumLoops, &SUVertexGetLoops, (SULoopRef l) => new Loop(l));

    private unsafe Point3D GetPosition() => Point3D.FromSU(Get<SUPoint3D>(&SUVertexGetPosition));

    private unsafe void SetPosition(Point3D position) => Set(&SUVertexSetPosition, position.ToSU());

    internal Vertex(SUVertexRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUVertexRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUVertexRef, SUEntityRef> ToEntity =>
        &SUVertexToEntity;
}
