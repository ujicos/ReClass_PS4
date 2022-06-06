// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.Legacy.BaseClassArrayNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Nodes;
using System;
using System.Drawing;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
  public abstract class BaseClassArrayNode : BaseWrapperNode
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

    public override bool CanChangeInnerNodeTo(BaseNode node)
    {
      return node is ClassNode;
    }

    public abstract BaseNode GetEquivalentNode(int count, ClassNode classNode);
  }
}
