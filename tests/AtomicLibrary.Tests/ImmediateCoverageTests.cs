using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;

using System.Diagnostics;
using System.Text.Json;

namespace AtomicLibrary.Tests;

/// <summary>Contains coverage tests that do not require production-code changes.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class ImmediateCoverageTests
{
	private static readonly PeriodicTable.PeriodicTable PeriodicTable = ElementDataLoader.LoadDefault();

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