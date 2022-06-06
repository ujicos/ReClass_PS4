// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.DoubleMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Util.Conversion;
using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class DoubleMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    private readonly int significantDigits;
    private readonly double minValue;
    private readonly double maxValue;
    private readonly EndianBitConverter bitConverter;

    public ScanCompareType CompareType { get; }

    public ScanRoundMode RoundType { get; }

    public double Value1 { get; }

    public double Value2 { get; }

    public int ValueSize
    {
      get
      {
        return 8;
      }
    }

    public DoubleMemoryComparer(
      ScanCompareType compareType,
      ScanRoundMode roundType,
      int significantDigits,
      double value1,
      double value2,
      EndianBitConverter bitConverter)
    {
      this.CompareType = compareType;
      this.RoundType = roundType;
      this.significantDigits = Math.Max(significantDigits, 1);
      this.Value1 = Math.Round(value1, this.significantDigits, MidpointRounding.AwayFromZero);
      this.Value2 = Math.Round(value2, this.significantDigits, MidpointRounding.AwayFromZero);
      int num = (int) Math.Pow(10.0, (double) this.significantDigits);
      this.minValue = value1 - 1.0 / (double) num;
      this.maxValue = value1 + 1.0 / (double) num;
      this.bitConverter = bitConverter;
    }

    private bool CheckRoundedEquality(double value)
    {
      switch (this.RoundType)
      {
        case ScanRoundMode.Strict:
          return this.Value1.IsNearlyEqual(Math.Round(value, this.significantDigits, MidpointRounding.AwayFromZero), 0.0001);
        case ScanRoundMode.Normal:
          return this.minValue < value && value < this.maxValue;
        case ScanRoundMode.Truncate:
          return (long) value == (long) this.Value1;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<double, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return this.CheckRoundedEquality(value);
          case ScanCompareType.NotEqual:
            return !this.CheckRoundedEquality(value);
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
      return this.Compare(data, index, (DoubleScanResult) previous, out result);
    }

    public bool Compare(byte[] data, int index, DoubleScanResult previous, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<double, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return this.CheckRoundedEquality(value);
          case ScanCompareType.NotEqual:
            return !this.CheckRoundedEquality(value);
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
      Func<double, bool> matcher,
      out ScanResult result)
    {
      result = (ScanResult) null;
      double num = this.bitConverter.ToDouble(data, index);
      if (!matcher(num))
        return false;
      result = (ScanResult) new DoubleScanResult(num);
      return true;
    }
  }
}
