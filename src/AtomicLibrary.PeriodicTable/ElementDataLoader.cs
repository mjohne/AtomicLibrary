using System.Text.Json;
using AtomicLibrary.Core.Elements;

namespace AtomicLibrary.PeriodicTable;

public static class ElementDataLoader
{
    public static PeriodicTable LoadFromJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var records = JsonSerializer.Deserialize<List<ElementRecord>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        return new PeriodicTable(records.Select(r => new Element
        {
            AtomicNumber = r.AtomicNumber,
            Symbol = r.Symbol,
            NameEnglish = r.NameEnglish,
            NameGerman = r.NameGerman,
            Period = r.Period,
            Group = r.Group,
            Block = Enum.Parse<ElementBlock>(r.Block, ignoreCase: true),
            Category = Enum.Parse<ElementCategory>(r.Category, ignoreCase: true),
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
        }));
    }

    public static PeriodicTable LoadDefault()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "data", "elements.json");
        return LoadFromJson(path);
    }

    private sealed class ElementRecord
    {
        public int AtomicNumber { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string NameEnglish { get; set; } = string.Empty;
        public string NameGerman { get; set; } = string.Empty;
        public int Period { get; set; }
        public int Group { get; set; }
        public string Block { get; set; } = "S";
        public string Category { get; set; } = "Nonmetal";
        public double StandardAtomicWeight { get; set; }
        public double Electronegativity { get; set; }
        public double IonizationEnergy { get; set; }
        public double ElectronAffinity { get; set; }
        public double AtomicRadius { get; set; }
        public double CovalentRadius { get; set; }
        public double Density { get; set; }
        public double MeltingPoint { get; set; }
        public double BoilingPoint { get; set; }
        public int[]? OxidationStates { get; set; }
        public string ElectronConfiguration { get; set; } = string.Empty;
    }
}
