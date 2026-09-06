using AtomicLibrary.Core.Elements;

namespace AtomicLibrary.PeriodicTable;

public sealed class PeriodicTable
{
    private readonly IReadOnlyDictionary<int, Element> _byAtomicNumber;
    private readonly IReadOnlyDictionary<string, Element> _bySymbol;

    public PeriodicTable(IEnumerable<Element> elements)
    {
        var list = elements.OrderBy(e => e.AtomicNumber).ToList();
        All = list;
        _byAtomicNumber = list.ToDictionary(e => e.AtomicNumber);
        _bySymbol = list.ToDictionary(e => e.Symbol, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<Element> All { get; }

    public Element GetByAtomicNumber(int atomicNumber) => _byAtomicNumber[atomicNumber];

    public Element GetBySymbol(string symbol) => _bySymbol[symbol];

    public bool TryGetByAtomicNumber(int atomicNumber, out Element? element) => _byAtomicNumber.TryGetValue(atomicNumber, out element);

    public bool TryGetBySymbol(string symbol, out Element? element) => _bySymbol.TryGetValue(symbol, out element);
}
