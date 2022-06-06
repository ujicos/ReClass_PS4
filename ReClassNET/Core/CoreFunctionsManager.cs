// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.CoreFunctionsManager
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Core
{
  public class CoreFunctionsManager : IDisposable
  {
    private readonly Dictionary<string, ICoreProcessFunctions> functionsRegistry = new Dictionary<string, ICoreProcessFunctions>();
    private readonly InternalCoreFunctions internalCoreFunctions;
    private ICoreProcessFunctions currentFunctions;

    public IEnumerable<string> FunctionProviders
    {
      get
      {
        return (IEnumerable<string>) this.functionsRegistry.Keys;
      }
    }

    public ICoreProcessFunctions CurrentFunctions
    {
      get
      {
        return this.currentFunctions;
      }
    }

    public string CurrentFunctionsProvider
    {
      get
      {
        return this.functionsRegistry.Where<KeyValuePair<string, ICoreProcessFunctions>>((Func<KeyValuePair<string, ICoreProcessFunctions>, bool>) (kv => kv.Value == this.currentFunctions)).Select<KeyValuePair<string, ICoreProcessFunctions>, string>((Func<KeyValuePair<string, ICoreProcessFunctions>, string>) (kv => kv.Key)).FirstOrDefault<string>();
      }
    }

    public CoreFunctionsManager()
    {
      this.internalCoreFunctions = InternalCoreFunctions.Create();
      this.RegisterFunctions("Default", (ICoreProcessFunctions) this.internalCoreFunctions);
      this.currentFunctions = (ICoreProcessFunctions) this.internalCoreFunctions;
    }

    public void Dispose()
    {
      this.internalCoreFunctions.Dispose();
    }

    public void RegisterFunctions(string provider, ICoreProcessFunctions functions)
    {
      this.functionsRegistry.Add(provider, functions);
    }

    public void SetActiveFunctionsProvider(string provider)
    {
      ICoreProcessFunctions processFunctions;
      if (!this.functionsRegistry.TryGetValue(provider, out processFunctions))
        throw new ArgumentException();
      this.currentFunctions = processFunctions;
    }

    public void EnumerateProcesses(Action<ProcessInfo> callbackProcess)
    {
      this.currentFunctions.EnumerateProcesses(callbackProcess == null ? (EnumerateProcessCallback) null : (EnumerateProcessCallback) ((ref EnumerateProcessData data) => callbackProcess(new ProcessInfo(data.Id, data.Name, data.Path))));
    }

    public IList<ProcessInfo> EnumerateProcesses()
    {
      List<ProcessInfo> processInfoList = new List<ProcessInfo>();
      this.EnumerateProcesses(new Action<ProcessInfo>(processInfoList.Add));
      return (IList<ProcessInfo>) processInfoList;
    }

    public void EnumerateRemoteSectionsAndModules(
      IntPtr process,
      Action<Section> callbackSection,
      Action<Module> callbackModule)
    {
      EnumerateRemoteSectionCallback callbackSection1 = callbackSection == null ? (EnumerateRemoteSectionCallback) null : (EnumerateRemoteSectionCallback) ((ref EnumerateRemoteSectionData data) => callbackSection(new Section()
      {
        Start = data.BaseAddress,
        End = data.BaseAddress.Add(data.Size),
        Size = data.Size,
        Name = data.Name,
        Protection = data.Protection,
        Type = data.Type,
        ModulePath = data.ModulePath,
        ModuleName = Path.GetFileName(data.ModulePath),
        Category = data.Category
      }));
      EnumerateRemoteModuleCallback callbackModule1 = callbackModule == null ? (EnumerateRemoteModuleCallback) null : (EnumerateRemoteModuleCallback) ((ref EnumerateRemoteModuleData data) => callbackModule(new Module()
      {
        Start = data.BaseAddress,
        End = data.BaseAddress.Add(data.Size),
        Size = data.Size,
        Path = data.Path,
        Name = Path.GetFileName(data.Path)
      }));
      this.currentFunctions.EnumerateRemoteSectionsAndModules(process, callbackSection1, callbackModule1);
    }

    public void EnumerateRemoteSectionsAndModules(
      IntPtr process,
      out List<Section> sections,
      out List<Module> modules)
    {
      sections = new List<Section>();
      modules = new List<Module>();
      this.EnumerateRemoteSectionsAndModules(process, new Action<Section>(sections.Add), new Action<Module>(modules.Add));
    }

    public IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess)
    {
      return this.currentFunctions.OpenRemoteProcess(pid, desiredAccess);
    }

    public bool IsProcessValid(IntPtr process)
    {
      return this.currentFunctions.IsProcessValid(process);
    }

    public void CloseRemoteProcess(IntPtr process)
    {
      this.currentFunctions.CloseRemoteProcess(process);
    }

    public bool ReadRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size)
    {
      return this.currentFunctions.ReadRemoteMemory(process, address, ref buffer, offset, size);
    }

    public bool WriteRemoteMemory(
      IntPtr process,
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int size)
    {
      return this.currentFunctions.WriteRemoteMemory(process, address, ref buffer, offset, size);
    }

    public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
    {
      this.currentFunctions.ControlRemoteProcess(process, action);
    }

    public bool AttachDebuggerToProcess(IntPtr id)
    {
      return this.currentFunctions.AttachDebuggerToProcess(id);
    }

    public void DetachDebuggerFromProcess(IntPtr id)
    {
      this.currentFunctions.DetachDebuggerFromProcess(id);
    }

    public void HandleDebugEvent(ref DebugEvent evt)
    {
      this.currentFunctions.HandleDebugEvent(ref evt);
    }

    public bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds)
    {
      return this.currentFunctions.AwaitDebugEvent(ref evt, timeoutInMilliseconds);
    }

    public bool SetHardwareBreakpoint(
      IntPtr id,
      IntPtr address,
      HardwareBreakpointRegister register,
      HardwareBreakpointTrigger trigger,
      HardwareBreakpointSize size,
      bool set)
    {
      return this.currentFunctions.SetHardwareBreakpoint(id, address, register, trigger, size, set);
    }

    public bool DisassembleCode(
      IntPtr address,
      int length,
      IntPtr virtualAddress,
      bool determineStaticInstructionBytes,
      EnumerateInstructionCallback callback)
    {
      return this.internalCoreFunctions.DisassembleCode(address, length, virtualAddress, determineStaticInstructionBytes, callback);
    }

    public IntPtr InitializeInput()
    {
      return this.internalCoreFunctions.InitializeInput();
    }

    public Keys[] GetPressedKeys(IntPtr handle)
    {
      return this.internalCoreFunctions.GetPressedKeys(handle);
    }

    public void ReleaseInput(IntPtr handle)
    {
      this.internalCoreFunctions.ReleaseInput(handle);
    }
  }
}
