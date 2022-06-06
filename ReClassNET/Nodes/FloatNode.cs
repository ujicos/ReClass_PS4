// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.FloatNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class FloatNode : BaseNumericNode
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
      name = "Float";
      icon = (Image) Resources.B16x16_Button_Float;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.DrawNumeric(context, x, y, context.IconProvider.Float, "Float", this.ReadValueFromMemory(context.Memory).ToString("0.000"), (string) null);
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      float result;
      if (spot.Id != 0 || !float.TryParse(spot.Text, out result))
        return;
      spot.Process.WriteRemoteMemory(spot.Address, result);
    }

    public float ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadFloat(this.Offset);
    }
  }
}
