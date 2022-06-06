// Decompiled with JetBrains decompiler
// Type: ReClassNET.CodeGenerator.ResolveWrappedTypeFunc
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
  public delegate string ResolveWrappedTypeFunc(
    BaseNode node,
    bool isAnonymousExpression,
    ILogger logger);
}
