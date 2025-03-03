using System.Diagnostics;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public readonly struct Vector3D
{
    public Vector3D(double x, double y, double z)
        : this(
            new SUVector3D()
            {
                x = x.ToSULength(),
                y = y.ToSULength(),
                z = z.ToSULength(),
            }
        ) { }

    public static unsafe Vector3D Create(Point3D from, Point3D to)
    {
        SUVector3D vector;
        SUPoint3D fromSU = from.ToSU();
        SUPoint3D toSU = to.ToSU();
        SUVector3DCreate(&vector, &fromSU, &toSU).CheckError();
        return new(vector);
    }

    public static unsafe Vector3D LinearCombination(
        Vector3D vector1,
        Vector3D vector2,
        double weight1
    )
    {
        SUVector3D[] vectors = [vector1._su, vector2._su];
        double[] weights = [weight1, 1 - weight1];
        SUVector3D vector;
        fixed (SUVector3D* vectorsPtr = vectors)
        fixed (double* weightsPtr = weights)
            SUVector3DLinearCombination(vectorsPtr, weightsPtr, 2, &vector).CheckError();
        return new(vector);
    }

    public static unsafe Vector3D LinearCombination(
        Vector3D vector1,
        Vector3D vector2,
        Vector3D vector3,
        double weight1,
        double weight2
    )
    {
        SUVector3D[] vectors = [vector1._su, vector2._su, vector3._su];
        double[] weights = [weight1, weight2, 1 - weight1 - weight2];
        SUVector3D vector;
        fixed (SUVector3D* vectorsPtr = vectors)
        fixed (double* weightsPtr = weights)
            SUVector3DLinearCombination(vectorsPtr, weightsPtr, 2, &vector).CheckError();
        return new(vector);
    }

    public double X => _su.x.FromSULength();
    public double Y => _su.y.FromSULength();
    public double Z => _su.z.FromSULength();

    public unsafe double Length
    {
        get
        {
            double length;
            fixed (SUVector3D* su = &_su)
                SUVector3DGetLength(su, &length).CheckError();
            return length.FromSULength();
        }
    }

    public unsafe Vector3D WithLength(double value)
    {
        SUVector3D su = _su;
        SUVector3DSetLength(&su, value.ToSULength()).CheckError();
        return new(su);
    }

    public unsafe bool IsValid
    {
        get
        {
            bool valid;
            fixed (SUVector3D* su = &_su)
                SUVector3DIsValid(su, &valid).CheckError();
            return valid;
        }
    }

    public unsafe bool IsUnitVector
    {
        get
        {
            bool valid;
            fixed (SUVector3D* su = &_su)
                SUVector3DIsUnitVector(su, &valid).CheckError();
            return valid;
        }
    }

    public unsafe Vector3D Transform(Transform transform)
    {
        SUTransformation t = transform.ToSU();
        SUVector3D su = _su;
        SUVector3DTransform(&t, &su).CheckError();
        return new(su);
    }

    public unsafe Vector3D Normalize()
    {
        SUVector3D su = _su;
        SUVector3DNormalize(&su).CheckError();
        return new(su);
    }

    public unsafe Vector3D Reverse()
    {
        SUVector3D su = _su;
        SUVector3DReverse(&su).CheckError();
        return new(su);
    }

    public unsafe bool IsParallelTo(Vector3D other)
    {
        bool parallel;
        fixed (SUVector3D* su = &_su)
            SUVector3DIsParallelTo(su, &other._su, &parallel).CheckError();
        return parallel;
    }

    public unsafe bool IsPerpendicularTo(Vector3D other)
    {
        bool perpendicular;
        fixed (SUVector3D* su = &_su)
            SUVector3DIsPerpendicularTo(su, &other._su, &perpendicular).CheckError();
        return perpendicular;
    }

    public unsafe bool IsSameDirectionAs(Vector3D other)
    {
        bool sameDirection;
        fixed (SUVector3D* su = &_su)
            SUVector3DIsSameDirectionAs(su, &other._su, &sameDirection).CheckError();
        return sameDirection;
    }

    public unsafe double Dot(Vector3D other)
    {
        double dot;
        fixed (SUVector3D* su = &_su)
            SUVector3DDot(su, &other._su, &dot).CheckError();
        return dot;
    }

    public unsafe Vector3D Cross(Vector3D other)
    {
        SUVector3D cross;
        fixed (SUVector3D* su = &_su)
            SUVector3DCross(su, &other._su, &cross).CheckError();
        return new(cross);
    }

    public unsafe (Vector3D, Vector3D) GetArbitraryAxis()
    {
        SUVector3D xAxis;
        SUVector3D yAxis;
        fixed (SUVector3D* su = &_su)
            SUVector3DGetArbitraryAxes(su, &xAxis, &yAxis).CheckError();
        return (new(xAxis), new(yAxis));
    }

    public unsafe double AngleBetween(Vector3D other)
    {
        double angle;
        fixed (SUVector3D* su = &_su)
            SUVector3DAngleBetween(su, &other._su, &angle).CheckError();
        return angle;
    }

    public override unsafe bool Equals(object? obj)
    {
        if (obj is Vector3D other)
        {
            bool equal;
            fixed (SUVector3D* su = &_su)
                SUVector3DIsEqual(su, &other._su, &equal).CheckError();
            return equal;
        }
        return false;
    }

    public unsafe bool LessThan(Vector3D other)
    {
        bool equal;
        fixed (SUVector3D* su = &_su)
            SUVector3DLessThan(su, &other._su, &equal).CheckError();
        return equal;
    }

    public override int GetHashCode() => _su.GetHashCode();

    public static bool operator ==(Vector3D left, Vector3D right) => left.Equals(right);

    public static bool operator !=(Vector3D left, Vector3D right) => !left.Equals(right);

    public static bool operator <(Vector3D left, Vector3D right) => left.LessThan(right);

    public static bool operator >(Vector3D left, Vector3D right) => right.LessThan(left);

    private readonly SUVector3D _su;

    internal Vector3D(SUVector3D su)
    {
        _su = su;
    }

    internal SUVector3D ToSU() => _su;

    internal Vector3D(double[] values)
        : this(
            new SUVector3D()
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
