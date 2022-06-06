// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.ColorBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  [DefaultEvent("ColorChanged")]
  [DefaultBindingProperty("Color")]
  public class ColorBox : UserControl
  {
    private bool updateTextBox = true;
    private const int DefaultWidth = 123;
    private const int DefaultHeight = 20;
    private Color color;
    private IContainer components;
    private TextBox valueTextBox;
    private Panel colorPanel;

    public event EventHandler ColorChanged;

    public Color Color
    {
      get
      {
        return this.color;
      }
      set
      {
        value = Color.FromArgb(value.ToArgb());
        if (this.color != value)
        {
          this.color = value;
          this.colorPanel.BackColor = value;
          if (this.updateTextBox)
            this.valueTextBox.Text = ColorTranslator.ToHtml(value);
          this.OnColorChanged(EventArgs.Empty);
        }
        this.updateTextBox = true;
      }
    }

    protected virtual void OnColorChanged(EventArgs e)
    {
      EventHandler colorChanged = this.ColorChanged;
      if (colorChanged == null)
        return;
      colorChanged((object) this, e);
    }

    public ColorBox()
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
      base.SetBoundsCore(x, y, 123, 20, specified);
    }

    private void OnTextChanged(object sender, EventArgs e)
    {
      try
      {
        string htmlColor = this.valueTextBox.Text;
        if (!htmlColor.StartsWith("#"))
          htmlColor = "#" + htmlColor;
        Color color = ColorTranslator.FromHtml(htmlColor);
        this.updateTextBox = false;
        this.Color = color;
      }
      catch
      {
      }
    }

    private void OnPanelClick(object sender, EventArgs e)
    {
      using (ColorDialog colorDialog = new ColorDialog()
      {
        FullOpen = true,
        Color = this.Color
      })
      {
        if (colorDialog.ShowDialog() != DialogResult.OK)
          return;
        this.Color = colorDialog.Color;
      }
    }

    private void OnPanelPaint(object sender, PaintEventArgs e)
    {
      Rectangle clientRectangle = this.colorPanel.ClientRectangle;
      --clientRectangle.Width;
      --clientRectangle.Height;
      e.Graphics.DrawRectangle(Pens.Black, clientRectangle);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.valueTextBox = new TextBox();
      this.colorPanel = new Panel();
      this.SuspendLayout();
      this.valueTextBox.Location = new Point(37, 0);
      this.valueTextBox.Name = "valueTextBox";
      this.valueTextBox.Size = new Size(86, 20);
      this.valueTextBox.TabIndex = 0;
      this.valueTextBox.TextChanged += new EventHandler(this.OnTextChanged);
      this.colorPanel.Location = new Point(0, 0);
      this.colorPanel.Name = "colorPanel";
      this.colorPanel.Size = new Size(30, 20);
      this.colorPanel.TabIndex = 1;
      this.colorPanel.Click += new EventHandler(this.OnPanelClick);
      this.colorPanel.Paint += new PaintEventHandler(this.OnPanelPaint);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.colorPanel);
      this.Controls.Add((Control) this.valueTextBox);
      this.Name = nameof (ColorBox);
      this.Size = new Size(123, 20);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
