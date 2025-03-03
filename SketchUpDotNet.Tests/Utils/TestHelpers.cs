using SketchUpDotNet.Model;

namespace SketchUpDotNet.Tests.Utils;

public static class TestHelpers
{
    public static void IgnoreAllButId<T>(this VerifySettings settings)
        where T : IEntity
    {
        settings.AddExtraSettings(_ =>
        {
            _.Converters.RemoveAll(c => c.CanConvert(typeof(T)));
            _.Converters.Add(new EntityWithOnlyIdConverter<T>());
        });
    }

    public static EntityIdSource EntityIdHandling(this VerifySettings settings)
    {
        var idSource = new EntityIdSource();
        settings.AddExtraSettings(_ =>
            _.Converters.AddRange(
                new EntityIdConverter(idSource),
                new EntityConverter<Face>(idSource),
                new EntityConverter<Loop>(idSource),
                new EntityConverter<Edge>(idSource),
                new EntityConverter<Vertex>(idSource),
                new EntityConverter<Group>(idSource),
                new EntityConverter<ComponentInstance>(idSource),
                new EntityConverter<ArcCurve>(idSource),
                new EntityConverter<SimpleCurve>(idSource),
                new EntityConverter<DimensionLinear>(idSource),
                new EntityConverter<DimensionRadial>(idSource),
                new EntityConverter<Font>(idSource)
            )
        );
        return idSource;
    }
}
