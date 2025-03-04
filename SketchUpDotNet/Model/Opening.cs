using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Opening : SUBase<SUOpeningRef>
{
    public unsafe IEnumerable<Point3D> Points =>
        GetMany<SUPoint3D>(&SUOpeningGetNumPoints, &SUOpeningGetPoints).Select(p => new Point3D(p));
    public unsafe int PointCount => GetInt(&SUOpeningGetNumPoints);

    internal unsafe Opening(SUOpeningRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUOpeningRef*, SUResult> Release => null;
}
