using System.Drawing;
using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class TypedValue : SUBase<SUTypedValueRef>
{
    public static unsafe TypedValue Create(object? obj)
    {
        var typedValue = new TypedValue();
        if (obj is bool b)
            typedValue.SetBool(&SUTypedValueSetBool, b);
        else if (obj is sbyte by)
            typedValue.SetByte(&SUTypedValueSetByte, by);
        else if (obj is Color c)
            typedValue.SetColor(&SUTypedValueSetColor, c);
        else if (obj is double d)
            typedValue.SetDouble(&SUTypedValueSetDouble, d);
        else if (obj is float f)
            typedValue.SetFloat(&SUTypedValueSetFloat, f);
        else if (obj is int i)
            typedValue.SetInt(&SUTypedValueSetInt32, i);
        else if (obj is short s)
            typedValue.SetShort(&SUTypedValueSetInt16, s);
        else if (obj is string str)
            typedValue.SetString(&SUTypedValueSetString, str);
        else if (obj is long l)
            typedValue.SetLong(&SUTypedValueSetTime, l);
        else if (obj is Point3D p)
            typedValue.SetVector(&SUTypedValueSetVector3d, p);
        else if (obj is Array a)
            SetArray(typedValue, a);

        return typedValue;
    }

    private static unsafe void SetArray(TypedValue typedValue, Array array)
    {
        if (array.Length == 0)
        {
            SUTypedValueSetArrayItems(typedValue.Reference, 0, null);
        }
        else
        {
            var typedValues = array.Cast<object>().Select(v => Create(v).Reference).ToArray();
            fixed (SUTypedValueRef* ptr = &typedValues[0])
            {
                SUTypedValueSetArrayItems(typedValue.Reference, (nuint)typedValues.Length, ptr);
            }
        }
    }

    public unsafe object? ToObject()
    {
        SUTypedValueType type;
        SUTypedValueGetType(Reference, &type).CheckError();
        switch (type)
        {
            case SUTypedValueType.SUTypedValueType_Bool:
                return GetBool(&SUTypedValueGetBool);
            case SUTypedValueType.SUTypedValueType_Byte:
                sbyte byteValue;
                SUTypedValueGetByte(Reference, &byteValue).CheckError();
                return byteValue;
            case SUTypedValueType.SUTypedValueType_Color:
                SUColor colorValue;
                SUTypedValueGetColor(Reference, &colorValue).CheckError();
                return colorValue.ToColor();
            case SUTypedValueType.SUTypedValueType_Double:
                double doubleValue;
                SUTypedValueGetDouble(Reference, &doubleValue).CheckError();
                return doubleValue;
            case SUTypedValueType.SUTypedValueType_Float:
                float floatValue;
                SUTypedValueGetFloat(Reference, &floatValue).CheckError();
                return floatValue;
            case SUTypedValueType.SUTypedValueType_Int32:
                int intValue;
                SUTypedValueGetInt32(Reference, &intValue).CheckError();
                return intValue;
            case SUTypedValueType.SUTypedValueType_Short:
                short shortValue;
                SUTypedValueGetInt16(Reference, &shortValue).CheckError();
                return shortValue;
            case SUTypedValueType.SUTypedValueType_String:
                SUStringRef stringRef;
                SUStringCreate(&stringRef).CheckError();
                try
                {
                    SUTypedValueGetString(Reference, &stringRef).CheckError();
                    return stringRef.GetString();
                }
                finally
                {
                    SUStringRelease(&stringRef);
                }
            case SUTypedValueType.SUTypedValueType_Time:
                long longValue;
                SUTypedValueGetTime(Reference, &longValue).CheckError();
                return longValue;
            case SUTypedValueType.SUTypedValueType_Vector3D:
                var vector = new double[3];
                fixed (double* vectorPtr = &vector[0])
                {
                    SUTypedValueGetVector3d(Reference, vectorPtr).CheckError();
                }
                return new Point3D(vector);
            case SUTypedValueType.SUTypedValueType_Array:
                return GetMany(
                        &SUTypedValueGetNumArrayItems,
                        &SUTypedValueGetArrayItems,
                        (SUTypedValueRef v) => new TypedValue(v)
                    )
                    .Select(tv => tv.ToObject())
                    .ToArray();
        }
        return null;
    }

    internal unsafe TypedValue()
        : base(&SUTypedValueCreate) { }

    internal TypedValue(SUTypedValueRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUTypedValueRef*, SUResult> Release =>
        &SUTypedValueRelease;
}
