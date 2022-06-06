// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.NodeDissector
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Memory
{
  public class NodeDissector
  {
    public static void DissectNodes(
      IEnumerable<BaseHexNode> nodes,
      IProcessReader reader,
      MemoryBuffer memory)
    {
      foreach (BaseHexNode node in nodes)
      {
        BaseNode guessedNode;
        if (NodeDissector.GuessNode(node, reader, memory, out guessedNode))
          node.GetParentContainer()?.ReplaceChildNode((BaseNode) node, guessedNode);
      }
    }

    public static bool GuessNode(
      BaseHexNode node,
      IProcessReader reader,
      MemoryBuffer memory,
      out BaseNode guessedNode)
    {
      guessedNode = (BaseNode) null;
      int offset = node.Offset;
      int num = offset % 4 == 0 ? 1 : 0;
      bool flag = offset % 8 == 0;
      if (num == 0)
        return false;
      UInt64FloatDoubleData uint64FloatDoubleData = new UInt64FloatDoubleData()
      {
        Raw1 = memory.ReadInt32(offset),
        Raw2 = memory.ReadInt32(offset + 4)
      };
      UInt32FloatData uint32FloatData = new UInt32FloatData()
      {
        Raw = memory.ReadInt32(offset)
      };
      byte[] numArray = memory.ReadBytes(offset, node.MemorySize);
      if (((IEnumerable<byte>) numArray).InterpretAsSingleByteCharacter().IsLikelyPrintableData())
      {
        guessedNode = (BaseNode) new Utf8TextNode();
        return true;
      }
      if (((IEnumerable<byte>) numArray).InterpretAsDoubleByteCharacter().IsLikelyPrintableData())
      {
        guessedNode = (BaseNode) new Utf16TextNode();
        return true;
      }
      if (flag && NodeDissector.GuessPointerNode(uint64FloatDoubleData.IntPtr, reader, out guessedNode))
        return true;
      if (uint32FloatData.IntValue != 0)
      {
        if (-999999.0 <= (double) uint32FloatData.FloatValue && (double) uint32FloatData.FloatValue <= 999999.0 && !uint32FloatData.FloatValue.IsNearlyEqual(0.0f, 1f / 1000f))
        {
          guessedNode = (BaseNode) new FloatNode();
          return true;
        }
        if (-999999 <= uint32FloatData.IntValue && uint32FloatData.IntValue <= 999999)
        {
          guessedNode = (BaseNode) new Int32Node();
          return true;
        }
      }
      if (!flag || uint64FloatDoubleData.LongValue == 0L || (-999999.0 > uint64FloatDoubleData.DoubleValue || uint64FloatDoubleData.DoubleValue > 999999.0) || uint64FloatDoubleData.DoubleValue.IsNearlyEqual(0.0, 0.001))
        return false;
      guessedNode = (BaseNode) new DoubleNode();
      return true;
    }

    private static bool GuessPointerNode(IntPtr address, IProcessReader process, out BaseNode node)
    {
      node = (BaseNode) null;
      if (address.IsNull())
        return false;
      Section sectionToPointer1 = process.GetSectionToPointer(address);
      if (sectionToPointer1 == null)
        return false;
      if (sectionToPointer1.Category == SectionCategory.CODE)
      {
        node = (BaseNode) new FunctionPtrNode();
        return true;
      }
      if (sectionToPointer1.Category != SectionCategory.DATA && sectionToPointer1.Category != SectionCategory.HEAP)
        return false;
      Section sectionToPointer2 = process.GetSectionToPointer(process.ReadRemoteIntPtr(address));
      if ((sectionToPointer2 != null ? (sectionToPointer2.Category == SectionCategory.CODE ? 1 : 0) : 0) != 0)
      {
        Section sectionToPointer3 = process.GetSectionToPointer(process.ReadRemoteIntPtr(address + IntPtr.Size));
        if ((sectionToPointer3 != null ? (sectionToPointer3.Category == SectionCategory.CODE ? 1 : 0) : 0) != 0)
        {
          Section sectionToPointer4 = process.GetSectionToPointer(process.ReadRemoteIntPtr(address + 2 * IntPtr.Size));
          if ((sectionToPointer4 != null ? (sectionToPointer4.Category == SectionCategory.CODE ? 1 : 0) : 0) != 0)
          {
            node = (BaseNode) new VirtualMethodTableNode();
            return true;
          }
        }
      }
      byte[] numArray = process.ReadRemoteMemory(address, IntPtr.Size * 2);
      if (((IEnumerable<byte>) numArray).Take<byte>(IntPtr.Size).InterpretAsSingleByteCharacter().IsLikelyPrintableData())
      {
        node = (BaseNode) new Utf8TextPtrNode();
        return true;
      }
      if (((IEnumerable<byte>) numArray).InterpretAsDoubleByteCharacter().IsLikelyPrintableData())
      {
        node = (BaseNode) new Utf16TextPtrNode();
        return true;
      }
      node = (BaseNode) new PointerNode();
      return true;
    }
  }
}
