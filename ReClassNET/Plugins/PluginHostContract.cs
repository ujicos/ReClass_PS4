// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.PluginHostContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
using System;
using System.Resources;

namespace ReClassNET.Plugins
{
  internal abstract class PluginHostContract : IPluginHost
  {
    public ILogger Logger
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public MainForm MainWindow
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public RemoteProcess Process
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public ResourceManager Resources
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Settings Settings
    {
      get
      {
        throw new NotImplementedException();
      }
    }
  }
}
