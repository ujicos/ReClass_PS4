// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.BreakpointAlreadySetException
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Debugger
{
  public class BreakpointAlreadySetException : Exception
  {
    public IBreakpoint Breakpoint { get; }

    public BreakpointAlreadySetException(IBreakpoint breakpoint)
      : base("This breakpoint is already set.")
    {
      this.Breakpoint = breakpoint;
    }
  }
}
