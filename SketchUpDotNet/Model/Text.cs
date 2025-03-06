using System.Drawing;
using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Text : DrawingElement<SUTextRef>
{
    public unsafe Text()
        : base(&SUTextCreate) { }

    public unsafe string String
    {
        get => GetString(&SUTextGetString);
        set => SetString(&SUTextSetString, value);
    }

    public unsafe Font Font
    {
        get => GetOne(&SUTextGetFont, (SUFontRef f) => new Font(f));
        set => SetOne<SUFontRef, Font>(&SUTextSetFont, value);
    }

    public unsafe void GetPoint(out Point3D point, out InstancePath? path)
    {
        SUPoint3D p;
        SUInstancePathRef i;
        SUTextGetPoint(Reference, &p, &i).CheckError();
        point = new(p);
        path = i.ptr != null ? new(i) : null;
    }

    public unsafe void SetPoint(Point3D? point, InstancePath? path)
    {
        SUPoint3D? su = point?.ToSU();
        SUPoint3D* suPtr = su is SUPoint3D p ? &p : (SUPoint3D*)null;
        SUTextSetPoint(Reference, suPtr, path?.Reference ?? new()).CheckError();
    }

    public unsafe SUTextLeaderType LeaderType
    {
        get => Get<SUTextLeaderType>(&SUTextGetLeaderType);
        set => Set(&SUTextSetLeaderType, value);
    }

    public unsafe Vector3D LeaderVector
    {
        get => new(Get<SUVector3D>(&SUTextGetLeaderVector));
        set => Set(&SUTextSetLeaderVector, value.ToSU());
    }

    public unsafe SUArrowType ArrowType
    {
        get => Get<SUArrowType>(&SUTextGetArrowType);
        set => Set(&SUTextSetArrowType, value);
    }

    public unsafe Color Color
    {
        get => Get<SUColor>(&SUTextGetColor).ToColor();
        set => Set(&SUTextSetColor, value.ToSU());
    }

    public unsafe (double PercentX, double PercentY) ScreenPosition
    {
        get
        {
            double percentX,
                percentY;
            SUTextGetScreenPosition(Reference, &percentX, &percentY).CheckError();
            return (percentX, percentY);
        }
        set => SUTextSetScreenPosition(Reference, value.PercentX, value.PercentY).CheckError();
    }

    internal unsafe Text(SUTextRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUTextRef*, SUResult> Release => &SUTextRelease;

    protected sealed override unsafe delegate* <SUTextRef, SUEntityRef> ToEntity => &SUTextToEntity;

    protected sealed override unsafe delegate* <SUTextRef, SUDrawingElementRef> ToDrawingElement =>
        &SUTextToDrawingElement;
}
