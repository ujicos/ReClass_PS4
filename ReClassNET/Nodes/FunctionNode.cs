// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.FunctionNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Drawing;
using System.Linq;

namespace ReClassNET.Nodes
{
  public class FunctionNode : BaseFunctionNode
  {
    private int memorySize = IntPtr.Size;

    public string Signature { get; set; } = "void function()";

    public ClassNode BelongsToClass { get; set; }

    public override int MemorySize
    {
      get
      {
        return this.memorySize;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Function";
      icon = (Image) Resources.B16x16_Button_Function;
    }

    public override string GetToolTipText(HotSpot spot)
    {
      this.DisassembleRemoteCode(spot.Process, spot.Address);
      return string.Join("\n", this.Instructions.Select<BaseFunctionNode.FunctionNodeInstruction, string>((Func<BaseFunctionNode.FunctionNodeInstruction, string>) (i => i.Instruction)));
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Function, -1, HotSpotType.None);
      int num2 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Function") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddOpenCloseIcon(context, x, y) + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      Size size1 = new Size(x - num1, context.Font.Height);
      IntPtr address = context.Address + this.Offset;
      this.DisassembleRemoteCode(context.Process, address);
      if (this.LevelsOpen[context.Level])
      {
        y += context.Font.Height;
        x = this.AddText(context, num2, y, context.Settings.TypeColor, -1, "Signature:") + context.Font.Width;
        x = this.AddText(context, x, y, context.Settings.ValueColor, 0, this.Signature);
        size1.Width = Math.Max(size1.Width, x - num1);
        size1.Height += context.Font.Height;
        y += context.Font.Height;
        x = this.AddText(context, num2, y, context.Settings.TextColor, -1, "Belongs to: ");
        x = this.AddText(context, x, y, context.Settings.ValueColor, -1, this.BelongsToClass == null ? "<None>" : "<" + this.BelongsToClass.Name + ">") + context.Font.Width;
        x = this.AddIcon(context, x, y, context.IconProvider.Change, 1, HotSpotType.ChangeClassType);
        size1.Width = Math.Max(size1.Width, x - num1);
        size1.Height += context.Font.Height;
        Size size2 = this.DrawInstructions(context, num2, y + 4);
        size1.Width = Math.Max(size1.Width, size2.Width + num2 - num1);
        size1.Height += size2.Height + 4;
      }
      return size1;
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.Instructions.Count * context.Font.Height;
      return height;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      if (spot.Id != 0)
        return;
      this.Signature = spot.Text;
    }

    private void DisassembleRemoteCode(RemoteProcess process, IntPtr address)
    {
      if (!(this.Address != address))
        return;
      this.Instructions.Clear();
      this.Address = address;
      if (!address.IsNull() && process.IsValid)
        this.DisassembleRemoteCode(process, address, out this.memorySize);
      this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
    }
  }
}
