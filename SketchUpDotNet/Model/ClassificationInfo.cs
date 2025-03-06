using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class ClassificationInfo : SUBase<SUClassificationInfoRef>
{
    public unsafe int SchemaCount => GetInt(&SUClassificationInfoGetNumSchemas);

    public unsafe string GetSchemaName(int i)
    {
        SUStringRef stringRef;
        SUStringCreate(&stringRef).CheckError();
        try
        {
            SUClassificationInfoGetSchemaName(
                    Reference.EnsureReferenceValid(),
                    (nuint)i,
                    &stringRef
                )
                .CheckError();
            return stringRef.GetString();
        }
        finally
        {
            SUStringRelease(&stringRef);
        }
    }

    public unsafe string GetSchemaType(int i)
    {
        SUStringRef stringRef;
        SUStringCreate(&stringRef).CheckError();
        try
        {
            SUClassificationInfoGetSchemaType(
                    Reference.EnsureReferenceValid(),
                    (nuint)i,
                    &stringRef
                )
                .CheckError();
            return stringRef.GetString();
        }
        finally
        {
            SUStringRelease(&stringRef);
        }
    }

    public unsafe ClassificationAttribute GetSchemaAttribute(int i)
    {
        SUClassificationAttributeRef attribute;
        SUClassificationInfoGetSchemaAttribute(
                Reference.EnsureReferenceValid(),
                (nuint)i,
                &attribute
            )
            .CheckError();
        return new(attribute);
    }

    public unsafe ClassificationAttribute GetSchemaAttribute(string path)
    {
        SUStringRef s;
        fixed (sbyte* bytesPtr = path.GetSBytes())
            SUStringCreateFromUTF8(&s, bytesPtr).CheckError();
        try
        {
            SUClassificationAttributeRef attribute;
            SUClassificationInfoGetSchemaAttributeByPath(
                    Reference.EnsureReferenceValid(),
                    s,
                    &attribute
                )
                .CheckError();
            return new(attribute);
        }
        finally
        {
            SUStringRelease(&s);
        }
    }

    internal unsafe ClassificationInfo(SUClassificationInfoRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUClassificationInfoRef*, SUResult> Release =>
        &SUClassificationInfoRelease;
}
