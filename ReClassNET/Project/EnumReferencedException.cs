// Decompiled with JetBrains decompiler
// Type: ReClassNET.Project.EnumReferencedException
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Nodes;
using System;
using System.Collections.Generic;

namespace ReClassNET.Project
{
  public class EnumReferencedException : Exception
  {
    public EnumDescription Enum { get; }

    public IEnumerable<ClassNode> References { get; }

    public EnumReferencedException(EnumDescription @enum, IEnumerable<ClassNode> references)
      : base("The enum '" + @enum.Name + "' is referenced in other classes.")
    {
      this.Enum = @enum;
      this.References = references;
    }
  }
}
