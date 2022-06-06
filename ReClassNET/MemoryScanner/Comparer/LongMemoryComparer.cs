// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.LongMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util.Conversion;
using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class LongMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    private readonly EndianBitConverter bitConverter;

    public ScanCompareType CompareType { get; }

    public long Value1 { get; }

    public long Value2 { get; }

    public int ValueSize
    {
      get
      {
        return 8;
      }
    }

    public LongMemoryComparer(
      ScanCompareType compareType,
      long value1,
      long value2,
      EndianBitConverter bitConverter)
    {
      this.CompareType = compareType;
      this.Value1 = value1;
      this.Value2 = value2;
      this.bitConverter = bitConverter;
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<long, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return value == this.Value1;
          case ScanCompareType.NotEqual:
            return value != this.Value1;
          case ScanCompareType.GreaterThan:
            return value > this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return value >= this.Value1;
          case ScanCompareType.LessThan:
            return value < this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return value <= this.Value1;
          case ScanCompareType.Between:
            return this.Value1 < value && value < this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return this.Value1 <= value && value <= this.Value2;
          case ScanCompareType.Unknown:
            return true;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
    {
      return this.Compare(data, index, (LongScanResult) previous, out result);
    }

    public bool Compare(byte[] data, int index, LongScanResult previous, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<long, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return value == this.Value1;
          case ScanCompareType.NotEqual:
            return value != this.Value1;
          case ScanCompareType.Changed:
            return value != previous.Value;
          case ScanCompareType.NotChanged:
            return value == previous.Value;
          case ScanCompareType.GreaterThan:
            return value > this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return value >= this.Value1;
          case ScanCompareType.Increased:
            return value > previous.Value;
          case ScanCompareType.IncreasedOrEqual:
            return value >= previous.Value;
          case ScanCompareType.LessThan:
            return value < this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return value <= this.Value1;
          case ScanCompareType.Decreased:
            return value < previous.Value;
          case ScanCompareType.DecreasedOrEqual:
            return value <= previous.Value;
          case ScanCompareType.Between:
            return this.Value1 < value && value < this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return this.Value1 <= value && value <= this.Value2;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    private bool CompareInternal(
      byte[] data,
      int index,
      Func<long, bool> matcher,
      out ScanResult result)
    {
      result = (ScanResult) null;
      long int64 = this.bitConverter.ToInt64(data, index);
      if (!matcher(int64))
        return false;
      result = (ScanResult) new LongScanResult(int64);
      return true;
    }
  }
}
