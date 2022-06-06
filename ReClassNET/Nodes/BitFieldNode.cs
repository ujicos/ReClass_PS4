// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BitFieldNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class BitFieldNode : BaseNode
  {
    private int size;
    private int bits;

    public int Bits
    {
      get
      {
        return this.bits;
      }
      set
      {
        this.bits = value < 64 ? (value < 32 ? (value < 16 ? 8 : 16) : 32) : 64;
        this.size = this.bits / 8;
      }
    }

    public override int MemorySize
    {
      get
      {
        return this.size;
      }
    }

    public BitFieldNode()
    {
      this.Bits = IntPtr.Size * 8;
      this.LevelsOpen.DefaultValue = true;
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Bitfield";
      icon = (Image) Resources.B16x16_Button_Bits;
    }

    public override void CopyFromNode(BaseNode node)
    {
      base.CopyFromNode(node);
      this.Bits = node.MemorySize * 8;
    }

    public BaseNumericNode GetUnderlayingNode()
    {
      switch (this.Bits)
      {
        case 8:
          return (BaseNumericNode) new UInt8Node();
        case 16:
          return (BaseNumericNode) new UInt16Node();
        case 32:
          return (BaseNumericNode) new UInt32Node();
        case 64:
          return (BaseNumericNode) new UInt64Node();
        default:
          throw new Exception();
      }
    }

    private string ConvertValueToBitString(MemoryBuffer memory)
    {
      switch (this.bits)
      {
        case 16:
          return BitString.ToString(memory.ReadInt16(this.Offset));
        case 32:
          return BitString.ToString(memory.ReadInt32(this.Offset));
        case 64:
          return BitString.ToString(memory.ReadInt64(this.Offset));
        default:
          return BitString.ToString(memory.ReadUInt8(this.Offset));
      }
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIconPadding(context, x);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Bits") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddOpenCloseIcon(context, x, y) + context.Font.Width;
      int num3 = x - 3;
      for (int id = 0; id < this.bits; ++id)
      {
        Rectangle spot = new Rectangle(x + (id + id / 4) * context.Font.Width, y, context.Font.Width, context.Font.Height);
        this.AddHotSpot(context, spot, string.Empty, id, HotSpotType.Edit);
      }
      string bitString = this.ConvertValueToBitString(context.Memory);
      x = this.AddText(context, x, y, context.Settings.ValueColor, -1, bitString) + context.Font.Width;
      x += context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      if (this.LevelsOpen[context.Level])
      {
        y += context.Font.Height;
        StringFormat format = new StringFormat(StringFormatFlags.DirectionVertical);
        using (SolidBrush solidBrush = new SolidBrush(context.Settings.ValueColor))
        {
          int num4 = this.bits + (this.bits / 4 - 1) - 1;
          int num5 = 0;
          int num6 = 0;
          while (num5 < this.bits)
          {
            context.Graphics.DrawString(num5.ToString(), context.Font.Font, (Brush) solidBrush, (float) (num3 + (num4 - num6) * context.Font.Width), (float) y, format);
            num5 += 4;
            num6 += 5;
          }
        }
        y += 8;
      }
      return new Size(x - num1, y - num2 + context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += context.Font.Height + 8;
      return height;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      if (spot.Id < 0 || spot.Id >= this.bits || !(spot.Text == "1") && !(spot.Text == "0"))
        return;
      int num1 = this.bits - 1 - spot.Id;
      int num2 = num1 / 8;
      int num3 = num1 % 8;
      byte num4 = spot.Memory.ReadUInt8(this.Offset + num2);
      byte num5 = !(spot.Text == "1") ? (byte) ((uint) num4 & (uint) (byte) ~(1 << num3)) : (byte) ((uint) num4 | (uint) (byte) (1 << num3));
      spot.Process.WriteRemoteMemory(spot.Address + num2, num5);
    }
  }
}
