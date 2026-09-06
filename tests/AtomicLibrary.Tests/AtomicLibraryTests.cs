using AtomicLibrary.Core.Atoms;
using AtomicLibrary.Core.Electrons;
using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;
using AtomicLibrary.Physics;

namespace AtomicLibrary.Tests;

public class AtomicLibraryTests
{
    private static readonly global::AtomicLibrary.PeriodicTable.PeriodicTable PeriodicTable = ElementDataLoader.LoadDefault();
    private static readonly IsotopeRepository Isotopes = IsotopeDataLoader.LoadDefault(PeriodicTable);

    [Fact]
    public void PeriodicTable_Lookup_ByAtomicNumber_AndSymbol_Works()
    {
        var carbonByNumber = PeriodicTable.GetByAtomicNumber(6);
        var carbonBySymbol = PeriodicTable.GetBySymbol("C");

        Assert.Equal("Carbon", carbonByNumber.NameEnglish);
        Assert.Equal(carbonByNumber.AtomicNumber, carbonBySymbol.AtomicNumber);
        Assert.Equal(118, PeriodicTable.All.Count);
    }

    [Fact]
    public void Isotope_Validation_MassNumberMustBeGreaterOrEqualAtomicNumber()
    {
        var hydrogen = PeriodicTable.GetBySymbol("H");

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Isotope(hydrogen, 0, 1.0, true, null, DecayMode.None, null, null));
    }

    [Fact]
    public void Atom_Charge_And_IonFlags_AreCalculated()
    {
        var oxygen16 = Isotopes.GetByAtomicAndMassNumber(8, 16);

        var neutral = new Atom(oxygen16, 8);
        var cation = new Atom(oxygen16, 6);
        var anion = new Atom(oxygen16, 10);

        Assert.True(neutral.IsNeutral);
        Assert.Equal(0, neutral.Charge);

        Assert.True(cation.IsCation);
        Assert.True(cation.IsIon);
        Assert.Equal(2, cation.Charge);

        Assert.True(anion.IsAnion);
        Assert.True(anion.IsIon);
        Assert.Equal(-2, anion.Charge);
    }

    [Fact]
    public void ElectronConfiguration_ToString_Format_IsExpected()
    {
        var configuration = new ElectronConfiguration(new[]
        {
            new Orbital(1, OrbitalType.S, 2),
            new Orbital(2, OrbitalType.S, 2),
            new Orbital(2, OrbitalType.P, 6)
        });

        Assert.Equal("1s2 2s2 2p6", configuration.ToString());
    }

    [Fact]
    public void ElectronConfiguration_Parse_And_FromAtomicNumber_Work()
    {
        var parsed = ElectronConfiguration.Parse("1s2 2s2 2p6");
        var fromNumber = ElectronConfiguration.FromAtomicNumber(10);

        Assert.Equal("1s2 2s2 2p6", parsed.ToString());
        Assert.Equal(10, fromNumber.ElectronCount);
        Assert.Throws<ArgumentOutOfRangeException>(() => ElectronConfiguration.FromAtomicNumber(0));
    }

    [Fact]
    public void ElectronConfiguration_Parse_Throws_ForUnsupportedFormats()
    {
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("[Ar] 4s2 3d10"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("2x6"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("1s2 invalid 2p6"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("1s3"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("1s2x"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("1sp2"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("1s0"));
        Assert.Throws<FormatException>(() => ElectronConfiguration.Parse("0s2"));
    }

    [Fact]
    public void IsotopeDataLoader_InvalidDecayMode_IncludesIsotopeContext()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, """[{ "AtomicNumber": 6, "MassNumber": 14, "AtomicMass": 14.0032419884, "IsStable": false, "HalfLifeSeconds": 1.0, "DecayMode": "InvalidMode" }]""");

        var exception = Assert.Throws<FormatException>(() => IsotopeDataLoader.LoadFromJson(path, PeriodicTable));
        Assert.Contains("Z=6, A=14", exception.Message);
    }

    [Fact]
    public void ElementDataLoader_InvalidEnums_IncludeElementContext()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, """[{ "AtomicNumber": 999, "Symbol": "Xx", "NameEnglish": "Test", "NameGerman": "Test", "Period": 1, "Group": 1, "Block": "invalid", "Category": "invalid", "ElectronConfiguration": "1s1" }]""");

        var exception = Assert.Throws<FormatException>(() => ElementDataLoader.LoadFromJson(path));
        Assert.Contains("Z=999 (Xx)", exception.Message);
    }

    [Fact]
    public void IsotopeDataLoader_LoadDecayChains_InvalidDecayMode_IncludesContext()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, """[{ "ParentAtomicNumber": 92, "ParentMassNumber": 238, "Steps": [{ "AtomicNumber": 90, "MassNumber": 234, "DecayMode": "InvalidMode" }] }]""");

        var exception = Assert.Throws<FormatException>(() => IsotopeDataLoader.LoadDecayChains(path));
        Assert.Contains("parent Z=92, A=238, step Z=90, A=234", exception.Message);
    }

    [Fact]
    public void RadioactiveDecayCalculator_UsesExpectedValues_ForCarbon14()
    {
        const double yearInSeconds = 365.2425 * 24 * 3600;
        var halfLife = 5730 * yearInSeconds;

        var decayConstant = RadioactiveDecayCalculator.DecayConstant(halfLife);
        var remainingHalfLife = RadioactiveDecayCalculator.RemainingFraction(halfLife, halfLife);
        var activity = RadioactiveDecayCalculator.Activity(1e20, halfLife);

        Assert.InRange(decayConstant, 3.8e-12, 3.9e-12);
        Assert.InRange(remainingHalfLife, 0.4999, 0.5001);
        Assert.InRange(activity, 3.8e8, 3.9e8);
    }

    [Fact]
    public void RadioactiveDecayCalculator_Guards_InvalidArguments()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => RadioactiveDecayCalculator.DecayConstant(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => RadioactiveDecayCalculator.RemainingFraction(10, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => RadioactiveDecayCalculator.RemainingAtoms(-1, 10, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => RadioactiveDecayCalculator.Activity(-1, 10));
    }

    [Fact]
    public void NuclearBindingEnergyCalculator_Fe56_HasHighBindingPerNucleon()
    {
        var fe56 = Isotopes.GetByAtomicAndMassNumber(26, 56);
        var bindingPerNucleon = NuclearBindingEnergyCalculator.BindingEnergyPerNucleonMeV(fe56);

        Assert.InRange(bindingPerNucleon, 8.0, 9.5);
    }
}
