// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.CustomProfessionalColorTable
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.UI
{
  internal class CustomProfessionalColorTable : ProfessionalColorTable
  {
    public override Color MenuStripGradientBegin
    {
      get
      {
        return SystemColors.Control;
      }
    }

    public override Color MenuStripGradientEnd
    {
      get
      {
        return SystemColors.Control;
      }
    }

    public override Color ToolStripGradientBegin
    {
      get
      {
        return SystemColors.Control;
      }
    }

    public override Color ToolStripGradientMiddle
    {
      get
      {
        return SystemColors.Control;
      }
    }

    public override Color ToolStripGradientEnd
    {
      get
      {
        return SystemColors.Control;
      }
    }
  }
}
