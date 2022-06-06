// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.IReClassImport
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;

namespace ReClassNET.DataExchange.ReClass
{
  public interface IReClassImport
  {
    void Load(string filePath, ILogger logger);
  }
}
