// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.LogForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class LogForm : IconForm
  {
    private readonly List<LogForm.LogItem> items = new List<LogForm.LogItem>();
    private IContainer components;
    private DataGridView entriesDataGridView;
    private DataGridViewImageColumn iconColumn;
    private DataGridViewTextBoxColumn messageColumn;
    private Button closeButton;
    private Button copyToClipboardButton;
    private ContextMenuStrip contextMenuStrip;
    private ToolStripMenuItem showDetailsToolStripMenuItem;

    public LogForm()
    {
      this.InitializeComponent();
      this.entriesDataGridView.AutoGenerateColumns = false;
      this.entriesDataGridView.DataSource = (object) this.items;
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

    private void copyToClipboardButton_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(this.items.Select<LogForm.LogItem, string>((Func<LogForm.LogItem, string>) (i => i.Message)).Aggregate<string>((Func<string, string, string>) ((a, b) => a + Environment.NewLine + b)));
    }

    private void closeButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void entriesDataGridView_CellContentDoubleClick(
      object sender,
      DataGridViewCellEventArgs e)
    {
      this.ShowDetailsForm();
    }

    private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ShowDetailsForm();
    }

    private void RefreshDataBinding()
    {
      if (!(this.entriesDataGridView.BindingContext[(object) this.items] is CurrencyManager currencyManager))
        return;
      currencyManager.Refresh();
    }

    public void Clear()
    {
      this.items.Clear();
      this.RefreshDataBinding();
    }

    public void Add(ReClassNET.Logger.LogLevel level, string message, Exception ex)
    {
      Image image;
      switch (level)
      {
        case ReClassNET.Logger.LogLevel.Information:
          image = (Image) Resources.B16x16_Information;
          break;
        case ReClassNET.Logger.LogLevel.Warning:
          image = (Image) Resources.B16x16_Warning;
          break;
        case ReClassNET.Logger.LogLevel.Error:
          image = (Image) Resources.B16x16_Error;
          break;
        default:
          image = (Image) Resources.B16x16_Gear;
          break;
      }
      this.items.Add(new LogForm.LogItem()
      {
        Icon = image,
        Message = message,
        Exception = ex
      });
      this.RefreshDataBinding();
    }

    private void ShowDetailsForm()
    {
      if ((this.entriesDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>()?.DataBoundItem is LogForm.LogItem dataBoundItem ? dataBoundItem.Exception : (Exception) null) == null)
        return;
      Program.ShowException(dataBoundItem.Exception);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.closeButton = new Button();
      this.copyToClipboardButton = new Button();
      this.entriesDataGridView = new DataGridView();
      this.iconColumn = new DataGridViewImageColumn();
      this.messageColumn = new DataGridViewTextBoxColumn();
      this.contextMenuStrip = new ContextMenuStrip(this.components);
      this.showDetailsToolStripMenuItem = new ToolStripMenuItem();
      ((ISupportInitialize) this.entriesDataGridView).BeginInit();
      this.contextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeButton.DialogResult = DialogResult.OK;
      this.closeButton.Location = new Point(466, 206);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(109, 23);
      this.closeButton.TabIndex = 2;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Click += new EventHandler(this.closeButton_Click);
      this.copyToClipboardButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.copyToClipboardButton.Image = (Image) Resources.B16x16_Page_Copy;
      this.copyToClipboardButton.Location = new Point(12, 206);
      this.copyToClipboardButton.Name = "copyToClipboardButton";
      this.copyToClipboardButton.Size = new Size(120, 23);
      this.copyToClipboardButton.TabIndex = 3;
      this.copyToClipboardButton.Text = "Copy to Clipboard";
      this.copyToClipboardButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.copyToClipboardButton.UseVisualStyleBackColor = true;
      this.copyToClipboardButton.Click += new EventHandler(this.copyToClipboardButton_Click);
      this.entriesDataGridView.AllowUserToAddRows = false;
      this.entriesDataGridView.AllowUserToDeleteRows = false;
      this.entriesDataGridView.AllowUserToResizeColumns = false;
      this.entriesDataGridView.AllowUserToResizeRows = false;
      this.entriesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.entriesDataGridView.BackgroundColor = SystemColors.Window;
      this.entriesDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.entriesDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      this.entriesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.entriesDataGridView.Columns.AddRange((DataGridViewColumn) this.iconColumn, (DataGridViewColumn) this.messageColumn);
      this.entriesDataGridView.Location = new Point(12, 12);
      this.entriesDataGridView.MultiSelect = false;
      this.entriesDataGridView.Name = "entriesDataGridView";
      this.entriesDataGridView.ReadOnly = true;
      this.entriesDataGridView.RowHeadersVisible = false;
      this.entriesDataGridView.ContextMenuStrip = this.contextMenuStrip;
      this.entriesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.entriesDataGridView.Size = new Size(563, 188);
      this.entriesDataGridView.TabIndex = 1;
      this.entriesDataGridView.CellContentDoubleClick += new DataGridViewCellEventHandler(this.entriesDataGridView_CellContentDoubleClick);
      this.iconColumn.DataPropertyName = "Icon";
      this.iconColumn.HeaderText = "";
      this.iconColumn.MinimumWidth = 18;
      this.iconColumn.Name = "iconColumn";
      this.iconColumn.ReadOnly = true;
      this.iconColumn.Resizable = DataGridViewTriState.False;
      this.iconColumn.SortMode = DataGridViewColumnSortMode.Automatic;
      this.iconColumn.Width = 18;
      this.messageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.messageColumn.DataPropertyName = "Message";
      this.messageColumn.HeaderText = "Message";
      this.messageColumn.Name = "messageColumn";
      this.messageColumn.ReadOnly = true;
      this.contextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.showDetailsToolStripMenuItem
      });
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.Size = new Size(150, 26);
      this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
      this.showDetailsToolStripMenuItem.Size = new Size(149, 22);
      this.showDetailsToolStripMenuItem.Text = "Show details...";
      this.showDetailsToolStripMenuItem.Click += new EventHandler(this.showDetailsToolStripMenuItem_Click);
      this.AcceptButton = (IButtonControl) this.closeButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(587, 237);
      this.Controls.Add((Control) this.copyToClipboardButton);
      this.Controls.Add((Control) this.closeButton);
      this.Controls.Add((Control) this.entriesDataGridView);
      this.Name = nameof (LogForm);
      this.Text = "ReClass.NET - Diagnostic Messages";
      ((ISupportInitialize) this.entriesDataGridView).EndInit();
      this.contextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private class LogItem
    {
      public Image Icon { get; set; }

      public string Message { get; set; }

      public Exception Exception { get; set; }
    }
  }
}
