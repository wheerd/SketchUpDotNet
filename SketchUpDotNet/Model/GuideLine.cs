using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class GuideLine : DrawingElement<SUGuideLineRef>
{
    public static unsafe GuideLine CreateFinite(Point3D start, Point3D end)
    {
        SUGuideLineRef gp;
        var s = start.ToSU();
        var e = end.ToSU();
        SUGuideLineCreateFinite(&gp, &s, &e).CheckError();
        return new(gp);
    }

    public static unsafe GuideLine CreateInfinite(Point3D start, Vector3D direction)
    {
        SUGuideLineRef gp;
        var s = start.ToSU();
        var d = direction.ToSU();
        SUGuideLineCreateInfinite(&gp, &s, &d).CheckError();
        return new(gp);
    }

    public unsafe Point3D Start => GetData().start;
    public unsafe Vector3D Direction => GetData().direction;
    public unsafe bool IsInfinite => GetData().isInfinite;

    private unsafe (Point3D start, Vector3D direction, bool isInfinite) GetData()
    {
        SUPoint3D s;
        SUVector3D dir;
        bool inf;
        SUGuideLineGetData(Reference, &s, &dir, &inf).CheckError();
        return (start: new Point3D(s), direction: new Vector3D(dir), isInfinite: inf);
    }

    internal unsafe GuideLine(SUGuideLineRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUGuideLineRef*, SUResult> Release =>
        &SUGuideLineRelease;

    protected sealed override unsafe delegate* <SUGuideLineRef, SUEntityRef> ToEntity =>
        &SUGuideLineToEntity;

    protected sealed override unsafe delegate* <
        SUGuideLineRef,
        SUDrawingElementRef> ToDrawingElement => &SUGuideLineToDrawingElement;
}
