// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Utf8TextNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using System.Drawing;
using System.Text;

namespace ReClassNET.Nodes
{
  public class Utf8TextNode : BaseTextNode
  {
    public override Encoding Encoding
    {
      get
      {
        return Encoding.UTF8;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "UTF8 / ASCII Text";
      icon = (Image) Resources.B16x16_Button_Text;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.DrawText(context, x, y, "Text8");
    }
  }
}
