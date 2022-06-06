// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.Legacy.ClassPointerArrayNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
  public class ClassPointerArrayNode : BaseClassArrayNode
  {
    protected override bool PerformCycleCheck
    {
      get
      {
        return false;
      }
    }

    public override BaseNode GetEquivalentNode(int count, ClassNode classNode)
    {
      ClassInstanceNode classInstanceNode = new ClassInstanceNode();
      classInstanceNode.ChangeInnerNode((BaseNode) classNode);
      PointerNode pointerNode = new PointerNode();
      pointerNode.ChangeInnerNode((BaseNode) classInstanceNode);
      ArrayNode arrayNode = new ArrayNode();
      arrayNode.Count = count;
      arrayNode.ChangeInnerNode((BaseNode) pointerNode);
      arrayNode.CopyFromNode((BaseNode) this);
      return (BaseNode) arrayNode;
    }
  }
}
