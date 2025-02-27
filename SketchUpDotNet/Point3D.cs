using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public readonly record struct Point3D(int X, int Y, int Z)
{
    public unsafe Point3D Transform(SUTransformation t)
    {
        SUPoint3D su = ToSU();
        SUPoint3DTransform(&t, &su).CheckError();
        return FromSU(su);
    }

    internal SUPoint3D ToSU() =>
        new()
        {
            x = X * SketchUp.MM_TO_INCH,
            y = Y * SketchUp.MM_TO_INCH,
            z = Z * SketchUp.MM_TO_INCH,
        };

    internal static Point3D FromSU(SUPoint3D point) =>
        new(
            (int)Math.Round(point.x / SketchUp.MM_TO_INCH),
            (int)Math.Round(point.y / SketchUp.MM_TO_INCH),
            (int)Math.Round(point.z / SketchUp.MM_TO_INCH)
        );

    internal static Point3D FromVector(double[] values) =>
        new(
            (int)Math.Round(values[0] / SketchUp.MM_TO_INCH),
            (int)Math.Round(values[1] / SketchUp.MM_TO_INCH),
            (int)Math.Round(values[2] / SketchUp.MM_TO_INCH)
        );
}
