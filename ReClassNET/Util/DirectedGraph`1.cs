// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.DirectedGraph`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Util
{
  public class DirectedGraph<T>
  {
    private readonly IDictionary<T, HashSet<T>> adjacencyList = (IDictionary<T, HashSet<T>>) new Dictionary<T, HashSet<T>>();

    public IEnumerable<T> Vertices
    {
      get
      {
        return (IEnumerable<T>) this.adjacencyList.Keys;
      }
    }

    public bool AddVertex(T vertex)
    {
      if (this.adjacencyList.ContainsKey(vertex))
        return false;
      this.adjacencyList.Add(vertex, new HashSet<T>());
      return true;
    }

    public void AddVertices(IEnumerable<T> vertices)
    {
      foreach (T vertex in vertices)
        this.AddVertex(vertex);
    }

    public bool ContainsVertex(T vertex)
    {
      return this.adjacencyList.ContainsKey(vertex);
    }

    public bool AddEdge(T from, T to)
    {
      HashSet<T> objSet;
      if (!this.ContainsVertex(to) || !this.adjacencyList.TryGetValue(from, out objSet))
        throw new ArgumentException("Vertex does not exist in graph.");
      return objSet.Add(to);
    }

    public bool ContainsEdge(T from, T to)
    {
      HashSet<T> objSet;
      if (!this.ContainsVertex(to) || !this.adjacencyList.TryGetValue(from, out objSet))
        throw new ArgumentException("Vertex does not exist in graph.");
      return objSet.Contains(to);
    }

    public IEnumerable<T> GetNeighbours(T vertex)
    {
      HashSet<T> objSet;
      if (!this.adjacencyList.TryGetValue(vertex, out objSet))
        throw new ArgumentException("Vertex does not exist in graph.");
      return (IEnumerable<T>) objSet;
    }

    public bool ContainsCycle()
    {
      HashSet<T> visited = new HashSet<T>();
      HashSet<T> recursionStack = new HashSet<T>();
      return this.adjacencyList.Keys.Any<T>(new Func<T, bool>(IsCyclic));

      bool IsCyclic(T source)
      {
        if (visited.Add(source))
        {
          recursionStack.Add(source);
          foreach (T neighbour in this.GetNeighbours(source))
          {
            if (!visited.Contains(neighbour) && IsCyclic(neighbour) || recursionStack.Contains(neighbour))
              return true;
          }
        }
        recursionStack.Remove(source);
        return false;
      }
    }
  }
}
