using AtomicLibrary.Core.Elements;

using System.Diagnostics;

namespace AtomicLibrary.WinFormsDemo;

/// <summary>Displays the periodic table in the classical grid layout.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal sealed partial class PeriodicTableForm : Form
{
	private const int MainPeriodRowCount = 7;
	private const int LanthanideRowIndex = 8;
	private const int ActinideRowIndex = 9;
	private readonly AtomicLibrary.PeriodicTable.PeriodicTable _periodicTable;

	/// <summary>Initializes a new instance of the <see cref="PeriodicTableForm"/> class.</summary>
	/// <param name="periodicTable">The periodic table to render.</param>
	public PeriodicTableForm(AtomicLibrary.PeriodicTable.PeriodicTable periodicTable)
	{
		_periodicTable = periodicTable ?? throw new ArgumentNullException(paramName: nameof(periodicTable));
		InitializeComponent();
		BuildPeriodicTable();
		Element? firstElement = _periodicTable.All.OrderBy(keySelector: static element => element.AtomicNumber).FirstOrDefault();
		if (firstElement is not null)
		{
			ShowElementDetails(element: firstElement);
		}
	}

	/// <summary>Builds the periodic table grid dynamically from the loaded element data.</summary>
	private void BuildPeriodicTable()
	{
		periodicTableLayout.SuspendLayout();
		try
		{
			AddSeriesLabels();

			foreach (Element element in _periodicTable.All)
			{
				(int column, int row) = GetGridPosition(element: element);
				Button button = CreateElementButton(element: element);
				periodicTableLayout.Controls.Add(button, column, row);
			}
		}
		finally
		{
			periodicTableLayout.ResumeLayout(performLayout: true);
		}
	}

	/// <summary>Creates an element button for the specified element.</summary>
	/// <param name="element">The element to render.</param>
	/// <returns>The configured button.</returns>
	private Button CreateElementButton(Element element)
	{
		Color backColor = ElementCategoryColorMapper.GetBackColor(category: element.Category);
		Button button = new()
		{
			BackColor = backColor,
			Dock = DockStyle.Fill,
			ForeColor = ElementCategoryColorMapper.GetForeColor(category: element.Category),
			Margin = new Padding(all: 2),
			MinimumSize = new Size(width: 48, height: 48),
			Tag = element,
			Text = $"{element.AtomicNumber}{Environment.NewLine}{element.Symbol}",
			TextAlign = ContentAlignment.MiddleCenter,
			UseVisualStyleBackColor = false
		};
		button.FlatAppearance.BorderColor = ControlPaint.Dark(baseColor: backColor);
		button.Click += ElementButtonOnClick;
		elementToolTip.SetToolTip(control: button, caption: $"{element.NameEnglish} / {element.NameGerman}{Environment.NewLine}{element.Category}");
		return button;
	}

	/// <summary>Adds labels for the separated f-block series rows.</summary>
	private void AddSeriesLabels()
	{
		Label lanthanideLabel = CreateSeriesLabel(text: "Lanthanoide");
		Label actinideLabel = CreateSeriesLabel(text: "Actinoide");
		periodicTableLayout.Controls.Add(lanthanideLabel, 0, LanthanideRowIndex);
		periodicTableLayout.Controls.Add(actinideLabel, 0, ActinideRowIndex);
		periodicTableLayout.SetColumnSpan(control: lanthanideLabel, value: 2);
		periodicTableLayout.SetColumnSpan(control: actinideLabel, value: 2);
	}

	/// <summary>Creates a label for one of the separated element series.</summary>
	/// <param name="text">The label text.</param>
	/// <returns>The configured label.</returns>
	private static Label CreateSeriesLabel(string text)
	{
		return new Label
		{
			AutoSize = false,
			Dock = DockStyle.Fill,
			Margin = new Padding(left: 2, top: 2, right: 4, bottom: 2),
			Text = text,
			TextAlign = ContentAlignment.MiddleRight
		};
	}

	/// <summary>Shows the selected element's details in the side panel.</summary>
	/// <param name="element">The selected element.</param>
	private void ShowElementDetails(Element element)
	{
		lblSelectedElement.Text = $"{element.Symbol} — {element.NameEnglish} / {element.NameGerman}";
		txtElementDetails.Text =
			$"Ordnungszahl: {element.AtomicNumber}{Environment.NewLine}" +
			$"Symbol: {element.Symbol}{Environment.NewLine}" +
			$"Name (DE): {element.NameGerman}{Environment.NewLine}" +
			$"Name (EN): {element.NameEnglish}{Environment.NewLine}" +
			$"Gruppe: {element.Group}{Environment.NewLine}" +
			$"Periode: {element.Period}{Environment.NewLine}" +
			$"Block: {element.Block}{Environment.NewLine}" +
			$"Kategorie: {element.Category}{Environment.NewLine}" +
			$"Standardatomgewicht: {element.StandardAtomicWeight}{Environment.NewLine}" +
			$"Elektronenkonfiguration: {element.ElectronConfiguration}";
	}

	/// <summary>Gets the grid position for the specified element.</summary>
	/// <param name="element">The element to position.</param>
	/// <returns>The zero-based column and row indices.</returns>
	private static (int Column, int Row) GetGridPosition(Element element)
	{
		if (TryGetSeriesPosition(element: element, out int column, out int row))
		{
			return (Column: column, Row: row);
		}
		ArgumentOutOfRangeException.ThrowIfLessThan(value: element.Group, other: 1, paramName: nameof(element.Group));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: element.Group, other: 18, paramName: nameof(element.Group));
		ArgumentOutOfRangeException.ThrowIfLessThan(value: element.Period, other: 1, paramName: nameof(element.Period));
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value: element.Period, other: MainPeriodRowCount, paramName: nameof(element.Period));
		return (Column: element.Group - 1, Row: element.Period - 1);
	}

	/// <summary>Determines whether the element is displayed in one of the separated lower rows.</summary>
	/// <param name="element">The element to position.</param>
	/// <param name="column">The zero-based target column.</param>
	/// <param name="row">The zero-based target row.</param>
	/// <returns><c>true</c> if the element belongs to a separated series row; otherwise, <c>false</c>.</returns>
	private static bool TryGetSeriesPosition(Element element, out int column, out int row)
	{
		if (element.Period == 6 && element.AtomicNumber is >= 57 and <= 71)
		{
			column = element.AtomicNumber - 55;
			row = LanthanideRowIndex;
			return true;
		}
		if (element.Period == 7 && element.AtomicNumber is >= 89 and <= 103)
		{
			column = element.AtomicNumber - 87;
			row = ActinideRowIndex;
			return true;
		}
		column = -1;
		row = -1;
		return false;
	}

	/// <summary>Handles clicks on dynamically created element buttons.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data.</param>
	private void ElementButtonOnClick(object? sender, EventArgs e)
	{
		if (sender is Button { Tag: Element element })
		{
			ShowElementDetails(element: element);
		}
	}

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString() ?? string.Empty;
	}
}
