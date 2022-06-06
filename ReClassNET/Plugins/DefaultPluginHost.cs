// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.DefaultPluginHost
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using System.Resources;

namespace ReClassNET.Plugins
{
  internal sealed class DefaultPluginHost : IPluginHost
  {
    public MainForm MainWindow { get; }

    public ResourceManager Resources
    {
      get
      {
        return ReClassNET.Properties.Resources.ResourceManager;
      }
    }

    public RemoteProcess Process { get; }

    public ILogger Logger { get; }

    public ReClassNET.Settings Settings
    {
      get
      {
        return Program.Settings;
      }
    }

    public DefaultPluginHost(MainForm form, RemoteProcess process, ILogger logger)
    {
      this.MainWindow = form;
      this.Process = process;
      this.Logger = logger;
    }
  }
}
