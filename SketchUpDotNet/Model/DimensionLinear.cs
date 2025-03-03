using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class DimensionLinear : Dimension<SUDimensionLinearRef>
{
    protected override unsafe delegate* <SUDimensionLinearRef, SUDimensionRef> ToDimension =>
        &SUDimensionLinearToDimension;

    protected override unsafe delegate* <SUDimensionLinearRef*, SUResult> Release =>
        &SUDimensionLinearRelease;

    internal DimensionLinear(SUDimensionLinearRef @ref)
        : base(@ref) { }

    private static unsafe SUEntityRef ToEntityRef(SUDimensionLinearRef @ref) =>
        SUDimensionToEntity(SUDimensionLinearToDimension(@ref));

    private static unsafe SUDrawingElementRef ToDrawingElementRef(SUDimensionLinearRef @ref) =>
        SUDimensionToDrawingElement(SUDimensionLinearToDimension(@ref));

    protected override unsafe delegate* <SUDimensionLinearRef, SUEntityRef> ToEntity =>
        &ToEntityRef;

    protected override unsafe delegate* <
        SUDimensionLinearRef,
        SUDrawingElementRef> ToDrawingElement => &ToDrawingElementRef;
}
