using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Loop : Entity<SULoopRef>
{
    public unsafe Face Face => GetOne(&SULoopGetFace, (SUFaceRef f) => new Face(f));

    public unsafe IEnumerable<Edge> Edges =>
        GetMany(&SULoopGetNumVertices, &SULoopGetEdges, (SUEdgeRef e) => new Edge(e));
    public unsafe IEnumerable<EdgeUse> EdgeUses =>
        GetMany(&SULoopGetNumVertices, &SULoopGetEdgeUses, (SUEdgeUseRef e) => new EdgeUse(e));

    public unsafe IEnumerable<Vertex> Vertices =>
        GetMany(&SULoopGetNumVertices, &SULoopGetVertices, (SUVertexRef v) => new Vertex(v));

    public unsafe int VertexCount => GetInt(&SULoopGetNumVertices);

    internal unsafe Loop(SULoopRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SULoopRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SULoopRef, SUEntityRef> ToEntity => &SULoopToEntity;
}
