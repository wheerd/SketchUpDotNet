using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

class EntityWithOnlyIdConverter<T>(EntityIdSource? idSource = null) : WriteOnlyJsonConverter<T>
    where T : IEntity
{
    private EntityIdSource _idSource = idSource ?? new();

    public override void Write(VerifyJsonWriter writer, T entity)
    {
        _idSource.GetId(typeof(T).Name, entity.Id, out var id);
        writer.WriteValue(id);
    }
}
