using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class ComponentInstance : DrawingElement<SUComponentInstanceRef>
{
    public unsafe string Name
    {
        get => GetString(&SUComponentInstanceGetName);
        set => SetString(&SUComponentInstanceSetName, value);
    }

    public unsafe Transform3D Transform
    {
        get => new(Get<SUTransformation>(&SUComponentInstanceGetTransform));
        set => Set(&SUComponentInstanceSetTransform, value.ToSU());
    }

    public unsafe Component Definition =>
        GetOne<SUComponentDefinitionRef, Component>(&SUComponentInstanceGetDefinition, d => new(d));

    internal unsafe ComponentInstance(SUComponentInstanceRef @ref, bool attached)
        : base(@ref)
    {
        this.attached = attached;
    }

    protected sealed override unsafe delegate* <SUComponentInstanceRef*, SUResult> Release =>
        &SUComponentInstanceRelease;

    protected sealed override unsafe delegate* <SUComponentInstanceRef, SUEntityRef> ToEntity =>
        &SUComponentInstanceToEntity;

    protected sealed override unsafe delegate* <
        SUComponentInstanceRef,
        SUDrawingElementRef> ToDrawingElement => &SUComponentInstanceToDrawingElement;
}
