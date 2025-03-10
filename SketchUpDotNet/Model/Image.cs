using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Image : DrawingElement<SUImageRef>
{
    public static unsafe Image CreateFromFile(string path)
    {
        SUImageRef image;
        fixed (sbyte* bytesPtr = path.GetSBytes())
            SUImageCreateFromFile(&image, bytesPtr).CheckError();
        return new(image);
    }

    public static unsafe Image Create(ImageRep rep)
    {
        SUImageRef image;
        SUImageCreateFromImageRep(&image, rep.Reference).CheckError();
        return new(image);
    }

    public unsafe string FileName => GetString(&SUImageGetFileName);

    public unsafe ImageRep ImageRep
    {
        get
        {
            var imageRep = new ImageRep();
            fixed (SUImageRepRef* ptr = &imageRep.Reference)
                SUImageGetImageRep(Reference, ptr).CheckError();
            return imageRep;
        }
    }

    public unsafe Transform3D Transform
    {
        get => new(Get<SUTransformation>(&SUImageGetTransform));
        set => Set(&SUImageSetTransform, value.ToSU());
    }

    public unsafe double Width => GetDimensions().width.FromSULength();
    public unsafe double Height => GetDimensions().height.FromSULength();

    public unsafe Component Definition =>
        GetOne(&SUImageGetDefinition, (SUComponentDefinitionRef d) => new Component(d));

    private unsafe (double width, double height) GetDimensions()
    {
        double width,
            height;
        SUImageGetDimensions(Reference, &width, &height).CheckError();
        return (width, height);
    }

    internal unsafe Image(SUImageRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUImageRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUImageRef, SUEntityRef> ToEntity =>
        &SUImageToEntity;

    protected sealed override unsafe delegate* <SUImageRef, SUDrawingElementRef> ToDrawingElement =>
        &SUImageToDrawingElement;
}
