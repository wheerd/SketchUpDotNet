using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Geometry;

public sealed class Plane3D
{
    public static unsafe Plane3D FromPoints(Point3D point1, Point3D point2, Point3D point3)
    {
        SUPoint3D p1 = point1.ToSU();
        SUPoint3D p2 = point2.ToSU();
        SUPoint3D p3 = point3.ToSU();
        SUPlane3D su;
        SUPlane3DSetWithPoints(&su, &p1, &p2, &p3).CheckError();
        return new(su);
    }

    public static unsafe Plane3D FromPointAndNormal(Point3D point, Vector3D normal)
    {
        SUPoint3D p = point.ToSU();
        SUVector3D n = normal.ToSU();
        SUPlane3D su;
        SUPlane3DSetWithPointAndNormal(&su, &p, &n).CheckError();
        return new(su);
    }

    public static unsafe Plane3D FromCoefficients(double a, double b, double c, double d)
    {
        SUPlane3D su;
        SUPlane3DSetWithCoefficients(
                &su,
                a.ToSULength(),
                b.ToSULength(),
                c.ToSULength(),
                d.ToSULength()
            )
            .CheckError();
        return new(su);
    }

    public double A => _su.a.FromSULength();
    public double B => _su.b.FromSULength();
    public double C => _su.c.FromSULength();
    public double D => _su.d.FromSULength();

    public unsafe Point3D Position
    {
        get
        {
            SUPoint3D origin;
            fixed (SUPlane3D* su = &_su)
                SUPlane3DGetPosition(su, &origin).CheckError();
            return new(origin);
        }
    }

    public unsafe Vector3D Normal
    {
        get
        {
            SUVector3D normal;
            fixed (SUPlane3D* su = &_su)
                SUPlane3DGetNormal(su, &normal).CheckError();
            return new(normal);
        }
    }

    public unsafe Plane3D Transform(Transform transform)
    {
        SUTransformation t = transform.ToSU();
        SUPlane3D su = _su;
        SUPlane3DTransform(&t, &su).CheckError();
        return new(su);
    }

    public unsafe bool IsOn(Point3D point)
    {
        bool on;
        SUPoint3D pointSU = point.ToSU();
        fixed (SUPlane3D* su = &_su)
            SUPlane3DIsOn(su, &pointSU, &on).CheckError();
        return on;
    }

    public unsafe double Distance(Point3D point)
    {
        double dist;
        SUPoint3D pointSU = point.ToSU();
        fixed (SUPlane3D* su = &_su)
            SUPlane3DDistanceTo(su, &pointSU, &dist).CheckError();
        return dist.FromSULength();
    }

    public unsafe Point3D Project(Point3D point)
    {
        SUPoint3D projected;
        SUPoint3D pointSU = point.ToSU();
        fixed (SUPlane3D* su = &_su)
            SUPlane3DProjectTo(su, &pointSU, &projected).CheckError();
        return new(projected);
    }

    private readonly SUPlane3D _su;

    internal Plane3D(SUPlane3D su)
    {
        _su = su;
    }

    internal SUPlane3D ToSU() => _su;
}
