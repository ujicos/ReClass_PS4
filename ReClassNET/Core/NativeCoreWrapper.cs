// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.NativeCoreWrapper
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Debugger;
using ReClassNET.Extensions;
using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Core
{
  public class NativeCoreWrapper : ICoreProcessFunctions
  {
    private readonly NativeCoreWrapper.EnumerateProcessesDelegate enumerateProcessesDelegate;
    private readonly NativeCoreWrapper.EnumerateRemoteSectionsAndModulesDelegate enumerateRemoteSectionsAndModulesDelegate;
    private readonly NativeCoreWrapper.OpenRemoteProcessDelegate openRemoteProcessDelegate;
    private readonly NativeCoreWrapper.IsProcessValidDelegate isProcessValidDelegate;
    private readonly NativeCoreWrapper.CloseRemoteProcessDelegate closeRemoteProcessDelegate;
    private readonly NativeCoreWrapper.ReadRemoteMemoryDelegate readRemoteMemoryDelegate;
    private readonly NativeCoreWrapper.WriteRemoteMemoryDelegate writeRemoteMemoryDelegate;
    private readonly NativeCoreWrapper.ControlRemoteProcessDelegate controlRemoteProcessDelegate;
    private readonly NativeCoreWrapper.AttachDebuggerToProcessDelegate attachDebuggerToProcessDelegate;
    private readonly NativeCoreWrapper.DetachDebuggerFromProcessDelegate detachDebuggerFromProcessDelegate;
    private readonly NativeCoreWrapper.AwaitDebugEventDelegate awaitDebugEventDelegate;
    private readonly NativeCoreWrapper.HandleDebugEventDelegate handleDebugEventDelegate;
    private readonly NativeCoreWrapper.SetHardwareBreakpointDelegate setHardwareBreakpointDelegate;

    public NativeCoreWrapper(IntPtr handle)
    {
      if (handle.IsNull())
        throw new ArgumentNullException();
      this.enumerateProcessesDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.EnumerateProcessesDelegate>(handle, "EnumerateProcesses");
      this.enumerateRemoteSectionsAndModulesDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.EnumerateRemoteSectionsAndModulesDelegate>(handle, "EnumerateRemoteSectionsAndModules");
      this.openRemoteProcessDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.OpenRemoteProcessDelegate>(handle, "OpenRemoteProcess");
      this.isProcessValidDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.IsProcessValidDelegate>(handle, "IsProcessValid");
      this.closeRemoteProcessDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.CloseRemoteProcessDelegate>(handle, "CloseRemoteProcess");
      this.readRemoteMemoryDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.ReadRemoteMemoryDelegate>(handle, "ReadRemoteMemory");
      this.writeRemoteMemoryDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.WriteRemoteMemoryDelegate>(handle, "WriteRemoteMemory");
      this.controlRemoteProcessDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.ControlRemoteProcessDelegate>(handle, "ControlRemoteProcess");
      this.attachDebuggerToProcessDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.AttachDebuggerToProcessDelegate>(handle, "AttachDebuggerToProcess");
      this.detachDebuggerFromProcessDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.DetachDebuggerFromProcessDelegate>(handle, "DetachDebuggerFromProcess");
      this.awaitDebugEventDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.AwaitDebugEventDelegate>(handle, "AwaitDebugEvent");
      this.handleDebugEventDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.HandleDebugEventDelegate>(handle, "HandleDebugEvent");
      this.setHardwareBreakpointDelegate = NativeCoreWrapper.GetFunctionDelegate<NativeCoreWrapper.SetHardwareBreakpointDelegate>(handle, "SetHardwareBreakpoint");
    }

    protected static TDelegate GetFunctionDelegate<TDelegate>(IntPtr handle, string function)
    {
      IntPtr procAddress = ReClassNET.Native.NativeMethods.GetProcAddress(handle, function);
      if (procAddress.IsNull())
        throw new Exception("Function '" + function + "' not found.");
      return Marshal.GetDelegateForFunctionPointer<TDelegate>(procAddress);
    }

    public void EnumerateProcesses(EnumerateProcessCallback callbackProcess)
    {
      this.enumerateProcessesDelegate(callbackProcess);
    }

    public void EnumerateRemoteSectionsAndModules(
      IntPtr process,
      EnumerateRemoteSectionCallback callbackSection,
      EnumerateRemoteModuleCallback callbackModule)
    {
      this.enumerateRemoteSectionsAndModulesDelegate(process, callbackSection, callbackModule);
    }

    public IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess)
    {
      return this.openRemoteProcessDelegate(pid, desiredAccess);
    }

    public bool IsProcessValid(IntPtr process)
    {
      return this.isProcessValidDelegate(process);
    }

    public void CloseRemoteProcess(IntPtr process)
    {
      this.closeRemoteProcessDelegate(process);
    }

    public bool ReadRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size)
    {
      return this.readRemoteMemoryDelegate(process, address, buffer, offset, size);
    }

    public bool WriteRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size)
    {
      return this.writeRemoteMemoryDelegate(process, address, buffer, offset, size);
    }

    public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
    {
      this.controlRemoteProcessDelegate(process, action);
    }

    public bool AttachDebuggerToProcess(IntPtr id)
    {
      return this.attachDebuggerToProcessDelegate(id);
    }

    public void DetachDebuggerFromProcess(IntPtr id)
    {
      this.detachDebuggerFromProcessDelegate(id);
    }

    public bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds)
    {
      return this.awaitDebugEventDelegate(ref evt, timeoutInMilliseconds);
    }

    public void HandleDebugEvent(ref DebugEvent evt)
    {
      this.handleDebugEventDelegate(ref evt);
    }

    public bool SetHardwareBreakpoint(
      IntPtr id,
      IntPtr address,
      HardwareBreakpointRegister register,
      HardwareBreakpointTrigger trigger,
      HardwareBreakpointSize size,
      bool set)
    {
      return this.setHardwareBreakpointDelegate(id, address, register, trigger, size, set);
    }

    private delegate void EnumerateProcessesDelegate([MarshalAs(UnmanagedType.FunctionPtr)] EnumerateProcessCallback callbackProcess);

    private delegate void EnumerateRemoteSectionsAndModulesDelegate(
      IntPtr process,
      [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateRemoteSectionCallback callbackSection,
      [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateRemoteModuleCallback callbackModule);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool IsProcessValidDelegate(IntPtr process);

    private delegate IntPtr OpenRemoteProcessDelegate(IntPtr pid, ProcessAccess desiredAccess);

    private delegate void CloseRemoteProcessDelegate(IntPtr process);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool ReadRemoteMemoryDelegate(
      IntPtr process,
      IntPtr address,
      [Out] byte[] buffer,
      int offset,
      int size);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool WriteRemoteMemoryDelegate(
      IntPtr process,
      IntPtr address,
      [In] byte[] buffer,
      int offset,
      int size);

    private delegate void ControlRemoteProcessDelegate(
      IntPtr process,
      ControlRemoteProcessAction action);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool AttachDebuggerToProcessDelegate(IntPtr id);

    private delegate void DetachDebuggerFromProcessDelegate(IntPtr id);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool AwaitDebugEventDelegate([In, Out] ref DebugEvent evt, int timeoutInMilliseconds);

    private delegate void HandleDebugEventDelegate([In, Out] ref DebugEvent evt);

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool SetHardwareBreakpointDelegate(
      IntPtr id,
      IntPtr address,
      HardwareBreakpointRegister register,
      HardwareBreakpointTrigger trigger,
      HardwareBreakpointSize size,
      [MarshalAs(UnmanagedType.I1)] bool set);
  }
}
