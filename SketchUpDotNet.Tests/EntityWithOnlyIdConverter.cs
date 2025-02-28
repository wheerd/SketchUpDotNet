namespace SketchUpDotNet.Tests;

class EntityWithOnlyIdConverter<T> : WriteOnlyJsonConverter<T>
    where T : IEntity
{
    public override void Write(VerifyJsonWriter writer, T entity)
    {
        writer.WriteValue($"${entity.Id}");
    }
}
