using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class SectionPlane : DrawingElement<SUSectionPlaneRef>
{
    public unsafe SectionPlane()
        : base(&SUSectionPlaneCreate) { }

    public unsafe Plane3D Plane
    {
        get => new(Get<SUPlane3D>(&SUSectionPlaneGetPlane));
        set => Set(&SUSectionPlaneSetPlane, value.ToSU());
    }

    public unsafe string Name
    {
        get => GetString(&SUSectionPlaneGetName);
        set => SetString(&SUSectionPlaneSetName, value);
    }

    public unsafe string Symbol
    {
        get => GetString(&SUSectionPlaneGetSymbol);
        set => SetString(&SUSectionPlaneSetSymbol, value);
    }

    public unsafe bool IsActive => GetBool(&SUSectionPlaneIsActive);

    internal unsafe SectionPlane(SUSectionPlaneRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUSectionPlaneRef*, SUResult> Release =>
        &SUSectionPlaneRelease;

    protected sealed override unsafe delegate* <SUSectionPlaneRef, SUEntityRef> ToEntity =>
        &SUSectionPlaneToEntity;

    protected sealed override unsafe delegate* <
        SUSectionPlaneRef,
        SUDrawingElementRef> ToDrawingElement => &SUSectionPlaneToDrawingElement;
}
