// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.ISimpleScanComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.MemoryScanner.Comparer
{
  public interface ISimpleScanComparer : IScanComparer
  {
    int ValueSize { get; }

    bool Compare(byte[] data, int index, out ScanResult result);

    bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result);
  }
}
