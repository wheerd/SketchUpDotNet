namespace SketchUpDotNet.Tests;

[TestFixture]
public class EdgeUseTests
{
    [Test]
    public void NextAndPreviousAreLinkedList()
    {
        // Arrange
        var face = Face.CreateSimple(new(0, 0, 0), new(100, 0, 0), new(0, 100, 0));

        // Act
        var use = face.OuterLoop.EdgeUses.First();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(use.Next.Previous, Is.EqualTo(use));
            Assert.That(use.Previous.Next, Is.EqualTo(use));
        });
    }

    [Test]
    public void Snapshot()
    {
        // Arrange
        VerifySettings settings = new();
        settings.IgnoreAllButId<Face>();
        settings.IgnoreAllButId<Edge>();
        settings.IgnoreAllButId<Loop>();
        settings.IgnoreAllButId<Vertex>();

        // Unless owned by model the edge uses are freed immediately.
        using var model = new Model();
        var face = Face.CreateSimple(new(0, 0, 0), new(100, 0, 0), new(0, 100, 0));
        model.Entities.AddFace(face);

        // Act
        var result = face.OuterLoop.EdgeUses.ToList();

        // Assert
        Verify(result, settings).ToTask().Wait();
    }
}
