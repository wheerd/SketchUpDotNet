using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class EdgeUse : Entity<SUEdgeUseRef>
{
    public unsafe Edge Edge => GetOne(&SUEdgeUseGetEdge, (SUEdgeRef e) => new Edge(e));
    public unsafe Loop Loop => GetOne(&SUEdgeUseGetLoop, (SULoopRef l) => new Loop(l));
    public unsafe Face Face => GetOne(&SUEdgeUseGetFace, (SUFaceRef f) => new Face(f));

    public unsafe Vertex StartVertex =>
        GetOne(&SUEdgeUseGetStartVertex, (SUVertexRef v) => new Vertex(v));
    public unsafe Vector3D StartVertexNormal =>
        Vector3D.FromSU(Get<SUVector3D>(&SUEdgeUseGetStartVertexNormal));
    public unsafe Vertex EndVertex =>
        GetOne(&SUEdgeUseGetEndVertex, (SUVertexRef v) => new Vertex(v));
    public unsafe Vector3D EndVertexNormal =>
        Vector3D.FromSU(Get<SUVector3D>(&SUEdgeUseGetEndVertexNormal));

    public unsafe IEnumerable<EdgeUse> Partners =>
        GetMany(
            &SUEdgeUseGetNumPartners,
            &SUEdgeUseGetPartners,
            (SUEdgeUseRef u) => new EdgeUse(u)
        );
    public unsafe int PartnerCount => GetInt(&SUEdgeUseGetNumPartners);

    public unsafe bool IsReversed => GetBool(&SUEdgeUseIsReversed);

    public unsafe EdgeUse Previous =>
        GetOne(&SUEdgeUseGetPrevious, (SUEdgeUseRef u) => new EdgeUse(u));
    public unsafe EdgeUse Next => GetOne(&SUEdgeUseGetNext, (SUEdgeUseRef u) => new EdgeUse(u));

    internal unsafe EdgeUse(SUEdgeUseRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUEdgeUseRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUEdgeUseRef, SUEntityRef> ToEntity =>
        &SUEdgeUseToEntity;
}
