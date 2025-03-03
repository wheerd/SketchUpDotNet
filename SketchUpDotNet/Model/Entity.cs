using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public readonly record struct EntityId(int Value);

public abstract class Entity<T> : SUBase<T>, IEntity
    where T : unmanaged
{
    public EntityId Id => GetId();

    public IEnumerable<AttributeDictionary> AttributeDictionaries => GetAttributeDictionaries();

    public unsafe AttributeDictionary GetAttributeDictionary(string name) =>
        EntityRef.GetOneByKey(
            &SUEntityGetAttributeDictionary,
            (SUAttributeDictionaryRef d) => new AttributeDictionary(d),
            attached,
            name
        );

    public unsafe void AddAttributeDictionary(AttributeDictionary dict)
    {
        SUEntityAddAttributeDictionary(EntityRef, dict.Reference).CheckError();
        if (attached)
            dict.SetAttachedToModel(true);
    }

    internal Entity(T @ref)
        : base(@ref) { }

    internal Entity(T @ref, bool attached)
        : base(@ref, attached) { }

    internal unsafe Entity(delegate* <T*, SUResult> factory)
        : base(factory) { }

    private SUEntityRef? entityRef;

    protected abstract unsafe delegate* <T, SUEntityRef> ToEntity { get; }

    private unsafe SUEntityRef EntityRef => entityRef ??= ToEntity(Reference);

    SUEntityRef IEntity.EntityRef => EntityRef;

    private unsafe EntityId GetId() => new(EntityRef.GetInt(&SUEntityGetID));

    private unsafe AttributeDictionary[] GetAttributeDictionaries() =>
        EntityRef.GetMany(
            &SUEntityGetNumAttributeDictionaries,
            &SUEntityGetAttributeDictionaries,
            (SUAttributeDictionaryRef d) => new AttributeDictionary(d),
            attached
        );
}

public interface IEntity : IBase
{
    public EntityId Id { get; }

    internal unsafe SUEntityRef EntityRef { get; }
}
