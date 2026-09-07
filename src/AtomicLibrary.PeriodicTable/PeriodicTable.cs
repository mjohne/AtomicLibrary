using AtomicLibrary.Core.Elements;

namespace AtomicLibrary.PeriodicTable;

/// <summary>Represents the periodic table of elements, providing access to elements by atomic number and symbol.</summary>
public sealed class PeriodicTable
{
	/// <summary>A dictionary mapping atomic numbers to their corresponding elements for quick lookup.</summary>
	private readonly IReadOnlyDictionary<int, Element> _byAtomicNumber;

	/// <summary>A dictionary mapping element symbols to their corresponding elements for quick lookup.</summary>
	private readonly IReadOnlyDictionary<string, Element> _bySymbol;

	/// <summary>Initializes a new instance of the <see cref="PeriodicTable"/> class with the specified elements.</summary>
	/// <param name="elements">The elements to include in the periodic table.</param>
	public PeriodicTable(IEnumerable<Element> elements)
	{
		List<Element> list = [.. elements.OrderBy(keySelector: static e =>
		{
			ArgumentNullException.ThrowIfNull(argument: e);
			return e.AtomicNumber;
		})];
		All = list;
		_byAtomicNumber = list.ToDictionary(keySelector: static e =>
		{
			ArgumentNullException.ThrowIfNull(argument: e);
			return e.AtomicNumber;
		});
		_bySymbol = list.ToDictionary(keySelector: static e =>
		{
			ArgumentNullException.ThrowIfNull(argument: e);
			return e.Symbol;
		}, comparer: StringComparer.OrdinalIgnoreCase);
	}

	/// <summary>Gets a read-only list of all elements in the periodic table.</summary>
	public IReadOnlyList<Element> All { get; }

	/// <summary>Gets the element with the specified atomic number.</summary>
	/// <param name="atomicNumber">The atomic number of the element to retrieve.</param>
	/// <returns>The element with the specified atomic number.</returns>
	public Element GetByAtomicNumber(int atomicNumber)
	{
		return _byAtomicNumber[key: atomicNumber];
	}

	/// <summary>Gets the element with the specified symbol.</summary>
	/// <param name="symbol">The symbol of the element to retrieve.</param>
	/// <returns>The element with the specified symbol.</returns>
	public Element GetBySymbol(string symbol)
	{
		return _bySymbol[key: symbol];
	}

	/// <summary>Tries to get the element with the specified atomic number.</summary>
	/// <param name="atomicNumber">The atomic number of the element to retrieve.</param>
	/// <param name="element">When this method returns, contains the element with the specified atomic number, if found; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the element was found; otherwise, <c>false</c>.</returns>
	public bool TryGetByAtomicNumber(int atomicNumber, out Element? element)
	{
		return _byAtomicNumber.TryGetValue(key: atomicNumber, value: out element);
	}

	/// <summary>Tries to get the element with the specified symbol.</summary>
	/// <param name="symbol">The symbol of the element to retrieve.</param>
	/// <param name="element">When this method returns, contains the element with the specified symbol, if found; otherwise, <c>null</c>.</param>
	/// <returns><c>true</c> if the element was found; otherwise, <c>false</c>.</returns>
	public bool TryGetBySymbol(string symbol, out Element? element)
	{
		return _bySymbol.TryGetValue(key: symbol, value: out element);
	}
}
