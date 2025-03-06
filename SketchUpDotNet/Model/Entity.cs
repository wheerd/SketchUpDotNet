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

    internal static IEntity Create(SUEntityRef reference)
    {
        return SUEntityGetType(reference) switch
        {
            SURefType.SURefType_Axes => new Axes(SUAxesFromEntity(reference)),
            SURefType.SURefType_ArcCurve => new ArcCurve(SUArcCurveFromEntity(reference)),
            SURefType.SURefType_AttributeDictionary => new AttributeDictionary(
                SUAttributeDictionaryFromEntity(reference)
            ),
            SURefType.SURefType_ComponentInstance => new ComponentInstance(
                SUComponentInstanceFromEntity(reference),
                false
            ),
            SURefType.SURefType_ComponentDefinition => new Component(
                SUComponentDefinitionFromEntity(reference)
            ),
            SURefType.SURefType_Curve => ICurve.Create(SUCurveFromEntity(reference)),
            SURefType.SURefType_Dimension
            or SURefType.SURefType_DimensionLinear
            or SURefType.SURefType_DimensionRadial => IDimension.Create(
                SUDimensionFromEntity(reference)
            ),
            SURefType.SURefType_Edge => new Edge(SUEdgeFromEntity(reference)),
            SURefType.SURefType_EdgeUse => new EdgeUse(SUEdgeUseFromEntity(reference)),
            SURefType.SURefType_Face => new Face(SUFaceFromEntity(reference)),
            SURefType.SURefType_Font => new Font(SUFontFromEntity(reference)),
            SURefType.SURefType_Group => new Group(SUGroupFromEntity(reference)),
            SURefType.SURefType_GuidePoint => new GuidePoint(SUGuidePointFromEntity(reference)),
            SURefType.SURefType_GuideLine => new GuideLine(SUGuideLineFromEntity(reference)),
            SURefType.SURefType_Image => new Image(SUImageFromEntity(reference)),
            SURefType.SURefType_Layer => new Layer(SULayerFromEntity(reference)),
            SURefType.SURefType_Loop => new Loop(SULoopFromEntity(reference)),
            SURefType.SURefType_Material => new Material(SUMaterialFromEntity(reference)),
            SURefType.SURefType_Polyline3D => new Polyline3D(SUPolyline3dFromEntity(reference)),
            SURefType.SURefType_SectionPlane => new SectionPlane(
                SUSectionPlaneFromEntity(reference)
            ),
            SURefType.SURefType_Text => new Text(SUTextFromEntity(reference)),
            SURefType.SURefType_Texture => new Texture(SUTextureFromEntity(reference)),
            SURefType.SURefType_Vertex => new Vertex(SUVertexFromEntity(reference)),
            _ => throw new NotImplementedException(),
        };
    }

    internal unsafe SUEntityRef EntityRef { get; }
}
