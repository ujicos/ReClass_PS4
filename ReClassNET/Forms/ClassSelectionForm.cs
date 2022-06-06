// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.ClassSelectionForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Nodes;
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
  public class ClassSelectionForm : IconForm
  {
    private readonly List<ClassNode> allClasses;
    private IContainer components;
    private BannerBox bannerBox;
    private ListBox classesListBox;
    private PlaceholderTextBox filterNameTextBox;
    private Button cancelButton;
    private Button selectButton;

    public ClassNode SelectedClass
    {
      get
      {
        return this.classesListBox.SelectedItem as ClassNode;
      }
    }

    public ClassSelectionForm(IEnumerable<ClassNode> classes)
    {
      this.allClasses = classes.ToList<ClassNode>();
      this.InitializeComponent();
      this.ShowFilteredClasses();
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
      this.ShowFilteredClasses();
    }

    private void classesListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.selectButton.Enabled = this.SelectedClass != null;
    }

    private void classesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.SelectedClass == null)
        return;
      this.selectButton.PerformClick();
    }

    private void ShowFilteredClasses()
    {
      IEnumerable<ClassNode> source = (IEnumerable<ClassNode>) this.allClasses;
      if (!string.IsNullOrEmpty(this.filterNameTextBox.Text))
        source = source.Where<ClassNode>((Func<ClassNode, bool>) (c => c.Name.IndexOf(this.filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0));
      this.classesListBox.DataSource = (object) source.ToList<ClassNode>();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bannerBox = new BannerBox();
      this.classesListBox = new ListBox();
      this.filterNameTextBox = new PlaceholderTextBox();
      this.cancelButton = new Button();
      this.selectButton = new Button();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B16x16_Class_Type;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(516, 48);
      this.bannerBox.TabIndex = 9;
      this.bannerBox.Text = "Select a class of the project.";
      this.bannerBox.Title = "Class Selection";
      this.classesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.classesListBox.BackColor = Color.FromArgb(60, 63, 65);
      this.classesListBox.DisplayMember = "Name";
      this.classesListBox.ForeColor = Color.White;
      this.classesListBox.FormattingEnabled = true;
      this.classesListBox.Location = new Point(12, 80);
      this.classesListBox.Name = "classesListBox";
      this.classesListBox.Size = new Size(492, 186);
      this.classesListBox.TabIndex = 2;
      this.classesListBox.SelectedIndexChanged += new EventHandler(this.classesListBox_SelectedIndexChanged);
      this.classesListBox.MouseDoubleClick += new MouseEventHandler(this.classesListBox_MouseDoubleClick);
      this.filterNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.filterNameTextBox.BackColor = Color.FromArgb(60, 63, 65);
      this.filterNameTextBox.ForeColor = Color.White;
      this.filterNameTextBox.Location = new Point(12, 54);
      this.filterNameTextBox.Name = "filterNameTextBox";
      this.filterNameTextBox.PlaceholderText = "Filter by Class Name...";
      this.filterNameTextBox.Size = new Size(492, 20);
      this.filterNameTextBox.TabIndex = 1;
      this.filterNameTextBox.TextChanged += new EventHandler(this.filterNameTextBox_TextChanged);
      this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelButton.DialogResult = DialogResult.Cancel;
      this.cancelButton.FlatStyle = FlatStyle.Flat;
      this.cancelButton.Location = new Point(430, 272);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.selectButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.selectButton.DialogResult = DialogResult.OK;
      this.selectButton.Enabled = false;
      this.selectButton.FlatStyle = FlatStyle.Flat;
      this.selectButton.ForeColor = Color.White;
      this.selectButton.Location = new Point(328, 272);
      this.selectButton.Name = "selectButton";
      this.selectButton.Size = new Size(95, 23);
      this.selectButton.TabIndex = 3;
      this.selectButton.Text = "Select Class";
      this.selectButton.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.selectButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.CancelButton = (IButtonControl) this.cancelButton;
      this.ClientSize = new Size(516, 306);
      this.Controls.Add((Control) this.selectButton);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.filterNameTextBox);
      this.Controls.Add((Control) this.classesListBox);
      this.Controls.Add((Control) this.bannerBox);
      this.ForeColor = Color.White;
      this.Name = nameof (ClassSelectionForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Class Selection";
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
