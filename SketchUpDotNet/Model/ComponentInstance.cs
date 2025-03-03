using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class ComponentInstance : DrawingElement<SUComponentInstanceRef>
{
    public string Name
    {
        get => GetName();
        set => SetName(value);
    }

    public Transform Transform
    {
        get => GetTransform();
        set => SetTransform(value);
    }

    public Component Definition => GetDefinition();

    private unsafe string GetName() => GetString(&SUComponentInstanceGetName);

    private unsafe void SetName(string name) => SetString(&SUComponentInstanceSetName, name);

    private unsafe Component GetDefinition() =>
        GetOne<SUComponentDefinitionRef, Component>(&SUComponentInstanceGetDefinition, d => new(d));

    private unsafe Transform GetTransform() =>
        new(Get<SUTransformation>(&SUComponentInstanceGetTransform));

    private unsafe void SetTransform(Transform transform) =>
        Set(&SUComponentInstanceSetTransform, transform.ToSU());

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
