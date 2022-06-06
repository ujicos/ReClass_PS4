// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.HardwareBreakpoint
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Debugger
{
  public sealed class HardwareBreakpoint : IBreakpoint
  {
    private readonly BreakpointHandler handler;

    public IntPtr Address { get; }

    public HardwareBreakpointRegister Register { get; }

    public HardwareBreakpointTrigger Trigger { get; }

    public HardwareBreakpointSize Size { get; }

    public HardwareBreakpoint(
      IntPtr address,
      HardwareBreakpointRegister register,
      HardwareBreakpointTrigger trigger,
      HardwareBreakpointSize size,
      BreakpointHandler handler)
    {
      if (register == HardwareBreakpointRegister.InvalidRegister)
        throw new InvalidOperationException();
      this.Address = address;
      this.Register = register;
      this.Trigger = trigger;
      this.Size = size;
      this.handler = handler;
    }

    public bool Set(RemoteProcess process)
    {
      return process.CoreFunctions.SetHardwareBreakpoint(process.UnderlayingProcess.Id, this.Address, this.Register, this.Trigger, this.Size, true);
    }

    public void Remove(RemoteProcess process)
    {
      process.CoreFunctions.SetHardwareBreakpoint(process.UnderlayingProcess.Id, this.Address, this.Register, this.Trigger, this.Size, false);
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
      HardwareBreakpointRegister? nullable = obj is HardwareBreakpoint hardwareBreakpoint ? new HardwareBreakpointRegister?(hardwareBreakpoint.Register) : new HardwareBreakpointRegister?();
      HardwareBreakpointRegister register = this.Register;
      return nullable.GetValueOrDefault() == register & nullable.HasValue;
    }

    public override int GetHashCode()
    {
      return this.Register.GetHashCode();
    }
  }
}
