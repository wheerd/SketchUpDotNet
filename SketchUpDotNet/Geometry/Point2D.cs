using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public sealed class Point2D
{
    public Point2D(double x, double y)
        : this(new SUPoint2D() { x = x.ToSULength(), y = y.ToSULength() }) { }

    public double X => _su.x.FromSULength();
    public double Y => _su.y.FromSULength();

    public unsafe Point2D Transform(Transform2D transform)
    {
        SUTransformation2D t = transform.ToSU();
        SUPoint2D su = _su;
        SUPoint2DTransform(&t, &su).CheckError();
        return new(su);
    }

    public unsafe Point2D Offset(Vector2D offset)
    {
        SUPoint2D newPoint;
        SUVector2D vec = offset.ToSU();
        fixed (SUPoint2D* su = &_su)
            SUPoint2DOffset(su, &vec, &newPoint).CheckError();
        return new(newPoint);
    }

    public unsafe double Distance(Point2D other)
    {
        double dist;
        fixed (SUPoint2D* su = &_su)
        fixed (SUPoint2D* otherSU = &other._su)
            SUPoint2DDistanceToSUPoint2D(su, otherSU, &dist).CheckError();
        return dist;
    }

    public override unsafe bool Equals(object? obj)
    {
        if (obj is Point2D other)
        {
            bool equal;
            fixed (SUPoint2D* su = &_su)
            fixed (SUPoint2D* otherSU = &other._su)
                SUPoint2DGetEqual(su, otherSU, &equal).CheckError();
            return equal;
        }
        return false;
    }

    public override int GetHashCode() => _su.GetHashCode();

    public static bool operator ==(Point2D left, Point2D right) => left.Equals(right);

    public static bool operator !=(Point2D left, Point2D right) => !left.Equals(right);

    private readonly SUPoint2D _su;

    internal Point2D(SUPoint2D su)
    {
        _su = su;
    }

    internal SUPoint2D ToSU() => _su;
}
