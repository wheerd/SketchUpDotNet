using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class Entities : SUBase<SUEntitiesRef>
{
    public BoundingBox BoundingBox => GetBoundingBox();

    public IEnumerable<Face> Faces => GetFaces();

    public unsafe void AddFace(params Face[] faces) => AddMany(&SUEntitiesAddFaces, faces);

    public IEnumerable<Edge> Edges => GetEdges();
    public IEnumerable<Edge> StandaloneEdges => GetEdges(true);

    public unsafe void AddEdge(params Edge[] edges) => AddMany(&SUEntitiesAddEdges, edges);

    public IEnumerable<Group> Groups => GetGroups();

    public unsafe void AddGroup(Group group)
    {
        SUEntitiesAddGroup(Reference, group.Reference).CheckError();
        if (attached)
        {
            group.SetAttachedToModel(true);
        }
    }

    public IEnumerable<ComponentInstance> Instances => GetInstances();

    public unsafe void AddInstance(ComponentInstance instance)
    {
        SUEntitiesAddInstance(Reference, instance.Reference, null).CheckError();
        if (attached)
        {
            instance.SetAttachedToModel(true);
        }
    }

    public void Clear()
    {
        SUEntitiesClear(Reference).CheckError();
    }

    public unsafe void Remove(params IEntity[] entities)
    {
        var refs = entities.Select(e => e.EntityRef).ToArray();
        fixed (SUEntityRef* refsPtr = &refs[0])
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

    private unsafe BoundingBox GetBoundingBox() =>
        BoundingBox.FromSU(Get<SUBoundingBox3D>(&SUEntitiesGetBoundingBox));

    private unsafe Face[] GetFaces() =>
        GetMany(&SUEntitiesGetNumFaces, &SUEntitiesGetFaces, static (SUFaceRef e) => new Face(e));

    private unsafe Group[] GetGroups() =>
        GetMany(
            &SUEntitiesGetNumGroups,
            &SUEntitiesGetGroups,
            static (SUGroupRef g) => new Group(g)
        );

    private unsafe ComponentInstance[] GetInstances() =>
        GetMany(
            &SUEntitiesGetNumInstances,
            &SUEntitiesGetInstances,
            (SUComponentInstanceRef i) => new ComponentInstance(i, attached)
        );

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

    protected sealed override unsafe delegate* <SUEntitiesRef*, SUResult> Release => null;
}
