// Decompiled with JetBrains decompiler
// Type: ReClassNET.Logger.NullLogger
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Logger
{
  public class NullLogger : ILogger
  {
    public event NewLogEntryEventHandler NewLogEntry
    {
      add
      {
      }
      remove
      {
      }
    }

    public void Log(Exception ex)
    {
    }

    public void Log(LogLevel level, string message)
    {
    }
  }
}
