// Decompiled with JetBrains decompiler
// Type: ReClassNET.CodeGenerator.CustomCppCodeGenerator
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.Nodes;
using System.CodeDom.Compiler;

namespace ReClassNET.CodeGenerator
{
  public abstract class CustomCppCodeGenerator
  {
    public abstract bool CanHandle(BaseNode node);

    public virtual bool WriteNode(
      IndentedTextWriter writer,
      BaseNode node,
      WriteNodeFunc defaultWriteNodeFunc,
      ILogger logger)
    {
      return false;
    }

    public virtual BaseNode TransformNode(BaseNode node)
    {
      return node;
    }

    public virtual string GetTypeDefinition(
      BaseNode node,
      GetTypeDefinitionFunc defaultGetTypeDefinitionFunc,
      ResolveWrappedTypeFunc defaultResolveWrappedTypeFunc,
      ILogger logger)
    {
      return (string) null;
    }
  }
}
