namespace SketchUpDotNet.Tests;

class EntityIdConverter : WriteOnlyJsonConverter<EntityId>
{
    private Dictionary<EntityId, string> _idMapping = [];
    private int _counter = 1;

    public override void Write(VerifyJsonWriter writer, EntityId entityId)
    {
        if (!_idMapping.TryGetValue(entityId, out var id))
        {
            id = $"$Entity{_counter++}$";
            _idMapping[entityId] = id;
        }
        writer.WriteValue(id);
    }
}
