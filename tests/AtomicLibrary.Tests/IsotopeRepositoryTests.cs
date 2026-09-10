using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;

using System.Diagnostics;

namespace AtomicLibrary.Tests;

/// <summary>Tests for the <see cref="IsotopeRepository"/> class.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class IsotopeRepositoryTests
{
	/// <summary>Tests that the behavior of the <see cref="IsotopeRepository"/> class is consistent when invalid input is provided.</summary>
	[Fact]
	public void InvalidInputBehaviorIsConsistent()
	{
		Element carbon = new()
		{
			AtomicNumber = 6,
			Symbol = "C",
			NameEnglish = "Carbon",
			NameGerman = "Kohlenstoff",
			Period = 2,
			Group = 14,
			Block = ElementBlock.P,
			Category = ElementCategory.Nonmetal,
			StandardAtomicWeight = 12.011,
			Electronegativity = 2.55,
			IonizationEnergy = 11.2603,
			ElectronAffinity = 1.262,
			AtomicRadius = 70,
			CovalentRadius = 77,
			Density = 2.267,
			MeltingPoint = 3823,
			BoilingPoint = 4300,
			OxidationStates = [4, -4, 2, -2],
			ElectronConfiguration = "1s2 2s2 2p2",
			IsRadioactive = false
		};
		Isotope carbon12 = new(
			element: carbon,
			massNumber: 12,
			atomicMass: 12.0,
			isStable: true,
			halfLifeSeconds: null,
			decayMode: DecayMode.None,
			decayEnergyMeV: null,
			naturalAbundance: 98.93);
		IsotopeRepository repository = new(isotopes: [carbon12]);
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => repository.GetByAtomicAndMassNumber(atomicNumber: 6, massNumber: 0));
		bool found = repository.TryGetByAtomicAndMassNumber(atomicNumber: 6, massNumber: 0, out Isotope? isotope);
		Assert.False(condition: found);
		Assert.Null(@object: isotope);
	}

	/// <summary>Returns a string representation of the object for debugging purposes.</summary>
	/// <returns>A string representation of the object.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? string.Empty;
	}
}
