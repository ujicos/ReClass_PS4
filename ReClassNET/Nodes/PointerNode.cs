// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.PointerNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class PointerNode : BaseWrapperNode
  {
    private readonly MemoryBuffer memory = new MemoryBuffer();

    public override int MemorySize
    {
      get
      {
        return IntPtr.Size;
      }
    }

    protected override bool PerformCycleCheck
    {
      get
      {
        return false;
      }
    }

    public PointerNode()
    {
      this.LevelsOpen.DefaultValue = true;
    }

    public override void Initialize()
    {
      ClassInstanceNode classInstanceNode = new ClassInstanceNode();
      classInstanceNode.Initialize();
      ((BaseContainerNode) classInstanceNode.InnerNode).AddBytes(16 * IntPtr.Size);
      this.ChangeInnerNode((BaseNode) classInstanceNode);
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Pointer";
      icon = (Image) Resources.B16x16_Button_Pointer;
    }

    public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
    {
      address = spot.Memory.ReadIntPtr(this.Offset);
      return spot.Process.GetNamedAddress(address) != null;
    }

    public override bool CanChangeInnerNodeTo(BaseNode node)
    {
      bool flag;
      switch (node)
      {
        case ClassNode _:
          flag = false;
          break;
        case VirtualMethodNode _:
          flag = false;
          break;
        default:
          flag = true;
          break;
      }
      return flag;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.InnerNode == null ? this.AddIconPadding(context, x) : this.AddOpenCloseIcon(context, x, y);
      x = this.AddIcon(context, x, y, context.IconProvider.Pointer, -1, HotSpotType.None);
      int x1 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Ptr") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      if (this.InnerNode == null)
        x = this.AddText(context, x, y, context.Settings.ValueColor, -1, "<void>") + context.Font.Width;
      x = this.AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeWrappedType) + context.Font.Width;
      IntPtr address = context.Memory.ReadIntPtr(this.Offset);
      x = this.AddText(context, x, y, context.Settings.OffsetColor, -1, "->") + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, 0, "0x" + address.ToString("X016")) + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      y += context.Font.Height;
      Size size1 = new Size(x - num1, y - num2);
      if (this.LevelsOpen[context.Level] && this.InnerNode != null)
      {
        this.memory.Size = this.InnerNode.MemorySize;
        this.memory.UpdateFrom((IRemoteMemoryReader) context.Process, address, MainForm.PS4PID);
        DrawContext context1 = context.Clone();
        context1.Address = address;
        context1.Memory = this.memory;
        Size size2 = this.InnerNode.Draw(context1, x1, y);
        size1.Width = Math.Max(size1.Width, size2.Width + x1 - num1);
        size1.Height += size2.Height;
      }
      return size1;
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level] && this.InnerNode != null)
        height += this.InnerNode.CalculateDrawnHeight(context);
      return height;
    }
  }
}
