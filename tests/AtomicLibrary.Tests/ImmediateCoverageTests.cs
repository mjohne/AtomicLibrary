using AtomicLibrary.Core.Electrons;
using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;
using AtomicLibrary.Physics;

using System.Diagnostics;
using System.Text.Json;

namespace AtomicLibrary.Tests;

/// <summary>Contains coverage tests that do not require production-code changes.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class ImmediateCoverageTests
{
	private static readonly PeriodicTable.PeriodicTable PeriodicTable = ElementDataLoader.LoadDefault();
	private static readonly IsotopeRepository Isotopes = IsotopeDataLoader.LoadDefault(periodicTable: PeriodicTable);

	/// <summary>Verifies that the default periodic table contains each standard atomic number exactly once.</summary>
	[Fact]
	public void DefaultPeriodicTableContainsAllAtomicNumbersAndUniqueSymbols()
	{
		Assert.Equal(expected: 118, actual: PeriodicTable.All.Count);
		Assert.Equal(
			expected: Enumerable.Range(start: 1, count: 118),
			actual: PeriodicTable.All.Select(selector: static element => element.AtomicNumber));
		Assert.Equal(
			expected: PeriodicTable.All.Count,
			actual: PeriodicTable.All.Select(selector: static element => element.Symbol).Distinct(comparer: StringComparer.OrdinalIgnoreCase).Count());
	}

	/// <summary>Verifies successful and unsuccessful periodic-table lookups.</summary>
	[Fact]
	public void PeriodicTableLookupsHandleCaseInsensitiveAndUnknownValues()
	{
		Element oxygen = PeriodicTable.GetBySymbol(symbol: "o");
		Assert.Equal(expected: 8, actual: oxygen.AtomicNumber);
		Assert.True(condition: PeriodicTable.TryGetByAtomicNumber(atomicNumber: 8, element: out Element? oxygenByNumber));
		Assert.Same(expected: oxygen, actual: oxygenByNumber);
		Assert.False(condition: PeriodicTable.TryGetByAtomicNumber(atomicNumber: 119, element: out Element? unknownByNumber));
		Assert.Null(@object: unknownByNumber);
		Assert.False(condition: PeriodicTable.TryGetBySymbol(symbol: "Xx", element: out Element? unknownBySymbol));
		Assert.Null(@object: unknownBySymbol);
		_ = Assert.Throws<KeyNotFoundException>(() => PeriodicTable.GetByAtomicNumber(atomicNumber: 119));
		_ = Assert.Throws<KeyNotFoundException>(() => PeriodicTable.GetBySymbol(symbol: "Xx"));
	}

	/// <summary>Verifies that default isotopes reference elements from the same periodic table.</summary>
	[Fact]
	public void DefaultIsotopesReferenceElementsFromDefaultPeriodicTable()
	{
		Assert.NotEmpty(collection: Isotopes.All);
		Assert.All(
			collection: Isotopes.All,
			action: isotope => Assert.Same(
				expected: PeriodicTable.GetByAtomicNumber(atomicNumber: isotope.ProtonCount),
				actual: isotope.Element));
	}

	/// <summary>Verifies that the isotope repository sorts its entries by atomic and mass number.</summary>
	[Fact]
	public void IsotopeRepositoryOrdersIsotopesByAtomicAndMassNumber()
	{
		for (int index = 1; index < Isotopes.All.Count; index++)
		{
			Isotope previous = Isotopes.All[index - 1];
			Isotope current = Isotopes.All[index];
			Assert.True(condition: previous.ProtonCount < current.ProtonCount || (previous.ProtonCount == current.ProtonCount && previous.MassNumber <= current.MassNumber));
		}
	}

	/// <summary>Verifies successful and unsuccessful isotope lookups with valid input values.</summary>
	[Fact]
	public void IsotopeRepositoryReturnsExpectedResultsForExistingAndMissingIsotopes()
	{
		Assert.True(condition: Isotopes.TryGetByAtomicAndMassNumber(atomicNumber: 6, massNumber: 12, isotope: out Isotope? carbon12));
		Assert.NotNull(@object: carbon12);
		Assert.Equal(expected: "C-12", actual: carbon12.ToString());
		Assert.False(condition: Isotopes.TryGetByAtomicAndMassNumber(atomicNumber: 1, massNumber: 999, isotope: out Isotope? unknown));
		Assert.Null(@object: unknown);
		_ = Assert.Throws<KeyNotFoundException>(() => Isotopes.GetByAtomicAndMassNumber(atomicNumber: 1, massNumber: 999));
	}

	/// <summary>Verifies the lower and upper standard atomic-number boundaries.</summary>
	[Fact]
	public void ElectronConfigurationFromAtomicNumberHandlesStandardBoundaries()
	{
		ElectronConfiguration hydrogen = ElectronConfiguration.FromAtomicNumber(atomicNumber: 1);
		ElectronConfiguration oganesson = ElectronConfiguration.FromAtomicNumber(atomicNumber: 118);
		Assert.Equal(expected: "1s1", actual: hydrogen.ToString());
		Assert.Equal(expected: 1, actual: hydrogen.ElectronCount);
		Assert.Equal(expected: 118, actual: oganesson.ElectronCount);
	}

	/// <summary>Verifies empty and invalid electron-count handling.</summary>
	[Fact]
	public void ElectronConfigurationFromElectronCountHandlesZeroAndNegativeValues()
	{
		ElectronConfiguration empty = ElectronConfiguration.FromElectronCount(electronCount: 0);
		Assert.Empty(collection: empty.Orbitals);
		Assert.Equal(expected: string.Empty, actual: empty.ToString());
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => ElectronConfiguration.FromElectronCount(electronCount: -1));
	}

	/// <summary>Verifies that repeated spaces do not affect parsing or canonical formatting.</summary>
	[Fact]
	public void ElectronConfigurationParseNormalizesRepeatedSpaces()
	{
		ElectronConfiguration configuration = ElectronConfiguration.Parse(configuration: "  1s2   2s2  2p6  ");
		Assert.Equal(expected: 10, actual: configuration.ElectronCount);
		Assert.Equal(expected: "1s2 2s2 2p6", actual: configuration.ToString());
	}

	/// <summary>Verifies radioactive-decay results at zero, one, and multiple half-lives.</summary>
	[Fact]
	public void RadioactiveDecayCalculatorCalculatesExpectedHalfLifeBoundaries()
	{
		const double halfLifeSeconds = 10.0;
		Assert.InRange(
			actual: RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: halfLifeSeconds, timeSeconds: 0.0),
			low: 0.999999999999,
			high: 1.000000000001);
		Assert.InRange(
			actual: RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: halfLifeSeconds, timeSeconds: halfLifeSeconds),
			low: 0.499999999999,
			high: 0.500000000001);
		Assert.InRange(
			actual: RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: halfLifeSeconds, timeSeconds: 2.0 * halfLifeSeconds),
			low: 0.249999999999,
			high: 0.250000000001);
		Assert.InRange(
			actual: RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: halfLifeSeconds, timeSeconds: 10.0 * halfLifeSeconds),
			low: 0.0009765624,
			high: 0.0009765626);
	}

	/// <summary>Verifies zero-atom behavior for radioactive-decay calculations.</summary>
	[Fact]
	public void RadioactiveDecayCalculatorReturnsZeroForZeroAtoms()
	{
		Assert.Equal(expected: 0.0, actual: RadioactiveDecayCalculator.RemainingAtoms(initialAtoms: 0.0, halfLifeSeconds: 10.0, timeSeconds: 5.0));
		Assert.Equal(expected: 0.0, actual: RadioactiveDecayCalculator.Activity(atomCount: 0.0, halfLifeSeconds: 10.0));
	}

	/// <summary>Verifies binding-energy calculations for carbon-12 and null input.</summary>
	[Fact]
	public void NuclearBindingEnergyCalculatorHandlesCarbon12AndNull()
	{
		Isotope carbon12 = Isotopes.GetByAtomicAndMassNumber(atomicNumber: 6, massNumber: 12);
		double bindingEnergyPerNucleon = NuclearBindingEnergyCalculator.BindingEnergyPerNucleonMeV(isotope: carbon12);
		Assert.InRange(actual: bindingEnergyPerNucleon, low: 7.0, high: 8.0);
		_ = Assert.Throws<ArgumentNullException>(() => NuclearBindingEnergyCalculator.BindingEnergyMeV(isotope: null!));
		_ = Assert.Throws<ArgumentNullException>(() => NuclearBindingEnergyCalculator.BindingEnergyPerNucleonMeV(isotope: null!));
	}

	/// <summary>Verifies successful loading of a valid element JSON document.</summary>
	[Fact]
	public void ElementDataLoaderLoadsValidJson()
	{
		string path = WriteTemporaryJson(
			"""
			[
			  {
				"AtomicNumber": 1,
				"Symbol": "H",
				"NameEnglish": "Hydrogen",
				"NameGerman": "Wasserstoff",
				"Period": 1,
				"Group": 1,
				"Block": "S",
				"Category": "Nonmetal",
				"ElectronConfiguration": "1s1"
			  }
			]
			""");
		try
		{
			PeriodicTable.PeriodicTable table = ElementDataLoader.LoadFromJson(filePath: path);
			Element hydrogen = table.GetByAtomicNumber(atomicNumber: 1);
			Assert.Equal(expected: "H", actual: hydrogen.Symbol);
			Assert.Equal(expected: "Hydrogen", actual: hydrogen.NameEnglish);
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Verifies successful loading of a valid isotope JSON document.</summary>
	[Fact]
	public void IsotopeDataLoaderLoadsValidJson()
	{
		string path = WriteTemporaryJson(
			"""
			[
			  {
				"AtomicNumber": 6,
				"MassNumber": 12,
				"AtomicMass": 12.0,
				"IsStable": true,
				"HalfLifeSeconds": null,
				"DecayMode": "None",
				"DecayEnergyMeV": null,
				"NaturalAbundance": 98.93
			  }
			]
			""");
		try
		{
			IsotopeRepository repository = IsotopeDataLoader.LoadFromJson(filePath: path, periodicTable: PeriodicTable);
			Isotope carbon12 = repository.GetByAtomicAndMassNumber(atomicNumber: 6, massNumber: 12);
			Assert.True(condition: carbon12.IsStable);
			Assert.Equal(expected: DecayMode.None, actual: carbon12.DecayMode);
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Verifies successful loading of a valid decay-chain JSON document.</summary>
	[Fact]
	public void IsotopeDataLoaderLoadsValidDecayChainJson()
	{
		string path = WriteTemporaryJson(
			"""
			[
			  {
				"ParentAtomicNumber": 14,
				"ParentMassNumber": 32,
				"Steps": [
				  {
					"AtomicNumber": 15,
					"MassNumber": 32,
					"DecayMode": "BetaMinus"
				  }
				]
			  }
			]
			""");
		try
		{
			IReadOnlyList<DecayChain> chains = IsotopeDataLoader.LoadDecayChains(filePath: path);
			DecayChain chain = Assert.Single(collection: chains);
			Assert.Equal(expected: 14, actual: chain.ParentAtomicNumber);
			DecayChainStep step = Assert.Single(collection: chain.Steps);
			Assert.Equal(expected: DecayMode.BetaMinus, actual: step.DecayMode);
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Verifies that each loader propagates a missing-file error.</summary>
	[Fact]
	public void LoadersThrowForMissingFiles()
	{
		string path = Path.Combine(path1: Path.GetTempPath(), path2: Path.GetRandomFileName());
		_ = Assert.Throws<FileNotFoundException>(() => ElementDataLoader.LoadFromJson(filePath: path));
		_ = Assert.Throws<FileNotFoundException>(() => IsotopeDataLoader.LoadFromJson(filePath: path, periodicTable: PeriodicTable));
		_ = Assert.Throws<FileNotFoundException>(() => IsotopeDataLoader.LoadDecayChains(filePath: path));
	}

	/// <summary>Verifies that each loader rejects syntactically invalid JSON.</summary>
	[Fact]
	public void LoadersThrowForMalformedJson()
	{
		string path = WriteTemporaryJson(contents: "{");
		try
		{
			_ = Assert.Throws<JsonException>(() => ElementDataLoader.LoadFromJson(filePath: path));
			_ = Assert.Throws<JsonException>(() => IsotopeDataLoader.LoadFromJson(filePath: path, periodicTable: PeriodicTable));
			_ = Assert.Throws<JsonException>(() => IsotopeDataLoader.LoadDecayChains(filePath: path));
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Writes JSON content to a temporary file.</summary>
	/// <param name="contents">The JSON content to write.</param>
	/// <returns>The path to the temporary JSON file.</returns>
	private static string WriteTemporaryJson(string contents)
	{
		string path = Path.GetTempFileName();
		File.WriteAllText(path: path, contents: contents);
		return path;
	}

	/// <summary>Provides a debugger display string for the test class.</summary>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? string.Empty;
	}
}