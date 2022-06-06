// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.ListExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ReClassNET.Extensions
{
  public static class ListExtension
  {
    [DebuggerStepThrough]
    public static int BinarySearch<T>(this IList<T> source, Func<T, int> comparer)
    {
      int num1 = 0;
      int num2 = source.Count - 1;
      while (num1 <= num2)
      {
        int index = num1 + (num2 - num1 >> 1);
        int num3 = comparer(source[index]);
        if (num3 == 0)
          return index;
        if (num3 > 0)
          num1 = index + 1;
        else
          num2 = index - 1;
      }
      return ~num1;
    }
  }
}
