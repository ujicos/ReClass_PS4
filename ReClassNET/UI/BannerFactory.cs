// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.BannerFactory
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ReClassNET.UI
{
  public static class BannerFactory
  {
    private static readonly Dictionary<string, Image> imageCache = new Dictionary<string, Image>();
    private const int StdHeight = 48;
    private const int StdIconDim = 32;
    private const int MaxCacheEntries = 20;

    public static Image CreateBanner(
      int bannerWidth,
      int bannerHeight,
      Image icon,
      string title,
      string text,
      bool skipCache)
    {
      string key = string.Format("{0}x{1}:{2}:{3}", (object) bannerWidth, (object) bannerHeight, (object) title, (object) text);
      Image image;
      if (skipCache || !BannerFactory.imageCache.TryGetValue(key, out image))
      {
        image = (Image) new Bitmap(bannerWidth, bannerHeight, PixelFormat.Format24bppRgb);
        using (Graphics g = Graphics.FromImage(image))
        {
          int x1 = BannerFactory.DpiScaleInt(10, bannerHeight);
          Rectangle rect = new Rectangle(0, 0, bannerWidth, bannerHeight);
          using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, Color.FromArgb(151, 154, 173), Color.FromArgb(27, 27, 37), LinearGradientMode.Vertical))
            g.FillRectangle((Brush) linearGradientBrush, rect);
          int width1 = 32;
          if (icon != null)
          {
            width1 = (int) Math.Round((double) BannerFactory.DpiScaleFloat((float) ((double) icon.Width / (double) icon.Height * 32.0), bannerHeight));
            int height1 = BannerFactory.DpiScaleInt(32, bannerHeight);
            int y1 = (bannerHeight - height1) / 2;
            if (height1 == icon.Height)
              g.DrawImageUnscaled(icon, x1, y1);
            else
              g.DrawImage(icon, x1, y1, width1, height1);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(new ColorMatrix()
            {
              Matrix33 = 0.1f
            });
            int width2 = width1 * 2;
            int height2 = height1 * 2;
            int x2 = bannerWidth - width2 - x1;
            int y2 = (bannerHeight - height2) / 2;
            g.DrawImage(icon, new Rectangle(x2, y2, width2, height2), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, imageAttr);
          }
          int x3 = 2 * x1;
          int y3 = BannerFactory.DpiScaleInt(4, bannerHeight);
          if (icon != null)
            x3 += width1;
          using (Font font = new Font(FontFamily.GenericSansSerif, BannerFactory.DpiScaleFloat(1152f / g.DpiY, bannerHeight), FontStyle.Bold))
            BannerFactory.DrawText(g, title, x3, y3, font, Color.White);
          int x4 = x3 + x1;
          int y4 = y3 + (x1 * 2 + 2);
          using (Font font = new Font(FontFamily.GenericSansSerif, BannerFactory.DpiScaleFloat(864f / g.DpiY, bannerHeight), FontStyle.Regular))
            BannerFactory.DrawText(g, text, x4, y4, font, Color.White);
        }
        if (!skipCache)
        {
          while (BannerFactory.imageCache.Count > 20)
            BannerFactory.imageCache.Remove(BannerFactory.imageCache.Keys.First<string>());
          BannerFactory.imageCache[key] = image;
        }
      }
      return image;
    }

    private static void DrawText(Graphics g, string text, int x, int y, Font font, Color color)
    {
      using (SolidBrush solidBrush = new SolidBrush(color))
      {
        using (StringFormat format = new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip))
          g.DrawString(text, font, (Brush) solidBrush, (float) x, (float) y, format);
      }
    }

    private static int DpiScaleInt(int x, int height)
    {
      return (int) Math.Round((double) (x * height) / 48.0);
    }

    private static float DpiScaleFloat(float x, int height)
    {
      return (float) ((double) x * (double) height / 48.0);
    }
  }
}
