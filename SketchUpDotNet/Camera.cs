using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Camera : SUBase<SUCameraRef>
{
    public bool IsPerspective
    {
        get => GetPerspective();
        set => SetPerspective(value);
    }

    public unsafe void GetOrientation(out Point3D position, out Point3D target, out Vector3D up)
    {
        SUPoint3D positionSU;
        SUPoint3D targetSU;
        SUVector3D upSU;
        SUCameraGetOrientation(Reference, &positionSU, &targetSU, &upSU).CheckError();
        position = Point3D.FromSU(positionSU);
        target = Point3D.FromSU(targetSU);
        up = Vector3D.FromSU(upSU);
    }

    public unsafe void SetOrientation(Point3D position, Point3D target, Vector3D up)
    {
        SUPoint3D positionSU = position.ToSU();
        SUPoint3D targetSU = target.ToSU();
        SUVector3D upSU = up.ToSU();
        SUCameraSetOrientation(Reference, &positionSU, &targetSU, &upSU).CheckError();
    }

    public double PerspectiveFrustumFOV
    {
        get => GetPerspectiveFrustumFOV();
        set => SetPerspectiveFrustumFOV(value);
    }

    public double AspectRatio
    {
        get => GetAspectRatio();
        set => SetAspectRatio(value);
    }

    public double OrthographicFrustumHeight
    {
        get => GetOrthographicFrustumHeight();
        set => SetOrthographicFrustumHeight(value);
    }

    public bool FOVIsHeight
    {
        get => GetFOVIsHeight();
        set => SetFOVIsHeight(value);
    }

    public bool Is2D
    {
        get => Get2D();
        set => Set2D(value);
    }

    private unsafe double GetPerspectiveFrustumFOV() =>
        GetDouble(&SUCameraGetPerspectiveFrustumFOV);

    private unsafe void SetPerspectiveFrustumFOV(double value) =>
        SetDouble(&SUCameraSetPerspectiveFrustumFOV, value);

    private unsafe double GetAspectRatio() => GetDouble(&SUCameraGetAspectRatio);

    private unsafe void SetAspectRatio(double value) => SetDouble(&SUCameraSetAspectRatio, value);

    private unsafe double GetOrthographicFrustumHeight() =>
        GetDouble(&SUCameraGetOrthographicFrustumHeight);

    private unsafe void SetOrthographicFrustumHeight(double value) =>
        SetDouble(&SUCameraSetOrthographicFrustumHeight, value);

    private unsafe bool GetPerspective() => GetBool(&SUCameraGetPerspective);

    private unsafe void SetPerspective(bool value) => SetBool(&SUCameraSetPerspective, value);

    private unsafe bool GetFOVIsHeight() => GetBool(&SUCameraGetFOVIsHeight);

    private unsafe void SetFOVIsHeight(bool value) => SetBool(&SUCameraSetFOVIsHeight, value);

    private unsafe bool Get2D() => GetBool(&SUCameraGet2D);

    private unsafe void Set2D(bool value) => SetBool(&SUCameraSet2D, value);

    internal unsafe Camera(SUCameraRef @ref, bool attached)
        : base(@ref)
    {
        this.attached = attached;
    }

    protected sealed override unsafe delegate* <SUCameraRef*, SUResult> Release => &SUCameraRelease;
}
