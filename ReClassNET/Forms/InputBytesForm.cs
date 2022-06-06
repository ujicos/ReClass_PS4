// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.InputBytesForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class InputBytesForm : IconForm
  {
    private readonly int currentSize;
    private IContainer components;
    private Label label1;
    private RadioButton hexRadioButton;
    private RadioButton decimalRadioButton;
    private Label label2;
    private Label currentSizeLabel;
    private Label label4;
    private Label newSizeLabel;
    private Button okButton;
    private NumericUpDown bytesNumericUpDown;

    public int Bytes
    {
      get
      {
        return (int) this.bytesNumericUpDown.Value;
      }
    }

    public InputBytesForm(int currentSize)
    {
      this.currentSize = currentSize;
      this.InitializeComponent();
      this.bytesNumericUpDown.Maximum = new Decimal(int.MaxValue);
      this.FormatLabelText(this.currentSizeLabel, currentSize);
      this.FormatLabelText(this.newSizeLabel, currentSize);
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

    private void hexRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      this.bytesNumericUpDown.Hexadecimal = this.hexRadioButton.Checked;
    }

    private void bytesNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      this.FormatLabelText(this.newSizeLabel, this.currentSize + this.Bytes);
    }

    private void FormatLabelText(Label label, int size)
    {
      label.Text = string.Format("0x{0:X} / {1}", (object) size, (object) size);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.hexRadioButton = new RadioButton();
      this.decimalRadioButton = new RadioButton();
      this.label2 = new Label();
      this.currentSizeLabel = new Label();
      this.label4 = new Label();
      this.newSizeLabel = new Label();
      this.okButton = new Button();
      this.bytesNumericUpDown = new NumericUpDown();
      this.bytesNumericUpDown.BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(121, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Number of Bytes to add:";
      this.hexRadioButton.AutoSize = true;
      this.hexRadioButton.Location = new Point(77, 51);
      this.hexRadioButton.Name = "hexRadioButton";
      this.hexRadioButton.Size = new Size(44, 17);
      this.hexRadioButton.TabIndex = 2;
      this.hexRadioButton.Text = "Hex";
      this.hexRadioButton.UseVisualStyleBackColor = true;
      this.hexRadioButton.CheckedChanged += new EventHandler(this.hexRadioButton_CheckedChanged);
      this.decimalRadioButton.AutoSize = true;
      this.decimalRadioButton.Checked = true;
      this.decimalRadioButton.Location = new Point(8, 51);
      this.decimalRadioButton.Name = "decimalRadioButton";
      this.decimalRadioButton.Size = new Size(63, 17);
      this.decimalRadioButton.TabIndex = 3;
      this.decimalRadioButton.TabStop = true;
      this.decimalRadioButton.Text = "Decimal";
      this.decimalRadioButton.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 79);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Current class size:";
      this.currentSizeLabel.AutoSize = true;
      this.currentSizeLabel.Location = new Point(111, 79);
      this.currentSizeLabel.Name = "currentSizeLabel";
      this.currentSizeLabel.Size = new Size(19, 13);
      this.currentSizeLabel.TabIndex = 5;
      this.currentSizeLabel.Text = "<>";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(5, 98);
      this.label4.Name = "label4";
      this.label4.Size = new Size(80, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "New class size:";
      this.newSizeLabel.AutoSize = true;
      this.newSizeLabel.Location = new Point(111, 98);
      this.newSizeLabel.Name = "newSizeLabel";
      this.newSizeLabel.Size = new Size(19, 13);
      this.newSizeLabel.TabIndex = 7;
      this.newSizeLabel.Text = "<>";
      this.okButton.DialogResult = DialogResult.OK;
      this.okButton.Location = new Point(146, 121);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 23);
      this.okButton.TabIndex = 8;
      this.okButton.Text = "OK";
      this.okButton.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.okButton.UseVisualStyleBackColor = true;
      this.bytesNumericUpDown.Location = new Point(8, 25);
      this.bytesNumericUpDown.Name = "bytesNumericUpDown";
      this.bytesNumericUpDown.Size = new Size(212, 20);
      this.bytesNumericUpDown.TabIndex = 9;
      this.bytesNumericUpDown.ValueChanged += new EventHandler(this.bytesNumericUpDown_ValueChanged);
      this.AcceptButton = (IButtonControl) this.okButton;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(228, 151);
      this.Controls.Add((Control) this.bytesNumericUpDown);
      this.Controls.Add((Control) this.okButton);
      this.Controls.Add((Control) this.newSizeLabel);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.currentSizeLabel);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.decimalRadioButton);
      this.Controls.Add((Control) this.hexRadioButton);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InputBytesForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "<>";
      this.bytesNumericUpDown.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
