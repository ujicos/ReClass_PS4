// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.HotSpot
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using ReClassNET.Nodes;
using System;
using System.Drawing;

namespace ReClassNET.UI
{
  public class HotSpot
  {
    public const int NoneId = -1;
    public const int AddressId = 100;
    public const int NameId = 101;
    public const int CommentId = 102;
    public const int ReadOnlyId = 999;

    public int Id { get; set; }

    public HotSpotType Type { get; set; }

    public int Level { get; set; }

    public string Text { get; set; }

    public BaseNode Node { get; set; }

    public Rectangle Rect { get; set; }

    public IntPtr Address { get; set; }

    public RemoteProcess Process { get; set; }

    public MemoryBuffer Memory { get; set; }
  }
}
