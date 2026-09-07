using AtomicLibrary.Core.Elements;

using System.Text.Json;

namespace AtomicLibrary.PeriodicTable;

/// <summary>Provides functionality to load element data from JSON files into a <see cref="PeriodicTable"/> instance.</summary>
public static class ElementDataLoader
{
	/// <summary>Gets the JSON serializer options used for deserializing element data, with case-insensitive property name matching.</summary>
	private static readonly JsonSerializerOptions _jsonOptions = new()
	{
		PropertyNameCaseInsensitive = true
	};

	/// <summary>Loads element data from a JSON file and returns a <see cref="PeriodicTable"/> instance containing the elements.</summary>
	/// <param name="filePath">The path to the JSON file containing element data.</param>
	/// <returns>A <see cref="PeriodicTable"/> instance containing the elements.</returns>
	/// <exception cref="FormatException">Thrown if the JSON data contains invalid element block or category values.</exception>
	public static PeriodicTable LoadFromJson(string filePath)
	{
		string json = File.ReadAllText(filePath);
		List<ElementRecord> records = JsonSerializer.Deserialize<List<ElementRecord>>(json: json, options: _jsonOptions) ?? [];

		return new PeriodicTable(records.Select(selector: static r =>
		{
			ArgumentNullException.ThrowIfNull(argument: r);
			return !Enum.TryParse(value: r.Block, ignoreCase: true, result: out ElementBlock block)
				? throw new FormatException(message: $"Invalid element block '{r.Block}' for element Z={r.AtomicNumber} ({r.Symbol}).")
				: !Enum.TryParse(value: r.Category, ignoreCase: true, result: out ElementCategory category)
				? throw new FormatException(message: $"Invalid element category '{r.Category}' for element Z={r.AtomicNumber} ({r.Symbol}).")
				: new Element
				{
					AtomicNumber = r.AtomicNumber,
					Symbol = r.Symbol,
					NameEnglish = r.NameEnglish,
					NameGerman = r.NameGerman,
					Period = r.Period,
					Group = r.Group,
					Block = block,
					Category = category,
					StandardAtomicWeight = r.StandardAtomicWeight,
					Electronegativity = r.Electronegativity,
					IonizationEnergy = r.IonizationEnergy,
					ElectronAffinity = r.ElectronAffinity,
					AtomicRadius = r.AtomicRadius,
					CovalentRadius = r.CovalentRadius,
					Density = r.Density,
					MeltingPoint = r.MeltingPoint,
					BoilingPoint = r.BoilingPoint,
					OxidationStates = r.OxidationStates ?? [],
					ElectronConfiguration = r.ElectronConfiguration
				};
		}));
	}

	/// <summary>Loads the default element data from the embedded JSON file and returns a <see cref="PeriodicTable"/> instance containing the elements.</summary>
	/// <returns>A <see cref="PeriodicTable"/> instance containing the elements.</returns>
	public static PeriodicTable LoadDefault()
	{
		return LoadFromJson(filePath: Path.Combine(path1: AppContext.BaseDirectory, path2: "data", path3: "elements.json"));
	}

	/// <summary>Represents a record of an element's data as defined in the JSON file.</summary>
	private sealed class ElementRecord
	{
		/// <summary>Gets or sets the atomic number of the element.</summary>
		public int AtomicNumber { get; set; }

		/// <summary>Gets or sets the chemical symbol of the element.</summary>
		public string Symbol { get; set; } = string.Empty;

		/// <summary>Gets or sets the English name of the element.</summary>
		public string NameEnglish { get; set; } = string.Empty;

		/// <summary>Gets or sets the German name of the element.</summary>
		public string NameGerman { get; set; } = string.Empty;

		/// <summary>Gets or sets the period of the element in the periodic table.</summary>
		public int Period { get; set; }

		/// <summary>Gets or sets the group of the element in the periodic table.</summary>
		public int Group { get; set; }

		/// <summary>Gets or sets the block of the element in the periodic table (e.g., "s", "p", "d", "f").</summary>
		public string Block { get; set; } = "S";

		/// <summary>Gets or sets the category of the element (e.g., "Nonmetal", "Alkali Metal", etc.).</summary>
		public string Category { get; set; } = "Nonmetal";

		/// <summary>Gets or sets the standard atomic weight of the element.</summary>
		public double StandardAtomicWeight { get; set; }

		/// <summary>Gets or sets the electronegativity of the element.</summary>
		public double Electronegativity { get; set; }

		/// <summary>Gets or sets the ionization energy of the element.</summary>
		public double IonizationEnergy { get; set; }

		/// <summary>Gets or sets the electron affinity of the element.</summary>
		public double ElectronAffinity { get; set; }

		/// <summary>Gets or sets the atomic radius of the element.</summary>
		public double AtomicRadius { get; set; }

		/// <summary>Gets or sets the covalent radius of the element.</summary>
		public double CovalentRadius { get; set; }

		/// <summary>Gets or sets the density of the element.</summary>
		public double Density { get; set; }

		/// <summary>Gets or sets the melting point of the element.</summary>
		public double MeltingPoint { get; set; }

		/// <summary>Gets or sets the boiling point of the element.</summary>
		public double BoilingPoint { get; set; }

		/// <summary>Gets or sets the oxidation states of the element.</summary>
		public int[]? OxidationStates { get; set; }

		/// <summary>Gets or sets the electron configuration of the element.</summary>
		public string ElectronConfiguration { get; set; } = string.Empty;
	}
}