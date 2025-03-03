using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

public class EntityIdSource
{
    private readonly Dictionary<EntityId, string> _idMapping = [];
    private int _counter = 1;

    public bool GetId(string prefix, EntityId entityId, out string id)
    {
        if (!_idMapping.TryGetValue(entityId, out var eid))
        {
            id = $"${prefix}{_counter++}$";
            _idMapping[entityId] = id;
            return true;
        }
        else
        {
            id = eid;
            return false;
        }
    }

    public void Reset()
    {
        _idMapping.Clear();
        _counter = 1;
    }
}
