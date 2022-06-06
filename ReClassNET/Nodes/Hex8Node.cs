// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Hex8Node
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class Hex8Node : BaseHexNode
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
      name = "Hex8";
      icon = (Image) Resources.B16x16_Button_Hex_8;
    }

    public override string GetToolTipText(HotSpot spot)
    {
      byte num = spot.Memory.ReadUInt8(this.Offset);
      return string.Format("Int8: {0}\nUInt8: 0x{1:X02}", (object) (int) num, (object) num);
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, context.Settings.ShowNodeText ? context.Memory.ReadString(context.Settings.RawDataEncoding, this.Offset, 1) + "        " : (string) null, 1);
    }

    public override void Update(HotSpot spot)
    {
      this.Update(spot, 1);
    }
  }
}
