using System.Collections;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class RenderingOptions
    : SUBase<SURenderingOptionsRef>,
        IEnumerable<KeyValuePair<string, object?>>
{
    public ICollection<string> Keys => GetKeys();

    public int Count => GetCount();

    public object? this[string key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    internal RenderingOptions(SURenderingOptionsRef @ref)
        : base(@ref) { }

    private unsafe string[] GetKeys() =>
        GetStrings(&SURenderingOptionsGetNumKeys, &SURenderingOptionsGetKeys);

    private unsafe int GetCount() => GetInt(&SURenderingOptionsGetNumKeys);

    private unsafe void Set(string key, object? value)
    {
        using var typedValue = TypedValue.Create(value);
        fixed (sbyte* bytesPtr = key.GetSBytes())
        {
            SURenderingOptionsSetValue(Reference, bytesPtr, typedValue.Reference).CheckError();
        }
    }

    private unsafe object? Get(string key)
    {
        using var typedValue = new TypedValue();
        fixed (SUTypedValueRef* valuePtr = &typedValue.Reference)
        fixed (sbyte* bytesPtr = key.GetSBytes())
        {
            var result = SURenderingOptionsGetValue(Reference, bytesPtr, valuePtr);
            if (result == SUResult.SU_ERROR_NO_DATA)
            {
                return null;
            }
            result.CheckError();
        }
        return typedValue.ToObject();
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (var key in GetKeys())
        {
            yield return new(key, Get(key));
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    protected sealed override unsafe delegate* <SURenderingOptionsRef*, SUResult> Release => null;
}
