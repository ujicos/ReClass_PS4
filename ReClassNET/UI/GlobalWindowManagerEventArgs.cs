// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.GlobalWindowManagerEventArgs
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Windows.Forms;

namespace ReClassNET.UI
{
  public sealed class GlobalWindowManagerEventArgs : EventArgs
  {
    public Form Form { get; }

    public GlobalWindowManagerEventArgs(Form form)
    {
      this.Form = form;
    }
  }
}
