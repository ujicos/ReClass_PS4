// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseNodeContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  internal abstract class BaseNodeContract : BaseNode
  {
    public override int MemorySize
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      throw new NotImplementedException();
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      throw new NotImplementedException();
    }
  }
}
