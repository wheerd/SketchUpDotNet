using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Axes : DrawingElement<SUAxesRef>
{
    public unsafe Axes()
        : base(&SUAxesCreate) { }

    public static unsafe Axes Create(Point3D origin, Vector3D xAxis, Vector3D yAxis, Vector3D zAxis)
    {
        SUAxesRef axes;
        var o = origin.ToSU();
        var x = xAxis.ToSU();
        var y = yAxis.ToSU();
        var z = zAxis.ToSU();
        SUAxesCreateCustom(&axes, &o, &x, &y, &z).CheckError();
        return new(axes);
    }

    public unsafe Point3D Origin
    {
        get => new(Get<SUPoint3D>(&SUAxesGetOrigin));
        set => Set(&SUAxesSetOrigin, value.ToSU());
    }

    public unsafe Vector3D XAxis => new(Get<SUVector3D>(&SUAxesGetXAxis));
    public unsafe Vector3D YAxis => new(Get<SUVector3D>(&SUAxesGetYAxis));
    public unsafe Vector3D ZAxis => new(Get<SUVector3D>(&SUAxesGetZAxis));

    public unsafe void SetAxis(Vector3D xAxis, Vector3D yAxis, Vector3D zAxis)
    {
        var x = xAxis.ToSU();
        var y = yAxis.ToSU();
        var z = zAxis.ToSU();
        SUAxesSetAxesVecs(Reference, &x, &y, &z).CheckError();
    }

    public unsafe Transform3D Transform => new(Get<SUTransformation>(&SUAxesGetTransform));

    public unsafe Plane3D Plane => new(Get<SUPlane3D>(&SUAxesGetPlane));

    internal unsafe Axes(SUAxesRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUAxesRef*, SUResult> Release => &SUAxesRelease;

    protected override unsafe delegate* <SUAxesRef, SUEntityRef> ToEntity => &SUAxesToEntity;

    protected override unsafe delegate* <SUAxesRef, SUDrawingElementRef> ToDrawingElement =>
        &SUAxesToDrawingElement;
}
