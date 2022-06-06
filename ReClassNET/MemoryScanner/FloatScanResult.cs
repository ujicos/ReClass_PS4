// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.FloatScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class FloatScanResult : ScanResult, IEquatable<FloatScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Float;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 4;
      }
    }

    public float Value { get; }

    public FloatScanResult(float value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      FloatScanResult floatScanResult = new FloatScanResult(this.Value);
      floatScanResult.Address = this.Address;
      return (ScanResult) floatScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as FloatScanResult);
    }

    public bool Equals(FloatScanResult other)
    {
      return other != null && this.Address == other.Address && (double) this.Value == (double) other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
