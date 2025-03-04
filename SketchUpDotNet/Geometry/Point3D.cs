using System.Diagnostics;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public sealed class Point3D
{
    public Point3D(double x, double y, double z)
        : this(
            new SUPoint3D()
            {
                x = x.ToSULength(),
                y = y.ToSULength(),
                z = z.ToSULength(),
            }
        ) { }

    public double X => _su.x.FromSULength();
    public double Y => _su.y.FromSULength();
    public double Z => _su.z.FromSULength();

    public unsafe Point3D Transform(Transform3D transform)
    {
        SUTransformation t = transform.ToSU();
        SUPoint3D su = _su;
        SUPoint3DTransform(&t, &su).CheckError();
        return new(su);
    }

    public unsafe Point3D Offset(Vector3D offset)
    {
        SUPoint3D newPoint;
        SUVector3D vec = offset.ToSU();
        fixed (SUPoint3D* su = &_su)
            SUPoint3DOffset(su, &vec, &newPoint).CheckError();
        return new(newPoint);
    }

    public unsafe double Distance(Point3D other)
    {
        double dist;
        fixed (SUPoint3D* su = &_su)
        fixed (SUPoint3D* otherSU = &other._su)
            SUPoint3DDistanceToSUPoint3D(su, otherSU, &dist).CheckError();
        return dist;
    }

    public override unsafe bool Equals(object? obj)
    {
        if (obj is Point3D other)
        {
            bool equal;
            fixed (SUPoint3D* su = &_su)
            fixed (SUPoint3D* otherSU = &other._su)
                SUPoint3DGetEqual(su, otherSU, &equal).CheckError();
            return equal;
        }
        return false;
    }

    public unsafe bool LessThan(Point3D other)
    {
        bool equal;
        fixed (SUPoint3D* su = &_su)
        fixed (SUPoint3D* otherSU = &other._su)
            SUPoint3DLessThan(su, otherSU, &equal).CheckError();
        return equal;
    }

    public override int GetHashCode() => _su.GetHashCode();

    public static bool operator ==(Point3D left, Point3D right) => left.Equals(right);

    public static bool operator !=(Point3D left, Point3D right) => !left.Equals(right);

    public static bool operator <(Point3D left, Point3D right) => left.LessThan(right);

    public static bool operator >(Point3D left, Point3D right) => right.LessThan(left);

    private readonly SUPoint3D _su;

    internal Point3D(SUPoint3D su)
    {
        _su = su;
    }

    internal SUPoint3D ToSU() => _su;

    internal Point3D(double[] values)
        : this(
            new SUPoint3D()
            {
                x = values[0],
                y = values[1],
                z = values[2],
            }
        )
    {
        Debug.Assert(values.Length == 3);
    }
}
