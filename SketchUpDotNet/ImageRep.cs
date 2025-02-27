using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class ImageRep : SUBase<SUImageRepRef>
{
    public int Width => GetDimensions().Item1;
    public int Height => GetDimensions().Item2;

    public unsafe int RowPadding => GetInt(&SUImageRepGetRowPadding);

    public unsafe void SaveToFile(string path)
    {
        var bytes = path.GetSBytes();
        fixed (sbyte* ptr = &bytes[0])
            SUImageRepSaveToFile(Reference, ptr).CheckError();
    }

    public unsafe void LoadFile(string path)
    {
        var bytes = path.GetSBytes();
        fixed (sbyte* ptr = &bytes[0])
            SUImageRepLoadFile(Reference, ptr).CheckError();
    }

    public unsafe ImageRep Clone()
    {
        var clone = new ImageRep();
        SUImageRepCopy(Reference, clone.Reference).CheckError();
        return clone;
    }

    public unsafe void Resize(int width, int height) =>
        SUImageRepResize(Reference, (nuint)width, (nuint)height).CheckError();

    public unsafe void ConvertTo32BitsPerPixel() =>
        SUImageRepConvertTo32BitsPerPixel(Reference).CheckError();

    internal unsafe ImageRep()
        : base(&SUImageRepCreate) { }

    internal unsafe ImageRep(SUImageRepRef @ref)
        : base(@ref) { }

    private unsafe (int, int) GetDimensions()
    {
        nuint width,
            height;
        SUImageRepGetPixelDimensions(Reference, &width, &height).CheckError();
        return ((int)width, (int)height);
    }

    protected sealed override unsafe delegate* <SUImageRepRef*, SUResult> Release =>
        &SUImageRepRelease;
}
