using System.Diagnostics;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public readonly record struct Vector3D(double X, double Y, double Z)
{
    internal Vector3D(SUVector3D su)
        : this(su.x.FromSULength(), su.y.FromSULength(), su.z.FromSULength()) { }

    public unsafe Vector3D Transform(SUTransformation t)
    {
        SUVector3D su = ToSU();
        SUVector3DTransform(&t, &su).CheckError();
        return new(su);
    }

    internal SUVector3D ToSU() =>
        new()
        {
            x = X.ToSULength(),
            y = Y.ToSULength(),
            z = Z.ToSULength(),
        };

    internal Vector3D(double[] values)
        : this(values[0].FromSULength(), values[1].FromSULength(), values[2].FromSULength())
    {
        Debug.Assert(values.Length == 3);
    }
}
