//
// !!! Do not edit it manually !!!
// This is a automatically generated P/Invoke bindings
//
using System;

namespace SketchUpDotNet.Bindings;

public unsafe partial struct SUExtensionLicense
{
    [NativeTypeName("bool")]
    public byte is_licensed;

    [NativeTypeName("enum SUExtensionLicenseState")]
    public SUExtensionLicenseState state;

    [NativeTypeName("size_t")]
    public UIntPtr days_remaining;

    [NativeTypeName("char[512]")]
    public fixed sbyte error_description[512];
}
