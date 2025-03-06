using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class ClassificationAttribute : SUBase<SUClassificationAttributeRef>
{
    public unsafe string Path => GetString(&SUClassificationAttributeGetPath);

    public unsafe object? Value
    {
        get
        {
            using var typedValue = new TypedValue();
            fixed (SUTypedValueRef* valuePtr = &typedValue.Reference)
            {
                var result = SUClassificationAttributeGetValue(Reference, valuePtr);
                if (result == SUResult.SU_ERROR_NO_DATA)
                {
                    return null;
                }
                result.CheckError();
            }
            return typedValue.ToObject();
        }
    }

    public unsafe int ChildCount => GetInt(&SUClassificationAttributeGetNumChildren);

    public unsafe ClassificationAttribute GetChild(int i)
    {
        SUClassificationAttributeRef child;
        SUClassificationAttributeGetChild(Reference.EnsureReferenceValid(), (nuint)i, &child)
            .CheckError();
        return new(child);
    }

    public unsafe IEnumerable<ClassificationAttribute> Children =>
        Enumerable.Range(0, ChildCount).Select(i => GetChild(i));

    internal unsafe ClassificationAttribute(SUClassificationAttributeRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUClassificationAttributeRef*, SUResult> Release =>
        null;
}
