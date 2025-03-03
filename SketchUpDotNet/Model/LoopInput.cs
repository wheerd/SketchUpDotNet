using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class LoopInput : SUBase<SULoopInputRef>
{
    private readonly nuint edgeCount;

    public int[] Indices { get; init; }

    public unsafe LoopInput(params int[] indices)
        : base(&SULoopInputCreate)
    {
        Indices = indices;

        for (var i = 0; i < indices.Length; i++)
        {
            SULoopInputAddVertexIndex(Reference, (nuint)indices[i]).CheckError();
        }

        bool isClosed;
        SULoopInputIsClosed(Reference, &isClosed).CheckError();

        edgeCount = (nuint)(indices.Length - (isClosed ? 1 : 0));
    }

    public void SetLayer(Layer layer)
    {
        for (nuint i = 0; i < edgeCount; i++)
        {
            SULoopInputEdgeSetLayer(Reference, i, layer.Reference).CheckError();
        }
    }

    protected sealed override unsafe delegate* <SULoopInputRef*, SUResult> Release =>
        &SULoopInputRelease;
}
