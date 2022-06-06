// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.ByteMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class ByteMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    public ScanCompareType CompareType { get; }

    public byte Value1 { get; }

    public byte Value2 { get; }

    public int ValueSize
    {
      get
      {
        return 1;
      }
    }

    public ByteMemoryComparer(ScanCompareType compareType, byte value1, byte value2)
    {
      this.CompareType = compareType;
      this.Value1 = value1;
      this.Value2 = value2;
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      return ByteMemoryComparer.CompareInternal(data, index, (Func<short, bool>) (value =>
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
      return this.Compare(data, index, (ByteScanResult) previous, out result);
    }

    public bool Compare(byte[] data, int index, ByteScanResult previous, out ScanResult result)
    {
      return ByteMemoryComparer.CompareInternal(data, index, (Func<short, bool>) (value =>
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

    private static bool CompareInternal(
      byte[] data,
      int index,
      Func<short, bool> matcher,
      out ScanResult result)
    {
      result = (ScanResult) null;
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if ((long) (uint) index >= (long) data.Length)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (index > data.Length - 1)
        throw new ArgumentException();
      byte num = data[index];
      if (!matcher((short) num))
        return false;
      result = (ScanResult) new ByteScanResult(num);
      return true;
    }
  }
}
