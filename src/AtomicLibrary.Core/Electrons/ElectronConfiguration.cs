using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace AtomicLibrary.Core.Electrons;

/// <summary>Represents the electron configuration of an atom, which describes the distribution of electrons in atomic orbitals according to the Aufbau principle, Hund's rule, and the Pauli exclusion principle.</summary>
/// <remarks>Initializes a new instance of the <see cref="ElectronConfiguration"/> class with the specified orbitals. Only orbitals with a positive electron count are included in the configuration.</remarks>
/// <param name="orbitals">The collection of orbitals to include in the configuration.</param>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class ElectronConfiguration(IEnumerable<Orbital> orbitals)
{
	/// <summary>The order of orbitals according to the Aufbau principle, which specifies the sequence in which electrons fill atomic orbitals. Each tuple contains the principal quantum number (n), the type of orbital (s, p, d, f), and the maximum number of electrons that can occupy that orbital.</summary>
	private static readonly (int N, OrbitalType Type, int MaxElectrons)[] AufbauOrder =
	[
		(N: 1, OrbitalType.S, MaxElectrons: 2),
		(N: 2, OrbitalType.S, MaxElectrons: 2),
		(N: 2, OrbitalType.P, MaxElectrons: 6),
		(N: 3, OrbitalType.S, MaxElectrons: 2),
		(N: 3, OrbitalType.P, MaxElectrons: 6),
		(N: 4, OrbitalType.S, MaxElectrons: 2),
		(N: 3, OrbitalType.D, MaxElectrons: 10),
		(N: 4, OrbitalType.P, MaxElectrons: 6),
		(N: 5, OrbitalType.S, MaxElectrons: 2),
		(N: 4, OrbitalType.D, MaxElectrons: 10),
		(N: 5, OrbitalType.P, MaxElectrons: 6),
		(N: 6, OrbitalType.S, MaxElectrons: 2),
		(N: 4, OrbitalType.F, MaxElectrons: 14),
		(N: 5, OrbitalType.D, MaxElectrons: 10),
		(N: 6, OrbitalType.P, MaxElectrons: 6),
		(N: 7, OrbitalType.S, MaxElectrons: 2),
		(N: 5, OrbitalType.F, MaxElectrons: 14),
		(N: 6, OrbitalType.D, MaxElectrons: 10),
		(N: 7, OrbitalType.P, MaxElectrons: 6)
	];

	/// <summary>Gets the list of orbitals in the electron configuration. Each orbital specifies its principal quantum number, type (s, p, d, f), and the number of electrons it contains.</summary>
	public IReadOnlyList<Orbital> Orbitals { get; } = new ReadOnlyCollection<Orbital>(list: [.. orbitals.Where(predicate: static o => o.ElectronCount > 0)]);

	/// <summary>Gets the total number of electrons in the electron configuration.</summary>
	public int ElectronCount => Orbitals.Sum(selector: static o => o.ElectronCount);

	/// <summary>Creates an <see cref="ElectronConfiguration"/> from the specified number of electrons.</summary>
	/// <param name="electronCount">The number of electrons.</param>
	/// <returns>An <see cref="ElectronConfiguration"/> representing the distribution of electrons.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="electronCount"/> is negative.</exception>
	public static ElectronConfiguration FromElectronCount(int electronCount)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(value: electronCount);
		int remaining = electronCount;
		List<Orbital> orbitals = [];
		foreach ((int n, OrbitalType type, int maxElectrons) in AufbauOrder)
		{
			if (remaining <= 0)
			{
				break;
			}
			int fill = Math.Min(val1: remaining, val2: maxElectrons);
			orbitals.Add(item: new Orbital(PrincipalQuantumNumber: n, Type: type, ElectronCount: fill));
			remaining -= fill;
		}
		return new ElectronConfiguration(orbitals: orbitals);
	}

	/// <summary>Creates an <see cref="ElectronConfiguration"/> from the specified atomic number. The atomic number corresponds to the number of protons in the nucleus of an atom, which is equal to the number of electrons in a neutral atom.</summary>
	/// <param name="atomicNumber">The atomic number of the element.</param>
	/// <returns>An <see cref="ElectronConfiguration"/> representing the distribution of electrons.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="atomicNumber"/> is less than 1.</exception>
	public static ElectronConfiguration FromAtomicNumber(int atomicNumber)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(value: atomicNumber, other: 1);
		return FromElectronCount(electronCount: atomicNumber);
	}

	/// <summary>Parses a string representation of an electron configuration into an <see cref="ElectronConfiguration"/> object. The input string should consist of orbital tokens separated by spaces, where each token is in the format "nXy", with "n" being the principal quantum number, "X" being the orbital type (s, p, d, f), and "y" being the number of electrons in that orbital. For example, "1s2 2s2 2p6" represents the electron configuration for neon.</summary>
	/// <param name="configuration">The string representation of the electron configuration.</param>
	/// <returns>An <see cref="ElectronConfiguration"/> object representing the parsed electron configuration.</returns>
	/// <exception cref="FormatException">Thrown when the input string is not in a valid electron configuration format.</exception>
	public static ElectronConfiguration Parse(string configuration)
	{
		if (string.IsNullOrWhiteSpace(value: configuration))
		{
			return new ElectronConfiguration(orbitals: []);
		}
		List<Orbital> orbitals = [];
		string[] tokens = configuration.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);
		foreach (string token in tokens)
		{
			if (token.StartsWith('['))
			{
				throw new FormatException(message: "Bracketed noble-gas notation is not supported.");
			}
			StringBuilder nPart = new();
			char typePart = '\0';
			StringBuilder ePart = new();
			foreach (char ch in token)
			{
				if (char.IsDigit(c: ch) && typePart == '\0')
				{
					_ = nPart.Append(value: ch);
				}
				else if (char.IsLetter(c: ch) && typePart == '\0')
				{
					typePart = char.ToLowerInvariant(c: ch);
				}
				else
				{
					_ = char.IsDigit(c: ch) ? ePart.Append(value: ch) : throw new FormatException(message: $"Invalid orbital token '{token}'.");
				}
			}
			if (nPart.Length == 0 || ePart.Length == 0)
			{
				throw new FormatException(message: $"Invalid orbital token '{token}'.");
			}
			int n = int.Parse(s: nPart.ToString(), provider: CultureInfo.InvariantCulture);
			int electrons = int.Parse(s: ePart.ToString(), provider: CultureInfo.InvariantCulture);
			if (n < 1)
			{
				throw new FormatException(message: $"Orbital '{token}' has invalid principal quantum number.");
			}
			OrbitalType type = typePart switch
			{
				's' => OrbitalType.S,
				'p' => OrbitalType.P,
				'd' => OrbitalType.D,
				'f' => OrbitalType.F,
				_ => throw new FormatException(message: $"Unsupported orbital type '{typePart}'.")
			};
			Orbital orbital = new(PrincipalQuantumNumber: n, Type: type, ElectronCount: electrons);
			if (electrons < 1)
			{
				throw new FormatException(message: $"Orbital '{token}' has invalid occupancy.");
			}
			if (electrons > orbital.MaximumElectronCount)
			{
				throw new FormatException(message: $"Orbital '{token}' exceeds maximum occupancy.");
			}
			orbitals.Add(item: orbital);
		}
		return new ElectronConfiguration(orbitals: orbitals);
	}

	/// <summary>Returns a string representation of the electron configuration in the format "1s2 2s2 2p6", where each orbital is represented by its principal quantum number, type, and electron count.</summary>
	/// <returns>A string representation of the electron configuration.</returns>
	public override string ToString()
	{
		return string.Join(separator: ' ', values: Orbitals.Select(selector: static o => $"{o.PrincipalQuantumNumber}{o.Type.ToString().ToLowerInvariant()}{o.ElectronCount}"));
	}

	/// <summary>Returns a string that represents the current object for debugging purposes. This method is used by the debugger to display a concise representation of the electron configuration.</summary>
	/// <returns>A string representation of the electron configuration for debugging purposes.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}
}
