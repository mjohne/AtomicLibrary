using AtomicLibrary.Core.Atoms;
using AtomicLibrary.Core.Electrons;
using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;
using AtomicLibrary.Physics;

using System.Diagnostics;

namespace AtomicLibrary.Tests;

/// <summary>Contains unit tests for the AtomicLibrary, including tests for periodic table lookups, isotope validation, electron configuration parsing, radioactive decay calculations, and nuclear binding energy calculations.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class AtomicLibraryTests
{
	/// <summary>The periodic table loaded with default data for testing purposes.</summary>
	private static readonly PeriodicTable.PeriodicTable PeriodicTable = ElementDataLoader.LoadDefault();

	/// <summary>The isotope repository loaded with default data for testing purposes.</summary>
	private static readonly IsotopeRepository Isotopes = IsotopeDataLoader.LoadDefault(PeriodicTable);

	/// <summary>Tests that the periodic table lookup by atomic number and symbol works correctly.</summary>
	[Fact]
	public void PeriodicTableLookupByAtomicNumberAndSymbolWorks()
	{
		Element carbonByNumber = PeriodicTable.GetByAtomicNumber(atomicNumber: 6);
		Element carbonBySymbol = PeriodicTable.GetBySymbol(symbol: "C");
		Assert.Equal(expected: "Carbon", actual: carbonByNumber.NameEnglish);
		Assert.Equal(expected: carbonByNumber.AtomicNumber, actual: carbonBySymbol.AtomicNumber);
		Assert.Equal(expected: 118, actual: PeriodicTable.All.Count);
	}

	/// <summary>Tests that an isotope's mass number must be greater than or equal to its atomic number.</summary>
	[Fact]
	public void IsotopeValidationMassNumberMustBeGreaterOrEqualAtomicNumber()
	{
		Element hydrogen = PeriodicTable.GetBySymbol(symbol: "H");
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => new Isotope(element: hydrogen, massNumber: 0, atomicMass: 1.0, isStable: true, halfLifeSeconds: null, decayMode: DecayMode.None, decayEnergyMeV: null, naturalAbundance: null));
	}

	/// <summary>Tests that the charge and ion flags of an atom are calculated correctly based on the number of electrons compared to the number of protons.</summary>
	[Fact]
	public void AtomChargeAndIonFlagsAreCalculated()
	{
		Isotope oxygen16 = Isotopes.GetByAtomicAndMassNumber(atomicNumber: 8, massNumber: 16);
		Atom neutral = new(isotope: oxygen16, electronCount: 8);
		Atom cation = new(isotope: oxygen16, electronCount: 6);
		Atom anion = new(isotope: oxygen16, electronCount: 10);
		Assert.True(condition: neutral.IsNeutral);
		Assert.Equal(expected: 0, actual: neutral.Charge);
		Assert.True(condition: cation.IsCation);
		Assert.True(condition: cation.IsIon);
		Assert.Equal(expected: 2, actual: cation.Charge);
		Assert.True(condition: anion.IsAnion);
		Assert.True(condition: anion.IsIon);
		Assert.Equal(expected: -2, actual: anion.Charge);
	}

	/// <summary>Tests that isotope lookups validate atomic numbers for throwing APIs and return false for invalid Try lookups.</summary>
	[Fact]
	public void IsotopeRepositoryLookupGuardsInvalidAtomicAndMassNumbers()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => Isotopes.GetByAtomicAndMassNumber(atomicNumber: 0, massNumber: 1));
		_ = Assert.Throws<ArgumentOutOfRangeException>(() => Isotopes.GetByAtomicAndMassNumber(atomicNumber: 1, massNumber: 0));
		Assert.False(condition: Isotopes.TryGetByAtomicAndMassNumber(atomicNumber: 0, massNumber: 1, out Isotope? invalidAtomicNumber));
		Assert.Null(@object: invalidAtomicNumber);
		Assert.False(condition: Isotopes.TryGetByAtomicAndMassNumber(atomicNumber: 1, massNumber: 0, out Isotope? invalidMassNumber));
		Assert.Null(@object: invalidMassNumber);
	}

	/// <summary>Tests that the electron configuration's ToString method produces the expected format.</summary>
	[Fact]
	public void ElectronConfigurationToStringFormatIsExpected()
	{
		ElectronConfiguration configuration = new(
		[
			new Orbital(PrincipalQuantumNumber: 1, Type: OrbitalType.S, ElectronCount: 2),
			new Orbital(PrincipalQuantumNumber: 2, Type: OrbitalType.S, ElectronCount: 2),
			new Orbital(PrincipalQuantumNumber: 2, Type: OrbitalType.P, ElectronCount: 6)
		]);
		Assert.Equal(expected: "1s2 2s2 2p6", actual: configuration.ToString());
	}

	/// <summary>Tests that parsing an electron configuration string and creating one from an atomic number work correctly.</summary>
	[Fact]
	public void ElectronConfigurationParseAndFromAtomicNumberWork()
	{
		ElectronConfiguration parsed = ElectronConfiguration.Parse(configuration: "1s2 2s2 2p6");
		ElectronConfiguration fromNumber = ElectronConfiguration.FromAtomicNumber(atomicNumber: 10);
		Assert.Equal(expected: "1s2 2s2 2p6", actual: parsed.ToString());
		Assert.Equal(expected: 10, actual: fromNumber.ElectronCount);
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => ElectronConfiguration.FromAtomicNumber(atomicNumber: 0));
	}

	/// <summary>Tests that parsing an electron configuration string with unsupported formats throws a FormatException.</summary>
	[Fact]
	public void ElectronConfigurationParseThrowsForUnsupportedFormats()
	{
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "[Ar] 4s2 3d10"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "2x6"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "1s2 invalid 2p6"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "1s3"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "1s2x"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "1sp2"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "1s0"));
		_ = Assert.Throws<FormatException>(static () => ElectronConfiguration.Parse(configuration: "0s2"));
	}

	/// <summary>Tests that loading isotope data with an invalid decay mode includes the isotope context in the exception message.</summary>
	[Fact]
	public void IsotopeDataLoaderInvalidDecayModeIncludesIsotopeContext()
	{
		string path = Path.GetTempFileName();
		try
		{
			File.WriteAllText(path: path, contents: """[{ "AtomicNumber": 6, "MassNumber": 14, "AtomicMass": 14.0032419884, "IsStable": false, "HalfLifeSeconds": 1.0, "DecayMode": "InvalidMode" }]""");
			FormatException exception = Assert.Throws<FormatException>(() => IsotopeDataLoader.LoadFromJson(filePath: path, periodicTable: PeriodicTable));
			Assert.Contains(expectedSubstring: "Z=6, A=14", actualString: exception.Message, comparisonType: StringComparison.Ordinal);

		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Tests that loading element data with invalid enum values includes the element context in the exception message.</summary>
	[Fact]
	public void ElementDataLoaderInvalidEnumsIncludeElementContext()
	{
		string path = Path.GetTempFileName();
		try
		{
			File.WriteAllText(path: path, contents: """[{ "AtomicNumber": 999, "Symbol": "Xx", "NameEnglish": "Test", "NameGerman": "Test", "Period": 1, "Group": 1, "Block": "invalid", "Category": "invalid", "ElectronConfiguration": "1s1" }]""");
			FormatException exception = Assert.Throws<FormatException>(() => ElementDataLoader.LoadFromJson(filePath: path));
			Assert.Contains(expectedSubstring: "Z=999 (Xx)", actualString: exception.Message, comparisonType: StringComparison.Ordinal);
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Tests that loading decay chains with an invalid decay mode includes the context in the exception message.</summary>
	[Fact]
	public void IsotopeDataLoaderLoadDecayChainsInvalidDecayModeIncludesContext()
	{
		string path = Path.GetTempFileName();
		try
		{
			File.WriteAllText(path: path, contents: """[{ "ParentAtomicNumber": 92, "ParentMassNumber": 238, "Steps": [{ "AtomicNumber": 90, "MassNumber": 234, "DecayMode": "InvalidMode" }] }]""");

			FormatException exception = Assert.Throws<FormatException>(() => IsotopeDataLoader.LoadDecayChains(filePath: path));
			Assert.Contains(expectedSubstring: "parent Z=92, A=238, step Z=90, A=234", actualString: exception.Message, comparisonType: StringComparison.Ordinal);
		}
		finally
		{
			File.Delete(path: path);
		}
	}

	/// <summary>Tests that the radioactive decay calculator produces expected values for Carbon-14.</summary>
	[Fact]
	public void RadioactiveDecayCalculatorUsesExpectedValuesForCarbon14()
	{
		const double yearInSeconds = 365.2425 * 24 * 3600;
		double halfLife = 5730 * yearInSeconds;
		double decayConstant = RadioactiveDecayCalculator.DecayConstant(halfLifeSeconds: halfLife);
		double remainingHalfLife = RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: halfLife, timeSeconds: halfLife);
		double activity = RadioactiveDecayCalculator.Activity(atomCount: 1e20, halfLifeSeconds: halfLife);
		Assert.InRange(actual: decayConstant, low: 3.8e-12, high: 3.9e-12);
		Assert.InRange(actual: remainingHalfLife, low: 0.4999, high: 0.5001);
		Assert.InRange(actual: activity, low: 3.8e8, high: 3.9e8);
	}

	/// <summary>Tests that the radioactive decay calculator throws exceptions for invalid arguments.</summary>
	[Fact]
	public void RadioactiveDecayCalculatorGuardsInvalidArguments()
	{
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => RadioactiveDecayCalculator.DecayConstant(halfLifeSeconds: 0));
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => RadioactiveDecayCalculator.RemainingFraction(halfLifeSeconds: 10, timeSeconds: -1));
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => RadioactiveDecayCalculator.RemainingAtoms(initialAtoms: -1, halfLifeSeconds: 10, timeSeconds: 1));
		_ = Assert.Throws<ArgumentOutOfRangeException>(static () => RadioactiveDecayCalculator.Activity(atomCount: -1, halfLifeSeconds: 10));
	}

	/// <summary>Tests that the nuclear binding energy calculator produces a high binding energy per nucleon for Iron-56.</summary>
	[Fact]
	public void NuclearBindingEnergyCalculatorFe56HasHighBindingPerNucleon()
	{
		Isotope fe56 = Isotopes.GetByAtomicAndMassNumber(atomicNumber: 26, massNumber: 56);
		double bindingPerNucleon = NuclearBindingEnergyCalculator.BindingEnergyPerNucleonMeV(isotope: fe56);
		Assert.InRange(actual: bindingPerNucleon, low: 8.0, high: 9.5);
	}

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? string.Empty;
	}
}
