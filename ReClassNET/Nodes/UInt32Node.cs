// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.UInt32Node
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
  public class UInt32Node : BaseNumericNode
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
      name = "UInt32 / DWORD";
      icon = (Image) Resources.B16x16_Button_UInt_32;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      uint num = this.ReadValueFromMemory(context.Memory);
      return this.DrawNumeric(context, x, y, context.IconProvider.Unsigned, "UInt32", num.ToString(), string.Format("0x{0:X}", (object) num));
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      uint result;
      string s;
      if (spot.Id != 0 && spot.Id != 1 || !uint.TryParse(spot.Text, out result) && (!spot.Text.TryGetHexString(out s) || !uint.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result)))
        return;
      spot.Process.WriteRemoteMemory(spot.Address, result);
    }

    public uint ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadUInt32(this.Offset);
    }
  }
}
