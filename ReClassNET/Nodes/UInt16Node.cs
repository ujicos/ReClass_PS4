// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.UInt16Node
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
using System.Globalization;

namespace ReClassNET.Nodes
{
  public class UInt16Node : BaseNumericNode
  {
    public override int MemorySize
    {
      get
      {
        return 2;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "UInt16 / WORD";
      icon = (Image) Resources.B16x16_Button_UInt_16;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      ushort num = this.ReadValueFromMemory(context.Memory);
      return this.DrawNumeric(context, x, y, context.IconProvider.Unsigned, "UInt16", num.ToString(), string.Format("0x{0:X}", (object) num));
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      ushort result;
      string s;
      if (spot.Id != 0 && spot.Id != 1 || !ushort.TryParse(spot.Text, out result) && (!spot.Text.TryGetHexString(out s) || !ushort.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result)))
        return;
      spot.Process.WriteRemoteMemory(spot.Address, result);
    }

    public ushort ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadUInt16(this.Offset);
    }
  }
}
