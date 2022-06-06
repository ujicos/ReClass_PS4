// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.IntegerScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class IntegerScanResult : ScanResult, IEquatable<IntegerScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Integer;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 4;
      }
    }

    public int Value { get; }

    public IntegerScanResult(int value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      IntegerScanResult integerScanResult = new IntegerScanResult(this.Value);
      integerScanResult.Address = this.Address;
      return (ScanResult) integerScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as IntegerScanResult);
    }

    public bool Equals(IntegerScanResult other)
    {
      return other != null && this.Address == other.Address && this.Value == other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
