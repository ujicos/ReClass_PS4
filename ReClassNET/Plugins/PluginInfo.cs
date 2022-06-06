// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.PluginInfo
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Native;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ReClassNET.Plugins
{
  internal class PluginInfo : IDisposable
  {
    public const string PluginName = "ReClass.NET Plugin";
    public const string PluginNativeName = "ReClass.NET Native Plugin";

    public string FilePath { get; }

    public string FileVersion { get; }

    public string Name { get; }

    public string Description { get; }

    public string Author { get; }

    public bool IsNative { get; }

    public Plugin Interface { get; set; }

    public IntPtr NativeHandle { get; set; }

    public IReadOnlyList<INodeInfoReader> NodeInfoReaders { get; set; }

    public Plugin.CustomNodeTypes CustomNodeTypes { get; set; }

    public PluginInfo(string filePath, FileVersionInfo versionInfo)
    {
      this.FilePath = filePath;
      this.IsNative = versionInfo.ProductName == null || versionInfo.ProductName == "ReClass.NET Native Plugin";
      this.FileVersion = (versionInfo.FileVersion ?? string.Empty).Trim();
      this.Description = (versionInfo.Comments ?? string.Empty).Trim();
      this.Author = (versionInfo.CompanyName ?? string.Empty).Trim();
      this.Name = (versionInfo.FileDescription ?? string.Empty).Trim();
      if (!(this.Name == string.Empty))
        return;
      this.Name = Path.GetFileNameWithoutExtension(this.FilePath);
    }

    ~PluginInfo()
    {
      this.ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
      if (this.NativeHandle.IsNull())
        return;
      NativeMethods.FreeLibrary(this.NativeHandle);
      this.NativeHandle = IntPtr.Zero;
    }

    public void Dispose()
    {
      if (this.Interface != null)
      {
        try
        {
          this.Interface.Terminate();
        }
        catch
        {
        }
      }
      this.ReleaseUnmanagedResources();
      GC.SuppressFinalize((object) this);
    }
  }
}
