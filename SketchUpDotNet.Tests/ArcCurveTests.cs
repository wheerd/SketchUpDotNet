namespace SketchUpDotNet.Tests;

[TestFixture]
public class ArcCurveTests
{
    [Test]
    public async Task Snapshot()
    {
        // Arrange
        VerifySettings settings = new();
        settings.IgnoreMembers<Edge>(_ => _.Curve, _ => _.Faces);
        settings.IgnoreMembers<Vertex>(_ => _.Edges, _ => _.Loops, _ => _.Faces);
        settings.IgnoreMembersWithType<BoundingBox>();
        settings.IgnoreMembersWithType<Layer>();
        using var model = new Model();

        // Act
        var curve = ArcCurve.Create(new(0, 0, 0), new(100, 0, 0), new(100, 0, 0), new(0, 0, 1), 5);
        model.Entities.AddArcCurves(curve);

        // Assert
        await Verify(curve, settings);
    }
}
