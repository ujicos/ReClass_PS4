// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.NIntNode
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
  public class NIntNode : BaseNumericNode
  {
    public override int MemorySize
    {
      get
      {
        return IntPtr.Size;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "NInt";
      icon = (Image) Resources.B16x16_Button_NInt;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      long int64 = this.ReadValueFromMemory(context.Memory).ToInt64();
      return this.DrawNumeric(context, x, y, context.IconProvider.Signed, "NInt", int64.ToString(), string.Format("0x{0:X}", (object) int64));
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      long result;
      string s;
      if (spot.Id != 0 && spot.Id != 1 || !long.TryParse(spot.Text, out result) && (!spot.Text.TryGetHexString(out s) || !long.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result)))
        return;
      spot.Process.WriteRemoteMemory(spot.Address, result);
    }

    public IntPtr ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadIntPtr(this.Offset);
    }
  }
}
