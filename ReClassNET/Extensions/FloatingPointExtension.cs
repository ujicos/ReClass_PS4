// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.FloatingPointExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Diagnostics;

namespace ReClassNET.Extensions
{
  public static class FloatingPointExtension
  {
    [DebuggerStepThrough]
    public static bool IsNearlyEqual(this float val, float other, float epsilon)
    {
      return (double) val == (double) other || (double) Math.Abs(val - other) <= (double) epsilon;
    }

    [DebuggerStepThrough]
    public static bool IsNearlyEqual(this double val, double other, double epsilon)
    {
      return val == other || Math.Abs(val - other) <= epsilon;
    }
  }
}
