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
            SUClassificationInfoGetSchemaName(Reference, (nuint)i, &stringRef).CheckError();
            return stringRef.GetString();
        }
        finally
        {
            SUStringRelease(&stringRef).CheckError();
        }
    }

    public unsafe string GetSchemaType(int i)
    {
        SUStringRef stringRef;
        SUStringCreate(&stringRef).CheckError();
        try
        {
            SUClassificationInfoGetSchemaType(Reference, (nuint)i, &stringRef).CheckError();
            return stringRef.GetString();
        }
        finally
        {
            SUStringRelease(&stringRef).CheckError();
        }
    }

    public unsafe ClassificationAttribute GetSchemaAttribute(int i)
    {
        SUClassificationAttributeRef attribute;
        SUClassificationInfoGetSchemaAttribute(Reference, (nuint)i, &attribute).CheckError();
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
            SUClassificationInfoGetSchemaAttributeByPath(Reference, s, &attribute).CheckError();
            return new(attribute);
        }
        finally
        {
            SUStringRelease(&s).CheckError();
        }
    }

    internal unsafe ClassificationInfo(SUClassificationInfoRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUClassificationInfoRef*, SUResult> Release =>
        &SUClassificationInfoRelease;
}
