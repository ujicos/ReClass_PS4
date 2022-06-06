// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.SimpleScannerWorker
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.MemoryScanner.Comparer;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ReClassNET.MemoryScanner
{
  internal class SimpleScannerWorker : IScannerWorker
  {
    private readonly ScanSettings settings;
    private readonly ISimpleScanComparer comparer;

    public SimpleScannerWorker(ScanSettings settings, ISimpleScanComparer comparer)
    {
      this.settings = settings;
      this.comparer = comparer;
    }

    public IList<ScanResult> Search(byte[] data, int count, CancellationToken ct)
    {
      List<ScanResult> scanResultList = new List<ScanResult>();
      int num = count - this.comparer.ValueSize;
      for (int index = 0; index < num && !ct.IsCancellationRequested; index += this.settings.FastScanAlignment)
      {
        ScanResult result;
        if (this.comparer.Compare(data, index, out result))
        {
          result.Address = (IntPtr) index;
          scanResultList.Add(result);
        }
      }
      return (IList<ScanResult>) scanResultList;
    }

    public IList<ScanResult> Search(
      byte[] data,
      int count,
      IEnumerable<ScanResult> previousResults,
      CancellationToken ct)
    {
      List<ScanResult> scanResultList = new List<ScanResult>();
      int num = count - this.comparer.ValueSize;
      foreach (ScanResult previousResult in previousResults)
      {
        if (!ct.IsCancellationRequested)
        {
          int int32 = previousResult.Address.ToInt32();
          ScanResult result;
          if (int32 <= num && this.comparer.Compare(data, int32, previousResult, out result))
          {
            result.Address = previousResult.Address;
            scanResultList.Add(result);
          }
        }
        else
          break;
      }
      return (IList<ScanResult>) scanResultList;
    }
  }
}
