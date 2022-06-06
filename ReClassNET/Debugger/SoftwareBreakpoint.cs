// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.SoftwareBreakpoint
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Debugger
{
  public sealed class SoftwareBreakpoint : IBreakpoint
  {
    private byte orig;
    private readonly BreakpointHandler handler;

    public IntPtr Address { get; }

    public SoftwareBreakpoint(IntPtr address, BreakpointHandler handler)
    {
      this.Address = address;
      this.handler = handler;
    }

    public bool Set(RemoteProcess process)
    {
      byte[] buffer = new byte[1];
      if (!process.ReadRemoteMemoryIntoBuffer(this.Address, ref buffer))
        return false;
      this.orig = buffer[0];
      return process.WriteRemoteMemory(this.Address, new byte[1]
      {
        (byte) 204
      });
    }

    public void Remove(RemoteProcess process)
    {
      process.WriteRemoteMemory(this.Address, new byte[1]
      {
        this.orig
      });
    }

    public void Handler(ref DebugEvent evt)
    {
      BreakpointHandler handler = this.handler;
      if (handler == null)
        return;
      handler((IBreakpoint) this, ref evt);
    }

    public override bool Equals(object obj)
    {
      return obj is SoftwareBreakpoint softwareBreakpoint && this.Address == softwareBreakpoint.Address;
    }

    public override int GetHashCode()
    {
      return this.Address.GetHashCode();
    }
  }
}
