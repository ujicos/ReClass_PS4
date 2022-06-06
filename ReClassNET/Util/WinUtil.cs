// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.WinUtil
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace ReClassNET.Util
{
  public static class WinUtil
  {
    public static bool IsWindows9x { get; }

    public static bool IsWindows2000 { get; }

    public static bool IsWindowsXP { get; }

    public static bool IsAtLeastWindows2000 { get; }

    public static bool IsAtLeastWindowsVista { get; }

    public static bool IsAtLeastWindows7 { get; }

    public static bool IsAtLeastWindows8 { get; }

    public static bool IsAtLeastWindows10 { get; }

    public static bool IsAdministrator
    {
      get
      {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
      }
    }

    static WinUtil()
    {
      OperatingSystem osVersion = Environment.OSVersion;
      Version version = osVersion.Version;
      WinUtil.IsWindows9x = osVersion.Platform == PlatformID.Win32Windows;
      WinUtil.IsWindows2000 = version.Major == 5 && version.Minor == 0;
      WinUtil.IsWindowsXP = version.Major == 5 && version.Minor == 1;
      WinUtil.IsAtLeastWindows2000 = version.Major >= 5;
      WinUtil.IsAtLeastWindowsVista = version.Major >= 6;
      WinUtil.IsAtLeastWindows7 = version.Major >= 7 || version.Major == 6 && version.Minor >= 1;
      WinUtil.IsAtLeastWindows8 = version.Major >= 7 || version.Major == 6 && version.Minor >= 2;
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false))
        {
          uint result;
          if (registryKey == null || !uint.TryParse(registryKey.GetValue("CurrentMajorVersionNumber", (object) string.Empty)?.ToString(), out result))
            return;
          WinUtil.IsAtLeastWindows10 = result >= 10U;
        }
      }
      catch
      {
      }
    }

    public static bool RunElevated(string applicationPath, string arguments)
    {
      try
      {
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
          FileName = applicationPath,
          UseShellExecute = true,
          WindowStyle = ProcessWindowStyle.Normal
        };
        if (arguments != null)
          startInfo.Arguments = arguments;
        if (WinUtil.IsAtLeastWindowsVista)
          startInfo.Verb = "runas";
        Process.Start(startInfo);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }
  }
}
