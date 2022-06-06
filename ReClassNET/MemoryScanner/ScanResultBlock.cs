// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScanResultBlock
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;

namespace ReClassNET.MemoryScanner
{
  internal class ScanResultBlock
  {
    public IntPtr Start { get; }

    public IntPtr End { get; }

    public int Size
    {
      get
      {
        return this.End.Sub(this.Start).ToInt32();
      }
    }

    public IReadOnlyList<ScanResult> Results { get; }

    public ScanResultBlock(IntPtr start, IntPtr end, IReadOnlyList<ScanResult> results)
    {
      this.Start = start;
      this.End = end;
      this.Results = results;
    }
  }
}
