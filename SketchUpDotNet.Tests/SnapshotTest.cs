using SketchUpDotNet.Model;
using SketchUpDotNet.Tests.Utils;

namespace SketchUpDotNet.Tests;

public class SnapshotTest
{
    readonly VerifySettings settings = new();

    private readonly EntityIdSource idSource;

    public SnapshotTest()
    {
        idSource = settings.EntityIdHandling();
        settings.AddExtraSettings(_ =>
            _.Converters.AddRange(
                new LayerNameConverter(),
                new MaterialNameConverter(),
                new VertexConverter()
            )
        );
        settings.IgnoreMembers<Component>(_ => _.Instances);
        settings.IgnoreMembers<ComponentInstance>(_ => _.Definition);
        settings.IgnoreMembers<EdgeUse>(_ => _.Partners);
    }

    [SetUp]
    public void Setup()
    {
        idSource.Reset();
    }

    [Test]
    public async Task Empty_Snapshot()
    {
        // Arrange
        string path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            @"TestFiles\Empty.skp"
        );

        // Act
        using var result = SketchUpModel.Load(path);

        // Assert
        await Verify(result, settings).DisableDiff();
    }
}
