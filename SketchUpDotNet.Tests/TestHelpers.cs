using System.Runtime.CompilerServices;

namespace SketchUpDotNet.Tests;

public static class TestHelpers
{
    public static void IgnoreAllButId<T>(this VerifySettings settings)
        where T : IEntity
    {
        settings.AddExtraSettings(_ => _.Converters.Add(new EntityWithOnlyIdConverter<T>()));
    }

    [ModuleInitializer]
    public static void Init()
    {
        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new EntityIdConverter()));
    }
}
