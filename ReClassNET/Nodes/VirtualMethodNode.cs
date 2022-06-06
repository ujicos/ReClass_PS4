// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.VirtualMethodNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class VirtualMethodNode : BaseFunctionPtrNode
  {
    public string MethodName
    {
      get
      {
        return !string.IsNullOrEmpty(this.Name) ? this.Name : string.Format("Function{0}", (object) (this.Offset / IntPtr.Size));
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      throw new InvalidOperationException("The 'VirtualMethodNode' node should not be accessible from the ui.");
    }

    public VirtualMethodNode()
    {
      this.Name = string.Empty;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, string.Format("({0})", (object) (this.Offset / IntPtr.Size)), this.MethodName);
    }
  }
}
