namespace AtomicLibrary.Core.Elements;

public sealed class Element
{
    public required int AtomicNumber { get; init; }
    public required string Symbol { get; init; }
    public required string NameEnglish { get; init; }
    public required string NameGerman { get; init; }
    public required int Period { get; init; }
    public required int Group { get; init; }
    public required ElementBlock Block { get; init; }
    public required ElementCategory Category { get; init; }
    public required double StandardAtomicWeight { get; init; }
    public required double Electronegativity { get; init; }
    public required double IonizationEnergy { get; init; }
    public required double ElectronAffinity { get; init; }
    public required double AtomicRadius { get; init; }
    public required double CovalentRadius { get; init; }
    public required double Density { get; init; }
    public required double MeltingPoint { get; init; }
    public required double BoilingPoint { get; init; }
    public required int[] OxidationStates { get; init; }
    public required string ElectronConfiguration { get; init; }

    public bool IsMetal => Category is ElementCategory.AlkaliMetal
        or ElementCategory.AlkalineEarthMetal
        or ElementCategory.TransitionMetal
        or ElementCategory.PostTransitionMetal
        or ElementCategory.Lanthanide
        or ElementCategory.Actinide;

    public bool IsNonMetal => Category is ElementCategory.Nonmetal
        or ElementCategory.Halogen
        or ElementCategory.NobleGas;

    public override string ToString() => $"{Symbol} ({AtomicNumber})";
}
