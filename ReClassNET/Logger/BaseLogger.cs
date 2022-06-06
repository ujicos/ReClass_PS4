// Decompiled with JetBrains decompiler
// Type: ReClassNET.Logger.BaseLogger
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Microsoft.SqlServer.MessageBox;
using System;

namespace ReClassNET.Logger
{
  public abstract class BaseLogger : ILogger
  {
    private readonly object sync = new object();

    public event NewLogEntryEventHandler NewLogEntry;

    public void Log(Exception ex)
    {
      this.Log(LogLevel.Error, ExceptionMessageBox.GetMessageText(ex), ex);
    }

    public void Log(LogLevel level, string message)
    {
      this.Log(level, message, (Exception) null);
    }

    private void Log(LogLevel level, string message, Exception ex)
    {
      lock (this.sync)
      {
        NewLogEntryEventHandler newLogEntry = this.NewLogEntry;
        if (newLogEntry == null)
          return;
        newLogEntry(level, message, ex);
      }
    }
  }
}
