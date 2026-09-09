namespace AtomicLibrary.WinFormsDemo;

/// <summary>Required designer variable.</summary>
partial class Form1
{
	private System.ComponentModel.IContainer components = null!;
	private TextBox txtSearch = null!;
	private Label lblSearch = null!;
	private DataGridView dgvElements = null!;
	private TabControl tabControl = null!;
	private TabPage tabOverview = null!;
	private TabPage tabPeriodicTable = null!;
	private PeriodicTableView periodicTableView = null!;
	private TabPage tabDetails = null!;
	private TableLayoutPanel detailsLayout = null!;
	private TextBox txtElementDetails = null!;
	private Label lblElectronConfiguration = null!;
	private ListView lvIsotopes = null!;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		if (disposing)
		{
			_bindingSource.Dispose();
		}
		base.Dispose(disposing);
	}

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	private void InitializeComponent()
	{
		txtSearch = new TextBox();
		lblSearch = new Label();
		dgvElements = new DataGridView();
		tabControl = new TabControl();
		tabOverview = new TabPage();
		tabPeriodicTable = new TabPage();
		periodicTableView = new PeriodicTableView();
		tabDetails = new TabPage();
		detailsLayout = new TableLayoutPanel();
		txtElementDetails = new TextBox();
		lblElectronConfiguration = new Label();
		lvIsotopes = new ListView();
		((System.ComponentModel.ISupportInitialize)dgvElements).BeginInit();
		tabControl.SuspendLayout();
		tabOverview.SuspendLayout();
		tabPeriodicTable.SuspendLayout();
		tabDetails.SuspendLayout();
		detailsLayout.SuspendLayout();
		SuspendLayout();
		// 
		// txtSearch
		// 
		txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		txtSearch.Location = new Point(70, 12);
		txtSearch.Name = "txtSearch";
		txtSearch.Size = new Size(924, 23);
		txtSearch.TabIndex = 1;
		// 
		// lblSearch
		// 
		lblSearch.AutoSize = true;
		lblSearch.Location = new Point(12, 15);
		lblSearch.Name = "lblSearch";
		lblSearch.Size = new Size(42, 15);
		lblSearch.TabIndex = 2;
		lblSearch.Text = "Suche:";
		// 
		// dgvElements
		// 
		dgvElements.AllowUserToAddRows = false;
		dgvElements.AllowUserToDeleteRows = false;
		dgvElements.AutoGenerateColumns = false;
		dgvElements.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
		dgvElements.Dock = DockStyle.Fill;
		dgvElements.Location = new Point(0, 0);
		dgvElements.MultiSelect = false;
		dgvElements.Name = "dgvElements";
		dgvElements.ReadOnly = true;
		dgvElements.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		dgvElements.Size = new Size(976, 512);
		dgvElements.TabIndex = 0;
		// 
		// tabControl
		// 
		tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
		tabControl.Controls.Add(tabOverview);
		tabControl.Controls.Add(tabPeriodicTable);
		tabControl.Controls.Add(tabDetails);
		tabControl.Location = new Point(12, 45);
		tabControl.Name = "tabControl";
		tabControl.SelectedIndex = 0;
		tabControl.Size = new Size(984, 540);
		tabControl.TabIndex = 0;
		// 
		// tabOverview
		// 
		tabOverview.Controls.Add(dgvElements);
		tabOverview.Location = new Point(4, 24);
		tabOverview.Name = "tabOverview";
		tabOverview.Size = new Size(976, 512);
		tabOverview.TabIndex = 0;
		tabOverview.Text = "Elemente";
		// 
		// tabPeriodicTable
		// 
		tabPeriodicTable.Controls.Add(periodicTableView);
		tabPeriodicTable.Location = new Point(4, 24);
		tabPeriodicTable.Name = "tabPeriodicTable";
		tabPeriodicTable.Size = new Size(976, 512);
		tabPeriodicTable.TabIndex = 1;
		tabPeriodicTable.Text = "Periodensystem";
		// 
		// periodicTableView
		// 
		periodicTableView.Dock = DockStyle.Fill;
		periodicTableView.Location = new Point(0, 0);
		periodicTableView.Name = "periodicTableView";
		periodicTableView.Size = new Size(976, 512);
		periodicTableView.TabIndex = 0;
		// 
		// tabDetails
		// 
		tabDetails.Controls.Add(detailsLayout);
		tabDetails.Location = new Point(4, 24);
		tabDetails.Name = "tabDetails";
		tabDetails.Size = new Size(976, 512);
		tabDetails.TabIndex = 2;
		tabDetails.Text = "Details";
		// 
		// detailsLayout
		// 
		detailsLayout.ColumnCount = 1;
		detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
		detailsLayout.Controls.Add(txtElementDetails, 0, 0);
		detailsLayout.Controls.Add(lblElectronConfiguration, 0, 1);
		detailsLayout.Controls.Add(lvIsotopes, 0, 2);
		detailsLayout.Dock = DockStyle.Fill;
		detailsLayout.Location = new Point(0, 0);
		detailsLayout.Name = "detailsLayout";
		detailsLayout.RowCount = 3;
		detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 140F));
		detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
		detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
		detailsLayout.Size = new Size(976, 512);
		detailsLayout.TabIndex = 0;
		// 
		// txtElementDetails
		// 
		txtElementDetails.BackColor = SystemColors.Control;
		txtElementDetails.BorderStyle = BorderStyle.None;
		txtElementDetails.Dock = DockStyle.Fill;
		txtElementDetails.Location = new Point(3, 3);
		txtElementDetails.Multiline = true;
		txtElementDetails.Name = "txtElementDetails";
		txtElementDetails.ReadOnly = true;
		txtElementDetails.ScrollBars = ScrollBars.Vertical;
		txtElementDetails.Size = new Size(970, 134);
		txtElementDetails.TabIndex = 0;
		txtElementDetails.Text = "Element wählen...";
		// 
		// lblElectronConfiguration
		// 
		lblElectronConfiguration.AutoEllipsis = true;
		lblElectronConfiguration.Dock = DockStyle.Fill;
		lblElectronConfiguration.Location = new Point(3, 140);
		lblElectronConfiguration.Name = "lblElectronConfiguration";
		lblElectronConfiguration.Size = new Size(970, 40);
		lblElectronConfiguration.TabIndex = 1;
		lblElectronConfiguration.TextAlign = ContentAlignment.MiddleLeft;
		// 
		// lvIsotopes
		// 
		lvIsotopes.Dock = DockStyle.Fill;
		lvIsotopes.FullRowSelect = true;
		lvIsotopes.Location = new Point(3, 183);
		lvIsotopes.Name = "lvIsotopes";
		lvIsotopes.ShowItemToolTips = true;
		lvIsotopes.Size = new Size(970, 326);
		lvIsotopes.TabIndex = 2;
		lvIsotopes.UseCompatibleStateImageBehavior = false;
		lvIsotopes.View = View.Details;
		lvIsotopes.Columns.Add("Isotop", 120);
		lvIsotopes.Columns.Add("Stabil", 80);
		lvIsotopes.Columns.Add("Halbwertszeit (s)", 180);
		lvIsotopes.Columns.Add("Zerfall", 120);
		lvIsotopes.Columns.Add("Häufigkeit (%)", 120);
		// 
		// Form1
		// 
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1008, 601);
		Controls.Add(tabControl);
		Controls.Add(txtSearch);
		Controls.Add(lblSearch);
		Name = "Form1";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "AtomicLibrary Demo";
		((System.ComponentModel.ISupportInitialize)dgvElements).EndInit();
		tabControl.ResumeLayout(false);
		tabOverview.ResumeLayout(false);
		tabPeriodicTable.ResumeLayout(false);
		tabDetails.ResumeLayout(false);
		detailsLayout.ResumeLayout(false);
		detailsLayout.PerformLayout();
		ResumeLayout(false);
		PerformLayout();
	}
}
