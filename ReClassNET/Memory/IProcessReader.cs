// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.IProcessReader
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;

namespace ReClassNET.Memory
{
  public interface IProcessReader : IRemoteMemoryReader
  {
    Section GetSectionToPointer(IntPtr address);

    Module GetModuleToPointer(IntPtr address);

    Module GetModuleByName(string name);

    bool EnumerateRemoteSectionsAndModules(out List<Section> sections, out List<Module> modules);
  }
}
