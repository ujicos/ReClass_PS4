// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseTextPtrNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.UI;
using System;
using System.Drawing;
using System.Text;

namespace ReClassNET.Nodes
{
  public abstract class BaseTextPtrNode : BaseNode
  {
    private const int MaxStringCharacterCount = 256;

    public override int MemorySize
    {
      get
      {
        return IntPtr.Size;
      }
    }

    public abstract Encoding Encoding { get; }

    public Size DrawText(DrawContext context, int x, int y, string type)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      IntPtr address = context.Memory.ReadIntPtr(this.Offset);
      string text = context.Process.ReadRemoteString(address, this.Encoding, 256);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Text, -1, HotSpotType.None);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, "= '");
      x = this.AddText(context, x, y, context.Settings.TextColor, 999, text);
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, "'") + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      return new Size(x - num, context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      return !this.IsHidden || this.IsWrapped ? context.Font.Height : BaseNode.HiddenHeight;
    }
  }
}
