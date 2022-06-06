// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.IntPtrComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;

namespace ReClassNET.Util
{
  public class IntPtrComparer : IComparer<IntPtr>
  {
    public static IntPtrComparer Instance { get; } = new IntPtrComparer();

    public int Compare(IntPtr x, IntPtr y)
    {
      return x.CompareTo(y);
    }
  }
}
