// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.INodeInfoReader
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Nodes
{
  public interface INodeInfoReader
  {
    string ReadNodeInfo(
      BaseHexCommentNode node,
      IRemoteMemoryReader reader,
      MemoryBuffer memory,
      IntPtr nodeAddress,
      IntPtr nodeValue);
  }
}
