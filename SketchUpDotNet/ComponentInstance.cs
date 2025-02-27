using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class ComponentInstance : DrawingElement<SUComponentInstanceRef>
{
    public string Name
    {
        get => GetName();
        set => SetName(value);
    }

    public SUTransformation Transform
    {
        get => GetTransform();
        set => SetTransform(value);
    }

    public Component Definition => GetDefinition();

    private unsafe string GetName() => GetString(&SUComponentInstanceGetName);

    private unsafe void SetName(string name) => SetString(&SUComponentInstanceSetName, name);

    private unsafe Component GetDefinition() =>
        GetOne<SUComponentDefinitionRef, Component>(&SUComponentInstanceGetDefinition, d => new(d));

    private unsafe SUTransformation GetTransform()
    {
        SUTransformation transformation;
        SUComponentInstanceGetTransform(Reference, &transformation).CheckError();
        return transformation;
    }

    private unsafe void SetTransform(SUTransformation transform)
    {
        SUComponentInstanceSetTransform(Reference, &transform).CheckError();
    }

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
