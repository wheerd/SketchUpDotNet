using SketchUpDotNet.Bindings;

namespace SketchUpDotNet;

public sealed class BoundingBox
{
    private readonly SUBoundingBox3D _su;

    internal BoundingBox(SUBoundingBox3D su)
    {
        _su = su;
    }

    public Point3D Min => new(_su.min_point);
    public Point3D Max => new(_su.max_point);

    public double SizeX => Max.X - Min.X;
    public double SizeY => Max.Y - Min.Y;
    public double SizeZ => Max.Z - Min.Z;

    public Point3D Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);

    public Vector3D Size => Vector3D.Create(Min, Max);
}
