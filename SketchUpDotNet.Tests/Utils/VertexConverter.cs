using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

class VertexConverter : WriteOnlyJsonConverter<Vertex>
{
    private Dictionary<EntityId, string> _idMapping = [];
    private int _counter = 1;

    public override void Write(VerifyJsonWriter writer, Vertex entity)
    {
        if (!_idMapping.TryGetValue(entity.Id, out var id))
        {
            id = $"$Vertex{_counter++}$";
            _idMapping[entity.Id] = id;
            writer.WriteStartObject();
            writer.WriteMember(entity, id, "Id");
            writer.WriteMember(entity, entity.Position, "Position");
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteValue(id);
        }
    }
}
