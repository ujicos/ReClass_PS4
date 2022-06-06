// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.DoubleScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class DoubleScanResult : ScanResult, IEquatable<DoubleScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Double;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 8;
      }
    }

    public double Value { get; }

    public DoubleScanResult(double value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      DoubleScanResult doubleScanResult = new DoubleScanResult(this.Value);
      doubleScanResult.Address = this.Address;
      return (ScanResult) doubleScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as DoubleScanResult);
    }

    public bool Equals(DoubleScanResult other)
    {
      return other != null && this.Address == other.Address && this.Value == other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
