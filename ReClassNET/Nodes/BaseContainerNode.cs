// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseContainerNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;

namespace ReClassNET.Nodes
{
  public abstract class BaseContainerNode : BaseNode
  {
    private readonly List<BaseNode> nodes = new List<BaseNode>();
    private int updateCount;

    public IReadOnlyList<BaseNode> Nodes
    {
      get
      {
        return (IReadOnlyList<BaseNode>) this.nodes;
      }
    }

    protected abstract bool ShouldCompensateSizeChanges { get; }

    public abstract bool CanHandleChildNode(BaseNode node);

    private void CheckCanHandleChildNode(BaseNode node)
    {
      if (!this.CanHandleChildNode(node))
        throw new ArgumentException();
    }

    public override void ClearSelection()
    {
      base.ClearSelection();
      foreach (BaseNode node in (IEnumerable<BaseNode>) this.Nodes)
        node.ClearSelection();
    }

    public virtual void UpdateOffsets()
    {
      int num = 0;
      foreach (BaseNode node in (IEnumerable<BaseNode>) this.Nodes)
      {
        node.Offset = num;
        num += node.MemorySize;
      }
    }

    public int FindNodeIndex(BaseNode node)
    {
      return this.nodes.FindIndex((Predicate<BaseNode>) (n => n == node));
    }

    public bool ContainsNode(BaseNode node)
    {
      return this.FindNodeIndex(node) != -1;
    }

    public bool TryGetPredecessor(BaseNode node, out BaseNode predecessor)
    {
      return this.TryGetNeighbour(node, -1, out predecessor);
    }

    public bool TryGetSuccessor(BaseNode node, out BaseNode successor)
    {
      return this.TryGetNeighbour(node, 1, out successor);
    }

    private bool TryGetNeighbour(BaseNode node, int offset, out BaseNode neighbour)
    {
      neighbour = (BaseNode) null;
      int nodeIndex = this.FindNodeIndex(node);
      if (nodeIndex == -1)
        return false;
      int index = nodeIndex + offset;
      if (index < 0 || index >= this.nodes.Count)
        return false;
      neighbour = this.nodes[index];
      return true;
    }

    public void BeginUpdate()
    {
      ++this.updateCount;
    }

    public void EndUpdate()
    {
      this.updateCount = Math.Max(0, this.updateCount - 1);
      this.OnNodesUpdated();
    }

    private void OnNodesUpdated()
    {
      if (this.updateCount != 0)
        return;
      this.UpdateOffsets();
      this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
    }

    public void ReplaceChildNode(BaseNode oldNode, BaseNode newNode)
    {
      List<BaseNode> additionalCreatedNodes = (List<BaseNode>) null;
      this.ReplaceChildNode(oldNode, newNode, ref additionalCreatedNodes);
    }

    public void ReplaceChildNode(
      BaseNode oldNode,
      BaseNode newNode,
      ref List<BaseNode> additionalCreatedNodes)
    {
      this.CheckCanHandleChildNode(newNode);
      int nodeIndex = this.FindNodeIndex(oldNode);
      if (nodeIndex == -1)
        throw new ArgumentException(string.Format("Node {0} is not a child of {1}.", (object) oldNode, (object) this));
      newNode.CopyFromNode(oldNode);
      newNode.ParentNode = (BaseNode) this;
      this.nodes[nodeIndex] = newNode;
      if (this.ShouldCompensateSizeChanges)
      {
        int memorySize1 = oldNode.MemorySize;
        int memorySize2 = newNode.MemorySize;
        if (memorySize2 < memorySize1)
          this.InsertBytes(nodeIndex + 1, memorySize1 - memorySize2, ref additionalCreatedNodes);
      }
      this.OnNodesUpdated();
    }

    protected virtual BaseNode CreateDefaultNodeForSize(int size)
    {
      if (size >= 8)
        return (BaseNode) new Hex64Node();
      if (size >= 4)
        return (BaseNode) new Hex32Node();
      return size >= 2 ? (BaseNode) new Hex16Node() : (BaseNode) new Hex8Node();
    }

    public void AddBytes(int size)
    {
      List<BaseNode> createdNodes = (List<BaseNode>) null;
      this.InsertBytes(this.nodes.Count, size, ref createdNodes);
    }

    public void InsertBytes(BaseNode position, int size)
    {
      List<BaseNode> createdNodes = (List<BaseNode>) null;
      this.InsertBytes(this.FindNodeIndex(position), size, ref createdNodes);
    }

    protected void InsertBytes(int index, int size, ref List<BaseNode> createdNodes)
    {
      if (index < 0 || index > this.nodes.Count)
        throw new ArgumentOutOfRangeException(string.Format("The index {0} is not in the range [0, {1}].", (object) index, (object) this.nodes.Count));
      if (size == 0)
        return;
      while (size > 0)
      {
        BaseNode defaultNodeForSize = this.CreateDefaultNodeForSize(size);
        if (defaultNodeForSize != null)
        {
          defaultNodeForSize.ParentNode = (BaseNode) this;
          this.nodes.Insert(index, defaultNodeForSize);
          createdNodes?.Add(defaultNodeForSize);
          size -= defaultNodeForSize.MemorySize;
          ++index;
        }
        else
          break;
      }
      this.OnNodesUpdated();
    }

    public void AddNodes(IEnumerable<BaseNode> nodes)
    {
      foreach (BaseNode node in nodes)
        this.AddNode(node);
    }

    public void AddNode(BaseNode node)
    {
      this.CheckCanHandleChildNode(node);
      node.ParentNode = (BaseNode) this;
      this.nodes.Add(node);
      this.OnNodesUpdated();
    }

    public void InsertNode(BaseNode position, BaseNode node)
    {
      this.CheckCanHandleChildNode(node);
      int nodeIndex = this.FindNodeIndex(position);
      if (nodeIndex == -1)
        throw new ArgumentException();
      node.ParentNode = (BaseNode) this;
      this.nodes.Insert(nodeIndex, node);
      this.OnNodesUpdated();
    }

    public bool RemoveNode(BaseNode node)
    {
      int num = this.nodes.Remove(node) ? 1 : 0;
      if (num == 0)
        return num != 0;
      this.OnNodesUpdated();
      return num != 0;
    }

    protected internal virtual void ChildHasChanged(BaseNode child)
    {
    }
  }
}
