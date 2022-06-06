// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.ShortMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util.Conversion;
using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class ShortMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    private readonly EndianBitConverter bitConverter;

    public ScanCompareType CompareType { get; }

    public short Value1 { get; }

    public short Value2 { get; }

    public int ValueSize
    {
      get
      {
        return 2;
      }
    }

    public ShortMemoryComparer(
      ScanCompareType compareType,
      short value1,
      short value2,
      EndianBitConverter bitConverter)
    {
      this.CompareType = compareType;
      this.Value1 = value1;
      this.Value2 = value2;
      this.bitConverter = bitConverter;
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<short, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return (int) value == (int) this.Value1;
          case ScanCompareType.NotEqual:
            return (int) value != (int) this.Value1;
          case ScanCompareType.GreaterThan:
            return (int) value > (int) this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return (int) value >= (int) this.Value1;
          case ScanCompareType.LessThan:
            return (int) value < (int) this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return (int) value <= (int) this.Value1;
          case ScanCompareType.Between:
            return (int) this.Value1 < (int) value && (int) value < (int) this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return (int) this.Value1 <= (int) value && (int) value <= (int) this.Value2;
          case ScanCompareType.Unknown:
            return true;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
    {
      return this.Compare(data, index, (ShortScanResult) previous, out result);
    }

    public bool Compare(byte[] data, int index, ShortScanResult previous, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<short, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return (int) value == (int) this.Value1;
          case ScanCompareType.NotEqual:
            return (int) value != (int) this.Value1;
          case ScanCompareType.Changed:
            return (int) value != (int) previous.Value;
          case ScanCompareType.NotChanged:
            return (int) value == (int) previous.Value;
          case ScanCompareType.GreaterThan:
            return (int) value > (int) this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return (int) value >= (int) this.Value1;
          case ScanCompareType.Increased:
            return (int) value > (int) previous.Value;
          case ScanCompareType.IncreasedOrEqual:
            return (int) value >= (int) previous.Value;
          case ScanCompareType.LessThan:
            return (int) value < (int) this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return (int) value <= (int) this.Value1;
          case ScanCompareType.Decreased:
            return (int) value < (int) previous.Value;
          case ScanCompareType.DecreasedOrEqual:
            return (int) value <= (int) previous.Value;
          case ScanCompareType.Between:
            return (int) this.Value1 < (int) value && (int) value < (int) this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return (int) this.Value1 <= (int) value && (int) value <= (int) this.Value2;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    private bool CompareInternal(
      byte[] data,
      int index,
      Func<short, bool> matcher,
      out ScanResult result)
    {
      result = (ScanResult) null;
      short int16 = this.bitConverter.ToInt16(data, index);
      if (!matcher(int16))
        return false;
      result = (ScanResult) new ShortScanResult(int16);
      return true;
    }
  }
}
