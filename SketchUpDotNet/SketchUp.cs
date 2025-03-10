using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public static class SketchUp
{
    public static (int minor, int major) ApiVersion => GetApiVersion();

    private static unsafe (int minor, int major) GetApiVersion()
    {
        nuint minor,
            major;
        SUGetAPIVersion(&minor, &major);
        return (minor: (int)minor, major: (int)major);
    }

    public static LengthUnit LengthUnit { get; set; } = LengthUnit.Meter;

    public static double FromSULength(this double value) =>
        value
        * LengthUnit switch
        {
            LengthUnit.Inch => 1.0,
            LengthUnit.Feet => 1 / 12.0,
            LengthUnit.Yard => 1 / 36.0,
            LengthUnit.Millimeter => 25.4,
            LengthUnit.Centimeter => 2.54,
            LengthUnit.Meter => 0.0254,
            _ => throw new NotImplementedException(),
        };

    public static double FromSUVolume(this double value) =>
        value
        * LengthUnit switch
        {
            LengthUnit.Inch => 1.0,
            LengthUnit.Feet => 1 / (12.0 * 12.0 * 12.0),
            LengthUnit.Yard => 1 / (36.0 * 36.0 * 36.0),
            LengthUnit.Millimeter => 25.4 * 25.4 * 25.4,
            LengthUnit.Centimeter => 2.54 * 2.54 * 2.54,
            LengthUnit.Meter => 0.0254 * 0.0254 * 0.0254,
            _ => throw new NotImplementedException(),
        };

    public static double ToSULength(this double value) =>
        value
        * LengthUnit switch
        {
            LengthUnit.Inch => 1.0,
            LengthUnit.Feet => 12.0,
            LengthUnit.Yard => 36.0,
            LengthUnit.Millimeter => 1 / 25.4,
            LengthUnit.Centimeter => 10 / 25.4,
            LengthUnit.Meter => 1000 / 25.4,
            _ => throw new NotImplementedException(),
        };

    private static readonly Destructor destructor = new();

    static SketchUp()
    {
        SUInitialize();
    }

    sealed class Destructor
    {
        ~Destructor()
        {
            SUTerminate();
        }
    }
}

public enum LengthUnit
{
    Inch,
    Feet,
    Yard,
    Millimeter,
    Centimeter,
    Meter,
}
