// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.ClassUtil
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Nodes
{
  public class ClassUtil
  {
    public static bool IsCyclicIfClassIsAccessibleFromParent(
      ClassNode parent,
      ClassNode classToCheck,
      IEnumerable<ClassNode> classes)
    {
      DirectedGraph<ClassNode> directedGraph = new DirectedGraph<ClassNode>();
      directedGraph.AddVertices(classes);
      directedGraph.AddEdge(parent, classToCheck);
      foreach (ClassNode vertex in directedGraph.Vertices)
      {
        foreach (BaseWrapperNode baseWrapperNode in vertex.Nodes.OfType<BaseWrapperNode>())
        {
          if (baseWrapperNode.ShouldPerformCycleCheckForInnerNode() && baseWrapperNode.ResolveMostInnerNode() is ClassNode to)
            directedGraph.AddEdge(vertex, to);
        }
      }
      return directedGraph.ContainsCycle();
    }
  }
}
