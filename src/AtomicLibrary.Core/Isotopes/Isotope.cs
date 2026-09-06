using AtomicLibrary.Core.Elements;

namespace AtomicLibrary.Core.Isotopes;

public sealed class Isotope
{
    public Isotope(
        Element element,
        int massNumber,
        double atomicMass,
        bool isStable,
        double? halfLifeSeconds,
        DecayMode decayMode,
        double? decayEnergyMeV,
        double? naturalAbundance)
    {
        Element = element;
        MassNumber = massNumber;
        AtomicMass = atomicMass;
        IsStable = isStable;
        HalfLifeSeconds = halfLifeSeconds;
        DecayMode = decayMode;
        DecayEnergyMeV = decayEnergyMeV;
        NaturalAbundance = naturalAbundance;

        if (MassNumber < Element.AtomicNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(massNumber), "Mass number must be >= atomic number.");
        }
    }

    public Element Element { get; }
    public int MassNumber { get; }
    public int ProtonCount => Element.AtomicNumber;
    public int NeutronCount => MassNumber - Element.AtomicNumber;
    public double AtomicMass { get; }
    public bool IsStable { get; }
    public bool IsRadioactive => !IsStable;
    public double? HalfLifeSeconds { get; }
    public DecayMode DecayMode { get; }
    public double? DecayEnergyMeV { get; }
    public double? NaturalAbundance { get; }

    public override string ToString() => $"{Element.Symbol}-{MassNumber}";
}
