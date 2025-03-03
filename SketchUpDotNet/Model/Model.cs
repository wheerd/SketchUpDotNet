using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet.Model;

public class SketchUpModel : SUBase<SUModelRef>
{
    public unsafe SketchUpModel()
        : base(&SUModelCreate)
    {
        Entities = GetEntities();
    }

    public static unsafe SketchUpModel Load(string path)
    {
        var pathBytes = path.GetSBytes();
        SUModelRef model;
        SUModelLoadStatus status;
        fixed (sbyte* pathPtr = &pathBytes[0])
        {
            SUModelCreateFromFileWithStatus(&model, pathPtr, &status).CheckError();
        }
        return new SketchUpModel(model);
    }

    public Entities Entities { get; private init; }

    public SUModelUnits Units => GetUnits();

    public LengthFormatter LengthFormatter => GetLengthFormatter();

    public OptionsManager Options => GetOptionsManager();

    public RenderingOptions RenderingOptions => GetRenderingOptions();

    private unsafe RenderingOptions GetRenderingOptions() =>
        GetOne(&SUModelGetRenderingOptions, (SURenderingOptionsRef o) => new RenderingOptions(o));

    public Camera Camera
    {
        get => GetCamera();
        set => SetCamera(value);
    }

    public IEnumerable<Component> Components => GetComponents();
    public IEnumerable<Layer> Layers => GetLayers();
    public IEnumerable<Material> Materials => GetMaterials();

    public unsafe void AddComponents(params Component[] components) =>
        AddMany(&SUModelAddComponentDefinitions, components);

    public unsafe void AddLayers(params Layer[] layers) => AddMany(&SUModelAddLayers, layers);

    public unsafe void AddMaterials(params Material[] materials) =>
        AddMany(&SUModelAddMaterials, materials);

    public IEnumerable<AttributeDictionary> AttributeDictionaries => GetAttributeDictionaries();

    public unsafe AttributeDictionary GetAttributeDictionary(string name)
    {
        var bytes = name.GetSBytes();
        SUAttributeDictionaryRef dict;
        fixed (sbyte* bytesPtr = &bytes[0])
        {
            SUModelGetAttributeDictionary(Reference, bytesPtr, &dict).CheckError();
        }

        var ad = new AttributeDictionary(dict);
        ad.SetAttachedToModel(true);
        return ad;
    }

    public unsafe void FixErrors() => SUModelFixErrors(Reference).CheckError();

    public unsafe void MergeCoplanarFaces() => SUModelMergeCoplanarFaces(Reference).CheckError();

    public unsafe void OrientFacesConsistently(bool recurse)
    {
        byte byteValue = *(byte*)&recurse;
        SUModelOrientFacesConsistently(Reference, byteValue).CheckError();
    }

    public unsafe void Save(
        string path,
        SUModelVersion version = SUModelVersion.SUModelVersion_SU2017
    )
    {
        var pathBytes = path.GetSBytes();
        fixed (sbyte* pathPtr = pathBytes)
        {
            SUModelSaveToFileWithVersion(Reference, pathPtr, version);
        }
    }

    private unsafe AttributeDictionary[] GetAttributeDictionaries() =>
        GetMany(
            &SUModelGetNumAttributeDictionaries,
            &SUModelGetAttributeDictionaries,
            (SUAttributeDictionaryRef e) => new AttributeDictionary(e)
        );

    private unsafe Component[] GetComponents() =>
        GetMany(
            &SUModelGetNumComponentDefinitions,
            &SUModelGetComponentDefinitions,
            (SUComponentDefinitionRef d) => new Component(d)
        );

    private unsafe Layer[] GetLayers() =>
        GetMany(&SUModelGetNumLayers, &SUModelGetLayers, (SULayerRef l) => new Layer(l));

    private unsafe Material[] GetMaterials() =>
        GetMany(
            &SUModelGetNumMaterials,
            &SUModelGetMaterials,
            (SUMaterialRef m) => new Material(m)
        );

    private unsafe Entities GetEntities() =>
        GetOne(&SUModelGetEntities, (SUEntitiesRef e) => new Entities(e, true));

    private unsafe SUModelUnits GetUnits() => Get<SUModelUnits>(&SUModelGetUnits);

    private unsafe OptionsManager GetOptionsManager() =>
        GetOne(&SUModelGetOptionsManager, (SUOptionsManagerRef m) => new OptionsManager(m));

    private unsafe LengthFormatter GetLengthFormatter()
    {
        var formatter = new LengthFormatter();
        fixed (SULengthFormatterRef* ptr = &formatter.Reference)
            SUModelGetLengthFormatter(Reference, ptr).CheckError();
        return formatter;
    }

    private unsafe Camera GetCamera() =>
        GetOne(&SUModelGetCamera, (SUCameraRef c) => new Camera(c, true));

    private unsafe void SetCamera(Camera camera)
    {
        fixed (SUCameraRef* cameraRef = &camera.Reference)
        {
            SUModelSetCamera(Reference, cameraRef).CheckError();
        }
    }

    internal unsafe SketchUpModel(SUModelRef @ref)
        : base(@ref, true)
    {
        Entities = GetEntities();
    }

    protected sealed override unsafe delegate* <SUModelRef*, SUResult> Release => &SUModelRelease;
}
