// Decompiled with JetBrains decompiler
// Type: ReClassNET.Symbols.SymbolStore
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Microsoft.Win32;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReClassNET.Symbols
{
  public class SymbolStore
  {
    private readonly Dictionary<string, SymbolReader> symbolReaders = new Dictionary<string, SymbolReader>();
    private readonly HashSet<string> moduleBlacklist = new HashSet<string>();
    private const string BlackListFile = "blacklist.txt";

    public string SymbolCachePath { get; private set; } = "./SymbolsCache";

    public string SymbolDownloadPath { get; set; } = "http://msdl.microsoft.com/download/symbols";

    public string SymbolSearchPath
    {
      get
      {
        return "srv*" + this.SymbolCachePath + "*" + this.SymbolDownloadPath;
      }
    }

    public SymbolStore()
    {
      if (ReClassNET.Native.NativeMethods.IsUnix())
        return;
      this.ResolveSearchPath();
      if (!File.Exists(Path.Combine(this.SymbolCachePath, "blacklist.txt")))
        return;
      ((IEnumerable<string>) File.ReadAllLines(Path.Combine(this.SymbolCachePath, "blacklist.txt"))).Select<string, string>((Func<string, string>) (l => l.Trim().ToLower())).ForEach<string>((Action<string>) (l => this.moduleBlacklist.Add(l)));
    }

    private void ResolveSearchPath()
    {
      using (RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\VisualStudio"))
      {
        if (registryKey1 == null)
          return;
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          using (RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName + "\\Debugger"))
          {
            if (registryKey2?.GetValue("SymbolCacheDir") is string path)
            {
              if (!Directory.Exists(path))
                break;
              this.SymbolCachePath = path;
              break;
            }
          }
        }
      }
    }

    public void TryResolveSymbolsForModule(Module module)
    {
      if (ReClassNET.Native.NativeMethods.IsUnix())
        return;
      string lower = module.Name.ToLower();
      bool flag;
      lock (this.symbolReaders)
        flag = this.moduleBlacklist.Contains(lower);
      if (flag)
        return;
      try
      {
        SymbolReader.TryResolveSymbolsForModule(module, this.SymbolSearchPath);
      }
      catch
      {
        lock (this.symbolReaders)
        {
          this.moduleBlacklist.Add(lower);
          File.WriteAllLines(Path.Combine(this.SymbolCachePath, "blacklist.txt"), this.moduleBlacklist.ToArray<string>());
        }
      }
    }

    public void LoadSymbolsForModule(Module module)
    {
      if (ReClassNET.Native.NativeMethods.IsUnix())
        return;
      string lower = module.Name.ToLower();
      bool flag;
      lock (this.symbolReaders)
        flag = !this.symbolReaders.ContainsKey(lower);
      if (!flag)
        return;
      SymbolReader symbolReader = SymbolReader.FromModule(module, this.SymbolSearchPath);
      lock (this.symbolReaders)
        this.symbolReaders[lower] = symbolReader;
    }

    public void LoadSymbolsFromPDB(string path)
    {
      if (ReClassNET.Native.NativeMethods.IsUnix())
        return;
      string lower = Path.GetFileName(path)?.ToLower();
      if (string.IsNullOrEmpty(lower))
        return;
      bool flag;
      lock (this.symbolReaders)
        flag = !this.symbolReaders.ContainsKey(lower);
      if (!flag)
        return;
      SymbolReader symbolReader = SymbolReader.FromDatabase(path);
      lock (this.symbolReaders)
        this.symbolReaders[lower] = symbolReader;
    }

    public SymbolReader GetSymbolsForModule(Module module)
    {
      if (ReClassNET.Native.NativeMethods.IsUnix())
        return (SymbolReader) null;
      string lower = module.Name.ToLower();
      lock (this.symbolReaders)
      {
        SymbolReader symbolReader;
        if (!this.symbolReaders.TryGetValue(lower, out symbolReader))
          this.symbolReaders.TryGetValue(Path.ChangeExtension(lower, ".pdb"), out symbolReader);
        return symbolReader;
      }
    }
  }
}
