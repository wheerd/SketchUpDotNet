using System.Drawing;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Curve : Entity<SUCurveRef>
{
    public static unsafe Curve Create(params Edge[] edges)
    {
        SUCurveRef curve;
        SUEdgeRef[] edgeRefs = [.. edges.Select(e => e.Reference)];
        fixed (SUEdgeRef* edgeRefsPtr = edgeRefs)
            SUCurveCreateWithEdges(&curve, edgeRefsPtr, (nuint)edgeRefs.Length).CheckError();
        return new(curve);
    }

    public unsafe SUCurveType Type => Get<SUCurveType>(&SUCurveGetType);

    public unsafe bool IsPolygon => GetBool(&SUCurveIsPolygon);

    public unsafe IEnumerable<Edge> Edges =>
        GetMany(&SUCurveGetNumEdges, &SUCurveGetEdges, (SUEdgeRef e) => new Edge(e));
    public unsafe int EdgeCount => GetInt(&SUCurveGetNumEdges);

    internal unsafe Curve(SUCurveRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUCurveRef*, SUResult> Release => &SUCurveRelease;

    protected sealed override unsafe delegate* <SUCurveRef, SUEntityRef> ToEntity =>
        &SUCurveToEntity;
}
