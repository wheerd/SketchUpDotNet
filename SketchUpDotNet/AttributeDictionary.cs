using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class AttributeDictionary : Entity<SUAttributeDictionaryRef>
{
    public string Name => GetName();

    public ICollection<string> Keys => GetKeys();

    public int Count => GetCount();

    public object? this[string key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    public IReadOnlyDictionary<string, object?> Values =>
        new ReadOnlyDictionary<string, object?>(Keys.Select(k => (k, Get(k))).ToDictionary());

    public static unsafe AttributeDictionary Create(string name)
    {
        SUAttributeDictionaryRef dict;
        var bytes = name.GetSBytes();
        fixed (sbyte* namePtr = &bytes[0])
        {
            SUAttributeDictionaryCreate(&dict, namePtr).CheckError();
        }
        return new AttributeDictionary(dict);
    }

    internal AttributeDictionary(SUAttributeDictionaryRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUAttributeDictionaryRef*, SUResult> Release =>
        &SUAttributeDictionaryRelease;

    protected sealed override unsafe delegate* <SUAttributeDictionaryRef, SUEntityRef> ToEntity =>
        &SUAttributeDictionaryToEntity;

    private unsafe string GetName() => GetString(&SUAttributeDictionaryGetName);

    private unsafe string[] GetKeys() =>
        GetStrings(&SUAttributeDictionaryGetNumKeys, &SUAttributeDictionaryGetKeys);

    private unsafe int GetCount() => GetInt(&SUAttributeDictionaryGetNumKeys);

    private unsafe void Set(string key, object? value)
    {
        using var typedValue = TypedValue.Create(value);
        fixed (sbyte* bytesPtr = key.GetSBytes())
        {
            SUAttributeDictionarySetValue(Reference, bytesPtr, typedValue.Reference).CheckError();
        }
    }

    private unsafe object? Get(string key)
    {
        using var typedValue = new TypedValue();
        fixed (SUTypedValueRef* valuePtr = &typedValue.Reference)
        fixed (sbyte* bytesPtr = key.GetSBytes())
        {
            var result = SUAttributeDictionaryGetValue(Reference, bytesPtr, valuePtr);
            if (result == SUResult.SU_ERROR_NO_DATA)
            {
                return null;
            }
            result.CheckError();
        }
        return typedValue.ToObject();
    }
}
