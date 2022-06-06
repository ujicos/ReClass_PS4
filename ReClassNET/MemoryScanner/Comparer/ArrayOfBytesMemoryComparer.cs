// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.ArrayOfBytesMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class ArrayOfBytesMemoryComparer : ISimpleScanComparer, IScanComparer
  {
    private readonly BytePattern bytePattern;
    private readonly byte[] byteArray;

    public ScanCompareType CompareType
    {
      get
      {
        return ScanCompareType.Equal;
      }
    }

    public int ValueSize
    {
      get
      {
        BytePattern bytePattern = this.bytePattern;
        return bytePattern == null ? this.byteArray.Length : bytePattern.Length;
      }
    }

    public ArrayOfBytesMemoryComparer(BytePattern pattern)
    {
      this.bytePattern = pattern;
      if (this.bytePattern.HasWildcards)
        return;
      this.byteArray = this.bytePattern.ToByteArray();
    }

    public ArrayOfBytesMemoryComparer(byte[] pattern)
    {
      this.byteArray = pattern;
    }

    public bool Compare(byte[] data, int index, out ScanResult result)
    {
      result = (ScanResult) null;
      if (this.byteArray != null)
      {
        for (int index1 = 0; index1 < this.byteArray.Length; ++index1)
        {
          if ((int) data[index + index1] != (int) this.byteArray[index1])
            return false;
        }
      }
      else if (!this.bytePattern.Equals(data, index))
        return false;
      byte[] numArray = new byte[this.ValueSize];
      Array.Copy((Array) data, index, (Array) numArray, 0, numArray.Length);
      result = (ScanResult) new ArrayOfBytesScanResult(numArray);
      return true;
    }

    public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
    {
      return this.Compare(data, index, out result);
    }
  }
}
