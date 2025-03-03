using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Font : Entity<SUFontRef>
{
    public unsafe string FaceName => GetString(&SUFontGetFaceName);
    public unsafe int PointSize => GetInt(&SUFontGetPointSize);
    public unsafe bool IsBold => GetBool(&SUFontGetBold);
    public unsafe bool IsItalic => GetBool(&SUFontGetItalic);
    public unsafe bool UseWorldSize => GetBool(&SUFontGetUseWorldSize);
    public unsafe double WorldSize => GetDouble(&SUFontGetWorldSize);

    internal unsafe Font(SUFontRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUFontRef*, SUResult> Release => null;

    protected sealed override unsafe delegate* <SUFontRef, SUEntityRef> ToEntity => &SUFontToEntity;
}
