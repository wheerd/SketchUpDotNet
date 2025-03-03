using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public readonly struct Vector2D
{
    public Vector2D(double x, double y)
        : this(new SUVector2D() { x = x.ToSULength(), y = y.ToSULength() }) { }

    public static unsafe Vector2D Create(Point2D from, Point2D to)
    {
        SUVector2D vector;
        SUPoint2D fromSU = from.ToSU();
        SUPoint2D toSU = to.ToSU();
        SUVector2DCreate(&vector, &fromSU, &toSU).CheckError();
        return new(vector);
    }

    public double X => _su.x.FromSULength();
    public double Y => _su.y.FromSULength();

    public unsafe double Length
    {
        get
        {
            double length;
            fixed (SUVector2D* su = &_su)
                SUVector2DGetLength(su, &length).CheckError();
            return length.FromSULength();
        }
    }

    public unsafe Vector2D WithLength(double value)
    {
        SUVector2D su = _su;
        SUVector2DSetLength(&su, value.ToSULength()).CheckError();
        return new(su);
    }

    public unsafe bool IsValid
    {
        get
        {
            bool valid;
            fixed (SUVector2D* su = &_su)
                SUVector2DIsValid(su, &valid).CheckError();
            return valid;
        }
    }

    public unsafe bool IsUnitVector
    {
        get
        {
            bool valid;
            fixed (SUVector2D* su = &_su)
                SUVector2DIsUnitVector(su, &valid).CheckError();
            return valid;
        }
    }

    public unsafe Vector2D Transform(Transform2D transform)
    {
        SUTransformation2D t = transform.ToSU();
        SUVector2D su = _su;
        SUVector2DTransform(&t, &su).CheckError();
        return new(su);
    }

    public unsafe Vector2D Normalize()
    {
        SUVector2D su = _su;
        SUVector2DNormalize(&su).CheckError();
        return new(su);
    }

    public unsafe Vector2D Reverse()
    {
        SUVector2D su = _su;
        SUVector2DReverse(&su).CheckError();
        return new(su);
    }

    public unsafe bool IsParallelTo(Vector2D other)
    {
        bool parallel;
        fixed (SUVector2D* su = &_su)
            SUVector2DIsParallelTo(su, &other._su, &parallel).CheckError();
        return parallel;
    }

    public unsafe bool IsPerpendicularTo(Vector2D other)
    {
        bool perpendicular;
        fixed (SUVector2D* su = &_su)
            SUVector2DIsPerpendicularTo(su, &other._su, &perpendicular).CheckError();
        return perpendicular;
    }

    public unsafe bool IsSameDirectionAs(Vector2D other)
    {
        bool sameDirection;
        fixed (SUVector2D* su = &_su)
            SUVector2DIsSameDirectionAs(su, &other._su, &sameDirection).CheckError();
        return sameDirection;
    }

    public unsafe double Dot(Vector2D other)
    {
        double dot;
        fixed (SUVector2D* su = &_su)
            SUVector2DDot(su, &other._su, &dot).CheckError();
        return dot;
    }

    public unsafe double Cross(Vector2D other)
    {
        double cross;
        fixed (SUVector2D* su = &_su)
            SUVector2DCross(su, &other._su, &cross).CheckError();
        return cross;
    }

    public unsafe double AngleBetween(Vector2D other)
    {
        double angle;
        fixed (SUVector2D* su = &_su)
            SUVector2DAngleBetween(su, &other._su, &angle).CheckError();
        return angle;
    }

    public override unsafe bool Equals(object? obj)
    {
        if (obj is Vector2D other)
        {
            bool equal;
            fixed (SUVector2D* su = &_su)
                SUVector2DIsEqual(su, &other._su, &equal).CheckError();
            return equal;
        }
        return false;
    }

    public override int GetHashCode() => _su.GetHashCode();

    public static bool operator ==(Vector2D left, Vector2D right) => left.Equals(right);

    public static bool operator !=(Vector2D left, Vector2D right) => !left.Equals(right);

    private readonly SUVector2D _su;

    internal Vector2D(SUVector2D su)
    {
        _su = su;
    }

    internal SUVector2D ToSU() => _su;
}
