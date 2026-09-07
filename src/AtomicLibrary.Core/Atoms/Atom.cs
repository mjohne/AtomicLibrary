using AtomicLibrary.Core.Electrons;
using AtomicLibrary.Core.Isotopes;

using System.Diagnostics;
using System.Globalization;

namespace AtomicLibrary.Core.Atoms;

/// <summary>Represents an atom, which consists of a specific isotope and a number of electrons.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class Atom
{
	/// <summary>Initializes a new instance of the <see cref="Atom"/> class with the specified isotope and electron count.</summary>
	/// <param name="isotope">The isotope of the atom.</param>
	/// <param name="electronCount">The number of electrons in the atom.</param>
	public Atom(Isotope isotope, int electronCount)
	{
		Isotope = isotope;
		ElectronCount = electronCount;
		ArgumentOutOfRangeException.ThrowIfNegative(value: electronCount);
		ElectronConfiguration = ElectronConfiguration.FromElectronCount(electronCount: ElectronCount);
	}

	/// <summary>Gets the isotope of the atom.</summary>
	public Isotope Isotope { get; }

	/// <summary>Gets the number of electrons in the atom.</summary>
	public int ElectronCount { get; }

	/// <summary>Gets the number of protons in the atom.</summary>
	public int ProtonCount => Isotope.ProtonCount;

	/// <summary>Gets the number of neutrons in the atom.</summary>
	public int NeutronCount => Isotope.NeutronCount;

	/// <summary>Gets the charge of the atom.</summary>
	public int Charge => ProtonCount - ElectronCount;

	/// <summary>Gets a value indicating whether the atom is neutral.</summary>
	public bool IsNeutral => Charge == 0;

	/// <summary>Gets a value indicating whether the atom is an ion.</summary>
	public bool IsIon => Charge != 0;

	/// <summary>Gets a value indicating whether the atom is a cation.</summary>
	public bool IsCation => Charge > 0;

	/// <summary>Gets a value indicating whether the atom is an anion.</summary>
	public bool IsAnion => Charge < 0;

	/// <summary>Gets the electron configuration of the atom.</summary>
	public ElectronConfiguration ElectronConfiguration { get; }

	/// <summary>Returns a string representation of the atom, including its isotope and charge.</summary>
	/// <returns>A string representation of the atom.</returns>
	public override string ToString()
	{
		string chargeText = Charge > 0 ? $"+{Charge}" : Charge.ToString(provider: CultureInfo.InvariantCulture);
		return $"{Isotope.Element.Symbol}-{Isotope.MassNumber} ({chargeText})";
	}

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
