using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class SimpleCurve : Curve<SUCurveRef>
{
    public static unsafe SimpleCurve Create(params Edge[] edges)
    {
        SUCurveRef curve;
        SUEdgeRef[] edgeRefs = [.. edges.Select(e => e.Reference)];
        fixed (SUEdgeRef* edgeRefsPtr = edgeRefs)
            SUCurveCreateWithEdges(&curve, edgeRefsPtr, (nuint)edgeRefs.Length).CheckError();
        return new(curve);
    }

    private static SUCurveRef RefGetter(SUCurveRef r) => r;

    protected override unsafe delegate* <SUCurveRef, SUCurveRef> ToCurve => &RefGetter;

    protected override unsafe delegate* <SUCurveRef, SUEntityRef> ToEntity => &SUCurveToEntity;

    protected override unsafe delegate* <SUCurveRef*, SUResult> Release => &SUCurveRelease;

    internal SimpleCurve(SUCurveRef @ref)
        : base(@ref) { }
}
