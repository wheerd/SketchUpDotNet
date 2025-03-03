namespace SketchUpDotNet.Tests.Utils;

class EntityConverter<T>(EntityIdSource idSource) : WriteOnlyJsonConverter<T>
    where T : IEntity
{
    public override void Write(VerifyJsonWriter writer, T entity)
    {
        if (idSource.GetId(typeof(T).Name, entity.Id, out var id))
        {
            writer.WriteStartObject();
            var props = typeof(T).GetProperties(
                System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.FlattenHierarchy
            );
            foreach (var prop in props)
            {
                writer.WriteMember(entity, prop.GetValue(entity), prop.Name);
            }
            writer.WriteEndObject();
        }
        else
        {
            writer.WriteValue(id);
        }
    }
}
