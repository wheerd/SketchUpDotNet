using System.Numerics;
using System.Runtime.InteropServices;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public static class Transform
{
    public static unsafe SUTransformation Scale(double scale)
    {
        SUTransformation t;
        SUTransformationScale(&t, scale);
        return t;
    }

    public static unsafe SUTransformation Translate(Vector3D vector)
    {
        SUVector3D vectorSU = vector.ToSU();
        SUTransformation t;
        SUTransformationTranslation(&t, &vectorSU);
        return t;
    }

    public static unsafe SUTransformation Rotation(Point3D point, Vector3D vector, double angle)
    {
        SUPoint3D pointSU = point.ToSU();
        SUVector3D vectorSU = vector.ToSU();
        SUTransformation t;
        SUTransformationRotation(&t, &pointSU, &vectorSU, angle);
        return t;
    }

    public static unsafe SUTransformation Multiply(this SUTransformation t1, SUTransformation t2)
    {
        SUTransformation t;
        SUTransformationMultiply(&t1, &t2, &t);
        return t;
    }

    public static unsafe SUTransformation Inverse(this SUTransformation t)
    {
        SUTransformation inverse;
        SUTransformationGetInverse(&t, &inverse);
        return inverse;
    }

    public static unsafe double[] GetValues(this SUTransformation t) =>
        Marshal.PtrToStructure<TransformData>((nint)(&t)).values;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct TransformData
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public double[] values;
    }
}
