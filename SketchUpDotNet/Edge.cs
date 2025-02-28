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

    public unsafe Vertex Start => GetOne<SUVertexRef, Vertex>(&SUEdgeGetStartVertex, d => new(d));
    public unsafe Vertex End => GetOne<SUVertexRef, Vertex>(&SUEdgeGetEndVertex, d => new(d));

    public unsafe ICurve? Curve => GetCurve();

    public unsafe Color? Color
    {
        get => GetOptionalColor(&SUEdgeGetColor);
        set => SetColor(&SUEdgeSetColor, value ?? throw new ArgumentNullException(nameof(Color)));
    }

    public unsafe bool Soft
    {
        get => GetBool(&SUEdgeGetSoft);
        set => SetBool(&SUEdgeSetSoft, value);
    }

    public unsafe bool Smooth
    {
        get => GetBool(&SUEdgeGetSmooth);
        set => SetBool(&SUEdgeSetSmooth, value);
    }

    public unsafe IEnumerable<Face> Faces =>
        GetMany(&SUEdgeGetNumFaces, &SUEdgeGetFaces, (SUFaceRef f) => new Face(f));
    public unsafe int FaceCount => GetInt(&SUEdgeGetNumFaces);

    public unsafe double LengthWithTransform(SUTransformation transformation)
    {
        double value;
        SUEdgeGetLengthWithTransform(Reference, &transformation, &value).CheckError();
        return value;
    }

    public unsafe bool IsReversedInFace(Face face)
    {
        bool value;
        SUEdgeReversedInFace(Reference, face.Reference, &value).CheckError();
        return value;
    }

    private unsafe ICurve? GetCurve()
    {
        SUCurveRef curveRef;
        var result = SUEdgeGetCurve(Reference.EnsureReferenceValid(), &curveRef);
        if (result == SUResult.SU_ERROR_NO_DATA)
            return null;
        result.CheckError();
        var curve = ICurve.Create(curveRef);
        if (attached)
        {
            curve.SetAttachedToModel(true);
        }
        return curve;
    }

    internal unsafe Edge(SUEdgeRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUEdgeRef*, SUResult> Release => &SUEdgeRelease;

    protected sealed override unsafe delegate* <SUEdgeRef, SUEntityRef> ToEntity => &SUEdgeToEntity;

    protected sealed override unsafe delegate* <SUEdgeRef, SUDrawingElementRef> ToDrawingElement =>
        &SUEdgeToDrawingElement;
}
