using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Polyline3D : DrawingElement<SUPolyline3dRef>
{
    public unsafe Polyline3D()
        : base(&SUPolyline3dCreate) { }

    public unsafe IEnumerable<Point3D> Points =>
        GetMany<SUPoint3D>(&SUPolyline3dGetNumPoints, &SUPolyline3dGetPoints)
            .Select(p => new Point3D(p));
    public unsafe int PointCount => GetInt(&SUPolyline3dGetNumPoints);

    public unsafe void AddPoints(params Point3D[] points)
    {
        var suPoints = points.Select(p => p.ToSU()).ToArray();
        fixed (SUPoint3D* ptr = &suPoints[0])
        {
            SUPolyline3dAddPoints(Reference.EnsureReferenceValid(), (nuint)suPoints.Length, ptr)
                .CheckError();
        }
    }

    internal unsafe Polyline3D(SUPolyline3dRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUPolyline3dRef*, SUResult> Release =>
        &SUPolyline3dRelease;

    protected sealed override unsafe delegate* <SUPolyline3dRef, SUEntityRef> ToEntity =>
        &SUPolyline3dToEntity;

    protected override unsafe delegate* <SUPolyline3dRef, SUDrawingElementRef> ToDrawingElement =>
        &SUPolyline3dToDrawingElement;
}
