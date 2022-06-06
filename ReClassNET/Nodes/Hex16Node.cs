// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Hex16Node
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class Hex16Node : BaseHexNode
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
      name = "Hex16";
      icon = (Image) Resources.B16x16_Button_Hex_16;
    }

    public override string GetToolTipText(HotSpot spot)
    {
      UInt16Data uint16Data = new UInt16Data()
      {
        ShortValue = spot.Memory.ReadInt16(this.Offset)
      };
      return string.Format("Int16: {0}\nUInt16: 0x{1:X04}", (object) uint16Data.ShortValue, (object) uint16Data.UShortValue);
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, context.Settings.ShowNodeText ? context.Memory.ReadString(context.Settings.RawDataEncoding, this.Offset, 2) + "       " : (string) null, 2);
    }

    public override void Update(HotSpot spot)
    {
      this.Update(spot, 2);
    }
  }
}
