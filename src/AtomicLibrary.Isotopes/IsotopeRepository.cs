using AtomicLibrary.Core.Isotopes;

using System.Diagnostics;

namespace AtomicLibrary.Isotopes;

/// <summary>Represents a repository of isotopes, providing methods to retrieve isotopes by their atomic and mass numbers.</summary>

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class IsotopeRepository
{
	/// <summary>A dictionary that maps a tuple of atomic number and mass number to the corresponding isotope.</summary>
	private readonly IReadOnlyDictionary<(int AtomicNumber, int MassNumber), Isotope> _byKey;

	/// <summary>
	/// Initializes a new instance of the <see cref="IsotopeRepository"/> class with the specified isotopes.</summary>
	/// <param name="isotopes">The isotopes to include in the repository.</param>
	public IsotopeRepository(IEnumerable<Isotope> isotopes)
	{
		List<Isotope> list = [.. isotopes.OrderBy(keySelector: static i =>
		{
			ArgumentNullException.ThrowIfNull(argument: i);
			return i.ProtonCount;
		}).ThenBy(keySelector: static i => i.MassNumber)];
		All = list;
		_byKey = list.ToDictionary(keySelector: static i => (AtomicNumber: i.ProtonCount, i.MassNumber));
	}

	/// <summary>Gets a read-only list of all isotopes in the repository, ordered by atomic number and mass number.</summary>
	public IReadOnlyList<Isotope> All { get; }

	/// <summary>Retrieves an isotope from the repository based on its atomic number and mass number.</summary>
	/// <param name="atomicNumber">The atomic number of the isotope.</param>
	/// <param name="massNumber">The mass number of the isotope.</param>
	/// <returns>The isotope with the specified atomic and mass numbers.</returns>
	public Isotope GetByAtomicAndMassNumber(int atomicNumber, int massNumber)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value: massNumber);
		return _byKey[key: (AtomicNumber: atomicNumber, MassNumber: massNumber)];
	}

	/// <summary>Retrieves all isotopes from the repository with the specified atomic number.</summary>
	/// <param name="atomicNumber">The atomic number of the isotopes to retrieve.</param>
	/// <returns>An enumerable of isotopes with the specified atomic number.</returns>
	public IEnumerable<Isotope> GetByAtomicNumber(int atomicNumber)
	{
		return All.Where(predicate: i =>
		{
			ArgumentNullException.ThrowIfNull(argument: i);
			return i.ProtonCount == atomicNumber;
		});
	}

	/// <summary>Attempts to retrieve an isotope from the repository based on its atomic number and mass number.</summary>
	/// <param name="atomicNumber">The atomic number of the isotope.</param>
	/// <param name="massNumber">The mass number of the isotope.</param>
	/// <param name="isotope">When this method returns, contains the isotope with the specified atomic and mass numbers, if found; otherwise, null.</param>
	/// <returns>true if the isotope was found; otherwise, false.</returns>
	public bool TryGetByAtomicAndMassNumber(int atomicNumber, int massNumber, out Isotope? isotope)
	{
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value: massNumber);
		return _byKey.TryGetValue((AtomicNumber: atomicNumber, MassNumber: massNumber), out isotope);
	}

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
