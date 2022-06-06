// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.NodeInfoReaderContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;

namespace ReClassNET.Nodes
{
  internal abstract class NodeInfoReaderContract : INodeInfoReader
  {
    public string ReadNodeInfo(
      BaseHexCommentNode node,
      IRemoteMemoryReader reader,
      MemoryBuffer memory,
      IntPtr nodeAddress,
      IntPtr nodeValue)
    {
      throw new NotImplementedException();
    }
  }
}
