//
// !!! Do not edit it manually !!!
// This is a automatically generated P/Invoke bindings
//
namespace SketchUpDotNet.Bindings;

public unsafe partial struct SUModelStatistics
{
    [NativeTypeName("int[8]")]
    public fixed int entity_counts[8];

    public enum SUEntityType
    {
        SUEntityType_Edge = 0,
        SUEntityType_Face,
        SUEntityType_ComponentInstance,
        SUEntityType_Group,
        SUEntityType_Image,
        SUEntityType_ComponentDefinition,
        SUEntityType_Layer,
        SUEntityType_Material,
        SUNumEntityTypes,
    }
}
