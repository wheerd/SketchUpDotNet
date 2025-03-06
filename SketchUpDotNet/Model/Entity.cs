using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public readonly record struct EntityId(int Value);

public abstract class Entity<T> : SUBase<T>, IEntity
    where T : unmanaged
{
    public unsafe EntityId Id => new(EntityRef.GetInt(&SUEntityGetID));

    public unsafe long PersistentId
    {
        get
        {
            long id;
            SUEntityGetPersistentID(EntityRef, &id).CheckError();
            return id;
        }
    }

    public unsafe SketchUpModel? Model =>
        EntityRef.GetOptionalOne(
            &SUEntityGetModel,
            (SUModelRef model) => SketchUpModel.CreateOrGet(model),
            false
        );

    public unsafe Entities? ParentEntities =>
        EntityRef.GetOptionalOne(
            &SUEntityGetParentEntities,
            (SUEntitiesRef model) => Entities.CreateOrGet(model, attached),
            false
        );

    public unsafe IEnumerable<AttributeDictionary> AttributeDictionaries =>
        EntityRef.GetMany(
            &SUEntityGetNumAttributeDictionaries,
            &SUEntityGetAttributeDictionaries,
            (SUAttributeDictionaryRef d) => new AttributeDictionary(d),
            attached
        );

    public unsafe AttributeDictionary GetAttributeDictionary(string name) =>
        EntityRef.GetOneByKey(
            &SUEntityGetAttributeDictionary,
            (SUAttributeDictionaryRef d) => new AttributeDictionary(d),
            attached,
            name
        );

    public unsafe void AddAttributeDictionary(AttributeDictionary dict)
    {
        SUEntityAddAttributeDictionary(EntityRef, dict.Reference).CheckError();
        if (attached)
            dict.SetAttachedToModel(true);
    }

    internal Entity(T @ref)
        : base(@ref) { }

    internal Entity(T @ref, bool attached)
        : base(@ref, attached) { }

    internal unsafe Entity(delegate* <T*, SUResult> factory)
        : base(factory) { }

    private SUEntityRef? entityRef;

    protected abstract unsafe delegate* <T, SUEntityRef> ToEntity { get; }

    private unsafe SUEntityRef EntityRef => entityRef ??= ToEntity(Reference);

    SUEntityRef IEntity.EntityRef => EntityRef;
}

public interface IEntity : IBase
{
    public EntityId Id { get; }

    public long PersistentId { get; }

    public SketchUpModel? Model { get; }
    public Entities? ParentEntities { get; }

    internal static IEntity Create(SUEntityRef entityRef)
    {
        return SUEntityGetType(entityRef) switch
        {
            SURefType.SURefType_Axes => new Axes(SUAxesFromEntity(entityRef)),
            SURefType.SURefType_ArcCurve => new ArcCurve(SUArcCurveFromEntity(entityRef)),
            SURefType.SURefType_AttributeDictionary => new AttributeDictionary(
                SUAttributeDictionaryFromEntity(entityRef)
            ),
            SURefType.SURefType_ComponentInstance => new ComponentInstance(
                SUComponentInstanceFromEntity(entityRef),
                false
            ),
            SURefType.SURefType_ComponentDefinition => new Component(
                SUComponentDefinitionFromEntity(entityRef)
            ),
            SURefType.SURefType_Curve => ICurve.Create(SUCurveFromEntity(entityRef)),
            SURefType.SURefType_Dimension
            or SURefType.SURefType_DimensionLinear
            or SURefType.SURefType_DimensionRadial => IDimension.Create(
                SUDimensionFromEntity(entityRef)
            ),
            SURefType.SURefType_Edge => new Edge(SUEdgeFromEntity(entityRef)),
            SURefType.SURefType_EdgeUse => new EdgeUse(SUEdgeUseFromEntity(entityRef)),
            SURefType.SURefType_Face => new Face(SUFaceFromEntity(entityRef)),
            SURefType.SURefType_Font => new Font(SUFontFromEntity(entityRef)),
            SURefType.SURefType_Group => new Group(SUGroupFromEntity(entityRef)),
            SURefType.SURefType_GuidePoint => new GuidePoint(SUGuidePointFromEntity(entityRef)),
            SURefType.SURefType_Layer => new Layer(SULayerFromEntity(entityRef)),
            SURefType.SURefType_Loop => new Loop(SULoopFromEntity(entityRef)),
            SURefType.SURefType_Material => new Material(SUMaterialFromEntity(entityRef)),
            SURefType.SURefType_Texture => new Texture(SUTextureFromEntity(entityRef)),
            SURefType.SURefType_Vertex => new Vertex(SUVertexFromEntity(entityRef)),
            _ => throw new NotImplementedException(),
        };
    }

    internal unsafe SUEntityRef EntityRef { get; }
}
