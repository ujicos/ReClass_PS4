// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.BreakpointContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Debugger
{
  internal abstract class BreakpointContract : IBreakpoint
  {
    public IntPtr Address
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void Handler(ref DebugEvent evt)
    {
      throw new NotImplementedException();
    }

    public void Remove(RemoteProcess process)
    {
      throw new NotImplementedException();
    }

    public bool Set(RemoteProcess process)
    {
      throw new NotImplementedException();
    }
  }
}
