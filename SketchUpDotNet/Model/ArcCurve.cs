using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class ArcCurve : Curve<SUArcCurveRef>
{
    public static unsafe ArcCurve Create(
        Point3D center,
        Point3D start,
        Point3D end,
        Vector3D normal,
        int edgeCount
    )
    {
        SUArcCurveRef curve;
        SUPoint3D centerPoint = center.ToSU();
        SUPoint3D startPoint = start.ToSU();
        SUPoint3D endPoint = end.ToSU();
        SUVector3D normalSU = normal.ToSU();
        SUArcCurveCreate(&curve, &centerPoint, &startPoint, &endPoint, &normalSU, (nuint)edgeCount)
            .CheckError();
        return new(curve);
    }

    public unsafe double Radius => GetDouble(&SUArcCurveGetRadius).FromSULength();
    public unsafe bool IsFullCircle => GetBool(&SUArcCurveGetIsFullCircle);

    public unsafe Point3D StartPoint => new(Get<SUPoint3D>(&SUArcCurveGetStartPoint));
    public unsafe double StartAngle => GetDouble(&SUArcCurveGetStartAngle);
    public unsafe Point3D EndPoint => new(Get<SUPoint3D>(&SUArcCurveGetEndPoint));
    public unsafe double EndAngle => GetDouble(&SUArcCurveGetEndAngle);
    public unsafe Point3D Center => new(Get<SUPoint3D>(&SUArcCurveGetCenter));

    public unsafe Vector3D XAxis => new(Get<SUVector3D>(&SUArcCurveGetXAxis));
    public unsafe Vector3D YAxis => new(Get<SUVector3D>(&SUArcCurveGetYAxis));
    public unsafe Vector3D Normal => new(Get<SUVector3D>(&SUArcCurveGetNormal));

    internal unsafe ArcCurve(SUArcCurveRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUArcCurveRef*, SUResult> Release =>
        &SUArcCurveRelease;

    protected sealed override unsafe delegate* <SUArcCurveRef, SUEntityRef> ToEntity =>
        &SUArcCurveToEntity;

    protected override unsafe delegate* <SUArcCurveRef, SUCurveRef> ToCurve => &SUArcCurveToCurve;
}
