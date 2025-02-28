namespace SketchUpDotNet.Tests;

public class SnapshotTest
{
    readonly VerifySettings settings = new();

    public SnapshotTest()
    {
        settings.IgnoreMembers<Vertex>(_ => _.Faces, _ => _.Edges, _ => _.Loops);
        settings.IgnoreMembers<Loop>(_ => _.Edges, _ => _.Face);
        settings.IgnoreMembers<Edge>(_ => _.Faces);
        settings.IgnoreMembers<Component>(_ => _.Instances);
    }

    [Test]
    public Task Empty_Snapshot()
    {
        // Arrange
        string path = Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            @"TestFiles\Empty.skp"
        );

        // Act
        var result = Model.Load(path);

        // Assert
        return Verify(result, settings);
    }
}
