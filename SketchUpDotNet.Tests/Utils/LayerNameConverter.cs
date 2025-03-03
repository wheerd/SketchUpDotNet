using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

class LayerNameConverter : WriteOnlyJsonConverter<Layer>
{
    public override void Write(VerifyJsonWriter writer, Layer layer)
    {
        writer.WriteValue(layer.Name);
    }
}
