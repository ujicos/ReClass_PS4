// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.GlobalWindowManager
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.UI
{
  public static class GlobalWindowManager
  {
    private static readonly List<Form> windows = new List<Form>();

    public static Form TopWindow
    {
      get
      {
        return GlobalWindowManager.windows.LastOrDefault<Form>();
      }
    }

    public static IEnumerable<Form> Windows
    {
      get
      {
        return (IEnumerable<Form>) GlobalWindowManager.windows;
      }
    }

    public static event EventHandler<GlobalWindowManagerEventArgs> WindowAdded;

    public static event EventHandler<GlobalWindowManagerEventArgs> WindowRemoved;

    public static void AddWindow(Form form)
    {
      GlobalWindowManager.windows.Add(form);
      form.TopMost = Program.Settings.StayOnTop;
      EventHandler<GlobalWindowManagerEventArgs> windowAdded = GlobalWindowManager.WindowAdded;
      if (windowAdded == null)
        return;
      windowAdded((object) null, new GlobalWindowManagerEventArgs(form));
    }

    public static void RemoveWindow(Form form)
    {
      if (!GlobalWindowManager.windows.Remove(form))
        return;
      EventHandler<GlobalWindowManagerEventArgs> windowRemoved = GlobalWindowManager.WindowRemoved;
      if (windowRemoved == null)
        return;
      windowRemoved((object) null, new GlobalWindowManagerEventArgs(form));
    }
  }
}
