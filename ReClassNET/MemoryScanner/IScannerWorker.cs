// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.IScannerWorker
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Collections.Generic;
using System.Threading;

namespace ReClassNET.MemoryScanner
{
  internal interface IScannerWorker
  {
    IList<ScanResult> Search(byte[] data, int count, CancellationToken ct);

    IList<ScanResult> Search(
      byte[] data,
      int count,
      IEnumerable<ScanResult> previousResults,
      CancellationToken ct);
  }
}
