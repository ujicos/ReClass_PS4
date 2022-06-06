// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.IComplexScanComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Collections.Generic;

namespace ReClassNET.MemoryScanner.Comparer
{
  public interface IComplexScanComparer : IScanComparer
  {
    IEnumerable<ScanResult> Compare(byte[] data, int size);

    bool CompareWithPrevious(byte[] data, int size, ScanResult previous, out ScanResult result);
  }
}
