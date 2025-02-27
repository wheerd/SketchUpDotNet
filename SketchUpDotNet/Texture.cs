using System.Drawing;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Texture : Entity<SUTextureRef>
{
    public static unsafe Texture FromImage(ImageRep rep)
    {
        SUTextureRef reference;
        SUTextureCreateFromImageRep(&reference, rep.Reference).CheckError();
        return new Texture(reference);
    }

    public static unsafe Texture FromFile(string path, double sScale, double tScale)
    {
        var bytes = path.GetSBytes();
        SUTextureRef reference;
        fixed (sbyte* ptr = &bytes[0])
            SUTextureCreateFromFile(&reference, ptr, sScale, tScale).CheckError();
        return new Texture(reference);
    }

    public int Width => GetDimensions().Item1;
    public int Height => GetDimensions().Item2;
    public double SScale => GetDimensions().Item3;
    public double TScale => GetDimensions().Item4;

    public string FileName
    {
        get => GetFileName();
        set => SetFileName(value);
    }

    public unsafe bool UseAlphaChannel => GetBool(&SUTextureGetUseAlphaChannel);
    public unsafe Color AverageColor => GetColor(&SUTextureGetAverageColor);

    public unsafe ImageRep ImageRep
    {
        get
        {
            var imageRep = new ImageRep();
            fixed (SUImageRepRef* ptr = &imageRep.Reference)
                SUTextureGetImageRep(Reference, ptr).CheckError();
            return imageRep;
        }
    }

    public unsafe ImageRep ColorizedImageRep
    {
        get
        {
            var imageRep = new ImageRep();
            fixed (SUImageRepRef* ptr = &imageRep.Reference)
                SUTextureGetColorizedImageRep(Reference, ptr).CheckError();
            return imageRep;
        }
    }

    public unsafe void WriteToFile(string path)
    {
        var bytes = path.GetSBytes();
        fixed (sbyte* ptr = &bytes[0])
            SUTextureWriteToFile(Reference, ptr).CheckError();
    }

    public unsafe void WriteOriginalToFile(string path)
    {
        var bytes = path.GetSBytes();
        fixed (sbyte* ptr = &bytes[0])
            SUTextureWriteOriginalToFile(Reference, ptr).CheckError();
    }

    private unsafe string GetFileName() => GetString(&SUTextureGetFileName);

    private unsafe void SetFileName(string value) => SetString(&SUTextureSetFileName, value);

    private unsafe (int, int, double, double) GetDimensions()
    {
        nuint width,
            height;
        double sScale,
            tScale;
        SUTextureGetDimensions(Reference, &width, &height, &sScale, &tScale).CheckError();
        return ((int)width, (int)height, sScale, tScale);
    }

    internal unsafe Texture(SUTextureRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUTextureRef*, SUResult> Release =>
        &SUTextureRelease;

    protected sealed override unsafe delegate* <SUTextureRef, SUEntityRef> ToEntity =>
        &SUTextureToEntity;
}
