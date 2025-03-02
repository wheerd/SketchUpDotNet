using System.Diagnostics;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public readonly record struct Point3D(double X, double Y, double Z)
{
    internal Point3D(SUPoint3D su)
        : this(su.x.FromSULength(), su.y.FromSULength(), su.z.FromSULength()) { }

    public unsafe Point3D Transform(SUTransformation t)
    {
        SUPoint3D su = ToSU();
        SUPoint3DTransform(&t, &su).CheckError();
        return new(su);
    }

    internal SUPoint3D ToSU() =>
        new()
        {
            x = X.ToSULength(),
            y = Y.ToSULength(),
            z = Z.ToSULength(),
        };

    internal Point3D(double[] values)
        : this(values[0].FromSULength(), values[1].FromSULength(), values[2].FromSULength())
    {
        Debug.Assert(values.Length == 3);
    }
}
