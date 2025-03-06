using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Classifications : SUBase<SUClassificationsRef>
{
    public unsafe void Load(string path)
    {
        fixed (sbyte* bytesPtr = path.GetSBytes())
        {
            SUClassificationsLoadSchema(Reference, bytesPtr).CheckError();
        }
    }

    public unsafe Schema SchemaByName(string name)
    {
        SUSchemaRef schema;
        fixed (sbyte* bytesPtr = name.GetSBytes())
        {
            SUClassificationsGetSchema(Reference, bytesPtr, &schema).CheckError();
        }
        return new(schema);
    }

    public unsafe IEnumerable<Schema> Schemas =>
        GetMany(
            &SUClassificationsGetNumSchemas,
            &SUClassificationsGetSchemas,
            (SUSchemaRef s) => new Schema(s)
        );

    internal unsafe Classifications(SUClassificationsRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUClassificationsRef*, SUResult> Release => null;
}
