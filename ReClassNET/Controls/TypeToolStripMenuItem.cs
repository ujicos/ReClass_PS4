// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.TypeToolStripMenuItem
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.Controls
{
  [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
  public class TypeToolStripMenuItem : ToolStripMenuItem
  {
    public Type Value { get; set; }
  }
}
