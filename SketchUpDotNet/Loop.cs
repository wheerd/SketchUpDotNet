using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Loop : Entity<SULoopRef>
{
    public Face Face => GetFace();

    public IEnumerable<Edge> Edges => GetEdges();

    public IEnumerable<Vertex> Vertices => GetVertices();

    private unsafe Edge[] GetEdges() =>
        GetMany(&SULoopGetNumVertices, &SULoopGetEdges, (SUEdgeRef e) => new Edge(e));

    private unsafe Vertex[] GetVertices() =>
        GetMany(&SULoopGetNumVertices, &SULoopGetVertices, (SUVertexRef v) => new Vertex(v));

    private unsafe Face GetFace() => GetOne(&SULoopGetFace, (SUFaceRef f) => new Face(f));

    internal unsafe Loop(SULoopRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SULoopRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SULoopRef, SUEntityRef> ToEntity => &SULoopToEntity;
}
