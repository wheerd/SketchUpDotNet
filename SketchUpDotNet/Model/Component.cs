using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Component : DrawingElement<SUComponentDefinitionRef>
{
    public unsafe Component()
        : base(&SUComponentDefinitionCreate)
    {
        Entities = GetEntities();
    }

    public unsafe ComponentInstance Instantiate() =>
        GetOne<SUComponentInstanceRef, ComponentInstance>(
            &SUComponentDefinitionCreateInstance,
            d => new(d, attached)
        );

    public string Name
    {
        get => GetName();
        set => SetName(value);
    }

    public string Guid => GetGuid();

    public string Description
    {
        get => GetDescription();
        set => SetDescription(value);
    }

    public Entities Entities { get; private init; }

    public IEnumerable<ComponentInstance> Instances => GetInstances();

    internal Component(SUComponentDefinitionRef @ref)
        : base(@ref)
    {
        Entities = GetEntities();
    }

    private unsafe string GetName() => GetString(&SUComponentDefinitionGetName);

    private unsafe void SetName(string name) => SetString(&SUComponentDefinitionSetName, name);

    private unsafe string GetGuid() => GetString(&SUComponentDefinitionGetGuid);

    private unsafe string GetDescription() => GetString(&SUComponentDefinitionGetDescription);

    private unsafe void SetDescription(string description) =>
        SetString(&SUComponentDefinitionSetDescription, description);

    private unsafe Entities GetEntities() =>
        GetOne<SUEntitiesRef, Entities>(
            &SUComponentDefinitionGetEntities,
            d => Entities.CreateOrGet(d, attached)
        );

    internal override void SetAttachedToModel(bool attached)
    {
        base.SetAttachedToModel(attached);
        Entities.SetAttachedToModel(attached);
    }

    private unsafe ComponentInstance[] GetInstances() =>
        GetMany(
            &SUComponentDefinitionGetNumInstances,
            &SUComponentDefinitionGetInstances,
            (SUComponentInstanceRef e) => new ComponentInstance(e, attached)
        );

    protected sealed override unsafe delegate* <SUComponentDefinitionRef*, SUResult> Release =>
        &SUComponentDefinitionRelease;

    protected sealed override unsafe delegate* <SUComponentDefinitionRef, SUEntityRef> ToEntity =>
        &SUComponentDefinitionToEntity;

    protected sealed override unsafe delegate* <
        SUComponentDefinitionRef,
        SUDrawingElementRef> ToDrawingElement => &SUComponentDefinitionToDrawingElement;
}
