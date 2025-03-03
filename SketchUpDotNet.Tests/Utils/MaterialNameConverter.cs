using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

class MaterialNameConverter : WriteOnlyJsonConverter<Material>
{
    public override void Write(VerifyJsonWriter writer, Material material)
    {
        writer.WriteValue(material.Name);
    }
}
