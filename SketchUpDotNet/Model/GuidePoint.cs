using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class GuidePoint : DrawingElement<SUGuidePointRef>
{
    public static unsafe GuidePoint Create(Point3D point)
    {
        SUGuidePointRef gp;
        var p = point.ToSU();
        SUGuidePointCreate(&gp, &p).CheckError();
        return new(gp);
    }

    public unsafe Point3D Position => new(Get<SUPoint3D>(&SUGuidePointGetPosition));
    public unsafe Point3D FromPosition => new(Get<SUPoint3D>(&SUGuidePointGetFromPosition));

    public unsafe bool DisplayAsLine => GetBool(&SUGuidePointGetDisplayAsLine);

    internal unsafe GuidePoint(SUGuidePointRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUGuidePointRef*, SUResult> Release =>
        &SUGuidePointRelease;

    protected sealed override unsafe delegate* <SUGuidePointRef, SUEntityRef> ToEntity =>
        &SUGuidePointToEntity;

    protected sealed override unsafe delegate* <
        SUGuidePointRef,
        SUDrawingElementRef> ToDrawingElement => &SUGuidePointToDrawingElement;
}
