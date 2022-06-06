// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.IPluginHost
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using System.Resources;

namespace ReClassNET.Plugins
{
  public interface IPluginHost
  {
    MainForm MainWindow { get; }

    ResourceManager Resources { get; }

    RemoteProcess Process { get; }

    ILogger Logger { get; }

    Settings Settings { get; }
  }
}
