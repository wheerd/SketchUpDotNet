using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Schema : SUBase<SUSchemaRef>
{
    public unsafe string Name => GetString(&SUSchemaGetSchemaName);

    public unsafe SUSchemaTypeRef GetType(string name)
    {
        SUSchemaTypeRef type;
        var sBytes = name.GetSBytes();
        fixed (sbyte* bytesPtr = &sBytes[0])
            SUSchemaGetSchemaType(Reference, bytesPtr, &type).CheckError();
        return type;
    }

    internal unsafe Schema(SUSchemaRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUSchemaRef*, SUResult> Release => null;
}
