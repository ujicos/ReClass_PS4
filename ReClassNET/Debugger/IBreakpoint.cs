// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.IBreakpoint
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Debugger
{
  public interface IBreakpoint
  {
    IntPtr Address { get; }

    bool Set(RemoteProcess process);

    void Remove(RemoteProcess process);

    void Handler(ref DebugEvent evt);
  }
}
