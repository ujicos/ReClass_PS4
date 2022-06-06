// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.RemoteDebugger
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Forms;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ReClassNET.Debugger
{
  public class RemoteDebugger
  {
    private readonly object syncThread = new object();
    private volatile bool running = true;
    private readonly object syncBreakpoint = new object();
    private readonly HashSet<IBreakpoint> breakpoints = new HashSet<IBreakpoint>();
    private Thread thread;
    private volatile bool isAttached;
    private readonly RemoteProcess process;

    public bool IsAttached
    {
      get
      {
        return this.isAttached;
      }
    }

    public bool StartDebuggerIfNeeded(Func<bool> queryAttach)
    {
      if (!this.process.IsValid)
        return false;
      lock (this.syncThread)
      {
        if (this.thread != null && this.IsAttached)
          return true;
        if (queryAttach())
        {
          this.thread = new Thread(new ThreadStart(this.Run))
          {
            IsBackground = true
          };
          this.thread.Start();
          return true;
        }
      }
      return false;
    }

    private void Run()
    {
      try
      {
        if (!this.process.CoreFunctions.AttachDebuggerToProcess(this.process.UnderlayingProcess.Id))
          return;
        this.isAttached = true;
        DebugEvent evt = new DebugEvent();
        this.running = true;
        while (this.running)
        {
          if (this.process.CoreFunctions.AwaitDebugEvent(ref evt, 100))
          {
            evt.ContinueStatus = DebugContinueStatus.Handled;
            this.HandleExceptionEvent(ref evt);
            this.process.CoreFunctions.HandleDebugEvent(ref evt);
          }
          else if (!this.process.IsValid)
            this.Terminate(false);
        }
        this.process.CoreFunctions.DetachDebuggerFromProcess(this.process.UnderlayingProcess.Id);
      }
      finally
      {
        this.isAttached = false;
      }
    }

    public void Terminate()
    {
      this.Terminate(true);
    }

    private void Terminate(bool join)
    {
      lock (this.syncBreakpoint)
      {
        foreach (IBreakpoint breakpoint in this.breakpoints)
          breakpoint.Remove(this.process);
        this.breakpoints.Clear();
      }
      this.running = false;
      if (!join)
        return;
      lock (this.syncThread)
        this.thread?.Join();
    }

    public RemoteDebugger(RemoteProcess process)
    {
      this.process = process;
    }

    public void AddBreakpoint(IBreakpoint breakpoint)
    {
      lock (this.syncBreakpoint)
      {
        if (!this.breakpoints.Add(breakpoint))
          throw new BreakpointAlreadySetException(breakpoint);
        breakpoint.Set(this.process);
      }
    }

    public void RemoveBreakpoint(IBreakpoint bp)
    {
      lock (this.syncBreakpoint)
      {
        if (!this.breakpoints.Remove(bp))
          return;
        bp.Remove(this.process);
      }
    }

    public void FindWhatAccessesAddress(IntPtr address, int size)
    {
      this.FindCodeByBreakpoint(address, size, HardwareBreakpointTrigger.Access);
    }

    public void FindWhatWritesToAddress(IntPtr address, int size)
    {
      this.FindCodeByBreakpoint(address, size, HardwareBreakpointTrigger.Write);
    }

    public void FindCodeByBreakpoint(IntPtr address, int size, HardwareBreakpointTrigger trigger)
    {
      HardwareBreakpointRegister usableDebugRegister1 = this.GetUsableDebugRegister();
      if (usableDebugRegister1 == HardwareBreakpointRegister.InvalidRegister)
        throw new NoHardwareBreakpointAvailableException();
      List<BreakpointSplit> source = this.SplitBreakpoint(address, size);
      FoundCodeForm fcf = new FoundCodeForm(this.process, source[0].Address, trigger);
      List<IBreakpoint> localBreakpoints = new List<IBreakpoint>();
      fcf.Stop += (FoundCodeForm.StopEventHandler) ((sender, e) =>
      {
        lock (localBreakpoints)
        {
          foreach (IBreakpoint bp in localBreakpoints)
            this.RemoveBreakpoint(bp);
          localBreakpoints.Clear();
        }
      });
      HardwareBreakpoint hardwareBreakpoint1 = new HardwareBreakpoint(source[0].Address, usableDebugRegister1, trigger, (HardwareBreakpointSize) source[0].Size, new BreakpointHandler(HandleBreakpoint));
      try
      {
        this.AddBreakpoint((IBreakpoint) hardwareBreakpoint1);
        localBreakpoints.Add((IBreakpoint) hardwareBreakpoint1);
        fcf.Show();
      }
      catch
      {
        fcf.Dispose();
        throw;
      }
      if (source.Count <= 1)
        return;
      foreach (BreakpointSplit breakpointSplit in source.Skip<BreakpointSplit>(1))
      {
        HardwareBreakpointRegister usableDebugRegister2 = this.GetUsableDebugRegister();
        if (usableDebugRegister2 == HardwareBreakpointRegister.InvalidRegister)
          break;
        HardwareBreakpoint hardwareBreakpoint2 = new HardwareBreakpoint(breakpointSplit.Address, usableDebugRegister2, trigger, (HardwareBreakpointSize) breakpointSplit.Size, new BreakpointHandler(HandleBreakpoint));
        this.AddBreakpoint((IBreakpoint) hardwareBreakpoint2);
        localBreakpoints.Add((IBreakpoint) hardwareBreakpoint2);
      }

      void HandleBreakpoint(IBreakpoint bp, ref DebugEvent evt)
      {
        fcf.AddRecord(new ExceptionDebugInfo?(evt.ExceptionInfo));
      }
    }

    private List<BreakpointSplit> SplitBreakpoint(IntPtr address, int size)
    {
      List<BreakpointSplit> breakpointSplitList = new List<BreakpointSplit>();
      while (size > 0)
      {
        if (size >= 8 && address.Mod(8) == 0)
        {
          breakpointSplitList.Add(new BreakpointSplit()
          {
            Address = address,
            Size = 8
          });
          address += 8;
          size -= 8;
        }
        else if (size >= 4 && address.Mod(4) == 0)
        {
          breakpointSplitList.Add(new BreakpointSplit()
          {
            Address = address,
            Size = 4
          });
          address += 4;
          size -= 4;
        }
        else if (size >= 2 && address.Mod(2) == 0)
        {
          breakpointSplitList.Add(new BreakpointSplit()
          {
            Address = address,
            Size = 2
          });
          address += 2;
          size -= 2;
        }
        else
        {
          breakpointSplitList.Add(new BreakpointSplit()
          {
            Address = address,
            Size = 1
          });
          address += 1;
          --size;
        }
      }
      return breakpointSplitList;
    }

    private HardwareBreakpointRegister GetUsableDebugRegister()
    {
      HashSet<HardwareBreakpointRegister> source = new HashSet<HardwareBreakpointRegister>()
      {
        HardwareBreakpointRegister.Dr0,
        HardwareBreakpointRegister.Dr1,
        HardwareBreakpointRegister.Dr2,
        HardwareBreakpointRegister.Dr3
      };
      lock (this.syncBreakpoint)
      {
        foreach (IBreakpoint breakpoint in this.breakpoints)
        {
          if (breakpoint is HardwareBreakpoint hardwareBreakpoint)
            source.Remove(hardwareBreakpoint.Register);
        }
      }
      return source.Count > 0 ? source.First<HardwareBreakpointRegister>() : HardwareBreakpointRegister.InvalidRegister;
    }

    private void HandleExceptionEvent(ref DebugEvent evt)
    {
      lock (this.syncBreakpoint)
      {
        HardwareBreakpointRegister causedBy = evt.ExceptionInfo.CausedBy;
        foreach (IBreakpoint breakpoint in this.breakpoints)
        {
          if (breakpoint is HardwareBreakpoint hardwareBreakpoint && hardwareBreakpoint.Register == causedBy)
          {
            hardwareBreakpoint.Handler(ref evt);
            break;
          }
        }
      }
    }
  }
}
