// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.IconButton
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  [DefaultEvent("Click")]
  public class IconButton : Panel
  {
    private readonly ProfessionalColorTable colorTable = new ProfessionalColorTable();

    public bool Pressed { get; set; }

    public bool Selected { get; set; }

    public Image Image { get; set; }

    public Rectangle ImageRectangle { get; } = new Rectangle(3, 3, 16, 16);

    public IconButton()
    {
      this.DoubleBuffered = true;
    }

    protected override void SetBoundsCore(
      int x,
      int y,
      int width,
      int height,
      BoundsSpecified specified)
    {
      base.SetBoundsCore(x, y, 23, 22, specified);
    }

    protected override void Select(bool directed, bool forward)
    {
      base.Select(directed, forward);
      this.Selected = true;
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      this.Pressed = true;
      this.Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      this.Pressed = false;
      this.Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      this.Selected = true;
      this.Invalidate();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      this.Selected = false;
      this.Pressed = false;
      this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      this.RenderButtonBackground(e.Graphics);
      this.RenderImage(e.Graphics);
    }

    private void RenderButtonBackground(Graphics g)
    {
      Rectangle rectangle = new Rectangle(Point.Empty, this.Size);
      bool flag = true;
      if (this.Pressed)
        this.RenderPressedButtonFill(g, rectangle);
      else if (this.Selected)
      {
        this.RenderSelectedButtonFill(g, rectangle);
      }
      else
      {
        flag = false;
        using (SolidBrush solidBrush = new SolidBrush(this.BackColor))
          g.FillRectangle((Brush) solidBrush, rectangle);
      }
      if (!flag)
        return;
      using (Pen pen = new Pen(this.colorTable.ButtonSelectedBorder))
        g.DrawRectangle(pen, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
    }

    private void RenderPressedButtonFill(Graphics g, Rectangle bounds)
    {
      if (bounds.Width == 0 || bounds.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, this.colorTable.ButtonPressedGradientBegin, this.colorTable.ButtonPressedGradientEnd, LinearGradientMode.Vertical))
        g.FillRectangle((Brush) linearGradientBrush, bounds);
    }

    private void RenderSelectedButtonFill(Graphics g, Rectangle bounds)
    {
      if (bounds.Width == 0 || bounds.Height == 0)
        return;
      using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, this.colorTable.ButtonSelectedGradientBegin, this.colorTable.ButtonSelectedGradientEnd, LinearGradientMode.Vertical))
        g.FillRectangle((Brush) linearGradientBrush, bounds);
    }

    private void RenderImage(Graphics g)
    {
      Image image = this.Image;
      if (image == null)
        return;
      Rectangle imageRectangle = this.ImageRectangle;
      if (!this.Enabled)
      {
        bool flag = false;
        if (this.Pressed)
          ++imageRectangle.X;
        if (!this.Enabled)
        {
          image = ToolStripRenderer.CreateDisabledImage(image);
          flag = true;
        }
        g.DrawImage(image, imageRectangle);
        if (!flag)
          return;
        image.Dispose();
      }
      else
        g.DrawImage(image, imageRectangle);
    }
  }
}
