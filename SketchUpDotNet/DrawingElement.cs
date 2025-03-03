using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

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

    public BoundingBox BoundingBox => GetBoundingBox();

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

    private unsafe BoundingBox GetBoundingBox() =>
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

public interface IDrawingElement
{
    public Layer Layer { get; set; }

    public Material? Material { get; set; }

    public bool Hidden { get; set; }

    public bool CastsShadows { get; set; }

    public bool ReceivesShadows { get; set; }

    public BoundingBox BoundingBox { get; }

    internal unsafe SUDrawingElementRef ElementRef { get; }
}
