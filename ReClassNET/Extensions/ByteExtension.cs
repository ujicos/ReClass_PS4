// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.ByteExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Diagnostics;

namespace ReClassNET.Extensions
{
  public static class ByteExtension
  {
    [DebuggerStepThrough]
    public static void FillWithZero(this byte[] array)
    {
      Array.Clear((Array) array, 0, array.Length);
    }
  }
}
