using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class InstancePath : SUBase<SUInstancePathRef>
{
    internal unsafe InstancePath(SUInstancePathRef @ref)
        : base(@ref) { }

    protected sealed override unsafe delegate* <SUInstancePathRef*, SUResult> Release =>
        &SUInstancePathRelease;
}
