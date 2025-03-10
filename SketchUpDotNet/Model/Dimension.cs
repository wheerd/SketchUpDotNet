using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public abstract class Dimension<T> : DrawingElement<T>, IDimension
    where T : unmanaged
{
    public unsafe string Text
    {
        get => DimensionRef.GetString(&SUDimensionGetText);
        set => DimensionRef.SetString(&SUDimensionSetText, value);
    }

    public unsafe Plane3D Plane
    {
        get
        {
            SUPlane3D plane;
            SUDimensionGetPlane(DimensionRef, &plane).CheckError();
            return new(plane);
        }
    }

    public unsafe bool IsText3D
    {
        get => DimensionRef.GetBool(&SUDimensionGetText3D);
        set => DimensionRef.SetBool(&SUDimensionSetText3D, value);
    }

    public unsafe SUArrowType ArrowType
    {
        get
        {
            SUArrowType type;
            SUDimensionGetArrowType(DimensionRef, &type).CheckError();
            return type;
        }
        set => SUDimensionSetArrowType(DimensionRef, value).CheckError();
    }

    public unsafe Font Font
    {
        get => DimensionRef.GetOne(&SUDimensionGetFont, (SUFontRef f) => new Font(f), attached);
        set =>
            DimensionRef.SetOne<SUDimensionRef, SUFontRef, Font>(
                &SUDimensionSetFont,
                attached,
                value
            );
    }

    public unsafe SUDimensionType Type =>
        DimensionRef.Get<SUDimensionRef, SUDimensionType>(&SUDimensionGetType);

    internal unsafe Dimension(T @ref)
        : base(@ref) { }

    private SUDimensionRef? curveRef;

    protected abstract unsafe delegate* <T, SUDimensionRef> ToDimension { get; }

    private unsafe SUDimensionRef DimensionRef => curveRef ??= ToDimension(Reference);
}

public interface IDimension : IDrawingElement
{
    internal static unsafe IDimension Create(SUDimensionRef @ref)
    {
        return @ref.Get<SUDimensionRef, SUDimensionType>(&SUDimensionGetType) switch
        {
            SUDimensionType.SUDimensionType_Linear => new DimensionLinear(
                SUDimensionLinearFromDimension(@ref)
            ),
            SUDimensionType.SUDimensionType_Radial => new DimensionRadial(
                SUDimensionRadialFromDimension(@ref)
            ),
            _ => throw new ArgumentOutOfRangeException(nameof(@ref)),
        };
    }

    public string Text { get; set; }

    public Plane3D Plane { get; }

    public bool IsText3D { get; set; }

    public SUArrowType ArrowType { get; set; }

    public Font Font { get; set; }

    public unsafe SUDimensionType Type { get; }
}
