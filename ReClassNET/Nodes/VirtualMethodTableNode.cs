// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.VirtualMethodTableNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ReClassNET.Nodes
{
  public class VirtualMethodTableNode : BaseContainerNode
  {
    private readonly MemoryBuffer memory = new MemoryBuffer();

    public override int MemorySize
    {
      get
      {
        return IntPtr.Size;
      }
    }

    protected override bool ShouldCompensateSizeChanges
    {
      get
      {
        return false;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "VTable Pointer";
      icon = (Image) Resources.B16x16_Button_VTable;
    }

    public override bool CanHandleChildNode(BaseNode node)
    {
      return node is VirtualMethodNode;
    }

    public override void Initialize()
    {
      for (int index = 0; index < 10; ++index)
        this.AddNode(this.CreateDefaultNodeForSize(IntPtr.Size));
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddOpenCloseIcon(context, x, y);
      x = this.AddIcon(context, x, y, context.IconProvider.VirtualTable, -1, HotSpotType.None);
      int x1 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.VTableColor, -1, string.Format("VTable[{0}]", (object) this.Nodes.Count)) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      y += context.Font.Height;
      Size size1 = new Size(x - num1, y - num2);
      if (this.LevelsOpen[context.Level])
      {
        IntPtr address = context.Memory.ReadIntPtr(this.Offset);
        this.memory.Size = this.Nodes.Count * IntPtr.Size;
        this.memory.UpdateFrom((IRemoteMemoryReader) context.Process, address, MainForm.PS4PID);
        DrawContext context1 = context.Clone();
        context1.Address = address;
        context1.Memory = this.memory;
        foreach (BaseNode node in (IEnumerable<BaseNode>) this.Nodes)
        {
          Size size2 = node.Draw(context1, x1, y);
          size1.Width = Math.Max(size1.Width, size2.Width + x1 - num1);
          size1.Height += size2.Height;
          y += size2.Height;
        }
      }
      return size1;
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.Nodes.Sum<BaseNode>((Func<BaseNode, int>) (n => n.CalculateDrawnHeight(context)));
      return height;
    }

    protected override BaseNode CreateDefaultNodeForSize(int size)
    {
      return (BaseNode) new VirtualMethodNode();
    }
  }
}
