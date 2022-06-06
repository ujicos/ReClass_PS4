// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.RemoteDebuggerExtensions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Windows.Forms;

namespace ReClassNET.Debugger
{
  public static class RemoteDebuggerExtensions
  {
    public static bool AskUserAndAttachDebugger(this RemoteDebugger debugger)
    {
      return debugger.StartDebuggerIfNeeded((Func<bool>) (() => MessageBox.Show("This will attach the debugger of ReClass.NET to the current process. Continue?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes));
    }
  }
}
