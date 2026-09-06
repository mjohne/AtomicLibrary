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
    public void NuclearBindingEnergyCalculator_Fe56_HasHighBindingPerNucleon()
    {
        var fe56 = Isotopes.GetByAtomicAndMassNumber(26, 56);
        var bindingPerNucleon = NuclearBindingEnergyCalculator.BindingEnergyPerNucleonMeV(fe56);

        Assert.InRange(bindingPerNucleon, 8.0, 9.5);
    }
}
