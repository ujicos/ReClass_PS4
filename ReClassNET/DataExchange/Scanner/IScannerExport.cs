// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.Scanner.IScannerExport
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.MemoryScanner;
using System.Collections.Generic;

namespace ReClassNET.DataExchange.Scanner
{
  public interface IScannerExport
  {
    void Save(IEnumerable<MemoryRecord> records, string filePath, ILogger logger);
  }
}
