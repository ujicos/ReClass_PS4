// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.RemoteProcess
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.AddressParser;
using ReClassNET.Core;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Native;
using ReClassNET.Symbols;
using ReClassNET.Util.Conversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReClassNET.Memory
{
  public class RemoteProcess : IDisposable, IRemoteMemoryReader, IRemoteMemoryWriter, IProcessReader
  {
    private readonly object processSync = new object();
    private readonly Dictionary<string, Func<RemoteProcess, IntPtr>> formulaCache = new Dictionary<string, Func<RemoteProcess, IntPtr>>();
    private readonly Dictionary<IntPtr, string> rttiCache = new Dictionary<IntPtr, string>();
    private readonly List<Module> modules = new List<Module>();
    private readonly List<Section> sections = new List<Section>();
    private readonly SymbolStore symbols = new SymbolStore();
    private readonly CoreFunctionsManager coreFunctions;
    private readonly RemoteDebugger debugger;
    private ProcessInfo process;
    private IntPtr handle;

    public event RemoteProcessEvent ProcessAttached;

    public event RemoteProcessEvent ProcessClosing;

    public event RemoteProcessEvent ProcessClosed;

    public CoreFunctionsManager CoreFunctions
    {
      get
      {
        return this.coreFunctions;
      }
    }

    public RemoteDebugger Debugger
    {
      get
      {
        return this.debugger;
      }
    }

    public ProcessInfo UnderlayingProcess
    {
      get
      {
        return this.process;
      }
    }

    public SymbolStore Symbols
    {
      get
      {
        return this.symbols;
      }
    }

    public EndianBitConverter BitConverter { get; set; } = EndianBitConverter.System;

    public IEnumerable<Module> Modules
    {
      get
      {
        lock (this.modules)
          return (IEnumerable<Module>) new List<Module>((IEnumerable<Module>) this.modules);
      }
    }

    public IEnumerable<Section> Sections
    {
      get
      {
        lock (this.sections)
          return (IEnumerable<Section>) new List<Section>((IEnumerable<Section>) this.sections);
      }
    }

    public Dictionary<IntPtr, string> NamedAddresses { get; } = new Dictionary<IntPtr, string>();

    public bool IsValid
    {
      get
      {
        return this.process != null && this.coreFunctions.IsProcessValid(this.handle);
      }
    }

    public RemoteProcess(CoreFunctionsManager coreFunctions)
    {
      this.coreFunctions = coreFunctions;
      this.debugger = new RemoteDebugger(this);
    }

    public void Dispose()
    {
      this.Close();
    }

    public void Open(ProcessInfo info)
    {
      if (this.process == info)
        return;
      lock (this.processSync)
      {
        this.Close();
        this.rttiCache.Clear();
        this.process = info;
        this.handle = this.coreFunctions.OpenRemoteProcess(this.process.Id, ProcessAccess.Full);
      }
      RemoteProcessEvent processAttached = this.ProcessAttached;
      if (processAttached == null)
        return;
      processAttached(this);
    }

    public void Close()
    {
      if (this.process == null)
        return;
      RemoteProcessEvent processClosing = this.ProcessClosing;
      if (processClosing != null)
        processClosing(this);
      lock (this.processSync)
      {
        this.debugger.Terminate();
        this.coreFunctions.CloseRemoteProcess(this.handle);
        this.handle = IntPtr.Zero;
        this.process = (ProcessInfo) null;
      }
      RemoteProcessEvent processClosed = this.ProcessClosed;
      if (processClosed == null)
        return;
      processClosed(this);
    }

    public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer)
    {
      return this.ReadRemoteMemoryIntoBuffer(address, ref buffer, 0, buffer.Length);
    }

    public bool ReadRemoteMemoryIntoBuffer(
      IntPtr address,
      ref byte[] buffer,
      int offset,
      int length)
    {
      if (this.IsValid)
        return this.coreFunctions.ReadRemoteMemory(this.handle, address, ref buffer, offset, length);
      this.Close();
      buffer.FillWithZero();
      return false;
    }

    public byte[] ReadRemoteMemory(IntPtr address, int size)
    {
      byte[] buffer = new byte[size];
      this.ReadRemoteMemoryIntoBuffer(address, ref buffer);
      return buffer;
    }

    public string ReadRemoteRuntimeTypeInformation(IntPtr address)
    {
      if (!address.MayBeValid())
        return (string) null;
      string str;
      if (!this.rttiCache.TryGetValue(address, out str))
      {
        IntPtr num = this.ReadRemoteIntPtr(address - IntPtr.Size);
        if (num.MayBeValid())
        {
          str = this.ReadRemoteRuntimeTypeInformation64(num);
          this.rttiCache[address] = str;
        }
      }
      return str;
    }

    private string ReadRemoteRuntimeTypeInformation32(IntPtr address)
    {
      IntPtr ptr1 = this.ReadRemoteIntPtr(address + 16);
      if (ptr1.MayBeValid())
      {
        int num1 = this.ReadRemoteInt32(ptr1 + 8);
        if (num1 > 0 && num1 < 25)
        {
          IntPtr ptr2 = this.ReadRemoteIntPtr(ptr1 + 12);
          if (ptr2.MayBeValid())
          {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < num1; ++index)
            {
              IntPtr num2 = this.ReadRemoteIntPtr(ptr2 + 4 * index);
              if (num2.MayBeValid())
              {
                IntPtr ptr3 = this.ReadRemoteIntPtr(num2);
                if (ptr3.MayBeValid())
                {
                  string str = this.ReadRemoteStringUntilFirstNullCharacter(ptr3 + 12, Encoding.UTF8, 60);
                  if (str.EndsWith("@@"))
                    str = NativeMethods.UndecorateSymbolName("?" + str);
                  stringBuilder.Append(str);
                  stringBuilder.Append(" : ");
                }
                else
                  break;
              }
              else
                break;
            }
            if (stringBuilder.Length != 0)
            {
              stringBuilder.Length -= 3;
              return stringBuilder.ToString();
            }
          }
        }
      }
      return (string) null;
    }

    private string ReadRemoteRuntimeTypeInformation64(IntPtr address)
    {
      int num1 = this.ReadRemoteInt32(address + 20);
      if (num1 != 0)
      {
        IntPtr num2 = address - num1;
        int num3 = this.ReadRemoteInt32(address + 16);
        if (num3 != 0)
        {
          IntPtr num4 = num2 + num3;
          int num5 = this.ReadRemoteInt32(num4 + 8);
          if (num5 > 0 && num5 < 25)
          {
            int num6 = this.ReadRemoteInt32(num4 + 12);
            if (num6 != 0)
            {
              IntPtr num7 = num2 + num6;
              StringBuilder stringBuilder = new StringBuilder();
              for (int index = 0; index < num5; ++index)
              {
                int num8 = this.ReadRemoteInt32(num7 + 4 * index);
                if (num8 != 0)
                {
                  int num9 = this.ReadRemoteInt32(num2 + num8);
                  if (num9 != 0)
                  {
                    string str = this.ReadRemoteStringUntilFirstNullCharacter(num2 + num9 + 20, Encoding.UTF8, 60);
                    if (!string.IsNullOrEmpty(str))
                    {
                      if (str.EndsWith("@@"))
                        str = NativeMethods.UndecorateSymbolName("?" + str);
                      stringBuilder.Append(str);
                      stringBuilder.Append(" : ");
                    }
                    else
                      break;
                  }
                  else
                    break;
                }
                else
                  break;
              }
              if (stringBuilder.Length != 0)
              {
                stringBuilder.Length -= 3;
                return stringBuilder.ToString();
              }
            }
          }
        }
      }
      return (string) null;
    }

    public bool WriteRemoteMemory(IntPtr address, byte[] data)
    {
      return this.IsValid && this.coreFunctions.WriteRemoteMemory(this.handle, address, ref data, 0, data.Length);
    }

    public Section GetSectionToPointer(IntPtr address)
    {
      lock (this.sections)
      {
        int index = this.sections.BinarySearch<Section>((Func<Section, int>) (s => address.CompareToRange(s.Start, s.End)));
        return index < 0 ? (Section) null : this.sections[index];
      }
    }

    public Module GetModuleToPointer(IntPtr address)
    {
      lock (this.modules)
      {
        int index = this.modules.BinarySearch<Module>((Func<Module, int>) (m => address.CompareToRange(m.Start, m.End)));
        return index < 0 ? (Module) null : this.modules[index];
      }
    }

    public Module GetModuleByName(string name)
    {
      lock (this.modules)
        return this.modules.FirstOrDefault<Module>((Func<Module, bool>) (m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)));
    }

    public string GetNamedAddress(IntPtr address)
    {
      string str;
      if (this.NamedAddresses.TryGetValue(address, out str))
        return str;
      Section sectionToPointer = this.GetSectionToPointer(address);
      if (sectionToPointer != null)
      {
        if (sectionToPointer.Category == SectionCategory.CODE || sectionToPointer.Category == SectionCategory.DATA)
          return string.Format("<{0}>{1}.{2}", (object) sectionToPointer.Category, (object) sectionToPointer.ModuleName, (object) address.ToString("X"));
        if (sectionToPointer.Category == SectionCategory.HEAP)
          return "<HEAP>" + address.ToString("X");
      }
      Module moduleToPointer = this.GetModuleToPointer(address);
      return moduleToPointer != null ? moduleToPointer.Name + "." + address.ToString("X") : (string) null;
    }

    public bool EnumerateRemoteSectionsAndModules(
      out List<Section> _sections,
      out List<Module> _modules)
    {
      if (!this.IsValid)
      {
        _sections = (List<Section>) null;
        _modules = (List<Module>) null;
        return false;
      }
      _sections = new List<Section>();
      _modules = new List<Module>();
      this.coreFunctions.EnumerateRemoteSectionsAndModules(this.handle, new Action<Section>(_sections.Add), new Action<Module>(_modules.Add));
      return true;
    }

    public void UpdateProcessInformations()
    {
      this.UpdateProcessInformationsAsync().Wait();
    }

    public Task UpdateProcessInformationsAsync()
    {
      if (this.IsValid)
        return Task.Run((Action) (() =>
        {
          List<Section> _sections;
          List<Module> _modules;
          this.EnumerateRemoteSectionsAndModules(out _sections, out _modules);
          _modules.Sort((Comparison<Module>) ((m1, m2) => m1.Start.CompareTo(m2.Start)));
          _sections.Sort((Comparison<Section>) ((s1, s2) => s1.Start.CompareTo(s2.Start)));
          lock (this.modules)
          {
            this.modules.Clear();
            this.modules.AddRange((IEnumerable<Module>) _modules);
          }
          lock (this.sections)
          {
            this.sections.Clear();
            this.sections.AddRange((IEnumerable<Section>) _sections);
          }
        }));
      lock (this.modules)
        this.modules.Clear();
      lock (this.sections)
        this.sections.Clear();
      return (Task) Task.FromResult<bool>(true);
    }

    public IntPtr ParseAddress(string addressFormula)
    {
      Func<RemoteProcess, IntPtr> func;
      if (!this.formulaCache.TryGetValue(addressFormula, out func))
      {
        func = (Func<RemoteProcess, IntPtr>) DynamicCompiler.CompileExpression(Parser.Parse(addressFormula));
        this.formulaCache.Add(addressFormula, func);
      }
      return func(this);
    }

    public Task LoadAllSymbolsAsync(
      IProgress<Tuple<Module, IReadOnlyList<Module>>> progress,
      CancellationToken token)
    {
      List<Module> copy;
      lock (this.modules)
        copy = this.modules.ToList<Module>();
      return Task.Run((Action) (() =>
      {
        foreach (Module module in copy)
        {
          token.ThrowIfCancellationRequested();
          progress?.Report(Tuple.Create<Module, IReadOnlyList<Module>>(module, (IReadOnlyList<Module>) copy));
          this.Symbols.TryResolveSymbolsForModule(module);
        }
      }), token).ContinueWith((Action<Task>) (_ =>
      {
        foreach (Module module in copy)
        {
          token.ThrowIfCancellationRequested();
          try
          {
            this.Symbols.LoadSymbolsForModule(module);
          }
          catch
          {
          }
        }
      }), token, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void ControlRemoteProcess(ControlRemoteProcessAction action)
    {
      if (!this.IsValid)
        return;
      this.coreFunctions.ControlRemoteProcess(this.handle, action);
    }
  }
}
