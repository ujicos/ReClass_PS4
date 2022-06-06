// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public abstract class ScanResult
  {
    public abstract ScanValueType ValueType { get; }

    public IntPtr Address { get; set; }

    public abstract int ValueSize { get; }

    public abstract ScanResult Clone();
  }
}
