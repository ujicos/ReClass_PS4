// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.EnumEditorForm
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
  public class EnumEditorForm : IconForm
  {
    private readonly EnumDescription @enum;
    private IContainer components;
    private BannerBox bannerBox;
    private Button saveButton;
    private Label enumNameLabel;
    private Button cancelButton;
    private CheckBox enumFlagCheckBox;
    private TextBox enumNameTextBox;
    private DataGridView enumDataGridView;
    private Label enumUnderlyingTypeSizeLabel;
    private DataGridViewTextBoxColumn enumValueKeyColumn;
    private DataGridViewTextBoxColumn enumValueNameColumn;
    private UnderlyingSizeComboBox enumUnderlyingTypeSizeComboBox;

    public EnumEditorForm(EnumDescription @enum)
    {
      this.InitializeComponent();
      this.@enum = @enum;
      this.enumNameTextBox.Text = @enum.Name;
      this.enumUnderlyingTypeSizeComboBox.SelectedValue = @enum.Size;
      this.enumFlagCheckBox.Checked = @enum.UseFlagsMode;
      foreach (KeyValuePair<string, long> keyValuePair in (IEnumerable<KeyValuePair<string, long>>) @enum.Values)
        this.enumDataGridView.Rows.Add((object) keyValuePair.Value, (object) keyValuePair.Key);
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

    private void enumDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
      long num = (long) e.Row.Index;
      if (this.enumFlagCheckBox.Checked)
        num = (long) Math.Pow(2.0, (double) e.Row.Index);
      e.Row.Cells[0].Value = (object) num;
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
      this.@enum.Name = this.enumNameTextBox.Text;
      Dictionary<string, long> dictionary = new Dictionary<string, long>();
      foreach (DataGridViewRow dataGridViewRow in this.enumDataGridView.Rows.Cast<DataGridViewRow>().Where<DataGridViewRow>((Func<DataGridViewRow, bool>) (r => !r.IsNewRow)))
      {
        long result;
        if (long.TryParse(Convert.ToString(dataGridViewRow.Cells[0].Value), out result))
        {
          string key = Convert.ToString(dataGridViewRow.Cells[1].Value);
          dictionary.Add(key, result);
        }
      }
      this.@enum.SetData(this.enumFlagCheckBox.Checked, this.enumUnderlyingTypeSizeComboBox.SelectedValue, (IEnumerable<KeyValuePair<string, long>>) dictionary);
    }

    private void enumDataGridView_CellValidating(
      object sender,
      DataGridViewCellValidatingEventArgs e)
    {
      SetErrorText((string) null);
      string s = Convert.ToString(e.FormattedValue);
      if (e.ColumnIndex == 0 && !long.TryParse(s, out long _))
      {
        SetErrorText("'" + s + "' is not a valid value.");
      }
      else
      {
        if (e.ColumnIndex != 1 || !string.IsNullOrWhiteSpace(s))
          return;
        SetErrorText("Empty names are not allowed.");
      }

      void SetErrorText(string text)
      {
        this.enumDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = text;
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
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      this.bannerBox = new BannerBox();
      this.saveButton = new Button();
      this.enumNameLabel = new Label();
      this.cancelButton = new Button();
      this.enumFlagCheckBox = new CheckBox();
      this.enumNameTextBox = new TextBox();
      this.enumDataGridView = new DataGridView();
      this.enumValueKeyColumn = new DataGridViewTextBoxColumn();
      this.enumValueNameColumn = new DataGridViewTextBoxColumn();
      this.enumUnderlyingTypeSizeLabel = new Label();
      this.enumUnderlyingTypeSizeComboBox = new UnderlyingSizeComboBox();
      this.bannerBox.BeginInit();
      ((ISupportInitialize) this.enumDataGridView).BeginInit();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B16x16_Class_Type;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(380, 48);
      this.bannerBox.TabIndex = 15;
      this.bannerBox.Text = "Edit an enum of the project.";
      this.bannerBox.Title = "Enum Editor";
      this.saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveButton.DialogResult = DialogResult.OK;
      this.saveButton.FlatStyle = FlatStyle.Flat;
      this.saveButton.Location = new Point(214, 250);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new Size(75, 23);
      this.saveButton.TabIndex = 22;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new EventHandler(this.saveButton_Click);
      this.enumNameLabel.AutoSize = true;
      this.enumNameLabel.Location = new Point(9, 57);
      this.enumNameLabel.Name = "enumNameLabel";
      this.enumNameLabel.Size = new Size(38, 13);
      this.enumNameLabel.TabIndex = 21;
      this.enumNameLabel.Text = "Name:";
      this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelButton.DialogResult = DialogResult.Cancel;
      this.cancelButton.FlatStyle = FlatStyle.Flat;
      this.cancelButton.Location = new Point(295, 250);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 23);
      this.cancelButton.TabIndex = 19;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.enumFlagCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.enumFlagCheckBox.AutoSize = true;
      this.enumFlagCheckBox.Location = new Point(267, 82);
      this.enumFlagCheckBox.Name = "enumFlagCheckBox";
      this.enumFlagCheckBox.Size = new Size(103, 17);
      this.enumFlagCheckBox.TabIndex = 18;
      this.enumFlagCheckBox.Text = "Use Flags Mode";
      this.enumFlagCheckBox.UseVisualStyleBackColor = true;
      this.enumNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.enumNameTextBox.BackColor = Color.FromArgb(82, 90, 95);
      this.enumNameTextBox.BorderStyle = BorderStyle.FixedSingle;
      this.enumNameTextBox.ForeColor = Color.White;
      this.enumNameTextBox.Location = new Point(53, 54);
      this.enumNameTextBox.Name = "enumNameTextBox";
      this.enumNameTextBox.Size = new Size(315, 20);
      this.enumNameTextBox.TabIndex = 17;
      this.enumDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.enumDataGridView.BackgroundColor = Color.FromArgb(60, 63, 65);
      this.enumDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.enumDataGridView.Columns.AddRange((DataGridViewColumn) this.enumValueKeyColumn, (DataGridViewColumn) this.enumValueNameColumn);
      this.enumDataGridView.GridColor = Color.White;
      this.enumDataGridView.Location = new Point(12, 108);
      this.enumDataGridView.Name = "enumDataGridView";
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = Color.FromArgb(60, 63, 65);
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = Color.White;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = Color.White;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.enumDataGridView.RowHeadersDefaultCellStyle = gridViewCellStyle1;
      this.enumDataGridView.RowHeadersVisible = false;
      gridViewCellStyle2.BackColor = Color.FromArgb(69, 73, 74);
      gridViewCellStyle2.ForeColor = Color.White;
      this.enumDataGridView.RowsDefaultCellStyle = gridViewCellStyle2;
      this.enumDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.enumDataGridView.Size = new Size(358, 136);
      this.enumDataGridView.TabIndex = 16;
      this.enumDataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(this.enumDataGridView_CellValidating);
      this.enumDataGridView.DefaultValuesNeeded += new DataGridViewRowEventHandler(this.enumDataGridView_DefaultValuesNeeded);
      this.enumValueKeyColumn.HeaderText = "Value";
      this.enumValueKeyColumn.Name = "enumValueKeyColumn";
      this.enumValueNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.enumValueNameColumn.HeaderText = "Name";
      this.enumValueNameColumn.Name = "enumValueNameColumn";
      this.enumUnderlyingTypeSizeLabel.AutoSize = true;
      this.enumUnderlyingTypeSizeLabel.Location = new Point(9, 83);
      this.enumUnderlyingTypeSizeLabel.Name = "enumUnderlyingTypeSizeLabel";
      this.enumUnderlyingTypeSizeLabel.Size = new Size(30, 13);
      this.enumUnderlyingTypeSizeLabel.TabIndex = 24;
      this.enumUnderlyingTypeSizeLabel.Text = "Size:";
      this.enumUnderlyingTypeSizeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.enumUnderlyingTypeSizeComboBox.BackColor = Color.FromArgb(69, 73, 74);
      this.enumUnderlyingTypeSizeComboBox.FlatStyle = FlatStyle.Flat;
      this.enumUnderlyingTypeSizeComboBox.ForeColor = Color.White;
      this.enumUnderlyingTypeSizeComboBox.Location = new Point(53, 80);
      this.enumUnderlyingTypeSizeComboBox.Name = "enumUnderlyingTypeSizeComboBox";
      this.enumUnderlyingTypeSizeComboBox.Size = new Size(208, 21);
      this.enumUnderlyingTypeSizeComboBox.TabIndex = 25;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.ClientSize = new Size(380, 282);
      this.Controls.Add((Control) this.enumUnderlyingTypeSizeComboBox);
      this.Controls.Add((Control) this.enumUnderlyingTypeSizeLabel);
      this.Controls.Add((Control) this.saveButton);
      this.Controls.Add((Control) this.enumNameLabel);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.enumFlagCheckBox);
      this.Controls.Add((Control) this.enumNameTextBox);
      this.Controls.Add((Control) this.enumDataGridView);
      this.Controls.Add((Control) this.bannerBox);
      this.ForeColor = Color.White;
      this.MinimumSize = new Size(396, 321);
      this.Name = nameof (EnumEditorForm);
      this.Text = "ReClass.NET - Enum Editor";
      this.bannerBox.EndInit();
      ((ISupportInitialize) this.enumDataGridView).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
