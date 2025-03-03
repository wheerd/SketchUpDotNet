using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class DimensionRadial : Dimension<SUDimensionRadialRef>
{
    protected override unsafe delegate* <SUDimensionRadialRef, SUDimensionRef> ToDimension =>
        &SUDimensionRadialToDimension;

    protected override unsafe delegate* <SUDimensionRadialRef*, SUResult> Release =>
        &SUDimensionRadialRelease;

    internal DimensionRadial(SUDimensionRadialRef @ref)
        : base(@ref) { }

    private static unsafe SUEntityRef ToEntityRef(SUDimensionRadialRef @ref) =>
        SUDimensionToEntity(SUDimensionRadialToDimension(@ref));

    private static unsafe SUDrawingElementRef ToDrawingElementRef(SUDimensionRadialRef @ref) =>
        SUDimensionToDrawingElement(SUDimensionRadialToDimension(@ref));

    protected override unsafe delegate* <SUDimensionRadialRef, SUEntityRef> ToEntity =>
        &ToEntityRef;

    protected override unsafe delegate* <
        SUDimensionRadialRef,
        SUDrawingElementRef> ToDrawingElement => &ToDrawingElementRef;
}
