using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

class EntityIdConverter(EntityIdSource idSource) : WriteOnlyJsonConverter<EntityId>
{
    public override void Write(VerifyJsonWriter writer, EntityId entityId)
    {
        idSource.GetId("Entity", entityId, out var id);
        writer.WriteValue(id);
    }
}
