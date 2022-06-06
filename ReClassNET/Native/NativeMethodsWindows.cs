// Decompiled with JetBrains decompiler
// Type: ReClassNET.Native.NativeMethodsWindows
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Microsoft.Win32;
using ReClassNET.Extensions;
using ReClassNET.Util;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace ReClassNET.Native
{
  internal class NativeMethodsWindows : INativeMethods
  {
    private const uint SHGFI_ICON = 256;
    private const uint SHGFI_LARGEICON = 0;
    private const uint SHGFI_SMALLICON = 1;
    private const int SHCNE_ASSOCCHANGED = 134217728;
    private const uint SHCNF_IDLIST = 0;
    private const int BCM_SETSHIELD = 5644;

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll")]
    private static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("shell32.dll")]
    private static extern IntPtr SHGetFileInfo(
      string pszPath,
      int dwFileAttributes,
      ref NativeMethodsWindows.SHFILEINFO psfi,
      int cbSizeFileInfo,
      uint uFlags);

    [DllImport("user32.dll")]
    private static extern int DestroyIcon(IntPtr hIcon);

    [DllImport("advapi32.dll")]
    private static extern bool OpenProcessToken(
      IntPtr ProcessHandle,
      TokenAccessLevels DesiredAccess,
      out IntPtr TokenHandle);

    [DllImport("advapi32.dll")]
    private static extern bool AdjustTokenPrivileges(
      IntPtr TokenHandle,
      [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
      ref NativeMethodsWindows.TOKEN_PRIVILEGES NewState,
      uint Zero,
      IntPtr Null1,
      IntPtr Null2);

    [DllImport("dbghelp.dll", CharSet = CharSet.Unicode)]
    private static extern int UnDecorateSymbolName(
      string DecoratedName,
      StringBuilder UnDecoratedName,
      int UndecoratedLength,
      int Flags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetProcessDPIAware();

    [DllImport("shcore.dll")]
    private static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] NativeMethodsWindows.ProcessDpiAwareness a);

    [DllImport("shell32.dll")]
    private static extern void SHChangeNotify(
      int wEventId,
      uint uFlags,
      IntPtr dwItem1,
      IntPtr dwItem2);

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(
      IntPtr hWnd,
      int nMsg,
      IntPtr wParam,
      IntPtr lParam);

    IntPtr INativeMethods.LoadLibrary(string fileName)
    {
      return NativeMethodsWindows.LoadLibrary(fileName);
    }

    IntPtr INativeMethods.GetProcAddress(IntPtr handle, string name)
    {
      return NativeMethodsWindows.GetProcAddress(handle, name);
    }

    void INativeMethods.FreeLibrary(IntPtr handle)
    {
      NativeMethodsWindows.FreeLibrary(handle);
    }

    public Icon GetIconForFile(string path)
    {
      NativeMethodsWindows.SHFILEINFO psfi = new NativeMethodsWindows.SHFILEINFO();
      if (NativeMethodsWindows.SHGetFileInfo(path, 0, ref psfi, Marshal.SizeOf<NativeMethodsWindows.SHFILEINFO>(psfi), 257U).IsNull())
        return (Icon) null;
      Icon icon = Icon.FromHandle(psfi.hIcon).Clone() as Icon;
      NativeMethodsWindows.DestroyIcon(psfi.hIcon);
      return icon;
    }

    public void EnableDebugPrivileges()
    {
      IntPtr TokenHandle;
      if (!NativeMethodsWindows.OpenProcessToken(Process.GetCurrentProcess().Handle, TokenAccessLevels.AllAccess, out TokenHandle))
        return;
      NativeMethodsWindows.TOKEN_PRIVILEGES NewState = new NativeMethodsWindows.TOKEN_PRIVILEGES()
      {
        PrivilegeCount = 1,
        Luid = {
          LowPart = 20,
          HighPart = 0
        },
        Attributes = 2
      };
      NativeMethodsWindows.AdjustTokenPrivileges(TokenHandle, false, ref NewState, 0U, IntPtr.Zero, IntPtr.Zero);
      NativeMethodsWindows.CloseHandle(TokenHandle);
    }

    public string UndecorateSymbolName(string name)
    {
      StringBuilder UnDecoratedName = new StringBuilder((int) byte.MaxValue);
      return NativeMethodsWindows.UnDecorateSymbolName(name, UnDecoratedName, UnDecoratedName.Capacity, 4096) != 0 ? UnDecoratedName.ToString() : name;
    }

    public void SetProcessDpiAwareness()
    {
      if (WinUtil.IsAtLeastWindows10)
      {
        NativeMethodsWindows.SetProcessDpiAwareness(NativeMethodsWindows.ProcessDpiAwareness.SystemAware);
      }
      else
      {
        if (!WinUtil.IsAtLeastWindowsVista)
          return;
        NativeMethodsWindows.SetProcessDPIAware();
      }
    }

    public bool RegisterExtension(
      string fileExtension,
      string extensionId,
      string applicationPath,
      string applicationName)
    {
      try
      {
        RegistryKey classesRoot = Registry.ClassesRoot;
        using (RegistryKey subKey = classesRoot.CreateSubKey(fileExtension))
          subKey?.SetValue(string.Empty, (object) extensionId, RegistryValueKind.String);
        using (RegistryKey subKey1 = classesRoot.CreateSubKey(extensionId))
        {
          subKey1?.SetValue(string.Empty, (object) applicationName, RegistryValueKind.String);
          using (RegistryKey subKey2 = subKey1?.CreateSubKey("DefaultIcon"))
            subKey2?.SetValue(string.Empty, (object) ("\"" + applicationPath + "\",0"), RegistryValueKind.String);
          using (RegistryKey subKey2 = subKey1?.CreateSubKey("shell"))
          {
            using (RegistryKey subKey3 = subKey2?.CreateSubKey("open"))
            {
              subKey3?.SetValue(string.Empty, (object) ("&Open with " + applicationName), RegistryValueKind.String);
              using (RegistryKey subKey4 = subKey3?.CreateSubKey("command"))
                subKey4?.SetValue(string.Empty, (object) ("\"" + applicationPath + "\" \"%1\""), RegistryValueKind.String);
            }
          }
        }
        NativeMethodsWindows.ShChangeNotify();
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public void UnregisterExtension(string fileExtension, string extensionId)
    {
      try
      {
        RegistryKey classesRoot = Registry.ClassesRoot;
        classesRoot.DeleteSubKeyTree(fileExtension);
        classesRoot.DeleteSubKeyTree(extensionId);
        NativeMethodsWindows.ShChangeNotify();
      }
      catch
      {
      }
    }

    private static void ShChangeNotify()
    {
      try
      {
        NativeMethodsWindows.SHChangeNotify(134217728, 0U, IntPtr.Zero, IntPtr.Zero);
      }
      catch
      {
      }
    }

    public static void SetButtonShield(Button button, bool setShield)
    {
      try
      {
        if (button.FlatStyle != FlatStyle.System)
          button.FlatStyle = FlatStyle.System;
        IntPtr handle = button.Handle;
        if (handle == IntPtr.Zero)
          return;
        NativeMethodsWindows.SendMessage(handle, 5644, IntPtr.Zero, (IntPtr) (setShield ? 1 : 0));
      }
      catch
      {
      }
    }

    private struct SHFILEINFO
    {
      public IntPtr hIcon;
      public IntPtr iIcon;
      public uint dwAttributes;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
      public string szDisplayName;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
      public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct LUID
    {
      public uint LowPart;
      public int HighPart;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct TOKEN_PRIVILEGES
    {
      public uint PrivilegeCount;
      public NativeMethodsWindows.LUID Luid;
      public uint Attributes;
    }

    private enum ProcessDpiAwareness : uint
    {
      Unaware,
      SystemAware,
      PerMonitorAware,
    }
  }
}
