// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.PlaceholderTextBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class PlaceholderTextBox : TextBox
  {
    private Font fontBackup;
    private Color foreColorBackup;
    private Color backColorBackup;

    [DefaultValue(typeof (Color), "ControlDarkDark")]
    public Color PlaceholderColor { get; set; } = SystemColors.ControlDarkDark;

    [DefaultValue("")]
    public string PlaceholderText { get; set; }

    public PlaceholderTextBox()
    {
      this.fontBackup = this.Font;
      this.foreColorBackup = this.ForeColor;
      this.backColorBackup = this.BackColor;
      this.SetStyle(ControlStyles.UserPaint, true);
    }

    protected override void OnTextChanged(EventArgs e)
    {
      base.OnTextChanged(e);
      if (string.IsNullOrEmpty(this.Text))
      {
        if (this.GetStyle(ControlStyles.UserPaint))
          return;
        this.fontBackup = this.Font;
        this.foreColorBackup = this.ForeColor;
        this.backColorBackup = this.BackColor;
        this.SetStyle(ControlStyles.UserPaint, true);
      }
      else
      {
        if (!this.GetStyle(ControlStyles.UserPaint))
          return;
        this.SetStyle(ControlStyles.UserPaint, false);
        this.Font = this.fontBackup;
        this.ForeColor = this.foreColorBackup;
        this.BackColor = this.backColorBackup;
      }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (!string.IsNullOrEmpty(this.Text) || this.Focused)
        return;
      using (SolidBrush solidBrush = new SolidBrush(this.PlaceholderColor))
        e.Graphics.DrawString(this.PlaceholderText ?? string.Empty, this.Font, (Brush) solidBrush, new PointF(-1f, 1f));
    }
  }
}
