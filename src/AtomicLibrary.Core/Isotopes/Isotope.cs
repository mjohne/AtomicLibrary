using AtomicLibrary.Core.Elements;

using System.Diagnostics;

namespace AtomicLibrary.Core.Isotopes;

/// <summary>Represents an isotope of a chemical element, characterized by its mass number, atomic mass, stability, half-life, decay mode, decay energy, and natural abundance.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class Isotope
{
	/// <summary>Initializes a new instance of the <see cref="Isotope"/> class with the specified properties.</summary>
	/// <param name="element">The chemical element of the isotope.</param>
	/// <param name="massNumber">The mass number of the isotope.</param>
	/// <param name="atomicMass">The atomic mass of the isotope.</param>
	/// <param name="isStable">Indicates whether the isotope is stable.</param>
	/// <param name="halfLifeSeconds">The half-life of the isotope in seconds, if radioactive.</param>
	/// <param name="decayMode">The decay mode of the isotope, if radioactive.</param>
	/// <param name="decayEnergyMeV">The decay energy of the isotope in MeV, if radioactive.</param>
	/// <param name="naturalAbundance">The natural abundance of the isotope, if applicable.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the mass number is less than the atomic number of the element.</exception>
	public Isotope(
		Element element,
		int massNumber,
		double atomicMass,
		bool isStable,
		double? halfLifeSeconds,
		DecayMode decayMode,
		double? decayEnergyMeV,
		double? naturalAbundance)
	{
		Element = element;
		MassNumber = massNumber;
		AtomicMass = atomicMass;
		IsStable = isStable;
		HalfLifeSeconds = halfLifeSeconds;
		DecayMode = decayMode;
		DecayEnergyMeV = decayEnergyMeV;
		NaturalAbundance = naturalAbundance;
		if (MassNumber < Element.AtomicNumber)
		{
			throw new ArgumentOutOfRangeException(paramName: nameof(massNumber), message: "Mass number must be >= atomic number.");
		}
	}

	/// <summary>Gets the chemical element of the isotope.</summary>
	public Element Element { get; }

	/// <summary>Gets the mass number of the isotope, which is the total number of protons and neutrons in the nucleus.</summary>
	public int MassNumber { get; }

	/// <summary>Gets the number of protons in the isotope, which is equal to the atomic number of the element.</summary>
	public int ProtonCount => Element.AtomicNumber;

	/// <summary>Gets the number of neutrons in the isotope, calculated as the difference between the mass number and the atomic number.</summary>
	public int NeutronCount => MassNumber - Element.AtomicNumber;

	/// <summary>Gets the atomic mass of the isotope, which is the mass of a single atom of the isotope in atomic mass units (u).</summary>
	public double AtomicMass { get; }

	/// <summary>Gets a value indicating whether the isotope is stable (does not undergo radioactive decay).</summary>
	public bool IsStable { get; }

	/// <summary>Gets a value indicating whether the isotope is radioactive (undergoes radioactive decay). This is the inverse of the IsStable property.</summary>
	public bool IsRadioactive => !IsStable;

	/// <summary>Gets the half-life of the isotope in seconds, if it is radioactive. If the isotope is stable, this property will be null.</summary>
	public double? HalfLifeSeconds { get; }

	/// <summary>Gets the decay mode of the isotope. Stable isotopes use <see cref="DecayMode.None"/>.</summary>
	public DecayMode DecayMode { get; }

	/// <summary>Gets the decay energy of the isotope in MeV, if it is radioactive. If the isotope is stable, this property will be null.</summary>
	public double? DecayEnergyMeV { get; }
	/// <summary>Gets the natural abundance of the isotope, if applicable. If the isotope does not occur naturally, this property will be null.</summary>
	public double? NaturalAbundance { get; }

	/// <summary>Returns a string representation of the isotope in the format "ElementSymbol-MassNumber" (e.g., "C-12" for Carbon-12).</summary>
	/// <returns>A string representation of the isotope.</returns>
	public override string ToString()
	{
		return $"{Element.Symbol}-{MassNumber}";
	}

	/// <summary>Returns a string representation of the isotope for debugging purposes. This method is used by the DebuggerDisplay attribute to provide a concise display of the isotope in the debugger.</summary>
	/// <returns>A string representation of the isotope for debugging purposes.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
