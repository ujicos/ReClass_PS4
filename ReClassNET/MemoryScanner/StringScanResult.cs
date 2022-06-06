// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.StringScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Text;

namespace ReClassNET.MemoryScanner
{
  public class StringScanResult : ScanResult, IEquatable<StringScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.String;
      }
    }

    public override int ValueSize
    {
      get
      {
        return this.Value.Length * this.Encoding.GuessByteCountPerChar();
      }
    }

    public string Value { get; }

    public Encoding Encoding { get; }

    public StringScanResult(string value, Encoding encoding)
    {
      this.Value = value;
      this.Encoding = encoding;
    }

    public override ScanResult Clone()
    {
      StringScanResult stringScanResult = new StringScanResult(this.Value, this.Encoding);
      stringScanResult.Address = this.Address;
      return (ScanResult) stringScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as StringScanResult);
    }

    public bool Equals(StringScanResult other)
    {
      return other != null && this.Address == other.Address && this.Value == other.Value && this.Encoding.Equals((object) other.Encoding);
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode() * 19 + this.Encoding.GetHashCode();
    }
  }
}
