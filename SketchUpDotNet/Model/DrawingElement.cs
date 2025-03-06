using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public abstract class DrawingElement<T> : Entity<T>, IDrawingElement
    where T : unmanaged
{
    public Layer Layer
    {
        get => GetLayer();
        set => SetLayer(value);
    }

    public Material? Material
    {
        get => GetMaterial();
        set => SetMaterial(value);
    }

    public bool Hidden
    {
        get => GetHidden();
        set => SetHidden(value);
    }

    public bool CastsShadows
    {
        get => GetCastsShadows();
        set => SetCastsShadows(value);
    }

    public bool ReceivesShadows
    {
        get => GetReceivesShadows();
        set => SetReceivesShadows(value);
    }

    public BoundingBox3D BoundingBox => GetBoundingBox();

    internal DrawingElement(T @ref)
        : base(@ref) { }

    internal DrawingElement(T @ref, bool attached)
        : base(@ref, attached) { }

    internal unsafe DrawingElement(delegate* <T*, SUResult> factory)
        : base(factory) { }

    private SUDrawingElementRef? elementRef;

    protected abstract unsafe delegate* <T, SUDrawingElementRef> ToDrawingElement { get; }

    private unsafe SUDrawingElementRef ElementRef => elementRef ??= ToDrawingElement(Reference);

    SUDrawingElementRef IDrawingElement.ElementRef => ElementRef;

    private unsafe bool GetHidden() => ElementRef.GetBool(&SUDrawingElementGetHidden);

    private unsafe void SetHidden(bool hidden) =>
        ElementRef.SetBool(&SUDrawingElementSetHidden, hidden);

    private unsafe bool GetReceivesShadows() =>
        ElementRef.GetBool(&SUDrawingElementGetReceivesShadows);

    private unsafe void SetReceivesShadows(bool value) =>
        ElementRef.SetBool(&SUDrawingElementSetReceivesShadows, value);

    private unsafe bool GetCastsShadows() => ElementRef.GetBool(&SUDrawingElementGetCastsShadows);

    private unsafe void SetCastsShadows(bool value) =>
        ElementRef.SetBool(&SUDrawingElementSetCastsShadows, value);

    private unsafe Material? GetMaterial() =>
        ElementRef.GetOptionalOne(
            &SUDrawingElementGetMaterial,
            (SUMaterialRef m) => new Material(m),
            attached
        );

    private unsafe void SetMaterial(Material? material) =>
        SUDrawingElementSetMaterial(ElementRef, material?.Reference ?? default).CheckError();

    private unsafe BoundingBox3D GetBoundingBox() =>
        new(ElementRef.Get<SUDrawingElementRef, SUBoundingBox3D>(&SUDrawingElementGetBoundingBox));

    private unsafe Layer GetLayer() =>
        ElementRef.GetOptionalOne(
            &SUDrawingElementGetLayer,
            (SULayerRef l) => new Layer(l),
            attached
        )
        ?? throw new NotSupportedException(
            "Drawing Element does not have a layer because it is not added to a model."
        );

    private unsafe void SetLayer(Layer layer) =>
        SUDrawingElementSetLayer(ElementRef, layer.Reference).CheckError();
}

public interface IDrawingElement : IEntity
{
    public Layer Layer { get; set; }

    public Material? Material { get; set; }

    public bool Hidden { get; set; }

    public bool CastsShadows { get; set; }

    public bool ReceivesShadows { get; set; }

    public BoundingBox3D BoundingBox { get; }

    internal static IDrawingElement Create(SUDrawingElementRef reference)
    {
        return SUDrawingElementGetType(reference) switch
        {
            SURefType.SURefType_Axes => new Axes(SUAxesFromDrawingElement(reference)),
            SURefType.SURefType_ComponentInstance => new ComponentInstance(
                SUComponentInstanceFromDrawingElement(reference),
                false
            ),
            SURefType.SURefType_ComponentDefinition => new Component(
                SUComponentDefinitionFromDrawingElement(reference)
            ),
            SURefType.SURefType_Dimension
            or SURefType.SURefType_DimensionLinear
            or SURefType.SURefType_DimensionRadial => IDimension.Create(
                SUDimensionFromDrawingElement(reference)
            ),
            SURefType.SURefType_Edge => new Edge(SUEdgeFromDrawingElement(reference)),
            SURefType.SURefType_Face => new Face(SUFaceFromDrawingElement(reference)),
            SURefType.SURefType_Group => new Group(SUGroupFromDrawingElement(reference)),
            SURefType.SURefType_GuidePoint => new GuidePoint(
                SUGuidePointFromDrawingElement(reference)
            ),
            SURefType.SURefType_GuideLine => new GuideLine(
                SUGuideLineFromDrawingElement(reference)
            ),
            SURefType.SURefType_Polyline3D => new Polyline3D(
                SUPolyline3dFromDrawingElement(reference)
            ),
            SURefType.SURefType_SectionPlane => new SectionPlane(
                SUSectionPlaneFromDrawingElement(reference)
            ),
            _ => throw new NotImplementedException(),
        };
    }

    internal unsafe SUDrawingElementRef ElementRef { get; }
}
