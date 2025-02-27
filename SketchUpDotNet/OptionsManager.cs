using System.Collections;
using SketchUpDotNet.Bindings;
using static SketchUpDotNet.Bindings.Methods;

namespace SketchUpDotNet;

public class OptionsManager : SUBase<SUOptionsManagerRef>, IEnumerable<OptionsProvider>
{
    internal OptionsManager(SUOptionsManagerRef @ref)
        : base(@ref) { }

    public IEnumerable<string> OptionsProviderNames => GetOptionsProviderNames();

    public OptionsProvider this[string key] => GetOptionsProviderByName(key);

    public unsafe OptionsProvider GetOptionsProviderByName(string name)
    {
        var sbytes = name.GetSBytes();
        SUOptionsProviderRef providerRef;
        fixed (sbyte* sbytesPtr = &sbytes[0])
        {
            SUOptionsManagerGetOptionsProviderByName(Reference, sbytesPtr, &providerRef)
                .CheckError();
        }
        return new OptionsProvider(providerRef);
    }

    public IEnumerator<OptionsProvider> GetEnumerator()
    {
        foreach (var name in GetOptionsProviderNames())
        {
            yield return GetOptionsProviderByName(name);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private unsafe string[] GetOptionsProviderNames() =>
        GetStrings(
            &SUOptionsManagerGetNumOptionsProviders,
            &SUOptionsManagerGetOptionsProviderNames
        );

    protected sealed override unsafe delegate* <SUOptionsManagerRef*, SUResult> Release => null;
}
