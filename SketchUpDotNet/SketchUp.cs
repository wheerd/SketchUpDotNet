using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public static class SketchUp
{
    public const double MM_TO_INCH = 1 / 25.4;

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
