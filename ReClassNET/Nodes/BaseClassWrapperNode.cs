// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseClassWrapperNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.Nodes
{
  public abstract class BaseClassWrapperNode : BaseWrapperNode
  {
    public override void Initialize()
    {
      ClassNode classNode = ClassNode.Create();
      classNode.Initialize();
      this.ChangeInnerNode((BaseNode) classNode);
    }

    public override bool CanChangeInnerNodeTo(BaseNode node)
    {
      return node is ClassNode;
    }
  }
}
