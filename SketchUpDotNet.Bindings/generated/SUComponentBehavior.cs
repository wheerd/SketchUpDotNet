//
// !!! Do not edit it manually !!!
// This is a automatically generated P/Invoke bindings
//
using System;

namespace SketchUpDotNet.Bindings;

public partial struct SUComponentBehavior
{
    [NativeTypeName("enum SUSnapToBehavior")]
    public SUSnapToBehavior component_snap;

    [NativeTypeName("bool")]
    public byte component_cuts_opening;

    [NativeTypeName("bool")]
    public byte component_always_face_camera;

    [NativeTypeName("bool")]
    public byte component_shadows_face_sun;

    [NativeTypeName("size_t")]
    public UIntPtr component_no_scale_mask;

    public enum SUSnapToBehavior
    {
        SUSnapToBehavior_None = 0,
        SUSnapToBehavior_Any,
        SUSnapToBehavior_Horizontal,
        SUSnapToBehavior_Vertical,
        SUSnapToBehavior_Sloped,
    }
}

public partial struct SUComponentBehavior
{
}
