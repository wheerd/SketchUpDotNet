using System.Drawing;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Material : Entity<SUMaterialRef>
{
    public unsafe Material()
        : base(&SUMaterialCreate) { }

    public string Name
    {
        get => GetName();
        set => SetName(value);
    }

    public Color Color
    {
        get => GetColor();
        set => SetColor(value);
    }

    public double Opacity
    {
        get => GetOpacity();
        set => SetOpacity(value);
    }

    public bool UseOpacity
    {
        get => GetUseOpacity();
        set => SetUseOpacity(value);
    }

    public SUMaterialType Type
    {
        get => GetMaterialType();
        set => SetMaterialType(value);
    }

    public SUMaterialColorizeType ColorizeType
    {
        get => GetMaterialColorizeType();
        set => SetMaterialColorizeType(value);
    }

    public Texture? Texture
    {
        get => GetTexture();
        set => SetTexture(value);
    }

    private unsafe SUMaterialColorizeType GetMaterialColorizeType() =>
        Get<SUMaterialColorizeType>(&SUMaterialGetColorizeType);

    private unsafe void SetMaterialColorizeType(SUMaterialColorizeType type) =>
        Set(&SUMaterialSetColorizeType, type);

    private unsafe SUMaterialType GetMaterialType() => Get<SUMaterialType>(&SUMaterialGetType);

    private unsafe void SetMaterialType(SUMaterialType type) => Set(&SUMaterialSetType, type);

    private unsafe double GetOpacity() => GetDouble(&SUMaterialGetOpacity);

    private unsafe void SetOpacity(double value) => SetDouble(&SUMaterialSetOpacity, value);

    private unsafe bool GetUseOpacity() => GetBool(&SUMaterialGetUseOpacity);

    private unsafe void SetUseOpacity(bool value) => SetBool(&SUMaterialSetUseOpacity, value);

    private unsafe string GetName() => GetString(&SUMaterialGetName);

    private unsafe void SetName(string name) => SetString(&SUMaterialSetName, name);

    private unsafe Color GetColor() => GetColor(&SUMaterialGetColor);

    private unsafe void SetColor(Color color) => SetColor(&SUMaterialSetColor, color);

    private unsafe Texture? GetTexture() =>
        GetOptionalOne(&SUMaterialGetTexture, (SUTextureRef t) => new Texture(t));

    private unsafe void SetTexture(Texture? texture) =>
        SetOptionalOne<SUTextureRef, Texture>(&SUMaterialSetTexture, texture);

    internal unsafe Material(SUMaterialRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUMaterialRef*, SUResult> Release =>
        &SUMaterialRelease;

    protected sealed override unsafe delegate* <SUMaterialRef, SUEntityRef> ToEntity =>
        &SUMaterialToEntity;
}
