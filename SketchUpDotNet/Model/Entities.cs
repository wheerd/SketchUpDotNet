using SketchUpDotNet.Bindings;
using SketchUpDotNet.Geometry;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class Entities : SUBase<SUEntitiesRef>
{
    public unsafe BoundingBox BoundingBox => new(Get<SUBoundingBox3D>(&SUEntitiesGetBoundingBox));

    public unsafe IEnumerable<Face> Faces =>
        GetMany(&SUEntitiesGetNumFaces, &SUEntitiesGetFaces, static (SUFaceRef e) => new Face(e));
    public unsafe int FaceCount => GetInt(&SUEntitiesGetNumFaces);

    public unsafe void AddFaces(params Face[] faces) => AddMany(&SUEntitiesAddFaces, faces);

    public unsafe IEnumerable<Edge> Edges => GetEdges();
    public unsafe IEnumerable<Edge> StandaloneEdges => GetEdges(true);
    public unsafe int EdgeCount => GetEdgeCount();
    public unsafe int StandaloneEdgeCount => GetEdgeCount(true);

    public unsafe void AddEdges(params Edge[] edges) => AddMany(&SUEntitiesAddEdges, edges);

    public unsafe IEnumerable<Group> Groups =>
        GetMany(
            &SUEntitiesGetNumGroups,
            &SUEntitiesGetGroups,
            static (SUGroupRef g) => new Group(g)
        );
    public unsafe int GroupCount => GetInt(&SUEntitiesGetNumGroups);

    public unsafe void AddGroup(Group group) =>
        SetOne<SUGroupRef, Group>(&SUEntitiesAddGroup, group);

    public unsafe IEnumerable<ComponentInstance> Instances =>
        GetMany(
            &SUEntitiesGetNumInstances,
            &SUEntitiesGetInstances,
            (SUComponentInstanceRef i) => new ComponentInstance(i, attached)
        );
    public unsafe int InstanceCount => GetInt(&SUEntitiesGetNumInstances);

    public unsafe void AddInstances(ComponentInstance instance)
    {
        SUEntitiesAddInstance(Reference, instance.Reference, null).CheckError();
        if (attached)
        {
            instance.SetAttachedToModel(true);
        }
    }

    public unsafe IEnumerable<SimpleCurve> Curves =>
        GetMany(
            &SUEntitiesGetNumCurves,
            &SUEntitiesGetCurves,
            static (SUCurveRef e) => new SimpleCurve(e)
        );
    public unsafe int CurveCount => GetInt(&SUEntitiesGetNumCurves);

    public unsafe void AddCurves(params SimpleCurve[] curves) =>
        AddMany(&SUEntitiesAddCurves, curves);

    public unsafe IEnumerable<ArcCurve> ArcCurves =>
        GetMany(
            &SUEntitiesGetNumArcCurves,
            &SUEntitiesGetArcCurves,
            static (SUArcCurveRef e) => new ArcCurve(e)
        );
    public unsafe int ArcCurveCount => GetInt(&SUEntitiesGetNumArcCurves);

    public unsafe void AddArcCurves(params ArcCurve[] curves) =>
        AddMany(&SUEntitiesAddArcCurves, curves);

    public unsafe IEnumerable<IDimension> Dimensions =>
        GetMany(
            &SUEntitiesGetNumDimensions,
            &SUEntitiesGetDimensions,
            (SUDimensionRef d) => IDimension.Create(d)
        );

    public void Clear()
    {
        SUEntitiesClear(Reference).CheckError();
    }

    public unsafe void Remove(params IEntity[] entities)
    {
        var refs = entities.Select(e => e.EntityRef).ToArray();
        fixed (SUEntityRef* refsPtr = refs)
        {
            SUEntitiesErase(Reference, (nuint)refs.Length, refsPtr).CheckError();
        }
    }

    public unsafe void Fill(GeometryInput input, bool weldVertices)
    {
        byte weldByte = *(byte*)&weldVertices;
        SUEntitiesFill(Reference, input.Reference, weldByte).CheckError();
        if (attached)
            input.SetAttachedToModel(true);
    }

    internal Entities(SUEntitiesRef @ref, bool attached)
        : base(@ref)
    {
        this.attached = attached;
    }

    private unsafe Edge[] GetEdges(bool standaloneOnly = false)
    {
        nuint num;
        byte standaloneByte = *(byte*)&standaloneOnly;
        SUEntitiesGetNumEdges(Reference, standaloneByte, &num).CheckError();
        if (num == 0)
            return [];
        SUEdgeRef[] refs = new SUEdgeRef[num];
        nuint count;
        fixed (SUEdgeRef* refsPtr = &refs[0])
        {
            SUEntitiesGetEdges(Reference, standaloneByte, num, refsPtr, &count).CheckError();
        }
        var results = new Edge[count];
        for (nuint i = 0; i < count; i++)
        {
            results[i] = new Edge(refs[i]);
            if (attached)
            {
                results[i].SetAttachedToModel(true);
            }
        }
        return results;
    }

    private unsafe int GetEdgeCount(bool standaloneOnly = false)
    {
        nuint num;
        byte standaloneByte = *(byte*)&standaloneOnly;
        SUEntitiesGetNumEdges(Reference, standaloneByte, &num).CheckError();
        return (int)num;
    }

    protected sealed override unsafe delegate* <SUEntitiesRef*, SUResult> Release => null;
}
