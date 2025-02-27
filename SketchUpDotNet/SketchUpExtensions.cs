using SketchUpDotNet.Bindings;

namespace SketchUpDotNet;

public static class SketchUpExtensions
{
    public static double GetSizeX(this SUBoundingBox3D bb) => bb.max_point.x - bb.min_point.x;

    public static double GetSizeY(this SUBoundingBox3D bb) => bb.max_point.y - bb.min_point.y;

    public static double GetSizeZ(this SUBoundingBox3D bb) => bb.max_point.z - bb.min_point.z;
}
