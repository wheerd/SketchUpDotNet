using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Group : DrawingElement<SUGroupRef>
{
    public unsafe Group()
        : base(&SUGroupCreate)
    {
        Entities = GetEntities();
    }

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

    public string Guid => GetGuid();

    public Component Definition => GetDefinition();

    public Entities Entities { get; private init; }

    public static implicit operator ComponentInstance(Group group) =>
        new(SUGroupToComponentInstance(group.Reference), group.attached);

    internal Group(SUGroupRef @ref)
        : base(@ref)
    {
        Entities = GetEntities();
    }

    private unsafe string GetName() => GetString(&SUGroupGetName);

    private unsafe void SetName(string name) => SetString(&SUGroupSetName, name);

    private unsafe string GetGuid() => GetString(&SUGroupGetGuid);

    private unsafe Entities GetEntities() =>
        GetOne(&SUGroupGetEntities, (SUEntitiesRef e) => Entities.CreateOrGet(e, attached));

    internal override void SetAttachedToModel(bool attached)
    {
        base.SetAttachedToModel(attached);
        Entities.SetAttachedToModel(attached);
    }

    private unsafe Component GetDefinition() =>
        GetOne(&SUGroupGetDefinition, (SUComponentDefinitionRef e) => new Component(e));

    private unsafe Transform GetTransform() => new(Get<SUTransformation>(&SUGroupGetTransform));

    private unsafe void SetTransform(Transform transform) =>
        Set(&SUGroupSetTransform, transform.ToSU());

    protected sealed override unsafe delegate* <SUGroupRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUGroupRef, SUEntityRef> ToEntity =>
        &SUGroupToEntity;

    protected sealed override unsafe delegate* <SUGroupRef, SUDrawingElementRef> ToDrawingElement =>
        &SUGroupToDrawingElement;
}
