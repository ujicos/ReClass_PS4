// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.GraphicsExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Extensions
{
  public static class GraphicsExtension
  {
    public static void DrawStringEx(
      this Graphics g,
      string text,
      Font font,
      Color color,
      int x,
      int y)
    {
      TextRenderer.DrawText((IDeviceContext) g, text, font, new Point(x, y), color);
    }
  }
}
