// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Vector2Node
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class Vector2Node : BaseMatrixNode
  {
    public override int ValueTypeSize
    {
      get
      {
        return 4;
      }
    }

    public override int MemorySize
    {
      get
      {
        return 2 * this.ValueTypeSize;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Vector2";
      icon = (Image) Resources.B16x16_Button_Vector_2;
    }

    public override Size Draw(DrawContext context, int x2, int y2)
    {
      return this.DrawVectorType(context, x2, y2, "Vector2", 2);
    }

    protected override int CalculateValuesHeight(DrawContext context)
    {
      return 0;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      this.Update(spot, 2);
    }
  }
}
