using System.Runtime.InteropServices;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public sealed class Transform3D
{
    public static Transform3D Identity => Scale(1.0);

    public static unsafe Transform3D FromValues(double[] values)
    {
        if (values.Length != 16)
            throw new ArgumentException(
                "Transform matrix must have exactly 16 values",
                nameof(values)
            );

        SUTransformation t;
        for (int i = 0; i < 16; i++)
            t.values[i] = values[i];
        return new(t);
    }

    public static unsafe Transform3D FromPointAndNormal(Point3D point, Vector3D normal)
    {
        SUPoint3D pointSU = point.ToSU();
        SUVector3D normalSU = normal.ToSU();
        SUTransformation t;
        SUTransformationSetFromPointAndNormal(&t, &pointSU, &normalSU).CheckError();
        return new(t);
    }

    public static unsafe Transform3D FromPointAndNormal(
        Point3D point,
        Vector3D xAxis,
        Vector3D yAxis,
        Vector3D zAxis
    )
    {
        SUPoint3D pointSU = point.ToSU();
        SUVector3D xAxisSU = xAxis.ToSU();
        SUVector3D yAxisSU = yAxis.ToSU();
        SUVector3D zAxisSU = zAxis.ToSU();
        SUTransformation t;
        SUTransformationSetFromPointAndAxes(&t, &pointSU, &xAxisSU, &yAxisSU, &zAxisSU)
            .CheckError();
        return new(t);
    }

    public static unsafe Transform3D Scale(double scale)
    {
        SUTransformation t;
        SUTransformationScale(&t, scale).CheckError();
        return new(t);
    }

    public static unsafe Transform3D NonUniformScale(double scaleX, double scaleY, double scaleZ)
    {
        SUTransformation t;
        SUTransformationNonUniformScale(&t, scaleX, scaleY, scaleZ).CheckError();
        return new(t);
    }

    public static unsafe Transform3D ScaleAboutPoint(Point3D point, double scale)
    {
        SUPoint3D pointSU = point.ToSU();
        SUTransformation t;
        SUTransformationScaleAboutPoint(&t, &pointSU, scale).CheckError();
        return new(t);
    }

    public static unsafe Transform3D NonUniformScaleAboutPoint(
        Point3D point,
        double scaleX,
        double scaleY,
        double scaleZ
    )
    {
        SUPoint3D pointSU = point.ToSU();
        SUTransformation t;
        SUTransformationNonUniformScaleAboutPoint(&t, &pointSU, scaleX, scaleY, scaleZ)
            .CheckError();
        return new(t);
    }

    public static unsafe Transform3D Translate(Vector3D vector)
    {
        SUVector3D vectorSU = vector.ToSU();
        SUTransformation t;
        SUTransformationTranslation(&t, &vectorSU).CheckError();
        return new(t);
    }

    public static unsafe Transform3D Rotation(Point3D point, Vector3D vector, double angle)
    {
        SUPoint3D pointSU = point.ToSU();
        SUVector3D vectorSU = vector.ToSU();
        SUTransformation t;
        SUTransformationRotation(&t, &pointSU, &vectorSU, angle).CheckError();
        return new(t);
    }

    public unsafe Transform3D Multiply(Transform3D other)
    {
        SUTransformation t;
        fixed (SUTransformation* t1 = &_su)
        fixed (SUTransformation* t2 = &other._su)
            SUTransformationMultiply(t1, t2, &t).CheckError();
        return new(t);
    }

    public static Transform3D operator *(Transform3D left, Transform3D right) =>
        left.Multiply(right);

    public unsafe Transform3D Interpolate(Transform3D other, double weight)
    {
        SUTransformation t;
        fixed (SUTransformation* t1 = &_su)
        fixed (SUTransformation* t2 = &other._su)
            SUTransformationInterpolate(&t, t1, t2, weight).CheckError();
        return new(t);
    }

    public unsafe SUTransformation Inverse()
    {
        SUTransformation inverse;
        fixed (SUTransformation* su = &_su)
            SUTransformationGetInverse(su, &inverse).CheckError();
        return inverse;
    }

    public unsafe bool IsIdentity
    {
        get
        {
            bool value;
            fixed (SUTransformation* su = &_su)
                SUTransformationIsIdentity(su, &value).CheckError();
            return value;
        }
    }

    public unsafe bool IsMirrored
    {
        get
        {
            bool value;
            fixed (SUTransformation* su = &_su)
                SUTransformationIsMirrored(su, &value).CheckError();
            return value;
        }
    }

    public unsafe Point3D Origin
    {
        get
        {
            SUPoint3D origin;
            fixed (SUTransformation* su = &_su)
                SUTransformationGetOrigin(su, &origin).CheckError();
            return new(origin);
        }
    }

    public unsafe Vector3D XAxis
    {
        get
        {
            SUVector3D xAxis;
            fixed (SUTransformation* su = &_su)
                SUTransformationGetXAxis(su, &xAxis).CheckError();
            return new(xAxis);
        }
    }

    public unsafe Vector3D YAxis
    {
        get
        {
            SUVector3D yAxis;
            fixed (SUTransformation* su = &_su)
                SUTransformationGetYAxis(su, &yAxis).CheckError();
            return new(yAxis);
        }
    }

    public unsafe Vector3D ZAxis
    {
        get
        {
            SUVector3D zAxis;
            fixed (SUTransformation* su = &_su)
                SUTransformationGetZAxis(su, &zAxis).CheckError();
            return new(zAxis);
        }
    }

    public unsafe double ZRotation
    {
        get
        {
            double value;
            fixed (SUTransformation* su = &_su)
                SUTransformationGetZRotation(su, &value).CheckError();
            return value;
        }
    }

    public unsafe double[] GetValues()
    {
        fixed (SUTransformation* su = &_su)
            return Marshal.PtrToStructure<TransformData>((nint)su).values;
    }

    private readonly SUTransformation _su;

    internal Transform3D(SUTransformation su)
    {
        _su = su;
    }

    internal SUTransformation ToSU() => _su;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct TransformData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public double[] values;
    }
}
