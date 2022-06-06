// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.ArrayNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class ArrayNode : BaseWrapperArrayNode
  {
    public ArrayNode()
    {
      this.IsReadOnly = false;
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Array";
      icon = (Image) Resources.B16x16_Button_Array;
    }

    public override void Initialize()
    {
      this.ChangeInnerNode(IntPtr.Size == 4 ? (BaseNode) new Hex32Node() : (BaseNode) new Hex64Node());
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, "Array");
    }

    protected override Size DrawChild(DrawContext context, int x, int y)
    {
      DrawContext context1 = context.Clone();
      context1.Address = context.Address + this.Offset + this.InnerNode.MemorySize * this.CurrentIndex;
      context1.Memory = context.Memory.Clone();
      context1.Memory.Offset += this.Offset + this.InnerNode.MemorySize * this.CurrentIndex;
      return this.InnerNode.Draw(context1, x, y);
    }
  }
}
