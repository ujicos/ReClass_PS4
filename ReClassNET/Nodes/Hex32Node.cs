// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Hex32Node
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class Hex32Node : BaseHexCommentNode
  {
    public override int MemorySize
    {
      get
      {
        return 4;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Hex32";
      icon = (Image) Resources.B16x16_Button_Hex_32;
    }

    public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
    {
      UInt32FloatData uint32FloatData = Hex32Node.ReadFromBuffer(spot.Memory, this.Offset);
      address = uint32FloatData.IntPtr;
      return spot.Process?.GetNamedAddress(uint32FloatData.IntPtr) != null;
    }

    public override string GetToolTipText(HotSpot spot)
    {
      UInt32FloatData uint32FloatData = Hex32Node.ReadFromBuffer(spot.Memory, this.Offset);
      return string.Format("Int32: {0}\nUInt32: 0x{1:X08}\nFloat: {2:0.000}", (object) uint32FloatData.IntValue, (object) uint32FloatData.UIntValue, (object) uint32FloatData.FloatValue);
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, context.Settings.ShowNodeText ? context.Memory.ReadString(context.Settings.RawDataEncoding, this.Offset, 4) + "     " : (string) null, 4);
    }

    public override void Update(HotSpot spot)
    {
      this.Update(spot, 4);
    }

    protected override int AddComment(DrawContext context, int x, int y)
    {
      x = base.AddComment(context, x, y);
      UInt32FloatData uint32FloatData = Hex32Node.ReadFromBuffer(context.Memory, this.Offset);
      x = this.AddComment(context, x, y, uint32FloatData.FloatValue, uint32FloatData.IntPtr, uint32FloatData.UIntPtr);
      return x;
    }

    private static UInt32FloatData ReadFromBuffer(MemoryBuffer memory, int offset)
    {
      return new UInt32FloatData()
      {
        Raw = memory.ReadInt32(offset)
      };
    }
  }
}
