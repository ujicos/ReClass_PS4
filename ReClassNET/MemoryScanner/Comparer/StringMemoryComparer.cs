// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.StringMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Text;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class StringMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    public ScanCompareType CompareType
    {
      get
      {
        return ScanCompareType.Equal;
      }
    }

    public bool CaseSensitive { get; }

    public Encoding Encoding { get; }

    public string Value { get; }

    public int ValueSize { get; }

    public StringMemoryComparer(string value, Encoding encoding, bool caseSensitive)
    {
      this.Value = value;
      this.Encoding = encoding;
      this.CaseSensitive = caseSensitive;
      this.ValueSize = this.Value.Length * this.Encoding.GuessByteCountPerChar();
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      result = (ScanResult) null;
      string str = this.Encoding.GetString(data, index, this.ValueSize);
      if (!this.Value.Equals(str, this.CaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase))
        return false;
      result = (ScanResult) new StringScanResult(str, this.Encoding);
      return true;
    }

    public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
    {
      return this.Compare(data, index, out result);
    }
  }
}
