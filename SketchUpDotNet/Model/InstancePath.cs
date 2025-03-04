using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class InstancePath : SUBase<SUInstancePathRef>
{
    public unsafe InstancePath Clone()
    {
        SUInstancePathRef clone;
        SUInstancePathCreateCopy(&clone, Reference.EnsureReferenceValid()).CheckError();
        return new(clone);
    }

    public unsafe void PushInstance(ComponentInstance instance) =>
        SUInstancePathPushInstance(Reference.EnsureReferenceValid(), instance.Reference)
            .CheckError();

    public unsafe void PopInstance() =>
        SUInstancePathPopInstance(Reference.EnsureReferenceValid()).CheckError();

    public unsafe IEntity Leaf
    {
        get => GetOne(&SUInstancePathGetLeafAsEntity, (SUEntityRef leaf) => IEntity.Create(leaf));
        set
        {
            SUInstancePathSetLeaf(Reference.EnsureReferenceValid(), value.EntityRef).CheckError();
            if (attached)
            {
                value.SetAttachedToModel(true);
            }
        }
    }

    public unsafe int PathDepth => GetInt(&SUInstancePathGetPathDepth);
    public unsafe int FullDepth => GetInt(&SUInstancePathGetFullDepth);

    public unsafe Transform Transform => new(Get<SUTransformation>(&SUInstancePathGetTransform));

    public unsafe Transform GetTransformAtDepth(int depth)
    {
        SUTransformation transform;
        SUInstancePathGetTransformAtDepth(
                Reference.EnsureReferenceValid(),
                (nuint)depth,
                &transform
            )
            .CheckError();
        return new(transform);
    }

    public unsafe ComponentInstance GetInstanceAtDepth(int depth)
    {
        SUComponentInstanceRef instance;
        SUInstancePathGetInstanceAtDepth(Reference.EnsureReferenceValid(), (nuint)depth, &instance)
            .CheckError();
        return new(instance, attached);
    }

    public unsafe bool IsValid => GetBool(&SUInstancePathIsValid);
    public unsafe bool IsEmpty => GetBool(&SUInstancePathIsEmpty);

    public unsafe string PersistentId => GetString(&SUInstancePathGetPersistentID);

    public unsafe string GetPersistentIdAtDepth(int depth)
    {
        SUStringRef str;
        SUStringCreate(&str).CheckError();
        ;
        try
        {
            SUInstancePathGetPersistentIDAtDepth(
                    Reference.EnsureReferenceValid(),
                    (nuint)depth,
                    &str
                )
                .CheckError();
            return str.GetString();
        }
        finally
        {
            SUStringRelease(&str);
        }
    }

    public unsafe bool Contains(IEntity entity)
    {
        bool contains;
        SUInstancePathContains(Reference.EnsureReferenceValid(), entity.EntityRef, &contains)
            .CheckError();
        return contains;
    }

    internal unsafe InstancePath(SUInstancePathRef @ref)
        : base(@ref) { }

    internal unsafe InstancePath()
        : base(&SUInstancePathCreate) { }

    protected sealed override unsafe delegate* <SUInstancePathRef*, SUResult> Release =>
        &SUInstancePathRelease;
}
