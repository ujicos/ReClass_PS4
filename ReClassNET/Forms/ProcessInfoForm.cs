// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.ProcessInfoForm
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
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class ProcessInfoForm : IconForm
  {
    private readonly IProcessReader process;
    private IContainer components;
    private DataGridView sectionsDataGridView;
    private ContextMenuStrip contextMenuStrip;
    private ToolStripMenuItem setCurrentClassAddressToolStripMenuItem;
    private ToolStripMenuItem createClassAtAddressToolStripMenuItem;
    private BannerBox bannerBox1;
    private TabControl tabControl;
    private TabPage modulesTabPage;
    private DataGridView modulesDataGridView;
    private TabPage sectionsTabPage;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem dumpToolStripMenuItem;
    private DataGridViewImageColumn moduleIconDataGridViewImageColumn;
    private DataGridViewTextBoxColumn moduleNameDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn moduleAddressDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn moduleSizeDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn modulePathDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn addressColumn;
    private DataGridViewTextBoxColumn sizeColumn;
    private DataGridViewTextBoxColumn nameColumn;
    private DataGridViewTextBoxColumn protectionColumn;
    private DataGridViewTextBoxColumn typeColumn;
    private DataGridViewTextBoxColumn moduleColumn;

    public ContextMenuStrip GridContextMenu
    {
      get
      {
        return this.contextMenuStrip;
      }
    }

    public ProcessInfoForm(IProcessReader process)
    {
      this.process = process;
      this.InitializeComponent();
      this.tabControl.ImageList = new ImageList();
      this.tabControl.ImageList.Images.Add((Image) Resources.B16x16_Category);
      this.tabControl.ImageList.Images.Add((Image) Resources.B16x16_Page_White_Stack);
      this.modulesTabPage.ImageIndex = 0;
      this.sectionsTabPage.ImageIndex = 1;
      this.modulesDataGridView.AutoGenerateColumns = false;
      this.sectionsDataGridView.AutoGenerateColumns = false;
      if (!NativeMethods.IsUnix())
        return;
      this.moduleIconDataGridViewImageColumn.Visible = false;
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

    private async void ProcessInfoForm_Load(object sender, EventArgs e)
    {
      try
      {
        DataTable sectionsTable = new DataTable();
        sectionsTable.Columns.Add("address", typeof (string));
        sectionsTable.Columns.Add("size", typeof (string));
        sectionsTable.Columns.Add("name", typeof (string));
        sectionsTable.Columns.Add("protection", typeof (string));
        sectionsTable.Columns.Add("type", typeof (string));
        sectionsTable.Columns.Add("module", typeof (string));
        sectionsTable.Columns.Add("section", typeof (Section));
        DataTable modulesTable = new DataTable();
        modulesTable.Columns.Add("icon", typeof (Icon));
        modulesTable.Columns.Add("name", typeof (string));
        modulesTable.Columns.Add("address", typeof (string));
        modulesTable.Columns.Add("size", typeof (string));
        modulesTable.Columns.Add("path", typeof (string));
        modulesTable.Columns.Add("module", typeof (Module));
        TaskAwaiter awaiter = Task.Run((Action) (() =>
        {
          List<Section> sections;
          List<Module> modules;
          if (!this.process.EnumerateRemoteSectionsAndModules(out sections, out modules))
            return;
          foreach (Section section in sections)
          {
            DataRow row = sectionsTable.NewRow();
            row["address"] = (object) section.Start.ToString("X016");
            row["size"] = (object) section.Size.ToString("X016");
            row["name"] = (object) section.Name;
            row["protection"] = (object) section.Protection.ToString();
            row["type"] = (object) section.Type.ToString();
            row["module"] = (object) section.ModuleName;
            row["section"] = (object) section;
            sectionsTable.Rows.Add(row);
          }
          foreach (Module module in modules)
          {
            DataRow row = modulesTable.NewRow();
            row["icon"] = (object) NativeMethods.GetIconForFile(module.Path);
            row["name"] = (object) module.Name;
            row["address"] = (object) module.Start.ToString("X016");
            row["size"] = (object) module.Size.ToString("X016");
            row["path"] = (object) module.Path;
            row["module"] = (object) module;
            modulesTable.Rows.Add(row);
          }
        })).GetAwaiter();
        if (!awaiter.IsCompleted)
        {
          int num;
          // ISSUE: explicit reference operation
          // ISSUE: reference to a compiler-generated field
          (^this).\u003C\u003E1__state = num = 0;
          TaskAwaiter taskAwaiter = awaiter;
          // ISSUE: explicit reference operation
          // ISSUE: reference to a compiler-generated field
          (^this).\u003C\u003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProcessInfoForm.\u003CProcessInfoForm_Load\u003Ed__6>(ref awaiter, this);
          return;
        }
        awaiter.GetResult();
        this.sectionsDataGridView.DataSource = (object) sectionsTable;
        this.modulesDataGridView.DataSource = (object) modulesTable;
      }
      catch (Exception ex)
      {
        // ISSUE: explicit reference operation
        // ISSUE: reference to a compiler-generated field
        (^this).\u003C\u003E1__state = -2;
        // ISSUE: explicit reference operation
        // ISSUE: reference to a compiler-generated field
        (^this).\u003C\u003Et__builder.SetException(ex);
        return;
      }
      // ISSUE: explicit reference operation
      // ISSUE: reference to a compiler-generated field
      (^this).\u003C\u003E1__state = -2;
      // ISSUE: explicit reference operation
      // ISSUE: reference to a compiler-generated field
      (^this).\u003C\u003Et__builder.SetResult();
    }

    private void SelectRow_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
      if (!(sender is DataGridView dataGridView) || e.Button != MouseButtons.Right)
        return;
      int rowIndex = e.RowIndex;
      if (e.RowIndex == -1)
        return;
      dataGridView.Rows[rowIndex].Selected = true;
    }

    private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      Control control = sender is ContextMenuStrip contextMenuStrip ? contextMenuStrip.SourceControl : (Control) null;
      e.Cancel = control == null || control == this.modulesDataGridView && this.GetSelectedModule() == null || control == this.sectionsDataGridView && this.GetSelectedSection() == null;
    }

    private void setCurrentClassAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LinkedWindowFeatures.SetCurrentClassAddress(this.GetSelectedAddress(sender));
    }

    private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LinkedWindowFeatures.CreateClassAtAddress(this.GetSelectedAddress(sender), true);
    }

    private void dumpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Func<SaveFileDialog> func;
      Action<IRemoteMemoryReader, Stream> action;
      if (ProcessInfoForm.GetToolStripSourceControl(sender) == this.modulesDataGridView)
      {
        Module module = this.GetSelectedModule();
        if (module == null)
          return;
        func = (Func<SaveFileDialog>) (() =>
        {
          return new SaveFileDialog()
          {
            FileName = Path.GetFileNameWithoutExtension(module.Name) + "_Dumped" + Path.GetExtension(module.Name),
            InitialDirectory = Path.GetDirectoryName(module.Path)
          };
        });
        action = (Action<IRemoteMemoryReader, Stream>) ((reader, stream) =>
        {
          Dumper.DumpModule(reader, module, stream);
          int num = (int) MessageBox.Show("Module successfully dumped.", "ReClass.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        });
      }
      else
      {
        Section section = this.GetSelectedSection();
        if (section == null)
          return;
        func = (Func<SaveFileDialog>) (() =>
        {
          return new SaveFileDialog()
          {
            FileName = "Section_" + section.Start.ToString("X") + "_" + section.End.ToString("X") + ".dat"
          };
        });
        action = (Action<IRemoteMemoryReader, Stream>) ((reader, stream) =>
        {
          Dumper.DumpSection(reader, section, stream);
          int num = (int) MessageBox.Show("Section successfully dumped.", "ReClass.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        });
      }
      using (SaveFileDialog saveFileDialog = func())
      {
        saveFileDialog.Filter = "All|*.*";
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          using (Stream stream = saveFileDialog.OpenFile())
            action((IRemoteMemoryReader) this.process, stream);
        }
        catch (Exception ex)
        {
          Program.ShowException(ex);
        }
      }
    }

    private void sectionsDataGridView_CellMouseDoubleClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      this.setCurrentClassAddressToolStripMenuItem_Click(sender, (EventArgs) e);
      this.Close();
    }

    private IntPtr GetSelectedAddress(object sender)
    {
      if (ProcessInfoForm.GetToolStripSourceControl(sender) == this.modulesDataGridView)
      {
        Module selectedModule = this.GetSelectedModule();
        return selectedModule == null ? IntPtr.Zero : selectedModule.Start;
      }
      Section selectedSection = this.GetSelectedSection();
      return selectedSection == null ? IntPtr.Zero : selectedSection.Start;
    }

    private static Control GetToolStripSourceControl(object sender)
    {
      return !((sender is ToolStripMenuItem toolStripMenuItem ? toolStripMenuItem.GetCurrentParent() : (ToolStrip) null) is ContextMenuStrip contextMenuStrip) ? (Control) null : contextMenuStrip.SourceControl;
    }

    private Module GetSelectedModule()
    {
      return (this.modulesDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>()?.DataBoundItem is DataRowView dataBoundItem ? dataBoundItem["module"] : (object) null) as Module;
    }

    private Section GetSelectedSection()
    {
      return (this.sectionsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>()?.DataBoundItem is DataRowView dataBoundItem ? dataBoundItem["section"] : (object) null) as Section;
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
      this.contextMenuStrip = new ContextMenuStrip(this.components);
      this.setCurrentClassAddressToolStripMenuItem = new ToolStripMenuItem();
      this.createClassAtAddressToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.dumpToolStripMenuItem = new ToolStripMenuItem();
      this.sectionsDataGridView = new DataGridView();
      this.addressColumn = new DataGridViewTextBoxColumn();
      this.sizeColumn = new DataGridViewTextBoxColumn();
      this.nameColumn = new DataGridViewTextBoxColumn();
      this.protectionColumn = new DataGridViewTextBoxColumn();
      this.typeColumn = new DataGridViewTextBoxColumn();
      this.moduleColumn = new DataGridViewTextBoxColumn();
      this.bannerBox1 = new BannerBox();
      this.tabControl = new TabControl();
      this.modulesTabPage = new TabPage();
      this.modulesDataGridView = new DataGridView();
      this.moduleIconDataGridViewImageColumn = new DataGridViewImageColumn();
      this.moduleNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.moduleAddressDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.moduleSizeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.modulePathDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.sectionsTabPage = new TabPage();
      this.contextMenuStrip.SuspendLayout();
      ((ISupportInitialize) this.sectionsDataGridView).BeginInit();
      this.bannerBox1.BeginInit();
      this.tabControl.SuspendLayout();
      this.modulesTabPage.SuspendLayout();
      ((ISupportInitialize) this.modulesDataGridView).BeginInit();
      this.sectionsTabPage.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.setCurrentClassAddressToolStripMenuItem,
        (ToolStripItem) this.createClassAtAddressToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.dumpToolStripMenuItem
      });
      this.contextMenuStrip.Name = "contextMenuStrip";
      this.contextMenuStrip.Size = new Size(203, 98);
      this.contextMenuStrip.Opening += new CancelEventHandler(this.contextMenuStrip_Opening);
      this.setCurrentClassAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Exchange_Button;
      this.setCurrentClassAddressToolStripMenuItem.Name = "setCurrentClassAddressToolStripMenuItem";
      this.setCurrentClassAddressToolStripMenuItem.Size = new Size(202, 22);
      this.setCurrentClassAddressToolStripMenuItem.Text = "Set current class address";
      this.setCurrentClassAddressToolStripMenuItem.Click += new EventHandler(this.setCurrentClassAddressToolStripMenuItem_Click);
      this.createClassAtAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Class_Add;
      this.createClassAtAddressToolStripMenuItem.Name = "createClassAtAddressToolStripMenuItem";
      this.createClassAtAddressToolStripMenuItem.Size = new Size(202, 22);
      this.createClassAtAddressToolStripMenuItem.Text = "Create class at address";
      this.createClassAtAddressToolStripMenuItem.Click += new EventHandler(this.createClassAtAddressToolStripMenuItem_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(199, 6);
      this.dumpToolStripMenuItem.Image = (Image) Resources.B16x16_Drive_Go;
      this.dumpToolStripMenuItem.Name = "dumpToolStripMenuItem";
      this.dumpToolStripMenuItem.Size = new Size(202, 22);
      this.dumpToolStripMenuItem.Text = "Dump...";
      this.dumpToolStripMenuItem.Click += new EventHandler(this.dumpToolStripMenuItem_Click);
      this.sectionsDataGridView.AllowUserToAddRows = false;
      this.sectionsDataGridView.AllowUserToDeleteRows = false;
      this.sectionsDataGridView.AllowUserToResizeRows = false;
      this.sectionsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.sectionsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.sectionsDataGridView.Columns.AddRange((DataGridViewColumn) this.addressColumn, (DataGridViewColumn) this.sizeColumn, (DataGridViewColumn) this.nameColumn, (DataGridViewColumn) this.protectionColumn, (DataGridViewColumn) this.typeColumn, (DataGridViewColumn) this.moduleColumn);
      this.sectionsDataGridView.ContextMenuStrip = this.contextMenuStrip;
      this.sectionsDataGridView.Dock = DockStyle.Fill;
      this.sectionsDataGridView.Location = new Point(3, 3);
      this.sectionsDataGridView.MultiSelect = false;
      this.sectionsDataGridView.Name = "sectionsDataGridView";
      this.sectionsDataGridView.ReadOnly = true;
      this.sectionsDataGridView.RowHeadersVisible = false;
      this.sectionsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.sectionsDataGridView.Size = new Size(796, 386);
      this.sectionsDataGridView.TabIndex = 0;
      this.sectionsDataGridView.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.sectionsDataGridView_CellMouseDoubleClick);
      this.sectionsDataGridView.CellMouseDown += new DataGridViewCellMouseEventHandler(this.SelectRow_CellMouseDown);
      this.addressColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.addressColumn.DataPropertyName = "address";
      this.addressColumn.HeaderText = "Address";
      this.addressColumn.Name = "addressColumn";
      this.addressColumn.ReadOnly = true;
      this.addressColumn.Width = 70;
      this.sizeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.sizeColumn.DataPropertyName = "size";
      this.sizeColumn.HeaderText = "Size";
      this.sizeColumn.Name = "sizeColumn";
      this.sizeColumn.ReadOnly = true;
      this.sizeColumn.Width = 52;
      this.nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.nameColumn.DataPropertyName = "name";
      this.nameColumn.HeaderText = "Name";
      this.nameColumn.Name = "nameColumn";
      this.nameColumn.ReadOnly = true;
      this.nameColumn.Width = 60;
      this.protectionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.protectionColumn.DataPropertyName = "protection";
      this.protectionColumn.HeaderText = "Protection";
      this.protectionColumn.Name = "protectionColumn";
      this.protectionColumn.ReadOnly = true;
      this.protectionColumn.Width = 80;
      this.typeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.typeColumn.DataPropertyName = "type";
      this.typeColumn.HeaderText = "Type";
      this.typeColumn.Name = "typeColumn";
      this.typeColumn.ReadOnly = true;
      this.typeColumn.Width = 56;
      this.moduleColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.moduleColumn.DataPropertyName = "module";
      this.moduleColumn.HeaderText = "Module";
      this.moduleColumn.Name = "moduleColumn";
      this.moduleColumn.ReadOnly = true;
      this.bannerBox1.Dock = DockStyle.Top;
      this.bannerBox1.Icon = (Image) Resources.B32x32_Magnifier;
      this.bannerBox1.Location = new Point(0, 0);
      this.bannerBox1.Name = "bannerBox1";
      this.bannerBox1.Size = new Size(834, 48);
      this.bannerBox1.TabIndex = 2;
      this.bannerBox1.Text = "View informations about the current process.";
      this.bannerBox1.Title = "Process Informations";
      this.tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl.Controls.Add((Control) this.modulesTabPage);
      this.tabControl.Controls.Add((Control) this.sectionsTabPage);
      this.tabControl.Location = new Point(12, 60);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(810, 418);
      this.tabControl.TabIndex = 3;
      this.modulesTabPage.Controls.Add((Control) this.modulesDataGridView);
      this.modulesTabPage.Location = new Point(4, 22);
      this.modulesTabPage.Name = "modulesTabPage";
      this.modulesTabPage.Padding = new Padding(3);
      this.modulesTabPage.Size = new Size(802, 392);
      this.modulesTabPage.TabIndex = 1;
      this.modulesTabPage.Text = "Modules";
      this.modulesTabPage.UseVisualStyleBackColor = true;
      this.modulesDataGridView.AllowUserToAddRows = false;
      this.modulesDataGridView.AllowUserToDeleteRows = false;
      this.modulesDataGridView.AllowUserToResizeRows = false;
      this.modulesDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.modulesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.modulesDataGridView.Columns.AddRange((DataGridViewColumn) this.moduleIconDataGridViewImageColumn, (DataGridViewColumn) this.moduleNameDataGridViewTextBoxColumn, (DataGridViewColumn) this.moduleAddressDataGridViewTextBoxColumn, (DataGridViewColumn) this.moduleSizeDataGridViewTextBoxColumn, (DataGridViewColumn) this.modulePathDataGridViewTextBoxColumn);
      this.modulesDataGridView.ContextMenuStrip = this.contextMenuStrip;
      this.modulesDataGridView.Dock = DockStyle.Fill;
      this.modulesDataGridView.Location = new Point(3, 3);
      this.modulesDataGridView.MultiSelect = false;
      this.modulesDataGridView.Name = "modulesDataGridView";
      this.modulesDataGridView.ReadOnly = true;
      this.modulesDataGridView.RowHeadersVisible = false;
      this.modulesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.modulesDataGridView.Size = new Size(796, 386);
      this.modulesDataGridView.TabIndex = 1;
      this.modulesDataGridView.CellMouseDown += new DataGridViewCellMouseEventHandler(this.SelectRow_CellMouseDown);
      this.moduleIconDataGridViewImageColumn.DataPropertyName = "icon";
      this.moduleIconDataGridViewImageColumn.HeaderText = "";
      this.moduleIconDataGridViewImageColumn.MinimumWidth = 18;
      this.moduleIconDataGridViewImageColumn.Name = "moduleIconDataGridViewImageColumn";
      this.moduleIconDataGridViewImageColumn.ReadOnly = true;
      this.moduleIconDataGridViewImageColumn.Resizable = DataGridViewTriState.False;
      this.moduleIconDataGridViewImageColumn.SortMode = DataGridViewColumnSortMode.Automatic;
      this.moduleIconDataGridViewImageColumn.Width = 18;
      this.moduleNameDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.moduleNameDataGridViewTextBoxColumn.DataPropertyName = "name";
      this.moduleNameDataGridViewTextBoxColumn.HeaderText = "Module";
      this.moduleNameDataGridViewTextBoxColumn.Name = "moduleNameDataGridViewTextBoxColumn";
      this.moduleNameDataGridViewTextBoxColumn.ReadOnly = true;
      this.moduleNameDataGridViewTextBoxColumn.Width = 67;
      this.moduleAddressDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.moduleAddressDataGridViewTextBoxColumn.DataPropertyName = "address";
      this.moduleAddressDataGridViewTextBoxColumn.HeaderText = "Address";
      this.moduleAddressDataGridViewTextBoxColumn.Name = "moduleAddressDataGridViewTextBoxColumn";
      this.moduleAddressDataGridViewTextBoxColumn.ReadOnly = true;
      this.moduleAddressDataGridViewTextBoxColumn.Width = 70;
      this.moduleSizeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
      this.moduleSizeDataGridViewTextBoxColumn.DataPropertyName = "size";
      this.moduleSizeDataGridViewTextBoxColumn.HeaderText = "Size";
      this.moduleSizeDataGridViewTextBoxColumn.Name = "moduleSizeDataGridViewTextBoxColumn";
      this.moduleSizeDataGridViewTextBoxColumn.ReadOnly = true;
      this.moduleSizeDataGridViewTextBoxColumn.Width = 52;
      this.modulePathDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.modulePathDataGridViewTextBoxColumn.DataPropertyName = "path";
      this.modulePathDataGridViewTextBoxColumn.HeaderText = "Path";
      this.modulePathDataGridViewTextBoxColumn.Name = "modulePathDataGridViewTextBoxColumn";
      this.modulePathDataGridViewTextBoxColumn.ReadOnly = true;
      this.sectionsTabPage.Controls.Add((Control) this.sectionsDataGridView);
      this.sectionsTabPage.Location = new Point(4, 22);
      this.sectionsTabPage.Name = "sectionsTabPage";
      this.sectionsTabPage.Padding = new Padding(3);
      this.sectionsTabPage.Size = new Size(802, 392);
      this.sectionsTabPage.TabIndex = 0;
      this.sectionsTabPage.Text = "Sections";
      this.sectionsTabPage.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(834, 490);
      this.Controls.Add((Control) this.tabControl);
      this.Controls.Add((Control) this.bannerBox1);
      this.MinimumSize = new Size(586, 320);
      this.Name = nameof (ProcessInfoForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Process Informations";
      this.Load += new EventHandler(this.ProcessInfoForm_Load);
      this.contextMenuStrip.ResumeLayout(false);
      ((ISupportInitialize) this.sectionsDataGridView).EndInit();
      this.bannerBox1.EndInit();
      this.tabControl.ResumeLayout(false);
      this.modulesTabPage.ResumeLayout(false);
      ((ISupportInitialize) this.modulesDataGridView).EndInit();
      this.sectionsTabPage.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
