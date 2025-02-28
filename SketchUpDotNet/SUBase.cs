using System.Drawing;
using System.Runtime.CompilerServices;
using SketchUpDotNet.Bindings;

namespace SketchUpDotNet;

public abstract class SUBase<T> : IDisposable
    where T : unmanaged
{
    internal T Reference;

    internal unsafe SUBase(T @ref)
    {
        Reference = @ref;
    }

    internal unsafe SUBase(T @ref, bool attached)
        : this(@ref)
    {
        this.attached = attached;
    }

    internal unsafe SUBase(delegate* <T*, SUResult> factory)
    {
        T @ref;
        factory(&@ref).CheckError();
        Reference = @ref;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void AddMany<TElement>(
        delegate* <T, nuint, TElement*, SUResult> add,
        SUBase<TElement>[] elements
    )
        where TElement : unmanaged => Reference.AddMany(add, elements, attached || this is Model);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe TOut[] GetMany<TElement, TOut>(
        delegate* <T, nuint*, SUResult> getCount,
        delegate* <T, nuint, TElement*, nuint*, SUResult> get,
        Func<TElement, TOut> construct
    )
        where TElement : unmanaged
        where TOut : SUBase<TElement> =>
        Reference.GetMany(getCount, get, construct, attached || this is Model);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe string[] GetStrings(
        delegate* <T, nuint*, SUResult> getCount,
        delegate* <T, nuint, SUStringRef*, nuint*, SUResult> get
    ) => Reference.GetStrings(getCount, get);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe TOut GetOne<TElement, TOut>(
        delegate* <T, TElement*, SUResult> get,
        Func<TElement, TOut> construct
    )
        where TElement : unmanaged
        where TOut : SUBase<TElement> =>
        Reference.GetOne(get, construct, attached || this is Model);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe TOut? GetOptionalOne<TElement, TOut>(
        delegate* <T, TElement*, SUResult> get,
        Func<TElement, TOut> construct
    )
        where TElement : unmanaged
        where TOut : SUBase<TElement> =>
        Reference.GetOptionalOne(get, construct, attached || this is Model);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe string GetString(delegate* <T, SUStringRef*, SUResult> getter) =>
        Reference.GetString(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetString(delegate* <T, sbyte*, SUResult> setter, string value) =>
        Reference.SetString(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe bool GetBool(delegate* <T, bool*, SUResult> getter) =>
        Reference.GetBool(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetBool(delegate* <T, byte, SUResult> setter, bool value) =>
        Reference.SetBool(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe Color GetColor(delegate* <T, SUColor*, SUResult> getter) =>
        Reference.GetColor(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe Color? GetOptionalColor(delegate* <T, SUColor*, SUResult> getter) =>
        Reference.GetOptionalColor(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetColor(delegate* <T, SUColor*, SUResult> setter, Color value) =>
        Reference.SetColor(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe sbyte GetByte(delegate* <T, sbyte*, SUResult> getter) =>
        Reference.GetByte(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetByte(delegate* <T, sbyte, SUResult> setter, sbyte value) =>
        Reference.SetByte(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe int GetInt(delegate* <T, nuint*, SUResult> getter) => Reference.GetInt(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetInt(delegate* <T, int, SUResult> setter, int value) =>
        Reference.SetInt(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe double GetDouble(delegate* <T, double*, SUResult> getter) =>
        Reference.GetDouble(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe double? GetOptionalDouble(delegate* <T, double*, SUResult> getter) =>
        Reference.GetOptionalDouble(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetDouble(delegate* <T, double, SUResult> setter, double value) =>
        Reference.SetDouble(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe short GetShort(delegate* <T, short*, SUResult> getter) =>
        Reference.GetShort(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetShort(delegate* <T, short, SUResult> setter, short value) =>
        Reference.SetShort(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe Point3D GetVector(delegate* <T, double*, SUResult> getter) =>
        Reference.GetVector(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetVector(delegate* <T, double*, SUResult> setter, Point3D value) =>
        Reference.SetVector(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe long GetLong(delegate* <T, long*, SUResult> getter) =>
        Reference.GetLong(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetLong(delegate* <T, long, SUResult> setter, long value) =>
        Reference.SetLong(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe float GetFloat(delegate* <T, float*, SUResult> getter) =>
        Reference.GetFloat(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetFloat(delegate* <T, float, SUResult> setter, float value) =>
        Reference.SetFloat(setter, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe TValue Get<TValue>(delegate* <T, TValue*, SUResult> getter)
        where TValue : unmanaged => Reference.Get(getter);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetOne<TRef, TValue>(delegate* <T, TRef, SUResult> setter, TValue value)
        where TRef : unmanaged
        where TValue : SUBase<TRef> => Reference.SetOne(setter, attached, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void SetOptionalOne<TRef, TValue>(
        delegate* <T, TRef, SUResult> setter,
        TValue? value
    )
        where TRef : unmanaged
        where TValue : SUBase<TRef> => Reference.SetOptionalOne(setter, attached, value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void Set<TValue>(delegate* <T, TValue, SUResult> setter, TValue value)
        where TValue : unmanaged
    {
        setter(Reference, value).CheckError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe void Set<TValue>(delegate* <T, TValue*, SUResult> setter, TValue value)
        where TValue : unmanaged
    {
        setter(Reference, &value).CheckError();
    }

    private bool disposed = false;

    protected abstract unsafe delegate* <T*, SUResult> Release { get; }

    protected bool attached;

    internal virtual void SetAttachedToModel(bool attached)
    {
        this.attached = attached;
    }

    private unsafe void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (Release != null && (!attached || this is Model))
            {
                fixed (T* refPtr = &Reference)
                {
                    var innerPointer = (GenericRef*)refPtr;
                    ObjectDisposedException.ThrowIf(innerPointer->ptr == null, this);
                    Release(refPtr);
                    innerPointer->ptr = null;
                }
            }
            disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SUBase()
    {
        Dispose(false);
    }

    public override unsafe bool Equals(object? obj)
    {
        if (obj is SUBase<T> other)
        {
            fixed (T* referencePtr = &Reference)
            fixed (T* otherPtr = &other.Reference)
                return ((GenericRef*)referencePtr)->ptr == ((GenericRef*)otherPtr)->ptr;
        }
        return false;
    }

    public override unsafe int GetHashCode()
    {
        fixed (T* referencePtr = &Reference)
            return ((long)((GenericRef*)referencePtr)->ptr).GetHashCode();
    }

    public static bool operator ==(SUBase<T> left, SUBase<T> right) => left.Equals(right);

    public static bool operator !=(SUBase<T> left, SUBase<T> right) => !left.Equals(right);
}

unsafe struct GenericRef
{
    internal void* ptr;
}
