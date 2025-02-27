using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

internal static class InternalSketchUpExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe sbyte[] GetSBytes(this string s) =>
        (sbyte[])(Array)Encoding.ASCII.GetBytes(s + "\0");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe string GetString(this SUStringRef stringRef)
    {
        nuint strLen = 0;
        SUStringGetUTF8Length(stringRef, &strLen);
        if (strLen == 0)
            return string.Empty;

        sbyte[] strUtf8 = new sbyte[strLen + 1];
        fixed (sbyte* strPtr = &strUtf8[0])
        {
            SUStringGetUTF8(stringRef, strLen + 1, strPtr, &strLen);

            return Encoding.UTF8.GetString((byte*)strPtr, (int)strLen);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void CheckError(this SUResult result)
    {
        if (result != SUResult.SU_ERROR_NONE)
        {
            throw new Exception($"Sketchup error: {Enum.GetName(result)}");
        }
    }

    public static SUColor ToSU(this Color color) =>
        new()
        {
            alpha = color.A,
            red = color.R,
            green = color.G,
            blue = color.B,
        };

    public static Color ToColor(this SUColor color) =>
        Color.FromArgb(color.alpha, color.red, color.green, color.blue);
}
