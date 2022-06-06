// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.HotSpotTextBoxCommitEventArgs
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.UI;
using System;

namespace ReClassNET.Controls
{
  public class HotSpotTextBoxCommitEventArgs : EventArgs
  {
    public HotSpot HotSpot { get; set; }

    public HotSpotTextBoxCommitEventArgs(HotSpot hotSpot)
    {
      this.HotSpot = hotSpot;
    }
  }
}
