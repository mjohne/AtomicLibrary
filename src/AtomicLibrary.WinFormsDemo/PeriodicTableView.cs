using AtomicLibrary.Core.Elements;

using System.Diagnostics;

namespace AtomicLibrary.WinFormsDemo;

/// <summary>Displays a classic periodic table of elements as a grid of buttons, colored by element category. Raises <see cref="ElementSelected"/> when the user clicks an element.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class PeriodicTableView : UserControl
{
	/// <summary>Number of group columns in the main periodic table grid (groups 1..18).</summary>
	private const int GroupCount = 18;

	/// <summary>Number of rows in the layout: 7 main periods, 1 spacer row, plus 2 rows for the f-block (lanthanides and actinides).</summary>
	private const int RowCount = 10;

	/// <summary>Row index of the spacer between the main table and the f-block rows.</summary>
	private const int SpacerRow = 7;

	/// <summary>Row index of the lanthanide row in the f-block.</summary>
	private const int LanthanideRow = 8;

	/// <summary>Row index of the actinide row in the f-block.</summary>
	private const int ActinideRow = 9;

	/// <summary>The layout panel hosting the element buttons.</summary>
	private readonly TableLayoutPanel _grid;

	/// <summary>Tooltip shown when the user hovers over an element button.</summary>
	private readonly ToolTip _toolTip;

	/// <summary>Raised when the user selects an element by clicking its button.</summary>
	public event EventHandler<ElementSelectedEventArgs>? ElementSelected;

	/// <summary>Initializes a new instance of the <see cref="PeriodicTableView"/> class.</summary>
	public PeriodicTableView()
	{
		_toolTip = new ToolTip();
		_grid = new TableLayoutPanel
		{
			Dock = DockStyle.Fill,
			ColumnCount = GroupCount,
			RowCount = RowCount,
			BackColor = SystemColors.Control,
			Padding = new Padding(all: 4)
		};
		for (int c = 0; c < GroupCount; c++)
		{
			_ = _grid.ColumnStyles.Add(columnStyle: new ColumnStyle(sizeType: SizeType.Percent, width: 100f / GroupCount));
		}
		for (int r = 0; r < RowCount; r++)
		{
			float value = r == SpacerRow ? 40f : 100f;
			_ = _grid.RowStyles.Add(rowStyle: new RowStyle(sizeType: SizeType.Percent, height: value));
		}
		Controls.Add(value: _grid);
	}

	/// <summary>Populates the grid with buttons for every element in the given periodic table.</summary>
	/// <param name="periodicTable">The periodic table whose elements should be rendered.</param>
	public void LoadElements(PeriodicTable.PeriodicTable periodicTable)
	{
		ArgumentNullException.ThrowIfNull(argument: periodicTable);
		_grid.SuspendLayout();
		try
		{
			_grid.Controls.Clear();
			// Number the f-block cells left-to-right in atomic-number order (Ce..Lu, Th..Lr).
			int nextLanthanideColumn = 3;
			int nextActinideColumn = 3;
			foreach (Element element in periodicTable.All.OrderBy(keySelector: static e =>
			{
				ArgumentNullException.ThrowIfNull(argument: e);
				return e.AtomicNumber;
			}))
			{
				(int row, int column) = GetCell(element: element, nextLanthanideColumn: ref nextLanthanideColumn, nextActinideColumn: ref nextActinideColumn);
				if (row < 0 || column < 0)
				{
					continue;
				}
				Button button = CreateElementButton(element: element);
				_grid.Controls.Add(control: button, column: column, row: row);
			}
		}
		finally
		{
			_grid.ResumeLayout(performLayout: true);
		}
	}

	/// <summary>Computes the (row, column) cell for the given element within the periodic table grid.</summary>
	/// <param name="element">The element to place.</param>
	/// <param name="nextLanthanideColumn">The next column to use in the lanthanide row; incremented on placement.</param>
	/// <param name="nextActinideColumn">The next column to use in the actinide row; incremented on placement.</param>
	/// <returns>The row and column indices, or (-1, -1) if the element cannot be placed.</returns>
	private static (int Row, int Column) GetCell(Element element, ref int nextLanthanideColumn, ref int nextActinideColumn)
	{
		// f-block elements have Group == 0 in the data set and are placed in the two rows below the main table.
		if (element.Group == 0)
		{
			if (element.Category == ElementCategory.Lanthanide || (element.Period == 6 && element.Block == ElementBlock.F))
			{
				int column = nextLanthanideColumn++;
				return column >= GroupCount ? (-1, -1) : (LanthanideRow, column);
			}
			if (element.Category == ElementCategory.Actinide || (element.Period == 7 && element.Block == ElementBlock.F))
			{
				int column = nextActinideColumn++;
				return column >= GroupCount ? (-1, -1) : (ActinideRow, column);
			}
			return (-1, -1);
		}
		// Main-table placement: group 1..18 maps to columns 0..17, period 1..7 maps to rows 0..6.
		if (element.Group is < 1 or > GroupCount || element.Period is < 1 or > 7)
		{
			return (-1, -1);
		}
		return (element.Period - 1, element.Group - 1);
	}

	/// <summary>Creates a button representing a single element, with symbol, atomic number, category color, and click/tooltip handlers.</summary>
	/// <param name="element">The element to represent.</param>
	/// <returns>A configured <see cref="Button"/> instance.</returns>
	private Button CreateElementButton(Element element)
	{
		Button button = new()
		{
			Dock = DockStyle.Fill,
			Margin = new Padding(all: 1),
			FlatStyle = FlatStyle.Flat,
			TextAlign = ContentAlignment.MiddleCenter,
			Font = new Font(familyName: "Segoe UI", emSize: 8f, style: FontStyle.Bold),
			BackColor = GetCategoryColor(category: element.Category),
			Text = $"{element.AtomicNumber}\n{element.Symbol}",
			Tag = element,
			UseVisualStyleBackColor = false
		};
		button.FlatAppearance.BorderColor = Color.Gray;
		button.Click += OnElementButtonClick;
		_toolTip.SetToolTip(control: button, caption: $"{element.NameEnglish} / {element.NameGerman}\nZ = {element.AtomicNumber}, {element.StandardAtomicWeight:0.###} u\n{element.Category}");
		return button;
	}

	/// <summary>Handles clicks on element buttons and raises <see cref="ElementSelected"/> with the associated element.</summary>
	/// <param name="sender">The button that was clicked.</param>
	/// <param name="e">The event data.</param>
	private void OnElementButtonClick(object? sender, EventArgs e)
	{
		if (sender is Button { Tag: Element element })
		{
			ElementSelected?.Invoke(sender: this, e: new ElementSelectedEventArgs(element: element));
		}
	}

	/// <summary>Returns the background color used to visualize the given element category in the grid.</summary>
	/// <param name="category">The element category.</param>
	/// <returns>The color associated with <paramref name="category"/>.</returns>
	internal static Color GetCategoryColor(ElementCategory category)
	{
		return category switch
		{
			ElementCategory.AlkaliMetal => Color.LightSalmon,
			ElementCategory.AlkalineEarthMetal => Color.LightGoldenrodYellow,
			ElementCategory.TransitionMetal => Color.LightSteelBlue,
			ElementCategory.PostTransitionMetal => Color.Moccasin,
			ElementCategory.Metalloid => Color.PaleTurquoise,
			ElementCategory.Nonmetal => Color.Honeydew,
			ElementCategory.Halogen => Color.LavenderBlush,
			ElementCategory.NobleGas => Color.Lavender,
			ElementCategory.Lanthanide => Color.LightCyan,
			ElementCategory.Actinide => Color.LightPink,
			_ => Color.White
		};
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="PeriodicTableView"/> and optionally releases the managed resources.</summary>
	/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_toolTip.Dispose();
			_grid.Dispose();
		}
		base.Dispose(disposing: disposing);
	}

	/// <summary>Returns a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? nameof(PeriodicTableView);
	}
}

/// <summary>Provides data for the <see cref="PeriodicTableView.ElementSelected"/> event.</summary>
/// <param name="element">The element that was selected.</param>
public sealed class ElementSelectedEventArgs(Element element) : EventArgs
{
	/// <summary>Gets the element that was selected in the periodic table view.</summary>
	public Element Element { get; } = element;
}
