using AtomicLibrary.Core.Isotopes;

namespace AtomicLibrary.Isotopes;

public sealed class IsotopeRepository
{
    private readonly IReadOnlyDictionary<(int AtomicNumber, int MassNumber), Isotope> _byKey;

    public IsotopeRepository(IEnumerable<Isotope> isotopes)
    {
        var list = isotopes.OrderBy(i => i.ProtonCount).ThenBy(i => i.MassNumber).ToList();
        All = list;
        _byKey = list.ToDictionary(i => (i.ProtonCount, i.MassNumber));
    }

    public IReadOnlyList<Isotope> All { get; }

    public Isotope GetByAtomicAndMassNumber(int atomicNumber, int massNumber) => _byKey[(atomicNumber, massNumber)];

    public IEnumerable<Isotope> GetByAtomicNumber(int atomicNumber) => All.Where(i => i.ProtonCount == atomicNumber);

    public bool TryGetByAtomicAndMassNumber(int atomicNumber, int massNumber, out Isotope? isotope) =>
        _byKey.TryGetValue((atomicNumber, massNumber), out isotope);
}
