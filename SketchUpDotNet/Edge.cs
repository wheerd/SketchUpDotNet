using System.Drawing;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Edge : DrawingElement<SUEdgeRef>
{
    public static unsafe Edge Create(Point3D start, Point3D end)
    {
        SUEdgeRef edge;
        SUPoint3D startPoint = start.ToSU();
        SUPoint3D endPoint = end.ToSU();
        SUEdgeCreate(&edge, &startPoint, &endPoint).CheckError();
        return new(edge);
    }

    public Vertex Start => GetStart();
    public Vertex End => GetEnd();

    private unsafe Vertex GetStart() =>
        GetOne<SUVertexRef, Vertex>(&SUEdgeGetStartVertex, d => new(d));

    private unsafe Vertex GetEnd() => GetOne<SUVertexRef, Vertex>(&SUEdgeGetEndVertex, d => new(d));

    public Color Color
    {
        get => GetColor();
        set => SetColor(value);
    }

    public bool Soft
    {
        get => GetSoft();
        set => SetSoft(value);
    }

    public bool Smooth
    {
        get => GetSmooth();
        set => SetSmooth(value);
    }

    public IEnumerable<Face> Faces => GetFaces();

    private unsafe bool GetSoft() => GetBool(&SUEdgeGetSoft);

    private unsafe void SetSoft(bool value) => SetBool(&SUEdgeSetSoft, value);

    private unsafe bool GetSmooth() => GetBool(&SUEdgeGetSmooth);

    private unsafe void SetSmooth(bool value) => SetBool(&SUEdgeSetSmooth, value);

    private unsafe Face[] GetFaces() =>
        GetMany(&SUEdgeGetNumFaces, &SUEdgeGetFaces, (SUFaceRef f) => new Face(f));

    private unsafe Color GetColor() => GetColor(&SUEdgeGetColor);

    private unsafe void SetColor(Color color) => SetColor(&SUEdgeSetColor, color);

    internal unsafe Edge(SUEdgeRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUEdgeRef*, SUResult> Release => &SUEdgeRelease;

    protected sealed override unsafe delegate* <SUEdgeRef, SUEntityRef> ToEntity => &SUEdgeToEntity;

    protected sealed override unsafe delegate* <SUEdgeRef, SUDrawingElementRef> ToDrawingElement =>
        &SUEdgeToDrawingElement;
}
