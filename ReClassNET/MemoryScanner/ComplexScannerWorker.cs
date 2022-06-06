// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ComplexScannerWorker
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.MemoryScanner.Comparer;
using System.Collections.Generic;
using System.Threading;

namespace ReClassNET.MemoryScanner
{
  internal class ComplexScannerWorker : IScannerWorker
  {
    private readonly ScanSettings settings;
    private readonly IComplexScanComparer comparer;

    public ComplexScannerWorker(ScanSettings settings, IComplexScanComparer comparer)
    {
      this.settings = settings;
      this.comparer = comparer;
    }

    public IList<ScanResult> Search(byte[] data, int count, CancellationToken ct)
    {
      List<ScanResult> scanResultList = new List<ScanResult>();
      foreach (ScanResult scanResult in this.comparer.Compare(data, count))
      {
        scanResultList.Add(scanResult);
        if (ct.IsCancellationRequested)
          break;
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
      foreach (ScanResult previousResult in previousResults)
      {
        if (!ct.IsCancellationRequested)
        {
          ScanResult result;
          if (this.comparer.CompareWithPrevious(data, count, previousResult, out result))
            scanResultList.Add(result);
        }
        else
          break;
      }
      return (IList<ScanResult>) scanResultList;
    }
  }
}
