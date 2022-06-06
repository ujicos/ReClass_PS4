// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Matrix4x4Node
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using ReClassNET.UI;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class Matrix4x4Node : BaseMatrixNode
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
        return 16 * this.ValueTypeSize;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Matrix 4x4";
      icon = (Image) Resources.B16x16_Button_Matrix_4x4;
    }

    public override Size Draw(DrawContext context, int x2, int y2)
    {
      return this.DrawMatrixType(context, x2, y2, "Matrix (4x4)", 4, 4);
    }

    protected override int CalculateValuesHeight(DrawContext context)
    {
      return 4 * context.Font.Height;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      this.Update(spot, 16);
    }
  }
}
