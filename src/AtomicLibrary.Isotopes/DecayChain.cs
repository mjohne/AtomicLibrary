using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Isotopes;

/// <summary>Represents a decay chain of an isotope.</summary>
public sealed class DecayChain
{
	/// <summary>Gets the atomic number of the parent isotope.</summary>
	public required int ParentAtomicNumber { get; init; }

	/// <summary>Gets the mass number of the parent isotope.</summary>
	public required int ParentMassNumber { get; init; }

	/// <summary>Gets the steps in the decay chain.</summary>
	public required IReadOnlyList<DecayChainStep> Steps { get; init; }
}

/// <summary>Represents a step in a decay chain of an isotope.</summary>
public sealed class DecayChainStep
{
	/// <summary>Gets the atomic number of the isotope in this step.</summary>
	public required int AtomicNumber { get; init; }

	/// <summary>Gets the mass number of the isotope in this step.</summary>
	public required int MassNumber { get; init; }

	/// <summary>Gets the decay mode of the isotope in this step.</summary>
	public required DecayMode DecayMode { get; init; }
}
