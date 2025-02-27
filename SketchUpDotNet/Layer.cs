using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Layer : Entity<SULayerRef>
{
    public unsafe Layer()
        : base(&SULayerCreate) { }

    internal Layer(SULayerRef @ref)
        : base(@ref) { }

    public string Name
    {
        get => GetName();
        set => SetName(value);
    }

    public bool Visible
    {
        get => GetVisible();
        set => SetVisible(value);
    }

    private unsafe string GetName() => GetString(&SULayerGetName);

    private unsafe void SetName(string name) => SetString(&SULayerSetName, name);

    private unsafe bool GetVisible() => GetBool(&SULayerGetVisibility);

    private unsafe void SetVisible(bool visible) => SetBool(&SULayerSetVisibility, visible);

    protected sealed override unsafe delegate* <SULayerRef*, SUResult> Release => &SULayerRelease;

    protected sealed override unsafe delegate* <SULayerRef, SUEntityRef> ToEntity =>
        &SULayerToEntity;
}
