// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ArrayOfBytesScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.MemoryScanner
{
  public class ArrayOfBytesScanResult : ScanResult, IEquatable<ArrayOfBytesScanResult>
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.ArrayOfBytes;
      }
    }

    public override int ValueSize
    {
      get
      {
        return this.Value.Length;
      }
    }

    public byte[] Value { get; }

    public ArrayOfBytesScanResult(byte[] value)
    {
      this.Value = value;
    }

    public override ScanResult Clone()
    {
      ArrayOfBytesScanResult ofBytesScanResult = new ArrayOfBytesScanResult(this.Value);
      ofBytesScanResult.Address = this.Address;
      return (ScanResult) ofBytesScanResult;
    }

    public override bool Equals(object obj)
    {
      return this.Equals(obj as ArrayOfBytesScanResult);
    }

    public bool Equals(ArrayOfBytesScanResult other)
    {
      return other != null && this.Address == other.Address && ((IEnumerable<byte>) this.Value).SequenceEqual<byte>((IEnumerable<byte>) other.Value);
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode() * 19 + this.Value.GetHashCode();
    }
  }
}
