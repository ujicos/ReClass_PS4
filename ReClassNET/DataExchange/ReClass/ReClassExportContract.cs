// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.ReClassExportContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using System.IO;

namespace ReClassNET.DataExchange.ReClass
{
  internal abstract class ReClassExportContract : IReClassExport
  {
    public void Save(string filePath, ILogger logger)
    {
    }

    public void Save(Stream output, ILogger logger)
    {
    }
  }
}
