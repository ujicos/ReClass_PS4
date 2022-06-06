// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseNumericNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public abstract class BaseNumericNode : BaseNode
  {
    protected Size DrawNumeric(
      DrawContext context,
      int x,
      int y,
      Image icon,
      string type,
      string value,
      string alternativeValue)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, icon, -1, HotSpotType.None);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.NameColor, -1, "=") + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, 0, value) + context.Font.Width;
      if (alternativeValue != null)
        x = this.AddText(context, x, y, context.Settings.ValueColor, 1, alternativeValue) + context.Font.Width;
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
