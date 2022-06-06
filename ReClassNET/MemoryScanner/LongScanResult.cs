// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.LongScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class LongScanResult : ScanResult, IEquatable<LongScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Long;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 8;
      }
    }

    public long Value { get; }

    public LongScanResult(long value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      LongScanResult longScanResult = new LongScanResult(this.Value);
      longScanResult.Address = this.Address;
      return (ScanResult) longScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as LongScanResult);
    }

    public bool Equals(LongScanResult other)
    {
      return other != null && this.Address == other.Address && this.Value == other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
