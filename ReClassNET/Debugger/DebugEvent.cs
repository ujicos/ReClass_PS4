// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.DebugEvent
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Debugger
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct DebugEvent
  {
    public DebugContinueStatus ContinueStatus;
    public IntPtr ProcessId;
    public IntPtr ThreadId;
    public ExceptionDebugInfo ExceptionInfo;
  }
}
