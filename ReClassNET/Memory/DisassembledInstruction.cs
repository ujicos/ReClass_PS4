// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.DisassembledInstruction
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Core;
using System;

namespace ReClassNET.Memory
{
  public class DisassembledInstruction
  {
    public IntPtr Address { get; set; }

    public int Length { get; set; }

    public byte[] Data { get; set; }

    public string Instruction { get; set; }

    public bool IsValid
    {
      get
      {
        return this.Length > 0;
      }
    }

    public DisassembledInstruction(ref InstructionData data)
    {
      this.Address = data.Address;
      this.Length = data.Length;
      this.Data = data.Data;
      this.Instruction = data.Instruction;
    }

    public override string ToString()
    {
      return this.Address.ToString("X016") + " - " + this.Instruction;
    }
  }
}
