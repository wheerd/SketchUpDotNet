using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public sealed class Transform2D
{
    public double M11 => _su.m11;
    public double M12 => _su.m12;
    public double M21 => _su.m21;
    public double M22 => _su.m22;
    public double TX => _su.tx;
    public double TY => _su.ty;

    public static unsafe Transform2D Scale(double scale)
    {
        SUTransformation2D t;
        SUTransformation2DScale(&t, scale).CheckError();
        return new(t);
    }

    public static unsafe Transform2D NonUniformScale(double scaleX, double scaleY)
    {
        SUTransformation2D t;
        SUTransformation2DNonUniformScale(&t, scaleX, scaleY).CheckError();
        return new(t);
    }

    public static unsafe Transform2D ScaleAboutPoint(Point2D point, double scale)
    {
        SUPoint2D pointSU = point.ToSU();
        SUTransformation2D t;
        SUTransformation2DScaleAboutPoint(&t, &pointSU, scale).CheckError();
        return new(t);
    }

    public static unsafe Transform2D NonUniformScaleAboutPoint(
        Point2D point,
        double scaleX,
        double scaleY
    )
    {
        SUPoint2D pointSU = point.ToSU();
        SUTransformation2D t;
        SUTransformation2DNonUniformScaleAboutPoint(&t, &pointSU, scaleX, scaleY).CheckError();
        return new(t);
    }

    public static unsafe Transform2D Translate(Vector2D vector)
    {
        SUVector2D vectorSU = vector.ToSU();
        SUTransformation2D t;
        SUTransformation2DTranslation(&t, &vectorSU).CheckError();
        return new(t);
    }

    public static unsafe Transform2D Rotation(Point2D point, double angle)
    {
        SUPoint2D pointSU = point.ToSU();
        SUTransformation2D t;
        SUTransformation2DRotation(&t, &pointSU, angle).CheckError();
        return new(t);
    }

    public unsafe Transform2D Multiply(Transform2D other)
    {
        SUTransformation2D t;
        fixed (SUTransformation2D* t1 = &_su)
        fixed (SUTransformation2D* t2 = &other._su)
            SUTransformation2DMultiply(t1, t2, &t).CheckError();
        return new(t);
    }

    public static Transform2D operator *(Transform2D left, Transform2D right) =>
        left.Multiply(right);

    public unsafe SUTransformation2D Inverse()
    {
        SUTransformation2D inverse;
        fixed (SUTransformation2D* su = &_su)
            SUTransformation2DGetInverse(su, &inverse).CheckError();
        return inverse;
    }

    public unsafe bool IsIdentity
    {
        get
        {
            bool value;
            fixed (SUTransformation2D* su = &_su)
                SUTransformation2DIsIdentity(su, &value).CheckError();
            return value;
        }
    }

    private readonly SUTransformation2D _su;

    internal Transform2D(SUTransformation2D su)
    {
        _su = su;
    }

    internal SUTransformation2D ToSU() => _su;
}
