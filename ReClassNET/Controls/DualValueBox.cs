// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.DualValueBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  [Designer(typeof (DualValueControlDesigner))]
  public class DualValueBox : UserControl
  {
    private IContainer components;
    private Label label1;
    private TextBox value1TextBox;
    private Label label2;
    private TextBox value2TextBox;
    private TableLayoutPanel tableLayoutPanel;

    public bool ShowSecondInputField
    {
      get
      {
        return (double) this.tableLayoutPanel.ColumnStyles[0].Width <= 99.0;
      }
      set
      {
        if (value)
        {
          this.tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.Percent;
          this.tableLayoutPanel.ColumnStyles[1].Width = 50f;
          this.tableLayoutPanel.ColumnStyles[0].Width = 50f;
          this.value1TextBox.Margin = new Padding(0, 0, 1, 0);
        }
        else
        {
          this.tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.Absolute;
          this.tableLayoutPanel.ColumnStyles[1].Width = 0.0f;
          this.tableLayoutPanel.ColumnStyles[0].Width = 100f;
          this.value1TextBox.Margin = new Padding(0);
          this.value2TextBox.Text = (string) null;
        }
      }
    }

    public string Value1
    {
      get
      {
        return this.value1TextBox.Text;
      }
      set
      {
        this.value1TextBox.Text = value;
      }
    }

    public string Value2
    {
      get
      {
        return this.value2TextBox.Text;
      }
      set
      {
        this.value2TextBox.Text = value;
      }
    }

    public DualValueBox()
    {
      this.InitializeComponent();
    }

    protected override void SetBoundsCore(
      int x,
      int y,
      int width,
      int height,
      BoundsSpecified specified)
    {
      base.SetBoundsCore(x, y, width, 34, specified);
    }

    public void Clear()
    {
      this.Clear(true, true);
    }

    public void Clear(bool clearValue1, bool clearValue2)
    {
      if (clearValue1)
        this.value1TextBox.Clear();
      if (!clearValue2)
        return;
      this.value2TextBox.Clear();
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
      this.value1TextBox = new TextBox();
      this.label2 = new Label();
      this.value2TextBox = new TextBox();
      this.tableLayoutPanel = new TableLayoutPanel();
      this.tableLayoutPanel.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 0);
      this.label1.Margin = new Padding(0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(37, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Value:";
      this.value1TextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.value1TextBox.Location = new Point(0, 13);
      this.value1TextBox.Margin = new Padding(0);
      this.value1TextBox.Name = "value1TextBox";
      this.value1TextBox.Size = new Size((int) byte.MaxValue, 20);
      this.value1TextBox.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point((int) byte.MaxValue, 0);
      this.label2.Margin = new Padding(0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(1, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Value 2:";
      this.value2TextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.value2TextBox.Location = new Point((int) byte.MaxValue, 13);
      this.value2TextBox.Margin = new Padding(1, 0, 0, 0);
      this.value2TextBox.Name = "value2TextBox";
      this.value2TextBox.Size = new Size(1, 20);
      this.value2TextBox.TabIndex = 3;
      this.tableLayoutPanel.ColumnCount = 2;
      this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
      this.tableLayoutPanel.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel.Controls.Add((Control) this.value2TextBox, 1, 1);
      this.tableLayoutPanel.Controls.Add((Control) this.value1TextBox, 0, 1);
      this.tableLayoutPanel.Controls.Add((Control) this.label2, 1, 0);
      this.tableLayoutPanel.Dock = DockStyle.Fill;
      this.tableLayoutPanel.Location = new Point(0, 0);
      this.tableLayoutPanel.Margin = new Padding(0);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 2;
      this.tableLayoutPanel.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel.Size = new Size((int) byte.MaxValue, 34);
      this.tableLayoutPanel.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tableLayoutPanel);
      this.Name = nameof (DualValueBox);
      this.Size = new Size((int) byte.MaxValue, 34);
      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
