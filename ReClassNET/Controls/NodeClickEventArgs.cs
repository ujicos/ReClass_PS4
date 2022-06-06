// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.NodeClickEventArgs
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using ReClassNET.Nodes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class NodeClickEventArgs : EventArgs
  {
    public BaseNode Node { get; }

    public IntPtr Address { get; }

    public MemoryBuffer Memory { get; }

    public MouseButtons Button { get; }

    public Point Location { get; }

    public NodeClickEventArgs(
      BaseNode node,
      IntPtr address,
      MemoryBuffer memory,
      MouseButtons button,
      Point location)
    {
      this.Node = node;
      this.Address = address;
      this.Memory = memory;
      this.Button = button;
      this.Location = location;
    }
  }
}
