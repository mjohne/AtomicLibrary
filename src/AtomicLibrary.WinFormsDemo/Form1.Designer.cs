namespace AtomicLibrary.WinFormsDemo;

partial class Form1
{
    private System.ComponentModel.IContainer components = null!;
    private TextBox txtSearch = null!;
    private Label lblSearch = null!;
    private DataGridView dgvElements = null!;
    private TabControl tabControl = null!;
    private TabPage tabOverview = null!;
    private TabPage tabDetails = null!;
    private TableLayoutPanel detailsLayout = null!;
    private TextBox txtElementDetails = null!;
    private Label lblElectronConfiguration = null!;
    private ListView lvIsotopes = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        txtSearch = new TextBox();
        lblSearch = new Label();
        dgvElements = new DataGridView();
        tabControl = new TabControl();
        tabOverview = new TabPage();
        tabDetails = new TabPage();
        detailsLayout = new TableLayoutPanel();
        txtElementDetails = new TextBox();
        lblElectronConfiguration = new Label();
        lvIsotopes = new ListView();

        ((System.ComponentModel.ISupportInitialize)dgvElements).BeginInit();
        tabControl.SuspendLayout();
        tabOverview.SuspendLayout();
        tabDetails.SuspendLayout();
        detailsLayout.SuspendLayout();
        SuspendLayout();

        lblSearch.AutoSize = true;
        lblSearch.Location = new Point(12, 15);
        lblSearch.Text = "Suche:";

        txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtSearch.Location = new Point(70, 12);
        txtSearch.Size = new Size(900, 23);

        tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabControl.Location = new Point(12, 45);
        tabControl.Size = new Size(960, 600);
        tabControl.Controls.Add(tabOverview);
        tabControl.Controls.Add(tabDetails);

        tabOverview.Text = "Elemente";
        tabOverview.Controls.Add(dgvElements);

        dgvElements.Dock = DockStyle.Fill;
        dgvElements.ReadOnly = true;
        dgvElements.AllowUserToAddRows = false;
        dgvElements.AllowUserToDeleteRows = false;
        dgvElements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvElements.MultiSelect = false;
        dgvElements.AutoGenerateColumns = false;
        dgvElements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        tabDetails.Text = "Details";
        tabDetails.Controls.Add(detailsLayout);

        detailsLayout.Dock = DockStyle.Fill;
        detailsLayout.RowCount = 3;
        detailsLayout.ColumnCount = 1;
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        txtElementDetails.Dock = DockStyle.Fill;
        txtElementDetails.Multiline = true;
        txtElementDetails.ReadOnly = true;
        txtElementDetails.ScrollBars = ScrollBars.Vertical;
        txtElementDetails.BorderStyle = BorderStyle.None;
        txtElementDetails.BackColor = SystemColors.Control;
        txtElementDetails.Text = "Element wählen...";

        lblElectronConfiguration.Dock = DockStyle.Fill;
        lblElectronConfiguration.AutoSize = false;
        lblElectronConfiguration.AutoEllipsis = true;
        lblElectronConfiguration.TextAlign = ContentAlignment.MiddleLeft;

        lvIsotopes.Dock = DockStyle.Fill;
        lvIsotopes.View = View.Details;
        lvIsotopes.FullRowSelect = true;
        lvIsotopes.Columns.Add("Isotop", 120);
        lvIsotopes.Columns.Add("Stabil", 80);
        lvIsotopes.Columns.Add("Halbwertszeit (s)", 180);
        lvIsotopes.Columns.Add("Zerfall", 120);
        lvIsotopes.Columns.Add("Häufigkeit (%)", 120);

        detailsLayout.Controls.Add(txtElementDetails, 0, 0);
        detailsLayout.Controls.Add(lblElectronConfiguration, 0, 1);
        detailsLayout.Controls.Add(lvIsotopes, 0, 2);

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(984, 661);
        Controls.Add(tabControl);
        Controls.Add(txtSearch);
        Controls.Add(lblSearch);
        Text = "AtomicLibrary Demo";

        ((System.ComponentModel.ISupportInitialize)dgvElements).EndInit();
        tabControl.ResumeLayout(false);
        tabOverview.ResumeLayout(false);
        tabDetails.ResumeLayout(false);
        detailsLayout.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
