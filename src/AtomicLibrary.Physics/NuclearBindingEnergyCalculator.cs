using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Physics;

public static class NuclearBindingEnergyCalculator
{
    public static double BindingEnergyMeV(Isotope isotope)
    {
        var z = isotope.ProtonCount;
        var n = isotope.NeutronCount;

        var nucleusMass = isotope.AtomicMass * PhysicalConstants.AtomicMassUnit - (z * PhysicalConstants.ElectronMass);
        var nucleonsMass = (z * PhysicalConstants.ProtonMass) + (n * PhysicalConstants.NeutronMass);
        var massDefect = nucleonsMass - nucleusMass;
        var energyJoule = massDefect * PhysicalConstants.SpeedOfLight * PhysicalConstants.SpeedOfLight;

        return energyJoule / (PhysicalConstants.ElementaryCharge * 1e6);
    }

    public static double BindingEnergyPerNucleonMeV(Isotope isotope) => BindingEnergyMeV(isotope) / isotope.MassNumber;
}
