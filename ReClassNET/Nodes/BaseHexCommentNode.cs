// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseHexCommentNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReClassNET.Nodes
{
  public abstract class BaseHexCommentNode : BaseHexNode
  {
    protected int AddComment(
      DrawContext view,
      int x,
      int y,
      float fvalue,
      IntPtr ivalue,
      UIntPtr uvalue)
    {
      if (view.Settings.ShowCommentFloat)
        x = this.AddText(view, x, y, view.Settings.ValueColor, 999, (double) fvalue <= -999999.0 || (double) fvalue >= 999999.0 ? "#####" : fvalue.ToString("0.000")) + view.Font.Width;
      if (view.Settings.ShowCommentInteger)
      {
        if (ivalue == IntPtr.Zero)
        {
          x = this.AddText(view, x, y, view.Settings.ValueColor, 999, "0") + view.Font.Width;
        }
        else
        {
          x = this.AddText(view, x, y, view.Settings.ValueColor, 999, ivalue.ToInt64().ToString()) + view.Font.Width;
          x = this.AddText(view, x, y, view.Settings.ValueColor, 999, string.Format("0x{0:X}", (object) uvalue.ToUInt64())) + view.Font.Width;
        }
      }
      if (ivalue != IntPtr.Zero)
      {
        string namedAddress = view.Process.GetNamedAddress(ivalue);
        if (!string.IsNullOrEmpty(namedAddress))
        {
          if (view.Settings.ShowCommentPointer)
          {
            x = this.AddText(view, x, y, view.Settings.OffsetColor, -1, "->") + view.Font.Width;
            x = this.AddText(view, x, y, view.Settings.OffsetColor, 999, namedAddress) + view.Font.Width;
          }
          if (view.Settings.ShowCommentRtti)
          {
            string text = view.Process.ReadRemoteRuntimeTypeInformation(ivalue);
            if (!string.IsNullOrEmpty(text))
              x = this.AddText(view, x, y, view.Settings.OffsetColor, 999, text) + view.Font.Width;
          }
          if (view.Settings.ShowCommentSymbol)
          {
            Module moduleToPointer = view.Process.GetModuleToPointer(ivalue);
            if (moduleToPointer != null)
            {
              string symbolString = view.Process.Symbols.GetSymbolsForModule(moduleToPointer)?.GetSymbolString(ivalue, moduleToPointer);
              if (!string.IsNullOrEmpty(symbolString))
                x = this.AddText(view, x, y, view.Settings.OffsetColor, 999, symbolString) + view.Font.Width;
            }
          }
          if (view.Settings.ShowCommentString)
          {
            byte[] bytes = view.Process.ReadRemoteMemory(ivalue, 64);
            bool flag = false;
            string text = (string) null;
            if (((IEnumerable<byte>) bytes).Take<byte>(IntPtr.Size).InterpretAsSingleByteCharacter().IsPrintableData())
              text = new string(((IEnumerable<char>) Encoding.UTF8.GetChars(bytes)).TakeWhile<char>((Func<char, bool>) (c => c > char.MinValue)).ToArray<char>());
            else if (((IEnumerable<byte>) bytes).Take<byte>(IntPtr.Size * 2).InterpretAsDoubleByteCharacter().IsPrintableData())
            {
              flag = true;
              text = new string(((IEnumerable<char>) Encoding.Unicode.GetChars(bytes)).TakeWhile<char>((Func<char, bool>) (c => c > char.MinValue)).ToArray<char>());
            }
            if (text != null)
            {
              x = this.AddText(view, x, y, view.Settings.TextColor, -1, flag ? "L'" : "'");
              x = this.AddText(view, x, y, view.Settings.TextColor, 999, text);
              x = this.AddText(view, x, y, view.Settings.TextColor, -1, "'") + view.Font.Width;
            }
          }
          if (view.Settings.ShowCommentPluginInfo)
          {
            IntPtr nodeAddress = view.Address + this.Offset;
            foreach (INodeInfoReader nodeInfoReader in BaseNode.NodeInfoReader)
            {
              string text = nodeInfoReader.ReadNodeInfo(this, (IRemoteMemoryReader) view.Process, view.Memory, nodeAddress, ivalue);
              if (text != null)
                x = this.AddText(view, x, y, view.Settings.PluginColor, 999, text) + view.Font.Width;
            }
          }
        }
      }
      return x;
    }
  }
}
