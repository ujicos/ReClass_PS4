// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.PointExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Diagnostics;
using System.Drawing;

namespace ReClassNET.Extensions
{
  public static class PointExtension
  {
    [DebuggerStepThrough]
    public static Point Relocate(this Point point, int offsetX, int offsetY)
    {
      return new Point(point.X + offsetX, point.Y + offsetY);
    }
  }
}
