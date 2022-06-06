// Decompiled with JetBrains decompiler
// Type: ReClassNET.Native.NativeMethods
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Drawing;

namespace ReClassNET.Native
{
  public static class NativeMethods
  {
    private static readonly INativeMethods nativeMethods;
    private static bool? isUnix;
    private static PlatformID? plattformId;

    static NativeMethods()
    {
      if (NativeMethods.IsUnix())
        NativeMethods.nativeMethods = (INativeMethods) new NativeMethodsUnix();
      else
        NativeMethods.nativeMethods = (INativeMethods) new NativeMethodsWindows();
    }

    public static bool IsUnix()
    {
      if (NativeMethods.isUnix.HasValue)
        return NativeMethods.isUnix.Value;
      PlatformID platformId = NativeMethods.GetPlatformId();
      int num;
      switch (platformId)
      {
        case PlatformID.Unix:
        case PlatformID.MacOSX:
          num = 1;
          break;
        default:
          num = platformId == (PlatformID) 128 ? 1 : 0;
          break;
      }
      NativeMethods.isUnix = new bool?(num != 0);
      return NativeMethods.isUnix.Value;
    }

    public static PlatformID GetPlatformId()
    {
      if (NativeMethods.plattformId.HasValue)
        return NativeMethods.plattformId.Value;
      NativeMethods.plattformId = new PlatformID?(Environment.OSVersion.Platform);
      return NativeMethods.plattformId.Value;
    }

    public static IntPtr LoadLibrary(string name)
    {
      return NativeMethods.nativeMethods.LoadLibrary(name);
    }

    public static IntPtr GetProcAddress(IntPtr handle, string name)
    {
      return NativeMethods.nativeMethods.GetProcAddress(handle, name);
    }

    public static void FreeLibrary(IntPtr handle)
    {
      NativeMethods.nativeMethods.FreeLibrary(handle);
    }

    public static Icon GetIconForFile(string path)
    {
      return NativeMethods.nativeMethods.GetIconForFile(path);
    }

    public static void EnableDebugPrivileges()
    {
      NativeMethods.nativeMethods.EnableDebugPrivileges();
    }

    public static string UndecorateSymbolName(string name)
    {
      return NativeMethods.nativeMethods.UndecorateSymbolName(name);
    }

    public static void SetProcessDpiAwareness()
    {
      NativeMethods.nativeMethods.SetProcessDpiAwareness();
    }

    public static bool RegisterExtension(
      string fileExtension,
      string extensionId,
      string applicationPath,
      string applicationName)
    {
      return NativeMethods.nativeMethods.RegisterExtension(fileExtension, extensionId, applicationPath, applicationName);
    }

    public static void UnregisterExtension(string fileExtension, string extensionId)
    {
      NativeMethods.nativeMethods.UnregisterExtension(fileExtension, extensionId);
    }
  }
}
