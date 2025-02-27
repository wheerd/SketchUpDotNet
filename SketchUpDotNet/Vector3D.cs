using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public readonly record struct Vector3D(int X, int Y, int Z)
{
    public unsafe Vector3D Transform(SUTransformation t)
    {
        SUVector3D su = ToSU();
        SUVector3DTransform(&t, &su).CheckError();
        return FromSU(su);
    }

    internal SUVector3D ToSU() =>
        new()
        {
            x = X * SketchUp.MM_TO_INCH,
            y = Y * SketchUp.MM_TO_INCH,
            z = Z * SketchUp.MM_TO_INCH,
        };

    internal static Vector3D FromSU(SUVector3D vector) =>
        new(
            (int)Math.Round(vector.x / SketchUp.MM_TO_INCH),
            (int)Math.Round(vector.y / SketchUp.MM_TO_INCH),
            (int)Math.Round(vector.z / SketchUp.MM_TO_INCH)
        );

    internal static Vector3D FromVector(double[] values) =>
        new(
            (int)Math.Round(values[0] / SketchUp.MM_TO_INCH),
            (int)Math.Round(values[1] / SketchUp.MM_TO_INCH),
            (int)Math.Round(values[2] / SketchUp.MM_TO_INCH)
        );
}
