using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class DimensionLinear : Dimension<SUDimensionLinearRef>
{
    public static unsafe DimensionLinear Create(
        Point3D? start,
        InstancePath? startPath,
        Point3D? end,
        InstancePath? endPath,
        double offset
    )
    {
        SUDimensionLinearRef d;
        SUPoint3D? startSU = start?.ToSU();
        SUPoint3D* startPtr = startSU is SUPoint3D s ? &s : (SUPoint3D*)null;
        SUPoint3D? endSU = end?.ToSU();
        SUPoint3D* endPtr = endSU is SUPoint3D e ? &e : (SUPoint3D*)null;
        SUDimensionLinearCreate(
                &d,
                startPtr,
                startPath?.Reference ?? new(),
                endPtr,
                endPath?.Reference ?? new(),
                offset
            )
            .CheckError();
        return new(d);
    }

    public unsafe void GetStartPoint(out Point3D point, out InstancePath? path)
    {
        SUPoint3D p;
        SUInstancePathRef i;
        SUDimensionLinearGetStartPoint(Reference, &p, &i).CheckError();
        point = new(p);
        path = i.ptr != null ? new(i) : null;
    }

    public unsafe void SetStartPoint(Point3D? point, InstancePath? path)
    {
        SUPoint3D? su = point?.ToSU();
        SUPoint3D* suPtr = su is SUPoint3D p ? &p : (SUPoint3D*)null;
        SUDimensionLinearSetStartPoint(Reference, suPtr, path?.Reference ?? new()).CheckError();
    }

    public unsafe void GetEndPoint(out Point3D point, out InstancePath? path)
    {
        SUPoint3D p;
        SUInstancePathRef i;
        SUDimensionLinearGetEndPoint(Reference, &p, &i).CheckError();
        point = new(p);
        path = i.ptr != null ? new(i) : null;
    }

    public unsafe void SetEndPoint(Point3D? point, InstancePath? path)
    {
        SUPoint3D? su = point?.ToSU();
        SUPoint3D* suPtr = su is SUPoint3D p ? &p : (SUPoint3D*)null;
        SUDimensionLinearSetEndPoint(Reference, suPtr, path?.Reference ?? new()).CheckError();
    }

    public unsafe Vector3D XAxis
    {
        get => new(Get<SUVector3D>(&SUDimensionLinearGetXAxis));
        set => Set(&SUDimensionLinearSetXAxis, value.ToSU());
    }

    public unsafe Vector3D Normal
    {
        get => new(Get<SUVector3D>(&SUDimensionLinearGetNormal));
        set => Set(&SUDimensionLinearSetNormal, value.ToSU());
    }

    public unsafe Point2D Position
    {
        get => new(Get<SUPoint2D>(&SUDimensionLinearGetPosition));
        set => Set(&SUDimensionLinearSetPosition, value.ToSU());
    }

    public unsafe SUVerticalTextPositionType VerticalAlignment
    {
        get => Get<SUVerticalTextPositionType>(&SUDimensionLinearGetVerticalAlignment);
        set => Set(&SUDimensionLinearSetVerticalAlignment, value);
    }

    public unsafe SUHorizontalTextPositionType HorizontalAlignment
    {
        get => Get<SUHorizontalTextPositionType>(&SUDimensionLinearGetHorizontalAlignment);
        set => Set(&SUDimensionLinearSetHorizontalAlignment, value);
    }

    public unsafe SUDimensionLinearAlignmentType Alignment =>
        Get<SUDimensionLinearAlignmentType>(&SUDimensionLinearGetAlignment);

    public unsafe Point3D TextPosition => new(Get<SUPoint3D>(&SUDimensionLinearGetTextPosition));

    protected override unsafe delegate* <SUDimensionLinearRef, SUDimensionRef> ToDimension =>
        &SUDimensionLinearToDimension;

    protected override unsafe delegate* <SUDimensionLinearRef*, SUResult> Release =>
        &SUDimensionLinearRelease;

    internal DimensionLinear(SUDimensionLinearRef @ref)
        : base(@ref) { }

    private static unsafe SUEntityRef ToEntityRef(SUDimensionLinearRef @ref) =>
        SUDimensionToEntity(SUDimensionLinearToDimension(@ref));

    private static unsafe SUDrawingElementRef ToDrawingElementRef(SUDimensionLinearRef @ref) =>
        SUDimensionToDrawingElement(SUDimensionLinearToDimension(@ref));

    protected override unsafe delegate* <SUDimensionLinearRef, SUEntityRef> ToEntity =>
        &ToEntityRef;

    protected override unsafe delegate* <
        SUDimensionLinearRef,
        SUDrawingElementRef> ToDrawingElement => &ToDrawingElementRef;
}
