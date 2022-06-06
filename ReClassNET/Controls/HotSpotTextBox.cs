// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.HotSpotTextBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class HotSpotTextBox : TextBox
  {
    private HotSpot currentHotSpot;
    private FontEx font;
    private int minimumWidth;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public FontEx Font
    {
      get
      {
        return this.font;
      }
      set
      {
        if (this.font == value)
          return;
        this.font = value;
        this.Font = this.font.Font;
      }
    }

    public event HotSpotTextBoxCommitEventHandler Committed;

    public HotSpotTextBox()
    {
      this.BorderStyle = BorderStyle.None;
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
      base.OnVisibleChanged(e);
      if (!this.Visible)
        return;
      this.BackColor = Program.Settings.BackgroundColor;
      if (this.currentHotSpot == null)
        return;
      this.Focus();
      this.Select(0, this.TextLength);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        this.OnCommit();
        e.Handled = true;
        e.SuppressKeyPress = true;
      }
      base.OnKeyDown(e);
    }

    protected override void OnTextChanged(EventArgs e)
    {
      base.OnTextChanged(e);
      int num = (this.TextLength + 1) * this.font.Width;
      if (num <= this.minimumWidth)
        return;
      this.Width = num;
    }

    private void OnCommit()
    {
      this.Visible = false;
      this.currentHotSpot.Text = this.Text.Trim();
      HotSpotTextBoxCommitEventHandler committed = this.Committed;
      if (committed == null)
        return;
      committed((object) this, new HotSpotTextBoxCommitEventArgs(this.currentHotSpot));
    }

    public void ShowOnHotSpot(HotSpot hotSpot)
    {
      this.currentHotSpot = hotSpot;
      if (hotSpot == null)
      {
        this.Visible = false;
      }
      else
      {
        this.AlignToRect(hotSpot.Rect);
        this.Text = hotSpot.Text.Trim();
        this.ReadOnly = hotSpot.Id == 999;
        this.Visible = true;
      }
    }

    private void AlignToRect(Rectangle rect)
    {
      this.SetBounds(rect.Left + 2, rect.Top, rect.Width, rect.Height);
      this.minimumWidth = rect.Width;
    }
  }
}
