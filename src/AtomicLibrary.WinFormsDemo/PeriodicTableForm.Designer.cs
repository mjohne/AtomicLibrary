namespace AtomicLibrary.WinFormsDemo;

/// <summary>Required designer support for <see cref="PeriodicTableForm"/>.</summary>
partial class PeriodicTableForm
{
	private System.ComponentModel.IContainer components = null!;
	private SplitContainer splitContainer = null!;
	private TableLayoutPanel periodicTableLayout = null!;
	private Label lblSelectedElement = null!;
	private TextBox txtElementDetails = null!;
	private ToolTip elementToolTip = null!;

	/// <summary>Clean up any resources being used.</summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose(bool disposing)
	{
		if (disposing && (components != null))
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	/// <summary>Required method for designer support - do not modify the contents of this method with the code editor.</summary>
	private void InitializeComponent()
	{
		components = new System.ComponentModel.Container();
		splitContainer = new SplitContainer();
		periodicTableLayout = new TableLayoutPanel();
		lblSelectedElement = new Label();
		txtElementDetails = new TextBox();
		elementToolTip = new ToolTip(components);
		((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
		splitContainer.Panel1.SuspendLayout();
		splitContainer.Panel2.SuspendLayout();
		splitContainer.SuspendLayout();
		SuspendLayout();
		splitContainer.Dock = DockStyle.Fill;
		splitContainer.FixedPanel = FixedPanel.Panel2;
		splitContainer.Location = new Point(0, 0);
		splitContainer.Name = "splitContainer";
		splitContainer.Panel1.Controls.Add(periodicTableLayout);
		splitContainer.Panel2.Controls.Add(txtElementDetails);
		splitContainer.Panel2.Controls.Add(lblSelectedElement);
		splitContainer.Size = new Size(1264, 661);
		splitContainer.SplitterDistance = 920;
		splitContainer.TabIndex = 0;
		periodicTableLayout.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
		periodicTableLayout.ColumnCount = 18;
		for (int column = 0; column < periodicTableLayout.ColumnCount; column++)
		{
			periodicTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / periodicTableLayout.ColumnCount));
		}
		periodicTableLayout.Dock = DockStyle.Fill;
		periodicTableLayout.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point);
		periodicTableLayout.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
		periodicTableLayout.Location = new Point(0, 0);
		periodicTableLayout.Margin = new Padding(6);
		periodicTableLayout.Name = "periodicTableLayout";
		periodicTableLayout.RowCount = 10;
		for (int row = 0; row < 7; row++)
		{
			periodicTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / 9F));
		}
		periodicTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F));
		periodicTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / 9F));
		periodicTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / 9F));
		periodicTableLayout.Size = new Size(920, 661);
		periodicTableLayout.TabIndex = 0;
		lblSelectedElement.Dock = DockStyle.Top;
		lblSelectedElement.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point);
		lblSelectedElement.Location = new Point(0, 0);
		lblSelectedElement.Name = "lblSelectedElement";
		lblSelectedElement.Padding = new Padding(8);
		lblSelectedElement.Size = new Size(340, 52);
		lblSelectedElement.TabIndex = 0;
		lblSelectedElement.Text = "Element wählen...";
		txtElementDetails.BackColor = SystemColors.Control;
		txtElementDetails.BorderStyle = BorderStyle.None;
		txtElementDetails.Dock = DockStyle.Fill;
		txtElementDetails.Location = new Point(0, 52);
		txtElementDetails.Multiline = true;
		txtElementDetails.Name = "txtElementDetails";
		txtElementDetails.ReadOnly = true;
		txtElementDetails.ScrollBars = ScrollBars.Vertical;
		txtElementDetails.Size = new Size(340, 609);
		txtElementDetails.TabIndex = 1;
		txtElementDetails.Text = "Wählen Sie ein Element im Periodensystem aus.";
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(1264, 661);
		Controls.Add(splitContainer);
		MinimumSize = new Size(980, 520);
		Name = "PeriodicTableForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Periodensystem";
		splitContainer.Panel1.ResumeLayout(false);
		splitContainer.Panel2.ResumeLayout(false);
		splitContainer.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
		splitContainer.ResumeLayout(false);
		ResumeLayout(false);
	}
}
