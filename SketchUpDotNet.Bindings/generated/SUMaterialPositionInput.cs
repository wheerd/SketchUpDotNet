//
// !!! Do not edit it manually !!!
// This is a automatically generated P/Invoke bindings
//
using System;

namespace SketchUpDotNet.Bindings;

public partial struct SUMaterialPositionInput
{
    [NativeTypeName("size_t")]
    public UIntPtr num_uv_coords;

    [NativeTypeName("struct SUPoint2D[4]")]
    public _uv_coords_e__FixedBuffer uv_coords;

    [NativeTypeName("struct SUPoint3D[4]")]
    public _points_e__FixedBuffer points;

    public SUMaterialRef material;

    [NativeTypeName("struct SUVector3D")]
    public SUVector3D projection;

    public partial struct _uv_coords_e__FixedBuffer
    {
        public SUPoint2D e0;
        public SUPoint2D e1;
        public SUPoint2D e2;
        public SUPoint2D e3;

        public unsafe ref SUPoint2D this[int index]
        {
            get
            {
                fixed (SUPoint2D* pThis = &e0)
                {
                    return ref pThis[index];
                }
            }
        }
    }

    public partial struct _points_e__FixedBuffer
    {
        public SUPoint3D e0;
        public SUPoint3D e1;
        public SUPoint3D e2;
        public SUPoint3D e3;

        public unsafe ref SUPoint3D this[int index]
        {
            get
            {
                fixed (SUPoint3D* pThis = &e0)
                {
                    return ref pThis[index];
                }
            }
        }
    }
}
