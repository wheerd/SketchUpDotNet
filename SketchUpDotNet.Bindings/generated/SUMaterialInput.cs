//
// !!! Do not edit it manually !!!
// This is a automatically generated P/Invoke bindings
//
using System;

namespace SketchUpDotNet.Bindings;

public partial struct SUMaterialInput
{
    [NativeTypeName("size_t")]
    public UIntPtr num_uv_coords;

    [NativeTypeName("struct SUPoint2D[4]")]
    public _uv_coords_e__FixedBuffer uv_coords;

    [NativeTypeName("size_t[4]")]
    public _vertex_indices_e__FixedBuffer vertex_indices;

    public SUMaterialRef material;

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

    public partial struct _vertex_indices_e__FixedBuffer
    {
        public UIntPtr e0;
        public UIntPtr e1;
        public UIntPtr e2;
        public UIntPtr e3;

        public unsafe ref UIntPtr this[int index]
        {
            get
            {
                fixed (UIntPtr* pThis = &e0)
                {
                    return ref pThis[index];
                }
            }
        }
    }
}
