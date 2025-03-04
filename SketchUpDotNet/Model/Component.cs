using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Component : DrawingElement<SUComponentDefinitionRef>, IEntitiesParent
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

    public unsafe string Name
    {
        get => GetString(&SUComponentDefinitionGetName);
        set => SetString(&SUComponentDefinitionSetName, value);
    }

    public unsafe string Guid => GetString(&SUComponentDefinitionGetGuid);

    public unsafe string Description
    {
        get => GetString(&SUComponentDefinitionGetDescription);
        set => SetString(&SUComponentDefinitionSetDescription, value);
    }

    public Entities Entities { get; private init; }

    public unsafe IEnumerable<ComponentInstance> Instances =>
        GetMany(
            &SUComponentDefinitionGetNumInstances,
            &SUComponentDefinitionGetInstances,
            (SUComponentInstanceRef e) => new ComponentInstance(e, attached)
        );
    public unsafe int InstanceCount => GetInt(&SUComponentDefinitionGetNumInstances);
    public unsafe int UsedInstanceCount => GetInt(&SUComponentDefinitionGetNumUsedInstances);

    public unsafe SUComponentBehavior Behavior
    {
        get => Get<SUComponentBehavior>(&SUComponentDefinitionGetBehavior);
        set => Set(&SUComponentDefinitionSetBehavior, value);
    }

    public unsafe bool IsInternal => GetBool(&SUComponentDefinitionIsInternal);

    public unsafe bool IsManifold => GetBool(&SUComponentDefinitionIsManifold);

    public unsafe bool IsLiveComponent => GetBool(&SUComponentDefinitionIsLiveComponent);

    public unsafe string Path => GetString(&SUComponentDefinitionGetPath);

    public unsafe IEnumerable<Opening> Openings =>
        GetMany(
            &SUComponentDefinitionGetNumOpenings,
            &SUComponentDefinitionGetOpenings,
            (SUOpeningRef e) => new Opening(e)
        );
    public unsafe int OpeningCount => GetInt(&SUComponentDefinitionGetNumOpenings);

    public unsafe Point3D InsertPoint => new(Get<SUPoint3D>(&SUComponentDefinitionGetInsertPoint));

    public unsafe void OrientFacesConsistently() =>
        SUComponentDefinitionOrientFacesConsistently(Reference).CheckError();

    public unsafe void SaveToFile(string path, SUModelVersion version)
    {
        var bytes = path.GetSBytes();
        fixed (sbyte* bytesPtr = &bytes[0])
            SUComponentDefinitionSaveToFile(Reference, bytesPtr, version).CheckError();
    }

    public unsafe SUComponentType Type => Get<SUComponentType>(&SUComponentDefinitionGetType);

    internal Component(SUComponentDefinitionRef @ref)
        : base(@ref)
    {
        Entities = GetEntities();
    }

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

    protected sealed override unsafe delegate* <SUComponentDefinitionRef*, SUResult> Release =>
        &SUComponentDefinitionRelease;

    protected sealed override unsafe delegate* <SUComponentDefinitionRef, SUEntityRef> ToEntity =>
        &SUComponentDefinitionToEntity;

    protected sealed override unsafe delegate* <
        SUComponentDefinitionRef,
        SUDrawingElementRef> ToDrawingElement => &SUComponentDefinitionToDrawingElement;
}
