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

    public unsafe string Guid
    {
        get => GetString(&SUComponentInstanceGetGuid);
        set => SetString(&SUComponentInstanceSetGuid, value);
    }

    public unsafe Transform3D Transform
    {
        get => new(Get<SUTransformation>(&SUComponentInstanceGetTransform));
        set => Set(&SUComponentInstanceSetTransform, value.ToSU());
    }

    public unsafe Component Definition =>
        GetOne<SUComponentDefinitionRef, Component>(&SUComponentInstanceGetDefinition, d => new(d));

    public unsafe bool IsLocked
    {
        get => GetBool(&SUComponentInstanceIsLocked);
        set => SetBool(&SUComponentInstanceSetLocked, value);
    }

    public unsafe double ComputeVolume(Transform3D transform)
    {
        double volume;
        var t = transform.ToSU();
        SUComponentInstanceComputeVolume(Reference.EnsureReferenceValid(), &t, &volume)
            .CheckError();
        return volume.FromSUVolume();
    }

    public unsafe IEnumerable<ComponentInstance> AttachedInstances =>
        GetMany(
            &SUComponentInstanceGetNumAttachedInstances,
            &SUComponentInstanceGetAttachedInstances,
            (SUComponentInstanceRef i) => new ComponentInstance(i, attached)
        );
    public unsafe int AttachedInstanceCount => GetInt(&SUComponentInstanceGetNumAttachedInstances);

    public unsafe IEnumerable<IDrawingElement> AttachedTo =>
        GetMany(
            &SUComponentInstanceGetNumAttachedToDrawingElements,
            &SUComponentInstanceGetAttachedToDrawingElements,
            (SUDrawingElementRef d) => IDrawingElement.Create(d)
        );
    public unsafe int AttachedToCount =>
        GetInt(&SUComponentInstanceGetNumAttachedToDrawingElements);

    public unsafe ClassificationInfo? ClassificationInfo =>
        GetOptionalOne(
            &SUComponentInstanceCreateClassificationInfo,
            (SUClassificationInfoRef c) => new ClassificationInfo(c)
        );

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
