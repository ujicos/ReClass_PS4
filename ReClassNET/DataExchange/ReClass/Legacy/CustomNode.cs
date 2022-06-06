// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.Legacy.CustomNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
  public class CustomNode : BaseNode
  {
    public override int MemorySize
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      throw new NotImplementedException();
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      throw new NotImplementedException();
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<BaseNode> GetEquivalentNodes(int size)
    {
      CustomNode customNode = this;
      while (size != 0)
      {
        BaseNode baseNode = size < 8 ? (size < 4 ? (size < 2 ? (BaseNode) new Hex8Node() : (BaseNode) new Hex16Node()) : (BaseNode) new Hex32Node()) : (BaseNode) new Hex64Node();
        baseNode.Comment = customNode.Comment;
        size -= baseNode.MemorySize;
        yield return baseNode;
      }
    }
  }
}
