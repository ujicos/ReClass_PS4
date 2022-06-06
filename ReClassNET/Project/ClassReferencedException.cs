// Decompiled with JetBrains decompiler
// Type: ReClassNET.Project.ClassReferencedException
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Nodes;
using System;
using System.Collections.Generic;

namespace ReClassNET.Project
{
  public class ClassReferencedException : Exception
  {
    public ClassNode ClassNode { get; }

    public IEnumerable<ClassNode> References { get; }

    public ClassReferencedException(ClassNode node, IEnumerable<ClassNode> references)
      : base("The class '" + node.Name + "' is referenced in other classes.")
    {
      this.ClassNode = node;
      this.References = references;
    }
  }
}
