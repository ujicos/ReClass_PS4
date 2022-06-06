// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.CustomToolStripProfessionalRenderer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Windows.Forms;

namespace ReClassNET.UI
{
  internal class CustomToolStripProfessionalRenderer : ToolStripProfessionalRenderer
  {
    private readonly bool renderGrip;
    private readonly bool renderBorder;

    public CustomToolStripProfessionalRenderer(bool renderGrip, bool renderBorder)
      : base((ProfessionalColorTable) new CustomProfessionalColorTable())
    {
      this.renderGrip = renderGrip;
      this.renderBorder = renderBorder;
    }

    protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
    {
      if (!this.renderGrip)
        return;
      base.OnRenderGrip(e);
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
      if (!this.renderBorder)
        return;
      base.OnRenderToolStripBorder(e);
    }

    protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
    {
    }
  }
}
