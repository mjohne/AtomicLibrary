using AtomicLibrary.Core.Elements;
using AtomicLibrary.Core.Isotopes;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;

using System.Diagnostics;
using System.Globalization;

namespace AtomicLibrary.WinFormsDemo;

/// <summary>Represents the main form of the AtomicLibrary WinForms demo application, which displays a periodic table of elements and their isotopes.</summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal partial class Form1 : Form
{
	/// <summary>The periodic table instance containing all elements and their properties.</summary>
	private readonly PeriodicTable.PeriodicTable _periodicTable;

	/// <summary>The isotope repository instance containing all isotopes and their properties.</summary>
	private readonly IsotopeRepository _isotopes;

	/// <summary>The binding source for the DataGridView.</summary>
	private readonly BindingSource _bindingSource = [];

	/// <summary>Gets a string representation of the current instance for debugging purposes.</summary>
	/// <returns>A string representation of the current instance.</returns>
	private string GetDebuggerDisplay()
	{
		return ToString();
	}


	/// <summary>Initializes a new instance of the <see cref="Form1"/> class.</summary>
	public Form1()
	{
		InitializeComponent();
		// Load element and isotope data from JSON files located in the "data" directory relative to the application's base directory.
		string elementPath = Path.Combine(path1: AppContext.BaseDirectory, path2: "data", path3: "elements.json");
		string isotopePath = Path.Combine(path1: AppContext.BaseDirectory, path2: "data", path3: "isotopes.json");
		// Load the periodic table and isotopes from the specified JSON files.
		_periodicTable = ElementDataLoader.LoadFromJson(filePath: elementPath);
		_isotopes = IsotopeDataLoader.LoadFromJson(filePath: isotopePath, periodicTable: _periodicTable);
		// Configure the DataGridView to display the elements and bind the elements to the grid.
		ConfigureGrid();
		BindElements(elements: _periodicTable.All);
		// Populate the periodic table view with the loaded elements and route clicks back to the details view.
		periodicTableView.LoadElements(periodicTable: _periodicTable);
		periodicTableView.ElementSelected += OnPeriodicTableElementSelected;
		// Attach event handlers for search text changes, selection changes in the DataGridView, and row pre-painting to customize row appearance.
		txtSearch.TextChanged += (_, _) => ApplyFilter();
		dgvElements.SelectionChanged += (_, _) => ShowSelectedElementDetails();
		dgvElements.RowPrePaint += DgvElementsOnRowPrePaint;
	}

	/// <summary>Configures the DataGridView to display the elements with appropriate columns and data bindings.</summary>
	private void ConfigureGrid()
	{
		dgvElements.Columns.Clear();
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.AtomicNumber), HeaderText = "Z" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Symbol), HeaderText = "Symbol" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.NameEnglish), HeaderText = "Name" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Group), HeaderText = "Gruppe" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Period), HeaderText = "Periode" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Category), HeaderText = "Kategorie" });
		_ = dgvElements.Columns.Add(dataGridViewColumn: new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.StandardAtomicWeight), HeaderText = "Atomgewicht" });
		dgvElements.DataSource = _bindingSource;
	}

	/// <summary>Binds the given elements to the DataGridView.</summary>
	/// <param name="elements">The elements to bind.</param>
	private void BindElements(IEnumerable<Element> elements)
	{
		List<ElementRow> rows = [.. elements.OrderBy(keySelector: static e =>
		{
			ArgumentNullException.ThrowIfNull(argument: e);
			return e.AtomicNumber;
		}).Select(static e => new ElementRow(AtomicNumber: e.AtomicNumber, Symbol: e.Symbol, NameEnglish: e.NameEnglish, Group: e.Group, Period: e.Period, Category: e.Category.ToString(), StandardAtomicWeight: e.StandardAtomicWeight))];
		_bindingSource.DataSource = rows;
		if (dgvElements.Rows.Count > 0)
		{
			dgvElements.Rows[index: 0].Selected = true;
			ShowSelectedElementDetails();
		}
	}

	/// <summary>Applies the filter based on the search text and updates the DataGridView with the filtered elements.</summary>
	private void ApplyFilter()
	{
		string text = txtSearch.Text.Trim();
		IEnumerable<Element> filtered = string.IsNullOrWhiteSpace(value: text)
			? _periodicTable.All
			: _periodicTable.All.Where(predicate: e =>
			{
				ArgumentNullException.ThrowIfNull(argument: e);
				return e.Symbol.Contains(value: text, comparisonType: StringComparison.OrdinalIgnoreCase)
											|| e.NameEnglish.Contains(value: text, comparisonType: StringComparison.OrdinalIgnoreCase)
											|| e.NameGerman.Contains(value: text, comparisonType: StringComparison.OrdinalIgnoreCase);
			});
		BindElements(elements: filtered);
	}

	/// <summary>Displays the details of the selected element in the UI.</summary>
	private void ShowSelectedElementDetails()
	{
		if (dgvElements.SelectedRows.Count == 0 || dgvElements.SelectedRows[index: 0].DataBoundItem is not ElementRow row)
		{
			return;
		}
		Element element = _periodicTable.GetByAtomicNumber(atomicNumber: row.AtomicNumber);
		List<Isotope> isotopes = [.. _isotopes.GetByAtomicNumber(atomicNumber: element.AtomicNumber).OrderBy(keySelector: static i => i.MassNumber)];
		txtElementDetails.Text =
			$"{element.NameEnglish} / {element.NameGerman} ({element.Symbol})\n" +
			$"Ordnungszahl: {element.AtomicNumber}, Gruppe: {element.Group}, Periode: {element.Period}, Block: {element.Block}\n" +
			$"Kategorie: {element.Category}\n" +
			$"Atomgewicht: {element.StandardAtomicWeight}, Elektronegativität: {element.Electronegativity}, Ionisierungsenergie: {element.IonizationEnergy} eV\n" +
			$"Elektronenaffinität: {element.ElectronAffinity} eV, Radius: {element.AtomicRadius} pm, Kovalenzradius: {element.CovalentRadius} pm\n" +
			$"Dichte: {element.Density} g/cm³, Schmelzpunkt: {element.MeltingPoint} K, Siedepunkt: {element.BoilingPoint} K\n" +
			$"Oxidationsstufen: {string.Join(separator: ", ", values: element.OxidationStates)}";
		lblElectronConfiguration.Text = $"Elektronenkonfiguration: {element.ElectronConfiguration}";
		lvIsotopes.BeginUpdate();
		try
		{
			lvIsotopes.Items.Clear();
			foreach (Isotope isotope in isotopes)
			{
				_ = lvIsotopes.Items.Add(value: new ListViewItem(
				[
					isotope.ToString(),
					isotope.IsStable ? "Ja" : "Nein",
					isotope.HalfLifeSeconds?.ToString(format: "G6", provider: CultureInfo.InvariantCulture) ?? "-",
					isotope.DecayMode.ToString(),
					isotope.NaturalAbundance?.ToString(format: "G6", provider: CultureInfo.InvariantCulture) ?? "-"
				]));
			}
		}
		finally
		{
			lvIsotopes.EndUpdate();
		}
	}

	/// <summary>Handles an element being selected in the periodic table view: clears the search filter, selects the matching grid row, and switches to the details tab.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The event data containing the selected element.</param>
	private void OnPeriodicTableElementSelected(object? sender, ElementSelectedEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(argument: e);
		Element element = e.Element;
		if (!string.IsNullOrEmpty(value: txtSearch.Text))
		{
			txtSearch.Text = string.Empty;
		}
		foreach (DataGridViewRow row in dgvElements.Rows)
		{
			if (row.DataBoundItem is ElementRow elementRow && elementRow.AtomicNumber == element.AtomicNumber)
			{
				row.Selected = true;
				dgvElements.CurrentCell = row.Cells[0];
				break;
			}
		}
		tabControl.SelectedTab = tabDetails;
	}

	/// <summary>Handles the RowPrePaint event of the DataGridView to set the background color of each row based on the element's category.</summary>
	/// <param name="e">The <see cref="DataGridViewRowPrePaintEventArgs"/> instance containing the event data.</param>
	/// <param name="sender">The source of the event.</param>
	private void DgvElementsOnRowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
	{
		DataGridViewRow row = dgvElements.Rows[index: e.RowIndex];
		if (row.DataBoundItem is not ElementRow elementRow)
		{
			return;
		}
		Element element = _periodicTable.GetByAtomicNumber(atomicNumber: elementRow.AtomicNumber);
		row.DefaultCellStyle.BackColor = element.Category switch
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

	/// <summary>Represents a row in the DataGridView for displaying element information.</summary>
	/// <param name="AtomicNumber">The atomic number of the element.</param>
	/// <param name="Symbol">The symbol of the element.</param>
	/// <param name="NameEnglish">The English name of the element.</param>
	/// <param name="Group">The group of the element.</param>
	/// <param name="Period">The period of the element.</param>
	/// <param name="Category">The category of the element.</param>
	/// <param name="StandardAtomicWeight">The standard atomic weight of the element.</param>
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	private sealed record ElementRow(
		int AtomicNumber,
		string Symbol,
		string NameEnglish,
		int Group,
		int Period,
		string Category,
		double StandardAtomicWeight)
	{
		/// <summary>Returns a string representation of the current instance for debugging purposes.</summary>
		/// <returns>A string representation of the current instance.</returns>
		private string GetDebuggerDisplay()
		{
			return ToString() ?? string.Empty;
		}
	}
}
