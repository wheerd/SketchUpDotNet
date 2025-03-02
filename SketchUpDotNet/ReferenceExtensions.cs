using System.Drawing;
using System.Runtime.CompilerServices;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

internal static class ReferenceExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe T EnsureReferenceValid<T>(this T reference)
        where T : unmanaged
    {
        var genericRef = (GenericRef*)&reference;
        ObjectDisposedException.ThrowIf(genericRef->ptr == null, reference);
        return reference;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void AddMany<T, TElement>(
        this T reference,
        delegate* <T, nuint, TElement*, SUResult> add,
        SUBase<TElement>[] elements,
        bool attached
    )
        where T : unmanaged
        where TElement : unmanaged
    {
        var refs = elements.Select(f => f.Reference).ToArray();
        fixed (TElement* refsPtr = &refs[0])
        {
            add(reference.EnsureReferenceValid(), (nuint)refs.Length, refsPtr).CheckError();
        }
        if (attached)
        {
            foreach (var element in elements)
            {
                element.SetAttachedToModel(true);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe TOut[] GetMany<T, TElement, TOut>(
        this T reference,
        delegate* <T, nuint*, SUResult> getCount,
        delegate* <T, nuint, TElement*, nuint*, SUResult> get,
        Func<TElement, TOut> construct,
        bool attached
    )
        where T : unmanaged
        where TElement : unmanaged
        where TOut : SUBase<TElement>
    {
        nuint num;
        getCount(reference.EnsureReferenceValid(), &num).CheckError();
        if (num == 0)
            return [];
        TElement[] refs = new TElement[num];
        nuint count;
        fixed (TElement* refsPtr = &refs[0])
        {
            get(reference.EnsureReferenceValid(), num, refsPtr, &count).CheckError();
        }
        var results = new TOut[count];
        for (nuint i = 0; i < count; i++)
        {
            results[i] = construct(refs[i]);
            if (attached)
            {
                results[i].SetAttachedToModel(true);
            }
        }
        return results;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe string[] GetStrings<T>(
        this T reference,
        delegate* <T, nuint*, SUResult> getCount,
        delegate* <T, nuint, SUStringRef*, nuint*, SUResult> get
    )
        where T : unmanaged
    {
        nuint num;
        getCount(reference.EnsureReferenceValid(), &num).CheckError();
        if (num == 0)
            return [];
        SUStringRef[] refs = new SUStringRef[num];
        for (int i = 0; i < refs.Length; i++)
        {
            fixed (SUStringRef* strPtr = &refs[i])
            {
                SUStringCreate(strPtr);
            }
        }
        try
        {
            nuint count;
            fixed (SUStringRef* refsPtr = &refs[0])
            {
                get(reference.EnsureReferenceValid(), num, refsPtr, &count).CheckError();
            }
            var results = new string[count];
            for (nuint i = 0; i < count; i++)
            {
                results[i] = refs[i].GetString();
            }
            return results;
        }
        finally
        {
            for (int i = 0; i < refs.Length; i++)
            {
                fixed (SUStringRef* refPtr = &refs[i])
                {
                    SUStringRelease(refPtr);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe TOut GetOne<T, TElement, TOut>(
        this T reference,
        delegate* <T, TElement*, SUResult> get,
        Func<TElement, TOut> construct,
        bool attached
    )
        where T : unmanaged
        where TElement : unmanaged
        where TOut : SUBase<TElement>
    {
        TElement element;
        get(reference.EnsureReferenceValid(), &element).CheckError();
        var outElement = construct(element);
        if (attached)
        {
            outElement.SetAttachedToModel(true);
        }
        return outElement;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe TOut GetOneByKey<T, TElement, TOut>(
        this T reference,
        delegate* <T, sbyte*, TElement*, SUResult> get,
        Func<TElement, TOut> construct,
        bool attached,
        string key
    )
        where T : unmanaged
        where TElement : unmanaged
        where TOut : SUBase<TElement>
    {
        var bytes = key.GetSBytes();
        TElement dict;
        fixed (sbyte* bytesPtr = &bytes[0])
        {
            get(reference.EnsureReferenceValid(), bytesPtr, &dict).CheckError();
        }
        var result = construct(dict);
        if (attached)
            result.SetAttachedToModel(true);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe TOut? GetOptionalOne<T, TElement, TOut>(
        this T reference,
        delegate* <T, TElement*, SUResult> get,
        Func<TElement, TOut> construct,
        bool attached
    )
        where T : unmanaged
        where TElement : unmanaged
        where TOut : SUBase<TElement>
    {
        TElement element;
        var result = get(reference.EnsureReferenceValid(), &element);
        if (result == SUResult.SU_ERROR_NO_DATA)
            return null;
        result.CheckError();
        var outElement = construct(element);
        if (attached)
        {
            outElement.SetAttachedToModel(true);
        }
        return outElement;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe string GetString<T>(
        this T reference,
        delegate* <T, SUStringRef*, SUResult> getter
    )
        where T : unmanaged
    {
        SUStringRef stringRef;
        SUStringCreate(&stringRef).CheckError();
        try
        {
            getter(reference.EnsureReferenceValid(), &stringRef).CheckError();
            return stringRef.GetString();
        }
        finally
        {
            SUStringRelease(&stringRef);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetString<T>(
        this T reference,
        delegate* <T, sbyte*, SUResult> setter,
        string value
    )
        where T : unmanaged
    {
        fixed (sbyte* bytesPtr = value.GetSBytes())
        {
            setter(reference.EnsureReferenceValid(), bytesPtr).CheckError();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe bool GetBool<T>(this T reference, delegate* <T, bool*, SUResult> getter)
        where T : unmanaged
    {
        bool value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetBool<T>(
        this T reference,
        delegate* <T, byte, SUResult> setter,
        bool value
    )
        where T : unmanaged
    {
        byte byteValue = *(byte*)&value;
        setter(reference.EnsureReferenceValid(), byteValue).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Color GetColor<T>(
        this T reference,
        delegate* <T, SUColor*, SUResult> getter
    )
        where T : unmanaged
    {
        SUColor value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value.ToColor();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Color? GetOptionalColor<T>(
        this T reference,
        delegate* <T, SUColor*, SUResult> getter
    )
        where T : unmanaged
    {
        SUColor value;
        var result = getter(reference.EnsureReferenceValid(), &value);
        if (result == SUResult.SU_ERROR_NO_DATA)
        {
            return null;
        }
        result.CheckError();
        return value.ToColor();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetColor<T>(
        this T reference,
        delegate* <T, SUColor*, SUResult> setter,
        Color value
    )
        where T : unmanaged
    {
        SUColor suColor = value.ToSU();
        setter(reference.EnsureReferenceValid(), &suColor).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe sbyte GetByte<T>(
        this T reference,
        delegate* <T, sbyte*, SUResult> getter
    )
        where T : unmanaged
    {
        sbyte value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetByte<T>(
        this T reference,
        delegate* <T, sbyte, SUResult> setter,
        sbyte value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int GetInt<T>(this T reference, delegate* <T, int*, SUResult> getter)
        where T : unmanaged
    {
        int value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe int GetInt<T>(this T reference, delegate* <T, nuint*, SUResult> getter)
        where T : unmanaged
    {
        nuint value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return (int)value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetInt<T>(
        this T reference,
        delegate* <T, int, SUResult> setter,
        int value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe double GetDouble<T>(
        this T reference,
        delegate* <T, double*, SUResult> getter
    )
        where T : unmanaged
    {
        double value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe double? GetOptionalDouble<T>(
        this T reference,
        delegate* <T, double*, SUResult> getter
    )
        where T : unmanaged
    {
        double value;
        var result = getter(reference.EnsureReferenceValid(), &value);
        if (result == SUResult.SU_ERROR_NO_DATA)
            return null;
        result.CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetDouble<T>(
        this T reference,
        delegate* <T, double, SUResult> setter,
        double value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe short GetShort<T>(
        this T reference,
        delegate* <T, short*, SUResult> getter
    )
        where T : unmanaged
    {
        short value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetShort<T>(
        this T reference,
        delegate* <T, short, SUResult> setter,
        short value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe Point3D GetVector<T>(
        this T reference,
        delegate* <T, double*, SUResult> getter
    )
        where T : unmanaged
    {
        var vector = new double[3];
        fixed (double* vectorPtr = &vector[0])
        {
            getter(reference.EnsureReferenceValid(), vectorPtr).CheckError();
        }
        return new(vector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetVector<T>(
        this T reference,
        delegate* <T, double*, SUResult> setter,
        Point3D value
    )
        where T : unmanaged
    {
        var values = new double[] { value.X, value.Y, value.Z };
        fixed (double* valuesPtr = &values[0])
        {
            setter(reference.EnsureReferenceValid(), valuesPtr).CheckError();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe long GetLong<T>(this T reference, delegate* <T, long*, SUResult> getter)
        where T : unmanaged
    {
        long value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetLong<T>(
        this T reference,
        delegate* <T, long, SUResult> setter,
        long value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe float GetFloat<T>(
        this T reference,
        delegate* <T, float*, SUResult> getter
    )
        where T : unmanaged
    {
        float value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetFloat<T>(
        this T reference,
        delegate* <T, float, SUResult> setter,
        float value
    )
        where T : unmanaged
    {
        setter(reference.EnsureReferenceValid(), value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe TValue Get<T, TValue>(
        this T reference,
        delegate* <T, TValue*, SUResult> getter
    )
        where T : unmanaged
        where TValue : unmanaged
    {
        TValue value;
        getter(reference.EnsureReferenceValid(), &value).CheckError();
        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetOne<T, TRef, TValue>(
        this T reference,
        delegate* <T, TRef, SUResult> setter,
        bool attached,
        TValue value
    )
        where T : unmanaged
        where TRef : unmanaged
        where TValue : SUBase<TRef>
    {
        setter(reference.EnsureReferenceValid(), value.Reference).CheckError();
        if (attached)
        {
            value.SetAttachedToModel(true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static unsafe void SetOptionalOne<T, TRef, TValue>(
        this T reference,
        delegate* <T, TRef, SUResult> setter,
        bool attached,
        TValue? value
    )
        where T : unmanaged
        where TRef : unmanaged
        where TValue : SUBase<TRef>
    {
        setter(reference.EnsureReferenceValid(), value?.Reference ?? default).CheckError();
        if (attached)
        {
            value?.SetAttachedToModel(true);
        }
    }
}
