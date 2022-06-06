// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.EnumListForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Project;
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
  public class EnumListForm : IconForm
  {
    private readonly ReClassNetProject project;
    private IContainer components;
    private PlaceholderTextBox filterNameTextBox;
    private ListBox itemListBox;
    private BannerBox bannerBox;
    private IconButton addEnumIconButton;
    private IconButton removeEnumIconButton;
    private IconButton editEnumIconButton;

    public EnumDescription SelectedItem
    {
      get
      {
        return this.itemListBox.SelectedItem as EnumDescription;
      }
    }

    public EnumListForm(ReClassNetProject project)
    {
      this.project = project;
      this.InitializeComponent();
      this.ShowFilteredEnums();
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

    private void filterNameTextBox_TextChanged(object sender, EventArgs e)
    {
      this.ShowFilteredEnums();
    }

    private void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.editEnumIconButton.Enabled = this.removeEnumIconButton.Enabled = this.SelectedItem != null;
    }

    private void editEnumIconButton_Click(object sender, EventArgs e)
    {
      EnumDescription selectedItem = this.SelectedItem;
      if (selectedItem == null)
        return;
      using (EnumEditorForm enumEditorForm = new EnumEditorForm(selectedItem))
      {
        int num = (int) enumEditorForm.ShowDialog();
      }
    }

    private void addEnumIconButton_Click(object sender, EventArgs e)
    {
      EnumDescription @enum = new EnumDescription()
      {
        Name = "Enum"
      };
      using (EnumEditorForm enumEditorForm = new EnumEditorForm(@enum))
      {
        if (enumEditorForm.ShowDialog() != DialogResult.OK)
          return;
        this.project.AddEnum(@enum);
        this.ShowFilteredEnums();
      }
    }

    private void removeEnumIconButton_Click(object sender, EventArgs e)
    {
      EnumDescription selectedItem = this.SelectedItem;
      if (selectedItem == null)
        return;
      this.project.RemoveEnum(selectedItem);
      this.ShowFilteredEnums();
    }

    private void ShowFilteredEnums()
    {
      IEnumerable<EnumDescription> source = (IEnumerable<EnumDescription>) this.project.Enums;
      if (!string.IsNullOrEmpty(this.filterNameTextBox.Text))
        source = source.Where<EnumDescription>((Func<EnumDescription, bool>) (c => c.Name.IndexOf(this.filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0));
      this.itemListBox.DataSource = (object) source.ToList<EnumDescription>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.filterNameTextBox = new PlaceholderTextBox();
      this.itemListBox = new ListBox();
      this.bannerBox = new BannerBox();
      this.addEnumIconButton = new IconButton();
      this.removeEnumIconButton = new IconButton();
      this.editEnumIconButton = new IconButton();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.filterNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.filterNameTextBox.BackColor = Color.FromArgb(60, 63, 65);
      this.filterNameTextBox.ForeColor = Color.White;
      this.filterNameTextBox.Location = new Point(12, 60);
      this.filterNameTextBox.Name = "filterNameTextBox";
      this.filterNameTextBox.PlaceholderText = "Filter by Enum Name...";
      this.filterNameTextBox.Size = new Size(411, 20);
      this.filterNameTextBox.TabIndex = 10;
      this.filterNameTextBox.TextChanged += new EventHandler(this.filterNameTextBox_TextChanged);
      this.itemListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.itemListBox.BackColor = Color.FromArgb(60, 63, 65);
      this.itemListBox.DisplayMember = "Name";
      this.itemListBox.ForeColor = Color.White;
      this.itemListBox.FormattingEnabled = true;
      this.itemListBox.Location = new Point(12, 86);
      this.itemListBox.Name = "itemListBox";
      this.itemListBox.Size = new Size(492, 212);
      this.itemListBox.TabIndex = 11;
      this.itemListBox.SelectedIndexChanged += new EventHandler(this.itemListBox_SelectedIndexChanged);
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B16x16_Class_Type;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(516, 48);
      this.bannerBox.TabIndex = 14;
      this.bannerBox.Text = "Edit the enums of the project.";
      this.bannerBox.Title = "Enums";
      this.addEnumIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addEnumIconButton.Image = (Image) Resources.B16x16_Button_Add;
      this.addEnumIconButton.Location = new Point(456, 59);
      this.addEnumIconButton.Name = "addEnumIconButton";
      this.addEnumIconButton.Pressed = false;
      this.addEnumIconButton.Selected = false;
      this.addEnumIconButton.Size = new Size(23, 22);
      this.addEnumIconButton.TabIndex = 15;
      this.addEnumIconButton.Click += new EventHandler(this.addEnumIconButton_Click);
      this.removeEnumIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removeEnumIconButton.Enabled = false;
      this.removeEnumIconButton.Image = (Image) Resources.B16x16_Button_Remove;
      this.removeEnumIconButton.Location = new Point(481, 59);
      this.removeEnumIconButton.Name = "removeEnumIconButton";
      this.removeEnumIconButton.Pressed = false;
      this.removeEnumIconButton.Selected = false;
      this.removeEnumIconButton.Size = new Size(23, 22);
      this.removeEnumIconButton.TabIndex = 16;
      this.removeEnumIconButton.Click += new EventHandler(this.removeEnumIconButton_Click);
      this.editEnumIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.editEnumIconButton.Enabled = false;
      this.editEnumIconButton.Image = (Image) Resources.B16x16_Custom_Type;
      this.editEnumIconButton.Location = new Point(431, 59);
      this.editEnumIconButton.Name = "editEnumIconButton";
      this.editEnumIconButton.Pressed = false;
      this.editEnumIconButton.Selected = false;
      this.editEnumIconButton.Size = new Size(23, 22);
      this.editEnumIconButton.TabIndex = 16;
      this.editEnumIconButton.Click += new EventHandler(this.editEnumIconButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.ClientSize = new Size(516, 308);
      this.Controls.Add((Control) this.editEnumIconButton);
      this.Controls.Add((Control) this.addEnumIconButton);
      this.Controls.Add((Control) this.removeEnumIconButton);
      this.Controls.Add((Control) this.filterNameTextBox);
      this.Controls.Add((Control) this.itemListBox);
      this.Controls.Add((Control) this.bannerBox);
      this.ForeColor = Color.White;
      this.Name = nameof (EnumListForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Enums";
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
