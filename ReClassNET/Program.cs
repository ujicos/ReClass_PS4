// Decompiled with JetBrains decompiler
// Type: ReClassNET.Program
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Microsoft.SqlServer.MessageBox;
using ReClassNET.Core;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ReClassNET
{
  public static class Program
  {
    public static CommandLineArgs CommandLineArgs { get; private set; }

    public static Settings Settings { get; private set; }

    public static ILogger Logger { get; private set; }

    public static Random GlobalRandom { get; } = new Random();

    public static RemoteProcess RemoteProcess { get; private set; }

    public static CoreFunctionsManager CoreFunctions
    {
      get
      {
        return Program.RemoteProcess.CoreFunctions;
      }
    }

    public static MainForm MainForm { get; private set; }

    public static bool DesignMode { get; private set; } = true;

    public static FontEx MonoSpaceFont { get; private set; }

    [STAThread]
    private static void Main(string[] args)
    {
      Program.DesignMode = false;
      Program.CommandLineArgs = new CommandLineArgs(args);
      try
      {
        DpiUtil.ConfigureProcess();
        DpiUtil.TrySetDpiFromCurrentDesktop();
      }
      catch
      {
      }
      Program.MonoSpaceFont = new FontEx()
      {
        Font = new Font("Courier New", (float) DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
        Width = DpiUtil.ScaleIntX(8),
        Height = DpiUtil.ScaleIntY(16)
      };
      NativeMethods.EnableDebugPrivileges();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
      Program.Settings = SettingsSerializer.Load();
      Program.Logger = (ILogger) new GuiLogger();
      if (!NativeMethods.IsUnix() && Program.Settings.RunAsAdmin && !WinUtil.IsAdministrator)
      {
        WinUtil.RunElevated(Process.GetCurrentProcess().MainModule?.FileName, args.Length != 0 ? string.Join(" ", args) : (string) null);
      }
      else
      {
        try
        {
          using (CoreFunctionsManager coreFunctions = new CoreFunctionsManager())
          {
            Program.RemoteProcess = new RemoteProcess(coreFunctions);
            Program.MainForm = new MainForm();
            Application.Run((Form) Program.MainForm);
            Program.RemoteProcess.Dispose();
          }
        }
        catch (Exception ex)
        {
          Program.ShowException(ex);
        }
        SettingsSerializer.Save(Program.Settings);
      }
    }

    public static void ShowException(Exception ex)
    {
      ex.HelpLink = "https://github.com/ReClassNET/ReClass.NET/issues";
      int num = (int) new ExceptionMessageBox(ex)
      {
        Beep = false,
        ShowToolBar = true,
        Symbol = ExceptionMessageBoxSymbol.Error
      }.Show((IWin32Window) null);
    }
  }
}
