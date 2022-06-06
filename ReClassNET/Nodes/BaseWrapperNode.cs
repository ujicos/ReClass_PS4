// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseWrapperNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Nodes
{
  public abstract class BaseWrapperNode : BaseNode
  {
    public BaseNode InnerNode { get; private set; }

    public event NodeEventHandler InnerNodeChanged;

    protected abstract bool PerformCycleCheck { get; }

    public abstract bool CanChangeInnerNodeTo(BaseNode node);

    public void ChangeInnerNode(BaseNode node)
    {
      if (!this.CanChangeInnerNodeTo(node))
        throw new InvalidOperationException("Can't change inner node to '" + (node?.GetType().ToString() ?? "null") + "'");
      if (this.InnerNode == node)
        return;
      this.InnerNode = node;
      if (node != null)
        node.ParentNode = (BaseNode) this;
      NodeEventHandler innerNodeChanged = this.InnerNodeChanged;
      if (innerNodeChanged != null)
        innerNodeChanged((BaseNode) this);
      this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
    }

    public BaseNode ResolveMostInnerNode()
    {
      if (this.InnerNode == null)
        return (BaseNode) null;
      return this.InnerNode is BaseWrapperNode innerNode ? innerNode.ResolveMostInnerNode() : this.InnerNode;
    }

    public bool ShouldPerformCycleCheckForInnerNode()
    {
      if (!this.PerformCycleCheck)
        return false;
      for (BaseWrapperNode baseWrapperNode = this; baseWrapperNode.InnerNode is BaseWrapperNode innerNode; baseWrapperNode = innerNode)
      {
        if (!innerNode.PerformCycleCheck)
          return false;
      }
      return true;
    }

    public bool IsNodePresentInChain<TNode>() where TNode : BaseNode
    {
      for (BaseNode baseNode = (BaseNode) this; baseNode is BaseWrapperNode baseWrapperNode; baseNode = baseWrapperNode.InnerNode)
      {
        if (baseNode is TNode)
          return true;
      }
      return false;
    }
  }
}
