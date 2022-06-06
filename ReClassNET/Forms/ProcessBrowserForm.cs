// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.ProcessBrowserForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class ProcessBrowserForm : IconForm
  {
    private static readonly string[] commonProcesses = new string[11]
    {
      "[system process]",
      "system",
      "svchost.exe",
      "services.exe",
      "wininit.exe",
      "smss.exe",
      "csrss.exe",
      "lsass.exe",
      "winlogon.exe",
      "wininit.exe",
      "dwm.exe"
    };
    private const string NoPreviousProcess = "No previous process";
    private IContainer components;
    private DataGridView processDataGridView;
    private CheckBox filterCheckBox;
    private Button refreshButton;
    private Button attachToProcessButton;
    private CheckBox loadSymbolsCheckBox;
    private DataGridViewImageColumn iconColumn;
    private DataGridViewTextBoxColumn processNameColumn;
    private DataGridViewTextBoxColumn pidColumn;
    private DataGridViewTextBoxColumn pathColumn;
    private GroupBox filterGroupBox;
    private LinkLabel previousProcessLinkLabel;
    private Label label2;
    private Label label1;
    private TextBox filterTextBox;
    private BannerBox bannerBox;

    public ProcessInfo SelectedProcess
    {
      get
      {
        if (!(this.processDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>()?.DataBoundItem is DataRowView dataBoundItem))
          return (ProcessInfo) null;
        DataRow row = dataBoundItem.Row;
        return row == null ? (ProcessInfo) null : row.Field<ProcessInfo>("info");
      }
    }

    public bool LoadSymbols
    {
      get
      {
        return this.loadSymbolsCheckBox.Checked;
      }
    }

    public ProcessBrowserForm(string previousProcess)
    {
      this.InitializeComponent();
      this.processDataGridView.AutoGenerateColumns = false;
      if (NativeMethods.IsUnix())
        this.iconColumn.Visible = false;
      this.previousProcessLinkLabel.Text = string.IsNullOrEmpty(previousProcess) ? "No previous process" : previousProcess;
      this.RefreshProcessList();
      foreach (DataGridViewRow dataGridViewRow in this.processDataGridView.Rows.Cast<DataGridViewRow>())
      {
        if (dataGridViewRow.Cells[1].Value as string == previousProcess)
        {
          this.processDataGridView.CurrentCell = dataGridViewRow.Cells[1];
          break;
        }
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      GlobalWindowManager.AddWindow((Form) this);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      base.OnFormClosed(e);
      GlobalWindowManager.RemoveWindow((Form) this);
    }

    private void filterCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      this.RefreshProcessList();
    }

    private void filterTextBox_TextChanged(object sender, EventArgs e)
    {
      this.ApplyFilter();
    }

    private void refreshButton_Click(object sender, EventArgs e)
    {
      this.RefreshProcessList();
    }

    private void previousProcessLinkLabel_LinkClicked(
      object sender,
      LinkLabelLinkClickedEventArgs e)
    {
      this.filterTextBox.Text = this.previousProcessLinkLabel.Text == "No previous process" ? string.Empty : this.previousProcessLinkLabel.Text;
    }

    private void processDataGridView_CellMouseDoubleClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      this.AcceptButton.PerformClick();
    }

    private void RefreshProcessList()
    {
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add("icon", typeof (Image));
      dataTable.Columns.Add("name", typeof (string));
      dataTable.Columns.Add("id", typeof (IntPtr));
      dataTable.Columns.Add("path", typeof (string));
      dataTable.Columns.Add("info", typeof (ProcessInfo));
      bool shouldFilter = this.filterCheckBox.Checked;
      foreach (ProcessInfo processInfo in Program.CoreFunctions.EnumerateProcesses().Where<ProcessInfo>((Func<ProcessInfo, bool>) (p => !shouldFilter || !((IEnumerable<string>) ProcessBrowserForm.commonProcesses).Contains<string>(p.Name.ToLower()))))
      {
        DataRow row = dataTable.NewRow();
        row["icon"] = (object) processInfo.Icon;
        row["name"] = (object) processInfo.Name;
        row["id"] = (object) processInfo.Id;
        row["path"] = (object) processInfo.Path;
        row["info"] = (object) processInfo;
        dataTable.Rows.Add(row);
      }
      dataTable.DefaultView.Sort = "name ASC";
      this.processDataGridView.DataSource = (object) dataTable;
      this.ApplyFilter();
    }

    private void ApplyFilter()
    {
      string str = this.filterTextBox.Text;
      if (!string.IsNullOrEmpty(str))
        str = "name like '%" + str + "%' or path like '%" + str + "%'";
      ((DataTable) this.processDataGridView.DataSource).DefaultView.RowFilter = str;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.processDataGridView = new DataGridView();
      this.iconColumn = new DataGridViewImageColumn();
      this.processNameColumn = new DataGridViewTextBoxColumn();
      this.pidColumn = new DataGridViewTextBoxColumn();
      this.pathColumn = new DataGridViewTextBoxColumn();
      this.filterCheckBox = new CheckBox();
      this.refreshButton = new Button();
      this.attachToProcessButton = new Button();
      this.loadSymbolsCheckBox = new CheckBox();
      this.filterGroupBox = new GroupBox();
      this.previousProcessLinkLabel = new LinkLabel();
      this.label2 = new Label();
      this.label1 = new Label();
      this.filterTextBox = new TextBox();
      this.bannerBox = new BannerBox();
      ((ISupportInitialize) this.processDataGridView).BeginInit();
      this.filterGroupBox.SuspendLayout();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.processDataGridView.AllowUserToAddRows = false;
      this.processDataGridView.AllowUserToDeleteRows = false;
      this.processDataGridView.AllowUserToResizeColumns = false;
      this.processDataGridView.AllowUserToResizeRows = false;
      this.processDataGridView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.processDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.processDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.processDataGridView.Columns.AddRange((DataGridViewColumn) this.iconColumn, (DataGridViewColumn) this.processNameColumn, (DataGridViewColumn) this.pidColumn, (DataGridViewColumn) this.pathColumn);
      this.processDataGridView.Location = new Point(12, 199);
      this.processDataGridView.MultiSelect = false;
      this.processDataGridView.Name = "processDataGridView";
      this.processDataGridView.ReadOnly = true;
      this.processDataGridView.RowHeadersVisible = false;
      this.processDataGridView.ScrollBars = ScrollBars.Vertical;
      this.processDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.processDataGridView.Size = new Size(549, 291);
      this.processDataGridView.TabIndex = 0;
      this.processDataGridView.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.processDataGridView_CellMouseDoubleClick);
      this.iconColumn.DataPropertyName = "icon";
      this.iconColumn.HeaderText = "";
      this.iconColumn.MinimumWidth = 18;
      this.iconColumn.Name = "iconColumn";
      this.iconColumn.ReadOnly = true;
      this.iconColumn.Resizable = DataGridViewTriState.True;
      this.iconColumn.SortMode = DataGridViewColumnSortMode.Automatic;
      this.iconColumn.Width = 18;
      this.processNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.processNameColumn.DataPropertyName = "name";
      this.processNameColumn.HeaderText = "Process";
      this.processNameColumn.Name = "processNameColumn";
      this.processNameColumn.ReadOnly = true;
      this.processNameColumn.Width = 70;
      this.pidColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.pidColumn.DataPropertyName = "id";
      this.pidColumn.HeaderText = "PID";
      this.pidColumn.Name = "pidColumn";
      this.pidColumn.ReadOnly = true;
      this.pidColumn.Width = 50;
      this.pathColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.pathColumn.DataPropertyName = "path";
      this.pathColumn.HeaderText = "Path";
      this.pathColumn.Name = "pathColumn";
      this.pathColumn.ReadOnly = true;
      this.filterCheckBox.AutoSize = true;
      this.filterCheckBox.Checked = true;
      this.filterCheckBox.CheckState = CheckState.Checked;
      this.filterCheckBox.Location = new Point(9, 72);
      this.filterCheckBox.Name = "filterCheckBox";
      this.filterCheckBox.Size = new Size(158, 17);
      this.filterCheckBox.TabIndex = 1;
      this.filterCheckBox.Text = "Exclude common processes";
      this.filterCheckBox.UseVisualStyleBackColor = true;
      this.filterCheckBox.CheckedChanged += new EventHandler(this.filterCheckBox_CheckedChanged);
      this.refreshButton.Image = (Image) Resources.B16x16_Arrow_Refresh;
      this.refreshButton.Location = new Point(9, 99);
      this.refreshButton.Name = "refreshButton";
      this.refreshButton.Size = new Size(158, 23);
      this.refreshButton.TabIndex = 2;
      this.refreshButton.Text = "Refresh";
      this.refreshButton.TextAlign = ContentAlignment.MiddleRight;
      this.refreshButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.refreshButton.UseVisualStyleBackColor = true;
      this.refreshButton.Click += new EventHandler(this.refreshButton_Click);
      this.attachToProcessButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.attachToProcessButton.DialogResult = DialogResult.OK;
      this.attachToProcessButton.Image = (Image) Resources.B16x16_Accept;
      this.attachToProcessButton.Location = new Point(12, 519);
      this.attachToProcessButton.Name = "attachToProcessButton";
      this.attachToProcessButton.Size = new Size(549, 23);
      this.attachToProcessButton.TabIndex = 3;
      this.attachToProcessButton.Text = "Attach to Process";
      this.attachToProcessButton.TextAlign = ContentAlignment.MiddleRight;
      this.attachToProcessButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.attachToProcessButton.UseVisualStyleBackColor = true;
      this.loadSymbolsCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.loadSymbolsCheckBox.AutoSize = true;
      this.loadSymbolsCheckBox.Location = new Point(12, 496);
      this.loadSymbolsCheckBox.Name = "loadSymbolsCheckBox";
      this.loadSymbolsCheckBox.Size = new Size(92, 17);
      this.loadSymbolsCheckBox.TabIndex = 4;
      this.loadSymbolsCheckBox.Text = "Load Symbols";
      this.loadSymbolsCheckBox.UseVisualStyleBackColor = true;
      this.filterGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.filterGroupBox.Controls.Add((Control) this.previousProcessLinkLabel);
      this.filterGroupBox.Controls.Add((Control) this.label2);
      this.filterGroupBox.Controls.Add((Control) this.label1);
      this.filterGroupBox.Controls.Add((Control) this.filterCheckBox);
      this.filterGroupBox.Controls.Add((Control) this.refreshButton);
      this.filterGroupBox.Controls.Add((Control) this.filterTextBox);
      this.filterGroupBox.Location = new Point(12, 60);
      this.filterGroupBox.Name = "filterGroupBox";
      this.filterGroupBox.Size = new Size(549, 133);
      this.filterGroupBox.TabIndex = 5;
      this.filterGroupBox.TabStop = false;
      this.filterGroupBox.Text = "Filter";
      this.previousProcessLinkLabel.AutoSize = true;
      this.previousProcessLinkLabel.Location = new Point(103, 47);
      this.previousProcessLinkLabel.Name = "previousProcessLinkLabel";
      this.previousProcessLinkLabel.Size = new Size(19, 13);
      this.previousProcessLinkLabel.TabIndex = 3;
      this.previousProcessLinkLabel.TabStop = true;
      this.previousProcessLinkLabel.Text = "<>";
      this.previousProcessLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.previousProcessLinkLabel_LinkClicked);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 47);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Previous Process:";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(79, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Process Name:";
      this.filterTextBox.Location = new Point(103, 19);
      this.filterTextBox.Name = "filterTextBox";
      this.filterTextBox.Size = new Size(270, 20);
      this.filterTextBox.TabIndex = 0;
      this.filterTextBox.TextChanged += new EventHandler(this.filterTextBox_TextChanged);
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_Magnifier;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(573, 48);
      this.bannerBox.TabIndex = 6;
      this.bannerBox.Text = "Select the process to which ReClass.NET is to be attached.";
      this.bannerBox.Title = "Attach to Process";
      this.AcceptButton = (IButtonControl) this.attachToProcessButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(573, 554);
      this.Controls.Add((Control) this.bannerBox);
      this.Controls.Add((Control) this.filterGroupBox);
      this.Controls.Add((Control) this.loadSymbolsCheckBox);
      this.Controls.Add((Control) this.attachToProcessButton);
      this.Controls.Add((Control) this.processDataGridView);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProcessBrowserForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Attach to Process";
      ((ISupportInitialize) this.processDataGridView).EndInit();
      this.filterGroupBox.ResumeLayout(false);
      this.filterGroupBox.PerformLayout();
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
