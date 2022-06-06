// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ShortScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class ShortScanResult : ScanResult, IEquatable<ShortScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Short;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 2;
      }
    }

    public short Value { get; }

    public ShortScanResult(short value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      ShortScanResult shortScanResult = new ShortScanResult(this.Value);
      shortScanResult.Address = this.Address;
      return (ScanResult) shortScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as ShortScanResult);
    }

    public bool Equals(ShortScanResult other)
    {
      return other != null && this.Address == other.Address && (int) this.Value == (int) other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
