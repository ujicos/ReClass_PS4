// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.Plugin
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.CodeGenerator;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ReClassNET.Plugins
{
  public class Plugin
  {
    public virtual Image Icon
    {
      get
      {
        return (Image) null;
      }
    }

    public virtual bool Initialize(IPluginHost host)
    {
      return true;
    }

    public virtual void Terminate()
    {
    }

    public virtual IReadOnlyList<INodeInfoReader> GetNodeInfoReaders()
    {
      return (IReadOnlyList<INodeInfoReader>) null;
    }

    public virtual Plugin.CustomNodeTypes GetCustomNodeTypes()
    {
      return (Plugin.CustomNodeTypes) null;
    }

    public class CustomNodeTypes
    {
      public IReadOnlyList<Type> NodeTypes { get; set; }

      public ICustomNodeSerializer Serializer { get; set; }

      public CustomCppCodeGenerator CodeGenerator { get; set; }
    }
  }
}
