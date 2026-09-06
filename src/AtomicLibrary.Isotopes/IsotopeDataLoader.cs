using System.Text.Json;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.PeriodicTable;

namespace AtomicLibrary.Isotopes;

public static class IsotopeDataLoader
{
    public static IsotopeRepository LoadFromJson(string filePath, global::AtomicLibrary.PeriodicTable.PeriodicTable periodicTable)
    {
        var json = File.ReadAllText(filePath);
        var records = JsonSerializer.Deserialize<List<IsotopeRecord>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        var isotopes = records.Select(record =>
        {
            var element = periodicTable.GetByAtomicNumber(record.AtomicNumber);
            if (!Enum.TryParse<DecayMode>(record.DecayMode, ignoreCase: true, out var decayMode))
            {
                throw new FormatException($"Invalid decay mode '{record.DecayMode}' for isotope Z={record.AtomicNumber}, A={record.MassNumber}.");
            }

            return new Isotope(
                element,
                record.MassNumber,
                record.AtomicMass,
                record.IsStable,
                record.HalfLifeSeconds,
                decayMode,
                record.DecayEnergyMeV,
                record.NaturalAbundance);
        });

        return new IsotopeRepository(isotopes);
    }

    public static IReadOnlyList<DecayChain> LoadDecayChains(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var records = JsonSerializer.Deserialize<List<DecayChainRecord>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        return records.Select(r => new DecayChain
        {
            ParentAtomicNumber = r.ParentAtomicNumber,
            ParentMassNumber = r.ParentMassNumber,
            Steps = r.Steps.Select(s => new DecayChainStep
            {
                AtomicNumber = s.AtomicNumber,
                MassNumber = s.MassNumber,
                DecayMode = Enum.Parse<DecayMode>(s.DecayMode, ignoreCase: true)
            }).ToList()
        }).ToList();
    }

    public static IsotopeRepository LoadDefault(global::AtomicLibrary.PeriodicTable.PeriodicTable periodicTable)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "data", "isotopes.json");
        return LoadFromJson(path, periodicTable);
    }

    private sealed class IsotopeRecord
    {
        public int AtomicNumber { get; set; }
        public int MassNumber { get; set; }
        public double AtomicMass { get; set; }
        public bool IsStable { get; set; }
        public double? HalfLifeSeconds { get; set; }
        public string DecayMode { get; set; } = "None";
        public double? DecayEnergyMeV { get; set; }
        public double? NaturalAbundance { get; set; }
    }

    private sealed class DecayChainRecord
    {
        public int ParentAtomicNumber { get; set; }
        public int ParentMassNumber { get; set; }
        public List<DecayChainStepRecord> Steps { get; set; } = [];
    }

    private sealed class DecayChainStepRecord
    {
        public int AtomicNumber { get; set; }
        public int MassNumber { get; set; }
        public string DecayMode { get; set; } = "None";
    }
}
