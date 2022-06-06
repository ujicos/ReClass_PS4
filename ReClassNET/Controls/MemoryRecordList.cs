// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.MemoryRecordList
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class MemoryRecordList : UserControl
  {
    private readonly BindingList<MemoryRecord> bindings;
    private IContainer components;
    private DataGridView resultDataGridView;
    private DataGridViewTextBoxColumn descriptionColumn;
    private DataGridViewTextBoxColumn addressColumn;
    private DataGridViewTextBoxColumn valueTypeColumn;
    private DataGridViewTextBoxColumn valueColumn;
    private DataGridViewTextBoxColumn previousValueColumn;

    public bool ShowDescriptionColumn
    {
      get
      {
        return this.descriptionColumn.Visible;
      }
      set
      {
        this.descriptionColumn.Visible = value;
      }
    }

    public bool ShowAddressColumn
    {
      get
      {
        return this.addressColumn.Visible;
      }
      set
      {
        this.addressColumn.Visible = value;
      }
    }

    public bool ShowValueTypeColumn
    {
      get
      {
        return this.valueTypeColumn.Visible;
      }
      set
      {
        this.valueTypeColumn.Visible = value;
      }
    }

    public bool ShowValueColumn
    {
      get
      {
        return this.valueColumn.Visible;
      }
      set
      {
        this.valueColumn.Visible = value;
      }
    }

    public bool ShowPreviousValueColumn
    {
      get
      {
        return this.previousValueColumn.Visible;
      }
      set
      {
        this.previousValueColumn.Visible = value;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList<MemoryRecord> Records
    {
      get
      {
        return (IList<MemoryRecord>) this.bindings;
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MemoryRecord SelectedRecord
    {
      get
      {
        return this.GetSelectedRecords().FirstOrDefault<MemoryRecord>();
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IList<MemoryRecord> SelectedRecords
    {
      get
      {
        return (IList<MemoryRecord>) this.GetSelectedRecords().ToList<MemoryRecord>();
      }
    }

    public override ContextMenuStrip ContextMenuStrip { get; set; }

    public event MemorySearchResultControlResultDoubleClickEventHandler RecordDoubleClick;

    public MemoryRecordList()
    {
      this.InitializeComponent();
      if (Program.DesignMode)
        return;
      this.bindings = new BindingList<MemoryRecord>()
      {
        AllowNew = true,
        AllowEdit = true,
        RaiseListChangedEvents = true
      };
      this.resultDataGridView.AutoGenerateColumns = false;
      this.resultDataGridView.DefaultCellStyle.Font = new Font(Program.MonoSpaceFont.Font.FontFamily, (float) DpiUtil.ScaleIntX(11), GraphicsUnit.Pixel);
      this.resultDataGridView.DataSource = (object) this.bindings;
    }

    private void resultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      this.OnRecordDoubleClick((MemoryRecord) this.resultDataGridView.Rows[e.RowIndex].DataBoundItem);
    }

    private void resultDataGridView_CellFormatting(
      object sender,
      DataGridViewCellFormattingEventArgs e)
    {
      if (e.ColumnIndex == 1)
      {
        if (!((MemoryRecord) this.resultDataGridView.Rows[e.RowIndex].DataBoundItem).IsRelativeAddress)
          return;
        e.CellStyle.ForeColor = Color.ForestGreen;
        e.FormattingApplied = true;
      }
      else
      {
        if (e.ColumnIndex != 3)
          return;
        MemoryRecord dataBoundItem = (MemoryRecord) this.resultDataGridView.Rows[e.RowIndex].DataBoundItem;
        e.CellStyle.ForeColor = dataBoundItem.HasChangedValue ? Color.Red : Color.Black;
        e.FormattingApplied = true;
      }
    }

    private void resultDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right || e.RowIndex == -1)
        return;
      DataGridViewRow row = this.resultDataGridView.Rows[e.RowIndex];
      if (!row.Selected && Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Control)
        this.resultDataGridView.ClearSelection();
      row.Selected = true;
    }

    private void resultDataGridView_RowContextMenuStripNeeded(
      object sender,
      DataGridViewRowContextMenuStripNeededEventArgs e)
    {
      e.ContextMenuStrip = this.ContextMenuStrip;
    }

    private IEnumerable<MemoryRecord> GetSelectedRecords()
    {
      return this.resultDataGridView.SelectedRows.Cast<DataGridViewRow>().Select<DataGridViewRow, MemoryRecord>((Func<DataGridViewRow, MemoryRecord>) (r => (MemoryRecord) r.DataBoundItem));
    }

    public void SetRecords(IEnumerable<MemoryRecord> records)
    {
      this.bindings.Clear();
      this.bindings.RaiseListChangedEvents = false;
      foreach (MemoryRecord record in records)
        this.bindings.Add(record);
      this.bindings.RaiseListChangedEvents = true;
      this.bindings.ResetBindings();
    }

    public void Clear()
    {
      this.bindings.Clear();
    }

    public void RefreshValues(RemoteProcess process)
    {
      foreach (MemoryRecord memoryRecord in this.resultDataGridView.GetVisibleRows().Select<DataGridViewRow, MemoryRecord>((Func<DataGridViewRow, MemoryRecord>) (r => (MemoryRecord) r.DataBoundItem)))
        memoryRecord.RefreshValue(process);
    }

    private void OnRecordDoubleClick(MemoryRecord record)
    {
      MemorySearchResultControlResultDoubleClickEventHandler recordDoubleClick = this.RecordDoubleClick;
      if (recordDoubleClick == null)
        return;
      recordDoubleClick((object) this, record);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.resultDataGridView = new DataGridView();
      this.descriptionColumn = new DataGridViewTextBoxColumn();
      this.addressColumn = new DataGridViewTextBoxColumn();
      this.valueTypeColumn = new DataGridViewTextBoxColumn();
      this.valueColumn = new DataGridViewTextBoxColumn();
      this.previousValueColumn = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.resultDataGridView).BeginInit();
      this.SuspendLayout();
      this.resultDataGridView.AllowUserToAddRows = false;
      this.resultDataGridView.AllowUserToDeleteRows = false;
      this.resultDataGridView.AllowUserToResizeRows = false;
      this.resultDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.resultDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
      this.resultDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.resultDataGridView.Columns.AddRange((DataGridViewColumn) this.descriptionColumn, (DataGridViewColumn) this.addressColumn, (DataGridViewColumn) this.valueTypeColumn, (DataGridViewColumn) this.valueColumn, (DataGridViewColumn) this.previousValueColumn);
      this.resultDataGridView.Dock = DockStyle.Fill;
      this.resultDataGridView.Location = new Point(0, 0);
      this.resultDataGridView.Name = "resultDataGridView";
      this.resultDataGridView.ReadOnly = true;
      this.resultDataGridView.RowHeadersVisible = false;
      this.resultDataGridView.RowTemplate.Height = 19;
      this.resultDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.resultDataGridView.Size = new Size(290, 327);
      this.resultDataGridView.TabIndex = 15;
      this.resultDataGridView.CellDoubleClick += new DataGridViewCellEventHandler(this.resultDataGridView_CellDoubleClick);
      this.resultDataGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(this.resultDataGridView_CellFormatting);
      this.resultDataGridView.CellMouseDown += new DataGridViewCellMouseEventHandler(this.resultDataGridView_CellMouseDown);
      this.resultDataGridView.RowContextMenuStripNeeded += new DataGridViewRowContextMenuStripNeededEventHandler(this.resultDataGridView_RowContextMenuStripNeeded);
      this.descriptionColumn.DataPropertyName = "Description";
      this.descriptionColumn.HeaderText = "Description";
      this.descriptionColumn.Name = "descriptionColumn";
      this.descriptionColumn.ReadOnly = true;
      this.addressColumn.DataPropertyName = "AddressStr";
      this.addressColumn.HeaderText = "Address";
      this.addressColumn.MinimumWidth = 70;
      this.addressColumn.Name = "addressColumn";
      this.addressColumn.ReadOnly = true;
      this.valueTypeColumn.DataPropertyName = "ValueType";
      this.valueTypeColumn.HeaderText = "Value Type";
      this.valueTypeColumn.Name = "valueTypeColumn";
      this.valueTypeColumn.ReadOnly = true;
      this.valueColumn.DataPropertyName = "ValueStr";
      this.valueColumn.HeaderText = "Value";
      this.valueColumn.Name = "valueColumn";
      this.valueColumn.ReadOnly = true;
      this.previousValueColumn.DataPropertyName = "PreviousValueStr";
      this.previousValueColumn.HeaderText = "Previous";
      this.previousValueColumn.Name = "previousValueColumn";
      this.previousValueColumn.ReadOnly = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.resultDataGridView);
      this.Name = nameof (MemoryRecordList);
      this.Size = new Size(290, 327);
      ((ISupportInitialize) this.resultDataGridView).EndInit();
      this.ResumeLayout(false);
    }
  }
}
