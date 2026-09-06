using AtomicLibrary.Core.Electrons;
using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Core.Atoms;

public sealed class Atom
{
    public Atom(Isotope isotope, int electronCount)
    {
        Isotope = isotope;
        ElectronCount = electronCount;

        if (electronCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(electronCount));
        }

        ElectronConfiguration = ElectronConfiguration.FromElectronCount(ElectronCount);
    }

    public Isotope Isotope { get; }
    public int ElectronCount { get; }
    public int ProtonCount => Isotope.ProtonCount;
    public int NeutronCount => Isotope.NeutronCount;
    public int Charge => ProtonCount - ElectronCount;
    public bool IsNeutral => Charge == 0;
    public bool IsIon => Charge != 0;
    public bool IsCation => Charge > 0;
    public bool IsAnion => Charge < 0;
    public ElectronConfiguration ElectronConfiguration { get; }

    public override string ToString()
    {
        var chargeText = Charge > 0 ? $"+{Charge}" : Charge.ToString();
        return $"{Isotope.Element.Symbol}-{Isotope.MassNumber} ({chargeText})";
    }
}
