using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;

using System.Diagnostics;
using System.Text.Json;

namespace AtomicLibrary.Isotopes;

/// <summary>Provides methods to load isotope data and decay chains from JSON files into an <see cref="IsotopeRepository"/> and a list of <see cref="DecayChain"/> objects.</summary>
public static class IsotopeDataLoader
{
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	/// <summary>Loads isotope data from a JSON file and returns an <see cref="IsotopeRepository"/> containing the isotopes.</summary>
	/// <param name="filePath">The path to the JSON file containing isotope data.</param>
	/// <param name="periodicTable">The <see cref="PeriodicTable.PeriodicTable"/> instance to use for element lookup.</param>
	/// <returns>An <see cref="IsotopeRepository"/> containing the isotopes.</returns>
	/// <exception cref="FormatException">Thrown if the JSON data contains invalid decay mode values.</exception>
	public static IsotopeRepository LoadFromJson(string filePath, PeriodicTable.PeriodicTable periodicTable)
	{
		string json = File.ReadAllText(path: filePath);
		List<IsotopeRecord> records = JsonSerializer.Deserialize<List<IsotopeRecord>>(json: json, options: _jsonOptions) ?? [];
		IEnumerable<Isotope> isotopes = records.Select(selector: record =>
		{
			ArgumentNullException.ThrowIfNull(argument: record);
			Element element = periodicTable.GetByAtomicNumber(atomicNumber: record.AtomicNumber);
			return !Enum.TryParse(value: record.DecayMode, ignoreCase: true, result: out DecayMode decayMode)
				? throw new FormatException(message: $"Invalid decay mode '{record.DecayMode}' for isotope Z={record.AtomicNumber}, A={record.MassNumber}.")
				: new Isotope(
					element,
					record.MassNumber,
					record.AtomicMass,
					record.IsStable,
					record.HalfLifeSeconds,
					decayMode,
					record.DecayEnergyMeV,
					record.NaturalAbundance);
		});
		return new IsotopeRepository(isotopes: isotopes);
	}

	/// <summary>Loads decay chains from a JSON file and returns a list of <see cref="DecayChain"/> objects.</summary>
	/// <param name="filePath">The path to the JSON file containing decay chain data.</param>
	/// <returns>A list of <see cref="DecayChain"/> objects.</returns>
	/// <exception cref="FormatException">Thrown if the JSON data contains invalid decay mode values.</exception>
	public static IReadOnlyList<DecayChain> LoadDecayChains(string filePath)
	{
		string json = File.ReadAllText(path: filePath);
		List<DecayChainRecord> records = JsonSerializer.Deserialize<List<DecayChainRecord>>(json: json, options: _jsonOptions) ?? [];
		return [.. records.Select(selector: r =>
		{
			ArgumentNullException.ThrowIfNull(argument: r);
			return new DecayChain
			{
				ParentAtomicNumber = r.ParentAtomicNumber,
				ParentMassNumber = r.ParentMassNumber,
				Steps = [.. r.Steps.Select(selector: s =>
				{
					ArgumentNullException.ThrowIfNull(argument: s);
					return !Enum.TryParse<DecayMode>(value: s.DecayMode, ignoreCase: true, result: out DecayMode decayMode)
						? throw new FormatException($"Invalid decay mode '{s.DecayMode}' for decay chain parent Z={r.ParentAtomicNumber}, A={r.ParentMassNumber}, step Z={s.AtomicNumber}, A={s.MassNumber}.")
						: new DecayChainStep
						{
							AtomicNumber = s.AtomicNumber,
							MassNumber = s.MassNumber,
							DecayMode = decayMode
						};
				})]
			};
		})];
	}

	/// <summary>Loads the default isotope data from the "data/isotopes.json" file and returns an <see cref="IsotopeRepository"/> containing the isotopes.</summary>
	/// <param name="periodicTable">The periodic table to use for looking up elements.</param>
	/// <returns>An <see cref="IsotopeRepository"/> containing the loaded isotopes.</returns>
	public static IsotopeRepository LoadDefault(PeriodicTable.PeriodicTable periodicTable)
	{
		return LoadFromJson(filePath: Path.Combine(path1: AppContext.BaseDirectory, path2: "data", path3: "isotopes.json"), periodicTable: periodicTable);
	}

	/// <summary>Represents a record of isotope data used for deserialization from JSON.</summary>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	private sealed class IsotopeRecord
	{
		/// <summary>Gets or sets the atomic number of the isotope.</summary>
		public int AtomicNumber { get; set; }

		/// <summary>Gets or sets the mass number of the isotope.</summary>
		public int MassNumber { get; set; }

		/// <summary>Gets or sets the atomic mass of the isotope.</summary>
		public double AtomicMass { get; set; }

		/// <summary>Gets or sets a value indicating whether the isotope is stable.</summary>
		public bool IsStable { get; set; }

		/// <summary>Gets or sets the half-life of the isotope in seconds. This value is optional and may be null if not applicable.</summary>
		public double? HalfLifeSeconds { get; set; }

		/// <summary>Gets or sets the decay mode of the isotope as a string. This value will be parsed into a <see cref="DecayMode"/> enum when creating an <see cref="Isotope"/> object.</summary>
		public string DecayMode { get; set; } = "None";

		/// <summary>Gets or sets the decay energy of the isotope in mega-electronvolts (MeV). This value is optional and may be null if not applicable.</summary>
		public double? DecayEnergyMeV { get; set; }

		/// <summary>Gets or sets the natural abundance of the isotope as a percentage (0 to 100). This value is optional and may be null if not applicable.</summary>
		public double? NaturalAbundance { get; set; }

		/// <summary>Returns a string representation of the decay chain step record for debugging purposes.</summary>
		/// <returns>A string representation of the decay chain step record.</returns>
		private string GetDebuggerDisplay()
		{
			return ToString() ?? string.Empty;
		}

	}

	/// <summary>Represents a record of decay chain data used for deserialization from JSON.</summary>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	private sealed class DecayChainRecord
	{
		/// <summary>Gets or sets the atomic number of the parent isotope in the decay chain.</summary>
		public int ParentAtomicNumber { get; set; }

		/// <summary>Gets or sets the mass number of the parent isotope in the decay chain.</summary>
		public int ParentMassNumber { get; set; }

		/// <summary>Gets or sets the list of decay chain steps for the parent isotope.</summary>
		public List<DecayChainStepRecord> Steps { get; set; } = [];

		/// <summary>Returns a string representation of the decay chain step record for debugging purposes.</summary>
		/// <returns>A string representation of the decay chain step record.</returns>
		private string GetDebuggerDisplay()
		{
			return ToString() ?? string.Empty;
		}
	}

	/// <summary>Represents a record of decay chain step data used for deserialization from JSON.</summary>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	private sealed class DecayChainStepRecord
	{
		/// <summary>Gets or sets the atomic number of the isotope in this decay chain step.</summary>
		public int AtomicNumber { get; set; }

		/// <summary>Gets or sets the mass number of the isotope in this decay chain step.</summary>
		public int MassNumber { get; set; }

		/// <summary>Gets or sets the decay mode of the isotope in this decay chain step as a string. This value will be parsed into a <see cref="DecayMode"/> enum when creating a <see cref="DecayChainStep"/> object.</summary>
		public string DecayMode { get; set; } = "None";

		/// <summary>Returns a string representation of the decay chain step record for debugging purposes.</summary>
		/// <returns>A string representation of the decay chain step record.</returns>
		private string GetDebuggerDisplay()
		{
			return ToString() ?? string.Empty;
		}
	}
}
