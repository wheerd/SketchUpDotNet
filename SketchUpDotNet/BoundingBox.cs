using SketchUpDotNet.Bindings;

namespace SketchUpDotNet;

public readonly record struct BoundingBox(Point3D Min, Point3D Max)
{
    internal static BoundingBox FromSU(SUBoundingBox3D bb) =>
        new(new Point3D(bb.min_point), new Point3D(bb.max_point));

    public double SizeX => Max.X - Min.X;
    public double SizeY => Max.Y - Min.Y;
    public double SizeZ => Max.Z - Min.Z;

    public Point3D Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2, (Min.Z + Max.Z) / 2);

    public Vector3D Size => new(Max.X - Min.X, Max.Y - Min.Y, Max.Z - Min.Z);
}
