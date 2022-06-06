// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.DrawContext
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ReClassNET.Controls
{
  public class DrawContext
  {
    public Settings Settings { get; set; }

    public Graphics Graphics { get; set; }

    public FontEx Font { get; set; }

    public IconProvider IconProvider { get; set; }

    public RemoteProcess Process { get; set; }

    public MemoryBuffer Memory { get; set; }

    public DateTime CurrentTime { get; set; }

    public Rectangle ClientArea { get; set; }

    public List<HotSpot> HotSpots { get; set; }

    public IntPtr Address { get; set; }

    public int Level { get; set; }

    public bool MultipleNodesSelected { get; set; }

    public DrawContext Clone()
    {
      return new DrawContext()
      {
        Settings = this.Settings,
        Graphics = this.Graphics,
        Font = this.Font,
        IconProvider = this.IconProvider,
        Process = this.Process,
        Memory = this.Memory,
        CurrentTime = this.CurrentTime,
        ClientArea = this.ClientArea,
        HotSpots = this.HotSpots,
        Address = this.Address,
        Level = this.Level,
        MultipleNodesSelected = this.MultipleNodesSelected
      };
    }
  }
}
