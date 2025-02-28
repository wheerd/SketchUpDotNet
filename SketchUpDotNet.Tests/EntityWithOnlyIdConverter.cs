namespace SketchUpDotNet.Tests;

class EntityWithOnlyIdConverter<T> : WriteOnlyJsonConverter<T>
    where T : IEntity
{
    private Dictionary<EntityId, string> _idMapping = [];
    private int _counter = 1;

    public override void Write(VerifyJsonWriter writer, T entity)
    {
        if (!_idMapping.TryGetValue(entity.Id, out var id))
        {
            id = $"${typeof(T).Name}{_counter++}$";
            _idMapping[entity.Id] = id;
        }
        writer.WriteValue(id);
    }
}
