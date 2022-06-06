// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.DrawContextRequestEventArgs
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;
using System;

namespace ReClassNET.Controls
{
  public class DrawContextRequestEventArgs : EventArgs
  {
    public DateTime CurrentTime { get; set; } = DateTime.UtcNow;

    public Settings Settings { get; set; }

    public IconProvider IconProvider { get; set; }

    public RemoteProcess Process { get; set; }

    public MemoryBuffer Memory { get; set; }

    public BaseNode Node { get; set; }

    public IntPtr BaseAddress { get; set; }
  }
}
