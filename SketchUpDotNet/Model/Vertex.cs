using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Vertex : Entity<SUVertexRef>
{
    public unsafe Point3D Position
    {
        get => new(Get<SUPoint3D>(&SUVertexGetPosition));
        set => Set(&SUVertexSetPosition, value.ToSU());
    }

    public unsafe Point3D GetPositionTransformed(Transform3D transform)
    {
        SUTransformation t = transform.ToSU();
        SUPoint3D su = Get<SUPoint3D>(&SUVertexGetPosition);
        SUPoint3DTransform(&t, &su).CheckError();
        return new Point3D(su);
    }

    public unsafe IEnumerable<Edge> Edges =>
        GetMany(&SUVertexGetNumEdges, &SUVertexGetEdges, (SUEdgeRef e) => new Edge(e));

    public unsafe IEnumerable<Face> Faces =>
        GetMany(&SUVertexGetNumFaces, &SUVertexGetFaces, (SUFaceRef f) => new Face(f));

    public unsafe IEnumerable<Loop> Loops =>
        GetMany(&SUVertexGetNumLoops, &SUVertexGetLoops, (SULoopRef l) => new Loop(l));

    internal Vertex(SUVertexRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUVertexRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUVertexRef, SUEntityRef> ToEntity =>
        &SUVertexToEntity;
}
