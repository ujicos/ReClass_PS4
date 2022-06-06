// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Hex64Node
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
  public class Hex64Node : BaseHexCommentNode
  {
    public override int MemorySize
    {
      get
      {
        return 8;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Hex64";
      icon = (Image) Resources.B16x16_Button_Hex_64;
    }

    public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
    {
      UInt64FloatDoubleData uint64FloatDoubleData = Hex64Node.ReadFromBuffer(spot.Memory, this.Offset);
      address = uint64FloatDoubleData.IntPtr;
      return spot.Process.GetSectionToPointer(uint64FloatDoubleData.IntPtr) != null;
    }

    public override string GetToolTipText(HotSpot spot)
    {
      UInt64FloatDoubleData uint64FloatDoubleData = Hex64Node.ReadFromBuffer(spot.Memory, this.Offset);
      return string.Format("Int64: {0}\nUInt64: 0x{1:X016}\nFloat: {2:0.000}\nDouble: {3:0.000}", (object) uint64FloatDoubleData.LongValue, (object) uint64FloatDoubleData.ULongValue, (object) uint64FloatDoubleData.FloatValue, (object) uint64FloatDoubleData.DoubleValue);
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, context.Settings.ShowNodeText ? context.Memory.ReadString(context.Settings.RawDataEncoding, this.Offset, 8) + " " : (string) null, 8);
    }

    public override void Update(HotSpot spot)
    {
      this.Update(spot, 8);
    }

    protected override int AddComment(DrawContext context, int x, int y)
    {
      x = base.AddComment(context, x, y);
      UInt64FloatDoubleData uint64FloatDoubleData = Hex64Node.ReadFromBuffer(context.Memory, this.Offset);
      x = this.AddComment(context, x, y, uint64FloatDoubleData.FloatValue, uint64FloatDoubleData.IntPtr, uint64FloatDoubleData.UIntPtr);
      return x;
    }

    private static UInt64FloatDoubleData ReadFromBuffer(
      MemoryBuffer memory,
      int offset)
    {
      return new UInt64FloatDoubleData()
      {
        Raw1 = memory.ReadInt32(offset),
        Raw2 = memory.ReadInt32(offset + 4)
      };
    }
  }
}
