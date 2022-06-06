// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ByteScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class ByteScanResult : ScanResult, IEquatable<ByteScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Byte;
      }
    }

    public override int ValueSize
    {
      get
      {
        return 1;
      }
    }

    public byte Value { get; }

    public ByteScanResult(byte value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      ByteScanResult byteScanResult = new ByteScanResult(this.Value);
      byteScanResult.Address = this.Address;
      return (ScanResult) byteScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as ByteScanResult);
    }

    public bool Equals(ByteScanResult other)
    {
      return other != null && this.Address == other.Address && (int) this.Value == (int) other.Value;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
