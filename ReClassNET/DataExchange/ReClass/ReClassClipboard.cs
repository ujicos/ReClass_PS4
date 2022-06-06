// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.ReClassClipboard
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ReClassNET.DataExchange.ReClass
{
  public class ReClassClipboard
  {
    private const string ClipboardFormat = "ReClass.NET::Nodes";

    public static bool ContainsNodes
    {
      get
      {
        return Clipboard.ContainsData("ReClass.NET::Nodes");
      }
    }

    public static void Copy(IEnumerable<BaseNode> nodes, ILogger logger)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        ReClassNetFile.SerializeNodesToStream((Stream) memoryStream, nodes, logger);
        Clipboard.SetData("ReClass.NET::Nodes", (object) memoryStream.ToArray());
      }
    }

    public static Tuple<List<ClassNode>, List<BaseNode>> Paste(
      ReClassNetProject templateProject,
      ILogger logger)
    {
      if (!ReClassClipboard.ContainsNodes || !(Clipboard.GetData("ReClass.NET::Nodes") is byte[] data))
        return Tuple.Create<List<ClassNode>, List<BaseNode>>(new List<ClassNode>(), new List<BaseNode>());
      using (MemoryStream memoryStream = new MemoryStream(data))
        return ReClassNetFile.DeserializeNodesFromStream((Stream) memoryStream, templateProject, logger);
    }
  }
}
