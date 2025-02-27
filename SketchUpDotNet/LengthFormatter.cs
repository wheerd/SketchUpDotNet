using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class LengthFormatter : SUBase<SULengthFormatterRef>
{
    public unsafe LengthFormatter()
        : base(&SULengthFormatterCreate) { }

    internal LengthFormatter(SULengthFormatterRef @ref)
        : base(@ref) { }

    public SULengthUnitType Units
    {
        get => GetUnits();
        set => SetUnits(value);
    }

    private unsafe SULengthUnitType GetUnits() => Get<SULengthUnitType>(&SULengthFormatterGetUnits);

    private unsafe void SetUnits(SULengthUnitType units) => Set(&SULengthFormatterSetUnits, units);

    protected sealed override unsafe delegate* <SULengthFormatterRef*, SUResult> Release =>
        &SULengthFormatterRelease;
}
