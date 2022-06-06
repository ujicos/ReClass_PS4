// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.FunctionPtrNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class FunctionPtrNode : BaseFunctionPtrNode
  {
    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Function Pointer";
      icon = (Image) Resources.B16x16_Button_Function_Pointer;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.Draw(context, x, y, "FunctionPtr", this.Name);
    }
  }
}
