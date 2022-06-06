// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BoolNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class BoolNode : BaseNumericNode
  {
    public override int MemorySize
    {
      get
      {
        return 1;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Bool";
      icon = (Image) Resources.B16x16_Button_Bool;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIconPadding(context, x);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Bool") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.NameColor, -1, "=") + context.Font.Width;
      byte num2 = context.Memory.ReadUInt8(this.Offset);
      x = this.AddText(context, x, y, context.Settings.ValueColor, 0, num2 == (byte) 0 ? "false" : "true") + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      return new Size(x - num1, context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      return !this.IsHidden || this.IsWrapped ? context.Font.Height : BaseNode.HiddenHeight;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      bool result;
      if (spot.Id != 0 || !bool.TryParse(spot.Text, out result))
        return;
      spot.Process.WriteRemoteMemory(spot.Address, result ? (byte) 1 : (byte) 0);
    }
  }
}
