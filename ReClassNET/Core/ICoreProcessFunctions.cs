// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.ICoreProcessFunctions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Debugger;
using System;

namespace ReClassNET.Core
{
  public interface ICoreProcessFunctions
  {
    void EnumerateProcesses(EnumerateProcessCallback callbackProcess);

    void EnumerateRemoteSectionsAndModules(
      IntPtr process,
      EnumerateRemoteSectionCallback callbackSection,
      EnumerateRemoteModuleCallback callbackModule);

    IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess);

    bool IsProcessValid(IntPtr process);

    void CloseRemoteProcess(IntPtr process);

    bool ReadRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size);

    bool WriteRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size);

    void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action);

    bool AttachDebuggerToProcess(IntPtr id);

    void DetachDebuggerFromProcess(IntPtr id);

    bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds);

    void HandleDebugEvent(ref DebugEvent evt);

    bool SetHardwareBreakpoint(
      IntPtr id,
      IntPtr address,
      HardwareBreakpointRegister register,
      HardwareBreakpointTrigger trigger,
      HardwareBreakpointSize size,
      bool set);
  }
}
