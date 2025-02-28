using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public abstract class Curve<T> : Entity<T>, ICurve
    where T : unmanaged
{
    public unsafe SUCurveType Type => CurveRef.Get<SUCurveRef, SUCurveType>(&SUCurveGetType);

    public unsafe bool IsPolygon => CurveRef.GetBool(&SUCurveIsPolygon);

    public unsafe IEnumerable<Edge> Edges =>
        CurveRef.GetMany(&SUCurveGetNumEdges, &SUCurveGetEdges, (SUEdgeRef e) => new Edge(e), true);
    public unsafe int EdgeCount => CurveRef.GetInt(&SUCurveGetNumEdges);

    internal unsafe Curve(T @ref)
        : base(@ref) { }

    private SUCurveRef? curveRef;

    protected abstract unsafe delegate* <T, SUCurveRef> ToCurve { get; }

    private unsafe SUCurveRef CurveRef => curveRef ??= ToCurve(Reference);
}

public interface ICurve : IEntity
{
    internal static unsafe ICurve Create(SUCurveRef @ref)
    {
        return @ref.Get<SUCurveRef, SUCurveType>(&SUCurveGetType) switch
        {
            SUCurveType.SUCurveType_Arc => new ArcCurve(SUArcCurveFromCurve(@ref)),
            _ => new SimpleCurve(@ref),
        };
    }

    public unsafe SUCurveType Type { get; }

    public unsafe bool IsPolygon { get; }

    public unsafe IEnumerable<Edge> Edges { get; }
    public unsafe int EdgeCount { get; }
}
