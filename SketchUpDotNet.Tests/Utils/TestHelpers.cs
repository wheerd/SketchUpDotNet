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
                new EntityConverter<EdgeUse>(idSource),
                new EntityConverter<Vertex>(idSource),
                new EntityConverter<Group>(idSource),
                new EntityConverter<ComponentInstance>(idSource),
                new EntityConverter<ArcCurve>(idSource),
                new EntityConverter<SimpleCurve>(idSource),
                new EntityConverter<DimensionLinear>(idSource),
                new EntityConverter<DimensionRadial>(idSource),
                new EntityConverter<Font>(idSource),
                new EntityConverter<Axes>(idSource),
                new EntityConverter<GuidePoint>(idSource),
                new EntityConverter<GuideLine>(idSource),
                new EntityConverter<Polyline3D>(idSource),
                new EntityConverter<SectionPlane>(idSource),
                new EntityConverter<Text>(idSource),
                new EntityConverter<Image>(idSource)
            )
        );
        settings.IgnoreMembers<IEntity>(e => e.Model, e => e.PersistentId, e => e.ParentEntities);
        settings.IgnoreMembers<Entities>(e => e.Parent);
        return idSource;
    }
}
