using System.Diagnostics;

namespace AtomicLibrary.Core.Elements;

/// <summary>Represents a chemical element with its properties and characteristics.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class Element
{
	/// <summary>Gets or sets the atomic number of the element, which is a unique identifier for each element in the periodic table.</summary>
	public required int AtomicNumber { get; init; }

	/// <summary>Gets or sets the symbol of the element, which is a short abbreviation used to represent the element.</summary>
	public required string Symbol { get; init; }

	/// <summary>Gets or sets the English name of the element.</summary>
	public required string NameEnglish { get; init; }

	/// <summary>Gets or sets the German name of the element.</summary>
	public required string NameGerman { get; init; }

	/// <summary>Gets or sets the period of the element in the periodic table.</summary>
	public required int Period { get; init; }

	/// <summary>Gets or sets the group of the element in the periodic table.</summary>
	public required int Group { get; init; }

	/// <summary>Gets or sets the block of the element in the periodic table.</summary>
	public required ElementBlock Block { get; init; }

	/// <summary>Gets or sets the category of the element.</summary>
	public required ElementCategory Category { get; init; }

	/// <summary>Gets or sets the standard atomic weight of the element.</summary>
	public required double StandardAtomicWeight { get; init; }

	/// <summary>Gets or sets the electronegativity of the element.</summary>
	public required double Electronegativity { get; init; }

	/// <summary>Gets or sets the ionization energy of the element.</summary>
	public required double IonizationEnergy { get; init; }

	/// <summary>Gets or sets the electron affinity of the element.</summary>
	public required double ElectronAffinity { get; init; }

	/// <summary>Gets or sets the atomic radius of the element.</summary>
	public required double AtomicRadius { get; init; }

	/// <summary>Gets or sets the covalent radius of the element.</summary>
	public required double CovalentRadius { get; init; }

	/// <summary>Gets or sets the density of the element.</summary>
	public required double Density { get; init; }

	/// <summary>Gets or sets the melting point of the element.</summary>
	public required double MeltingPoint { get; init; }

	/// <summary>Gets or sets the boiling point of the element.</summary>
	public required double BoilingPoint { get; init; }

	/// <summary>Gets or sets the oxidation states of the element.</summary>
	public required IReadOnlyList<int> OxidationStates { get; init; }

	/// <summary>Gets or sets the electron configuration of the element.</summary>
	public required string ElectronConfiguration { get; init; }

	/// <summary>Indicates whether the element is a metal, including alkali metals, alkaline earth metals, transition metals, post-transition metals, lanthanides, and actinides.</summary>
	public bool IsMetal => Category is ElementCategory.AlkaliMetal
		or ElementCategory.AlkalineEarthMetal
		or ElementCategory.TransitionMetal
		or ElementCategory.PostTransitionMetal
		or ElementCategory.Lanthanide
		or ElementCategory.Actinide;

	/// <summary>Indicates whether the element is a non-metal, halogen, or noble gas.</summary>
	public bool IsNonMetal => Category is ElementCategory.Nonmetal
		or ElementCategory.Halogen
		or ElementCategory.NobleGas;

	/// <summary>Returns a string representation of the element, including its symbol and atomic number.</summary>
	/// <returns>A string representation of the element.</returns>
	public override string ToString()
	{
		return $"{Symbol} ({AtomicNumber})";
	}

	/// <summary>Returns a string representation of the element for debugging purposes.</summary>
	/// <returns>A string representation of the element.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
