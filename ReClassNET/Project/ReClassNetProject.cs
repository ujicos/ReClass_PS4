// Decompiled with JetBrains decompiler
// Type: ReClassNET.Project.ReClassNetProject
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Nodes;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Project
{
  public class ReClassNetProject : IDisposable
  {
    private readonly List<EnumDescription> enums = new List<EnumDescription>();
    private readonly List<ClassNode> classes = new List<ClassNode>();

    public event ReClassNetProject.ClassesChangedEvent ClassAdded;

    public event ReClassNetProject.ClassesChangedEvent ClassRemoved;

    public event ReClassNetProject.EnumsChangedEvent EnumAdded;

    public event ReClassNetProject.EnumsChangedEvent EnumRemoved;

    public IReadOnlyList<EnumDescription> Enums
    {
      get
      {
        return (IReadOnlyList<EnumDescription>) this.enums;
      }
    }

    public IReadOnlyList<ClassNode> Classes
    {
      get
      {
        return (IReadOnlyList<ClassNode>) this.classes;
      }
    }

    public string Path { get; set; }

    public CustomDataMap CustomData { get; } = new CustomDataMap();

    public CppTypeMapping TypeMapping { get; } = new CppTypeMapping();

    public void Dispose()
    {
      this.Clear();
      this.ClassAdded = (ReClassNetProject.ClassesChangedEvent) null;
      this.ClassRemoved = (ReClassNetProject.ClassesChangedEvent) null;
      this.EnumAdded = (ReClassNetProject.EnumsChangedEvent) null;
      this.EnumRemoved = (ReClassNetProject.EnumsChangedEvent) null;
    }

    public void AddClass(ClassNode node)
    {
      this.classes.Add(node);
      node.NodesChanged += new NodeEventHandler(this.NodesChanged_Handler);
      ReClassNetProject.ClassesChangedEvent classAdded = this.ClassAdded;
      if (classAdded == null)
        return;
      classAdded(node);
    }

    public bool ContainsClass(Guid uuid)
    {
      return this.classes.Any<ClassNode>((Func<ClassNode, bool>) (c => c.Uuid.Equals(uuid)));
    }

    public ClassNode GetClassByUuid(Guid uuid)
    {
      return this.classes.First<ClassNode>((Func<ClassNode, bool>) (c => c.Uuid.Equals(uuid)));
    }

    private void NodesChanged_Handler(BaseNode sender)
    {
      this.classes.ForEach((Action<ClassNode>) (c => c.UpdateOffsets()));
    }

    public void Clear()
    {
      List<ClassNode> list1 = this.classes.ToList<ClassNode>();
      this.classes.Clear();
      foreach (ClassNode sender in list1)
      {
        sender.NodesChanged -= new NodeEventHandler(this.NodesChanged_Handler);
        ReClassNetProject.ClassesChangedEvent classRemoved = this.ClassRemoved;
        if (classRemoved != null)
          classRemoved(sender);
      }
      List<EnumDescription> list2 = this.enums.ToList<EnumDescription>();
      this.enums.Clear();
      foreach (EnumDescription sender in list2)
      {
        ReClassNetProject.EnumsChangedEvent enumRemoved = this.EnumRemoved;
        if (enumRemoved != null)
          enumRemoved(sender);
      }
    }

    private IEnumerable<ClassNode> GetClassReferences(ClassNode node)
    {
      return this.classes.Where<ClassNode>((Func<ClassNode, bool>) (c => c != node)).Where<ClassNode>((Func<ClassNode, bool>) (c => c.Nodes.OfType<BaseWrapperNode>().Any<BaseWrapperNode>((Func<BaseWrapperNode, bool>) (w => w.ResolveMostInnerNode() == node))));
    }

    public void Remove(ClassNode node)
    {
      List<ClassNode> list = this.GetClassReferences(node).ToList<ClassNode>();
      if (list.Any<ClassNode>())
        throw new ClassReferencedException(node, (IEnumerable<ClassNode>) list);
      if (!this.classes.Remove(node))
        return;
      node.NodesChanged -= new NodeEventHandler(this.NodesChanged_Handler);
      ReClassNetProject.ClassesChangedEvent classRemoved = this.ClassRemoved;
      if (classRemoved == null)
        return;
      classRemoved(node);
    }

    public void RemoveUnusedClasses()
    {
      foreach (ClassNode sender in this.classes.Except<ClassNode>(this.classes.Where<ClassNode>((Func<ClassNode, bool>) (x => this.GetClassReferences(x).Any<ClassNode>()))).Where<ClassNode>((Func<ClassNode, bool>) (c => c.Nodes.All<BaseNode>((Func<BaseNode, bool>) (n => n is BaseHexNode)))).ToList<ClassNode>())
      {
        if (this.classes.Remove(sender))
        {
          ReClassNetProject.ClassesChangedEvent classRemoved = this.ClassRemoved;
          if (classRemoved != null)
            classRemoved(sender);
        }
      }
    }

    public void AddEnum(EnumDescription @enum)
    {
      this.enums.Add(@enum);
      ReClassNetProject.EnumsChangedEvent enumAdded = this.EnumAdded;
      if (enumAdded == null)
        return;
      enumAdded(@enum);
    }

    public void RemoveEnum(EnumDescription @enum)
    {
      List<EnumNode> list = this.GetEnumReferences(@enum).ToList<EnumNode>();
      if (list.Any<EnumNode>())
        throw new EnumReferencedException(@enum, list.Select<EnumNode, ClassNode>((Func<EnumNode, ClassNode>) (e => e.GetParentClass())).Distinct<ClassNode>());
      if (!this.enums.Remove(@enum))
        return;
      ReClassNetProject.EnumsChangedEvent enumRemoved = this.EnumRemoved;
      if (enumRemoved == null)
        return;
      enumRemoved(@enum);
    }

    private IEnumerable<EnumNode> GetEnumReferences(EnumDescription @enum)
    {
      return this.classes.SelectMany<ClassNode, BaseNode>((Func<ClassNode, IEnumerable<BaseNode>>) (c => c.Nodes.Where<BaseNode>((Func<BaseNode, bool>) (n =>
      {
        BaseNode baseNode;
        switch (n)
        {
          case EnumNode _:
            return true;
          case BaseWrapperNode baseWrapperNode:
            baseNode = baseWrapperNode.ResolveMostInnerNode();
            break;
          default:
            baseNode = (BaseNode) null;
            break;
        }
        return baseNode is EnumNode;
      })))).Cast<EnumNode>().Where<EnumNode>((Func<EnumNode, bool>) (e => e.Enum == @enum));
    }

    public delegate void ClassesChangedEvent(ClassNode sender);

    public delegate void EnumsChangedEvent(EnumDescription sender);
  }
}
