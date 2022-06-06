// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.PluginForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Plugins;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class PluginForm : IconForm
  {
    private IContainer components;
    private TabControl tabControl;
    private TabPage pluginsTabPage;
    private GroupBox descriptionGroupBox;
    private DataGridView pluginsDataGridView;
    private TabPage nativesTabPage;
    private LinkLabel getMoreLinkLabel;
    private Button closeButton;
    private Label descriptionLabel;
    private Label label1;
    private BannerBox bannerBox;
    private ComboBox functionsProvidersComboBox;
    private Label label2;
    private DataGridViewImageColumn iconColumn;
    private DataGridViewTextBoxColumn nameColumn;
    private DataGridViewTextBoxColumn versionColumn;
    private DataGridViewTextBoxColumn authorColumn;

    internal PluginForm(PluginManager pluginManager)
    {
      this.InitializeComponent();
      this.pluginsDataGridView.AutoGenerateColumns = false;
      this.pluginsDataGridView.DataSource = (object) pluginManager.Plugins.Select<PluginInfo, PluginForm.PluginInfoRow>((Func<PluginInfo, PluginForm.PluginInfoRow>) (p => new PluginForm.PluginInfoRow(p))).ToList<PluginForm.PluginInfoRow>();
      this.UpdatePluginDescription();
      string[] array = Program.CoreFunctions.FunctionProviders.ToArray<string>();
      this.functionsProvidersComboBox.Items.AddRange((object[]) array);
      this.functionsProvidersComboBox.SelectedIndex = Array.IndexOf<string>(array, Program.CoreFunctions.CurrentFunctionsProvider);
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

    private void pluginsDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      this.UpdatePluginDescription();
    }

    private void functionsProvidersComboBox_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (!(this.functionsProvidersComboBox.SelectedItem is string selectedItem))
        return;
      Program.CoreFunctions.SetActiveFunctionsProvider(selectedItem);
    }

    private void getMoreLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("https://github.com/ReClassNET/ReClass.NET#plugins");
    }

    private void UpdatePluginDescription()
    {
      DataGridViewRow dataGridViewRow = this.pluginsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>();
      if (dataGridViewRow == null)
      {
        this.descriptionGroupBox.Text = string.Empty;
        this.descriptionLabel.Text = string.Empty;
      }
      else
      {
        if (!(dataGridViewRow.DataBoundItem is PluginForm.PluginInfoRow dataBoundItem))
          return;
        this.descriptionGroupBox.Text = dataBoundItem.Name;
        this.descriptionLabel.Text = dataBoundItem.Description;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabControl = new TabControl();
      this.pluginsTabPage = new TabPage();
      this.descriptionGroupBox = new GroupBox();
      this.descriptionLabel = new Label();
      this.pluginsDataGridView = new DataGridView();
      this.nativesTabPage = new TabPage();
      this.label2 = new Label();
      this.functionsProvidersComboBox = new ComboBox();
      this.label1 = new Label();
      this.getMoreLinkLabel = new LinkLabel();
      this.closeButton = new Button();
      this.bannerBox = new BannerBox();
      this.iconColumn = new DataGridViewImageColumn();
      this.nameColumn = new DataGridViewTextBoxColumn();
      this.versionColumn = new DataGridViewTextBoxColumn();
      this.authorColumn = new DataGridViewTextBoxColumn();
      this.tabControl.SuspendLayout();
      this.pluginsTabPage.SuspendLayout();
      this.descriptionGroupBox.SuspendLayout();
      ((ISupportInitialize) this.pluginsDataGridView).BeginInit();
      this.nativesTabPage.SuspendLayout();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.tabControl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabControl.Controls.Add((Control) this.pluginsTabPage);
      this.tabControl.Controls.Add((Control) this.nativesTabPage);
      this.tabControl.Location = new Point(12, 60);
      this.tabControl.Name = "tabControl";
      this.tabControl.SelectedIndex = 0;
      this.tabControl.Size = new Size(716, 328);
      this.tabControl.TabIndex = 0;
      this.pluginsTabPage.Controls.Add((Control) this.descriptionGroupBox);
      this.pluginsTabPage.Controls.Add((Control) this.pluginsDataGridView);
      this.pluginsTabPage.Location = new Point(4, 22);
      this.pluginsTabPage.Name = "pluginsTabPage";
      this.pluginsTabPage.Padding = new Padding(3);
      this.pluginsTabPage.Size = new Size(708, 302);
      this.pluginsTabPage.TabIndex = 0;
      this.pluginsTabPage.Text = "Plugins";
      this.pluginsTabPage.UseVisualStyleBackColor = true;
      this.descriptionGroupBox.Controls.Add((Control) this.descriptionLabel);
      this.descriptionGroupBox.Location = new Point(6, 206);
      this.descriptionGroupBox.Name = "descriptionGroupBox";
      this.descriptionGroupBox.Size = new Size(696, 90);
      this.descriptionGroupBox.TabIndex = 1;
      this.descriptionGroupBox.TabStop = false;
      this.descriptionGroupBox.Text = "<>";
      this.descriptionLabel.Location = new Point(6, 16);
      this.descriptionLabel.Name = "descriptionLabel";
      this.descriptionLabel.Size = new Size(684, 65);
      this.descriptionLabel.TabIndex = 0;
      this.descriptionLabel.Text = "<>";
      this.pluginsDataGridView.AllowUserToAddRows = false;
      this.pluginsDataGridView.AllowUserToDeleteRows = false;
      this.pluginsDataGridView.AllowUserToResizeRows = false;
      this.pluginsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.pluginsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.pluginsDataGridView.Columns.AddRange((DataGridViewColumn) this.iconColumn, (DataGridViewColumn) this.nameColumn, (DataGridViewColumn) this.versionColumn, (DataGridViewColumn) this.authorColumn);
      this.pluginsDataGridView.Dock = DockStyle.Top;
      this.pluginsDataGridView.Location = new Point(3, 3);
      this.pluginsDataGridView.MultiSelect = false;
      this.pluginsDataGridView.Name = "pluginsDataGridView";
      this.pluginsDataGridView.ReadOnly = true;
      this.pluginsDataGridView.RowHeadersVisible = false;
      this.pluginsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.pluginsDataGridView.Size = new Size(702, 197);
      this.pluginsDataGridView.TabIndex = 0;
      this.pluginsDataGridView.SelectionChanged += new EventHandler(this.pluginsDataGridView_SelectionChanged);
      this.nativesTabPage.Controls.Add((Control) this.label2);
      this.nativesTabPage.Controls.Add((Control) this.functionsProvidersComboBox);
      this.nativesTabPage.Controls.Add((Control) this.label1);
      this.nativesTabPage.Location = new Point(4, 22);
      this.nativesTabPage.Name = "nativesTabPage";
      this.nativesTabPage.Padding = new Padding(3);
      this.nativesTabPage.Size = new Size(708, 302);
      this.nativesTabPage.TabIndex = 1;
      this.nativesTabPage.Text = "Native Helper";
      this.nativesTabPage.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 66);
      this.label2.Name = "label2";
      this.label2.Size = new Size(98, 13);
      this.label2.TabIndex = 21;
      this.label2.Text = "Functions Provider:";
      this.functionsProvidersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.functionsProvidersComboBox.FormattingEnabled = true;
      this.functionsProvidersComboBox.Location = new Point(110, 63);
      this.functionsProvidersComboBox.Name = "functionsProvidersComboBox";
      this.functionsProvidersComboBox.Size = new Size(305, 21);
      this.functionsProvidersComboBox.TabIndex = 20;
      this.functionsProvidersComboBox.SelectionChangeCommitted += new EventHandler(this.functionsProvidersComboBox_SelectionChangeCommitted);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(409, 39);
      this.label1.TabIndex = 0;
      this.label1.Text = "Plugins can provide different methods how ReClass.NET accesses a remote process.\r\n\r\nWarning: You should detach from the current process before changing a function.";
      this.getMoreLinkLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.getMoreLinkLabel.AutoSize = true;
      this.getMoreLinkLabel.Location = new Point(9, 396);
      this.getMoreLinkLabel.Name = "getMoreLinkLabel";
      this.getMoreLinkLabel.Size = new Size(95, 13);
      this.getMoreLinkLabel.TabIndex = 1;
      this.getMoreLinkLabel.TabStop = true;
      this.getMoreLinkLabel.Text = "Get more plugins...";
      this.getMoreLinkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.getMoreLinkLabel_LinkClicked);
      this.closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeButton.DialogResult = DialogResult.OK;
      this.closeButton.Location = new Point(653, 391);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(75, 23);
      this.closeButton.TabIndex = 2;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_Plugin;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(740, 48);
      this.bannerBox.TabIndex = 3;
      this.bannerBox.Text = "Here you can configure all loaded ReClass.NET plugins.";
      this.bannerBox.Title = "Plugins";
      this.iconColumn.DataPropertyName = "Icon";
      this.iconColumn.HeaderText = "";
      this.iconColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
      this.iconColumn.MinimumWidth = 18;
      this.iconColumn.Name = "iconColumn";
      this.iconColumn.ReadOnly = true;
      this.iconColumn.Resizable = DataGridViewTriState.False;
      this.iconColumn.SortMode = DataGridViewColumnSortMode.Automatic;
      this.iconColumn.Width = 18;
      this.nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.nameColumn.DataPropertyName = "Name";
      this.nameColumn.HeaderText = "Name";
      this.nameColumn.Name = "nameColumn";
      this.nameColumn.ReadOnly = true;
      this.versionColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.versionColumn.DataPropertyName = "Version";
      this.versionColumn.HeaderText = "Version";
      this.versionColumn.Name = "versionColumn";
      this.versionColumn.ReadOnly = true;
      this.versionColumn.Width = 67;
      this.authorColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.authorColumn.DataPropertyName = "Author";
      this.authorColumn.HeaderText = "Author";
      this.authorColumn.Name = "authorColumn";
      this.authorColumn.ReadOnly = true;
      this.authorColumn.Width = 63;
      this.AcceptButton = (IButtonControl) this.closeButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(740, 423);
      this.Controls.Add((Control) this.bannerBox);
      this.Controls.Add((Control) this.closeButton);
      this.Controls.Add((Control) this.getMoreLinkLabel);
      this.Controls.Add((Control) this.tabControl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PluginForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Plugins";
      this.tabControl.ResumeLayout(false);
      this.pluginsTabPage.ResumeLayout(false);
      this.descriptionGroupBox.ResumeLayout(false);
      ((ISupportInitialize) this.pluginsDataGridView).EndInit();
      this.nativesTabPage.ResumeLayout(false);
      this.nativesTabPage.PerformLayout();
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class PluginInfoRow
    {
      private readonly PluginInfo plugin;

      public Image Icon
      {
        get
        {
          return this.plugin.Interface?.Icon ?? (Image) Resources.B16x16_Plugin;
        }
      }

      public string Name
      {
        get
        {
          return this.plugin.Name;
        }
      }

      public string Version
      {
        get
        {
          return this.plugin.FileVersion;
        }
      }

      public string Author
      {
        get
        {
          return this.plugin.Author;
        }
      }

      public string Description
      {
        get
        {
          return this.plugin.Description;
        }
      }

      public PluginInfoRow(PluginInfo plugin)
      {
        this.plugin = plugin;
      }
    }
  }
}
