// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.DpiUtil
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Native;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ReClassNET.UI
{
  public static class DpiUtil
  {
    private static int dpiX = 96;
    private static int dpiY = 96;
    private static double scaleX = 1.0;
    private static double scaleY = 1.0;
    public const int DefalutDpi = 96;

    public static void ConfigureProcess()
    {
      NativeMethods.SetProcessDpiAwareness();
    }

    public static void SetDpi(int x, int y)
    {
      DpiUtil.dpiX = x;
      DpiUtil.dpiY = y;
      if (DpiUtil.dpiX <= 0 || DpiUtil.dpiY <= 0)
      {
        DpiUtil.dpiX = 96;
        DpiUtil.dpiY = 96;
      }
      DpiUtil.scaleX = (double) DpiUtil.dpiX / 96.0;
      DpiUtil.scaleY = (double) DpiUtil.dpiY / 96.0;
    }

    public static void TrySetDpiFromCurrentDesktop()
    {
      try
      {
        using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
          DpiUtil.SetDpi((int) graphics.DpiX, (int) graphics.DpiY);
      }
      catch
      {
      }
    }

    public static int ScaleIntX(int i)
    {
      return (int) Math.Round((double) i * DpiUtil.scaleX);
    }

    public static int ScaleIntY(int i)
    {
      return (int) Math.Round((double) i * DpiUtil.scaleY);
    }

    public static Image ScaleImage(Image sourceImage)
    {
      if (sourceImage == null)
        return (Image) null;
      int width1 = sourceImage.Width;
      int height1 = sourceImage.Height;
      int width2 = DpiUtil.ScaleIntX(width1);
      int height2 = DpiUtil.ScaleIntY(height1);
      return width1 == width2 && height1 == height2 ? sourceImage : DpiUtil.ScaleImage(sourceImage, width2, height2);
    }

    private static Image ScaleImage(Image sourceImage, int width, int height)
    {
      Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        graphics.Clear(Color.Transparent);
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        int width1 = sourceImage.Width;
        int height1 = sourceImage.Height;
        InterpolationMode interpolationMode = InterpolationMode.HighQualityBicubic;
        if (width1 > 0 && height1 > 0 && (width % width1 == 0 && height % height1 == 0))
          interpolationMode = InterpolationMode.NearestNeighbor;
        graphics.InterpolationMode = interpolationMode;
        RectangleF srcRect = new RectangleF(0.0f, 0.0f, (float) width1, (float) height1);
        RectangleF destRect = new RectangleF(0.0f, 0.0f, (float) width, (float) height);
        DpiUtil.AdjustScaleRects(ref srcRect, ref destRect);
        graphics.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);
        return (Image) bitmap;
      }
    }

    private static void AdjustScaleRects(ref RectangleF srcRect, ref RectangleF destRect)
    {
      if ((double) destRect.Width > (double) srcRect.Width)
        srcRect.X -= 0.5f;
      if ((double) destRect.Height > (double) srcRect.Height)
        srcRect.Y -= 0.5f;
      if ((double) destRect.Width < (double) srcRect.Width)
        srcRect.X += 0.5f;
      if ((double) destRect.Height >= (double) srcRect.Height)
        return;
      srcRect.Y += 0.5f;
    }
  }
}
