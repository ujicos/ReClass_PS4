// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.FloatMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Util.Conversion;
using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class FloatMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    private readonly int significantDigits;
    private readonly float minValue;
    private readonly float maxValue;
    private readonly EndianBitConverter bitConverter;

    public ScanCompareType CompareType { get; }

    public ScanRoundMode RoundType { get; }

    public float Value1 { get; }

    public float Value2 { get; }

    public int ValueSize
    {
      get
      {
        return 4;
      }
    }

    public FloatMemoryComparer(
      ScanCompareType compareType,
      ScanRoundMode roundType,
      int significantDigits,
      float value1,
      float value2,
      EndianBitConverter bitConverter)
    {
      this.CompareType = compareType;
      this.RoundType = roundType;
      this.significantDigits = Math.Max(significantDigits, 1);
      this.Value1 = (float) Math.Round((double) value1, this.significantDigits, MidpointRounding.AwayFromZero);
      this.Value2 = (float) Math.Round((double) value2, this.significantDigits, MidpointRounding.AwayFromZero);
      int num = (int) Math.Pow(10.0, (double) this.significantDigits);
      this.minValue = value1 - 1f / (float) num;
      this.maxValue = value1 + 1f / (float) num;
      this.bitConverter = bitConverter;
    }

    private bool CheckRoundedEquality(float value)
    {
      switch (this.RoundType)
      {
        case ScanRoundMode.Strict:
          return this.Value1.IsNearlyEqual((float) Math.Round((double) value, this.significantDigits, MidpointRounding.AwayFromZero), 0.0001f);
        case ScanRoundMode.Normal:
          return (double) this.minValue < (double) value && (double) value < (double) this.maxValue;
        case ScanRoundMode.Truncate:
          return (int) value == (int) this.Value1;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<float, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return this.CheckRoundedEquality(value);
          case ScanCompareType.NotEqual:
            return !this.CheckRoundedEquality(value);
          case ScanCompareType.GreaterThan:
            return (double) value > (double) this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return (double) value >= (double) this.Value1;
          case ScanCompareType.LessThan:
            return (double) value < (double) this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return (double) value <= (double) this.Value1;
          case ScanCompareType.Between:
            return (double) this.Value1 < (double) value && (double) value < (double) this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return (double) this.Value1 <= (double) value && (double) value <= (double) this.Value2;
          case ScanCompareType.Unknown:
            return true;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
    {
      return this.Compare(data, index, (FloatScanResult) previous, out result);
    }

    public bool Compare(byte[] data, int index, FloatScanResult previous, out ScanResult result)
    {
      return this.CompareInternal(data, index, (Func<float, bool>) (value =>
      {
        switch (this.CompareType)
        {
          case ScanCompareType.Equal:
            return this.CheckRoundedEquality(value);
          case ScanCompareType.NotEqual:
            return !this.CheckRoundedEquality(value);
          case ScanCompareType.Changed:
            return (double) value != (double) previous.Value;
          case ScanCompareType.NotChanged:
            return (double) value == (double) previous.Value;
          case ScanCompareType.GreaterThan:
            return (double) value > (double) this.Value1;
          case ScanCompareType.GreaterThanOrEqual:
            return (double) value >= (double) this.Value1;
          case ScanCompareType.Increased:
            return (double) value > (double) previous.Value;
          case ScanCompareType.IncreasedOrEqual:
            return (double) value >= (double) previous.Value;
          case ScanCompareType.LessThan:
            return (double) value < (double) this.Value1;
          case ScanCompareType.LessThanOrEqual:
            return (double) value <= (double) this.Value1;
          case ScanCompareType.Decreased:
            return (double) value < (double) previous.Value;
          case ScanCompareType.DecreasedOrEqual:
            return (double) value <= (double) previous.Value;
          case ScanCompareType.Between:
            return (double) this.Value1 < (double) value && (double) value < (double) this.Value2;
          case ScanCompareType.BetweenOrEqual:
            return (double) this.Value1 <= (double) value && (double) value <= (double) this.Value2;
          default:
            throw new InvalidCompareTypeException(this.CompareType);
        }
      }), out result);
    }

    private bool CompareInternal(
      byte[] data,
      int index,
      Func<float, bool> matcher,
      out ScanResult result)
    {
      result = (ScanResult) null;
      float single = this.bitConverter.ToSingle(data, index);
      if (!matcher(single))
        return false;
      result = (ScanResult) new FloatScanResult(single);
      return true;
    }
  }
}
