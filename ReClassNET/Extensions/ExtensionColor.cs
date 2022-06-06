// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.ExtensionColor
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Diagnostics;
using System.Drawing;

namespace ReClassNET.Extensions
{
  public static class ExtensionColor
  {
    [DebuggerStepThrough]
    public static int ToRgb(this Color color)
    {
      return 16777215 & color.ToArgb();
    }

    [DebuggerStepThrough]
    public static Color Invert(this Color color)
    {
      return Color.FromArgb((int) color.A, (int) byte.MaxValue - (int) color.R, (int) byte.MaxValue - (int) color.G, (int) byte.MaxValue - (int) color.B);
    }
  }
}
