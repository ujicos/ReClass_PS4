// Decompiled with JetBrains decompiler
// Type: ReClassNET.Native.NativeMethodsUnix
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ReClassNET.Native
{
  internal class NativeMethodsUnix : INativeMethods
  {
    private const int RTLD_NOW = 2;

    [DllImport("libdl.so")]
    private static extern IntPtr dlopen(string fileName, int flags);

    [DllImport("libdl.so")]
    private static extern IntPtr dlsym(IntPtr handle, string symbol);

    [DllImport("libdl.so")]
    private static extern int dlclose(IntPtr handle);

    public IntPtr LoadLibrary(string fileName)
    {
      return NativeMethodsUnix.dlopen(fileName, 2);
    }

    public IntPtr GetProcAddress(IntPtr handle, string name)
    {
      return NativeMethodsUnix.dlsym(handle, name);
    }

    public void FreeLibrary(IntPtr handle)
    {
      NativeMethodsUnix.dlclose(handle);
    }

    public Icon GetIconForFile(string path)
    {
      return (Icon) null;
    }

    public void EnableDebugPrivileges()
    {
    }

    public string UndecorateSymbolName(string name)
    {
      return name;
    }

    public void SetProcessDpiAwareness()
    {
    }

    public bool RegisterExtension(
      string fileExtension,
      string extensionId,
      string applicationPath,
      string applicationName)
    {
      return false;
    }

    public void UnregisterExtension(string fileExtension, string extensionId)
    {
    }
  }
}
