namespace SketchUpDotNet.Tests;

public class SnapshotTest
{
    readonly VerifySettings settings = new();

    public SnapshotTest()
    {
        settings.IgnoreMembers<Vertex>(_ => _.Faces, _ => _.Edges, _ => _.Loops);
        settings.IgnoreMembers<Loop>(_ => _.Edges, _ => _.Face);
        settings.IgnoreMembers<Edge>(_ => _.Faces, _ => _.Curve);
        settings.IgnoreMembers<Component>(_ => _.Instances);
        settings.IgnoreMembers<EdgeUse>(
            _ => _.Face,
            _ => _.Edge,
            _ => _.Loop,
            _ => _.Partners,
            _ => _.Previous,
            _ => _.Next
        );
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
        using var result = Model.Load(path);

        // Assert
        return Verify(result, settings);
    }
}
