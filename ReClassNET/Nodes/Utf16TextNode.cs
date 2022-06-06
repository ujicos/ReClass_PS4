// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.Utf16TextNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using System.Drawing;
using System.Text;

namespace ReClassNET.Nodes
{
  public class Utf16TextNode : BaseTextNode
  {
    public override Encoding Encoding
    {
      get
      {
        return Encoding.Unicode;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "UTF16 / Unicode Text";
      icon = (Image) Resources.B16x16_Button_UText;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      return this.DrawText(context, x, y, "Text16");
    }
  }
}
