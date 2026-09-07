using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Physics;

/// <summary>Provides methods for calculating nuclear binding energy and binding energy per nucleon for isotopes.</summary>
public static class NuclearBindingEnergyCalculator
{
	/// <summary>Calculates the binding energy in MeV for a given isotope.</summary>
	/// <param name="isotope">The isotope for which to calculate the binding energy.</param>
	/// <returns>The binding energy in MeV.</returns>
	public static double BindingEnergyMeV(Isotope isotope)
	{
		ArgumentNullException.ThrowIfNull(argument: isotope, paramName: nameof(isotope));
		int z = isotope.ProtonCount;
		int n = isotope.NeutronCount;
		double nucleusMass = (isotope.AtomicMass * PhysicalConstants.AtomicMassUnit) - (z * PhysicalConstants.ElectronMass);
		double nucleonsMass = (z * PhysicalConstants.ProtonMass) + (n * PhysicalConstants.NeutronMass);
		double massDefect = nucleonsMass - nucleusMass;
		double energyJoule = massDefect * PhysicalConstants.SpeedOfLight * PhysicalConstants.SpeedOfLight;
		return energyJoule / (PhysicalConstants.ElementaryCharge * 1e6);
	}

	/// <summary>Calculates the binding energy per nucleon in MeV for a given isotope.</summary>
	/// <param name="isotope">The isotope for which to calculate the binding energy per nucleon.</param>
	/// <returns>The binding energy per nucleon in MeV.</returns>
	public static double BindingEnergyPerNucleonMeV(Isotope isotope)
	{
		ArgumentNullException.ThrowIfNull(argument: isotope, paramName: nameof(isotope));
		return BindingEnergyMeV(isotope: isotope) / isotope.MassNumber;
	}
}
