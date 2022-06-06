// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.BannerBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class BannerBox : Control, ISupportInitialize
  {
    public const int DefaultBannerHeight = 48;
    private bool inInitialize;
    private Image icon;
    private string title;
    private string text;
    private Image image;

    public Image Icon
    {
      get
      {
        return this.icon;
      }
      set
      {
        this.icon = value;
        this.UpdateBanner();
      }
    }

    public string Title
    {
      get
      {
        return this.title;
      }
      set
      {
        this.title = value ?? string.Empty;
        this.UpdateBanner();
      }
    }

    public override string Text
    {
      get
      {
        return this.text;
      }
      set
      {
        this.text = value ?? string.Empty;
        this.UpdateBanner();
      }
    }

    public BannerBox()
    {
      this.title = string.Empty;
      this.text = string.Empty;
    }

    protected override void SetBoundsCore(
      int x,
      int y,
      int width,
      int height,
      BoundsSpecified specified)
    {
      int width1 = this.Width;
      base.SetBoundsCore(x, y, width, DpiUtil.ScaleIntY(48), specified);
      int num = width;
      if (width1 == num || width <= 0)
        return;
      this.UpdateBanner();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (this.image == null)
        return;
      e.Graphics.DrawImage(this.image, this.ClientRectangle);
    }

    public void BeginInit()
    {
      this.inInitialize = true;
    }

    public void EndInit()
    {
      this.inInitialize = false;
      this.UpdateBanner();
    }

    private void UpdateBanner()
    {
      if (this.inInitialize)
        return;
      try
      {
        Image image = this.image;
        this.image = BannerFactory.CreateBanner(this.Width, this.Height, this.icon, this.title, this.text, true);
        image?.Dispose();
        this.Invalidate();
      }
      catch
      {
      }
    }
  }
}
