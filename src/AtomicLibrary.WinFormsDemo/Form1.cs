using AtomicLibrary.Core.Elements;
using AtomicLibrary.Isotopes;
using AtomicLibrary.PeriodicTable;

namespace AtomicLibrary.WinFormsDemo;

public partial class Form1 : Form
{
    private readonly global::AtomicLibrary.PeriodicTable.PeriodicTable _periodicTable;
    private readonly IsotopeRepository _isotopes;
    private readonly BindingSource _bindingSource = new();

    public Form1()
    {
        InitializeComponent();

        var elementPath = Path.Combine(AppContext.BaseDirectory, "data", "elements.json");
        var isotopePath = Path.Combine(AppContext.BaseDirectory, "data", "isotopes.json");

        _periodicTable = ElementDataLoader.LoadFromJson(elementPath);
        _isotopes = IsotopeDataLoader.LoadFromJson(isotopePath, _periodicTable);

        ConfigureGrid();
        BindElements(_periodicTable.All);

        txtSearch.TextChanged += (_, _) => ApplyFilter();
        dgvElements.SelectionChanged += (_, _) => ShowSelectedElementDetails();
        dgvElements.RowPrePaint += DgvElementsOnRowPrePaint;
    }

    private void ConfigureGrid()
    {
        dgvElements.Columns.Clear();
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.AtomicNumber), HeaderText = "Z" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Symbol), HeaderText = "Symbol" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.NameEnglish), HeaderText = "Name" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Group), HeaderText = "Gruppe" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Period), HeaderText = "Periode" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.Category), HeaderText = "Kategorie" });
        dgvElements.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(ElementRow.StandardAtomicWeight), HeaderText = "Atomgewicht" });

        dgvElements.DataSource = _bindingSource;
    }

    private void BindElements(IEnumerable<Element> elements)
    {
        var rows = elements.OrderBy(e => e.AtomicNumber)
            .Select(e => new ElementRow(e.AtomicNumber, e.Symbol, e.NameEnglish, e.Group, e.Period, e.Category.ToString(), e.StandardAtomicWeight))
            .ToList();

        _bindingSource.DataSource = rows;

        if (dgvElements.Rows.Count > 0)
        {
            dgvElements.Rows[0].Selected = true;
            ShowSelectedElementDetails();
        }
    }

    private void ApplyFilter()
    {
        var text = txtSearch.Text.Trim();
        var filtered = string.IsNullOrWhiteSpace(text)
            ? _periodicTable.All
            : _periodicTable.All.Where(e => e.Symbol.Contains(text, StringComparison.OrdinalIgnoreCase)
                                            || e.NameEnglish.Contains(text, StringComparison.OrdinalIgnoreCase)
                                            || e.NameGerman.Contains(text, StringComparison.OrdinalIgnoreCase));

        BindElements(filtered);
    }

    private void ShowSelectedElementDetails()
    {
        if (dgvElements.SelectedRows.Count == 0)
        {
            return;
        }

        var row = (ElementRow)dgvElements.SelectedRows[0].DataBoundItem!;
        var element = _periodicTable.GetByAtomicNumber(row.AtomicNumber);
        var isotopes = _isotopes.GetByAtomicNumber(element.AtomicNumber).OrderBy(i => i.MassNumber).ToList();

        lblElementDetails.Text =
            $"{element.NameEnglish} / {element.NameGerman} ({element.Symbol})\n" +
            $"Ordnungszahl: {element.AtomicNumber}, Gruppe: {element.Group}, Periode: {element.Period}, Block: {element.Block}\n" +
            $"Kategorie: {element.Category}\n" +
            $"Atomgewicht: {element.StandardAtomicWeight}, Elektronegativität: {element.Electronegativity}, Ionisierungsenergie: {element.IonizationEnergy} eV\n" +
            $"Elektronenaffinität: {element.ElectronAffinity} eV, Radius: {element.AtomicRadius} pm, Kovalenzradius: {element.CovalentRadius} pm\n" +
            $"Dichte: {element.Density} g/cm³, Schmelzpunkt: {element.MeltingPoint} K, Siedepunkt: {element.BoilingPoint} K\n" +
            $"Oxidationsstufen: {string.Join(", ", element.OxidationStates)}";

        lblElectronConfiguration.Text = $"Elektronenkonfiguration: {element.ElectronConfiguration}";

        lvIsotopes.Items.Clear();
        foreach (var isotope in isotopes)
        {
            lvIsotopes.Items.Add(new ListViewItem(
            [
                isotope.ToString(),
                isotope.IsStable ? "Ja" : "Nein",
                isotope.HalfLifeSeconds?.ToString("G6") ?? "-",
                isotope.DecayMode.ToString(),
                isotope.NaturalAbundance?.ToString("G6") ?? "-"
            ]));
        }
    }

    private void DgvElementsOnRowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        var row = dgvElements.Rows[e.RowIndex];
        if (row.DataBoundItem is not ElementRow elementRow)
        {
            return;
        }

        var element = _periodicTable.GetByAtomicNumber(elementRow.AtomicNumber);
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

    private sealed record ElementRow(
        int AtomicNumber,
        string Symbol,
        string NameEnglish,
        int Group,
        int Period,
        string Category,
        double StandardAtomicWeight);
}
