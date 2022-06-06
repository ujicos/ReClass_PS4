// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.IReClassExport
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using System.IO;

namespace ReClassNET.DataExchange.ReClass
{
  public interface IReClassExport
  {
    void Save(string filePath, ILogger logger);

    void Save(Stream output, ILogger logger);
  }
}
