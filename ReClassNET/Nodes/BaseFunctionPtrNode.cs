// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseFunctionPtrNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;
using System;
using System.Drawing;
using System.Linq;

namespace ReClassNET.Nodes
{
  public abstract class BaseFunctionPtrNode : BaseFunctionNode
  {
    public override int MemorySize
    {
      get
      {
        return IntPtr.Size;
      }
    }

    public override string GetToolTipText(HotSpot spot)
    {
      IntPtr address = spot.Memory.ReadIntPtr(this.Offset);
      this.DisassembleRemoteCode(spot.Process, address);
      return string.Join("\n", this.Instructions.Select<BaseFunctionNode.FunctionNodeInstruction, string>((Func<BaseFunctionNode.FunctionNodeInstruction, string>) (i => i.Instruction)));
    }

    protected Size Draw(DrawContext context, int x, int y, string type, string name)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Function, -1, HotSpotType.None);
      int tx = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, name) + context.Font.Width;
      x = this.AddOpenCloseIcon(context, x, y) + context.Font.Width;
      x = this.AddComment(context, x, y);
      if (context.Settings.ShowCommentSymbol)
      {
        IntPtr address = context.Memory.ReadIntPtr(this.Offset);
        Module moduleToPointer = context.Process.GetModuleToPointer(address);
        if (moduleToPointer != null)
        {
          string symbolString = context.Process.Symbols.GetSymbolsForModule(moduleToPointer)?.GetSymbolString(address, moduleToPointer);
          if (!string.IsNullOrEmpty(symbolString))
            x = this.AddText(context, x, y, context.Settings.OffsetColor, 999, symbolString);
        }
      }
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      Size size1 = new Size(x - num, context.Font.Height);
      if (this.LevelsOpen[context.Level])
      {
        IntPtr address = context.Memory.ReadIntPtr(this.Offset);
        this.DisassembleRemoteCode(context.Process, address);
        Size size2 = this.DrawInstructions(context, tx, y);
        size1.Width = Math.Max(size1.Width, size2.Width + tx - num);
        size1.Height += size2.Height;
      }
      return size1;
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.Instructions.Count * context.Font.Height;
      return height;
    }

    private void DisassembleRemoteCode(RemoteProcess process, IntPtr address)
    {
      if (!(this.Address != address))
        return;
      this.Instructions.Clear();
      this.Address = address;
      if (address.IsNull() || !process.IsValid)
        return;
      this.DisassembleRemoteCode(process, address, out int _);
    }
  }
}
