// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseFunctionNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ReClassNET.Nodes
{
  public abstract class BaseFunctionNode : BaseNode
  {
    protected IntPtr Address = IntPtr.Zero;
    protected readonly List<BaseFunctionNode.FunctionNodeInstruction> Instructions = new List<BaseFunctionNode.FunctionNodeInstruction>();

    protected Size DrawInstructions(DrawContext view, int tx, int y)
    {
      int num1 = y;
      int num2 = 26 * view.Font.Width;
      int num3 = 0;
      using (SolidBrush solidBrush = new SolidBrush(view.Settings.HiddenColor))
      {
        foreach (BaseFunctionNode.FunctionNodeInstruction instruction in this.Instructions)
        {
          y += view.Font.Height;
          int x1 = this.AddText(view, tx, y, view.Settings.AddressColor, 999, instruction.Address) + 6;
          view.Graphics.FillRectangle((Brush) solidBrush, x1, y, 1, view.Font.Height);
          int x2 = x1 + 6;
          int x3 = Math.Max(this.AddText(view, x2, y, view.Settings.HexColor, 999, instruction.Data) + 6, x2 + num2);
          view.Graphics.FillRectangle((Brush) solidBrush, x3, y, 1, view.Font.Height);
          int x4 = x3 + 6;
          num3 = Math.Max(this.AddText(view, x4, y, view.Settings.ValueColor, 999, instruction.Instruction) - tx, num3);
        }
      }
      return new Size(num3, y - num1);
    }

    protected void DisassembleRemoteCode(RemoteProcess process, IntPtr address, out int memorySize)
    {
      memorySize = 0;
      foreach (DisassembledInstruction disassembledInstruction in (IEnumerable<DisassembledInstruction>) new Disassembler(process.CoreFunctions).RemoteDisassembleFunction((IRemoteMemoryReader) process, address, 8192))
      {
        memorySize += disassembledInstruction.Length;
        this.Instructions.Add(new BaseFunctionNode.FunctionNodeInstruction()
        {
          Address = disassembledInstruction.Address.ToString("X016"),
          Data = string.Join(" ", ((IEnumerable<byte>) disassembledInstruction.Data).Take<byte>(disassembledInstruction.Length).Select<byte, string>((Func<byte, string>) (b => string.Format("{0:X2}", (object) b)))),
          Instruction = disassembledInstruction.Instruction
        });
      }
    }

    protected class FunctionNodeInstruction
    {
      public string Address { get; set; }

      public string Data { get; set; }

      public string Instruction { get; set; }
    }
  }
}
