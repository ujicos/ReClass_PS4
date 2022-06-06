// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.Disassembler
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Core;
using ReClassNET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ReClassNET.Memory
{
  public class Disassembler
  {
    public const int MaximumInstructionLength = 15;
    private readonly CoreFunctionsManager coreFunctions;

    public Disassembler(CoreFunctionsManager coreFunctions)
    {
      this.coreFunctions = coreFunctions;
    }

    public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(
      IRemoteMemoryReader process,
      IntPtr address,
      int length)
    {
      return this.RemoteDisassembleCode(process, address, length, -1);
    }

    public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(
      IRemoteMemoryReader process,
      IntPtr address,
      int length,
      int maxInstructions)
    {
      return this.DisassembleCode(process.ReadRemoteMemory(address, length), address, maxInstructions);
    }

    public IReadOnlyList<DisassembledInstruction> DisassembleCode(
      byte[] data,
      IntPtr virtualAddress,
      int maxInstructions)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      try
      {
        List<DisassembledInstruction> instructions = new List<DisassembledInstruction>();
        this.coreFunctions.DisassembleCode(gcHandle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (EnumerateInstructionCallback) ((ref InstructionData instruction) =>
        {
          instructions.Add(new DisassembledInstruction(ref instruction));
          return maxInstructions == -1 || instructions.Count < maxInstructions;
        }));
        return (IReadOnlyList<DisassembledInstruction>) instructions;
      }
      finally
      {
        if (gcHandle.IsAllocated)
          gcHandle.Free();
      }
    }

    public IReadOnlyList<DisassembledInstruction> RemoteDisassembleFunction(
      IRemoteMemoryReader process,
      IntPtr address,
      int maxLength)
    {
      return this.DisassembleFunction(process.ReadRemoteMemory(address, maxLength), address);
    }

    public IReadOnlyList<DisassembledInstruction> DisassembleFunction(
      byte[] data,
      IntPtr virtualAddress)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) data, GCHandleType.Pinned);
      try
      {
        List<DisassembledInstruction> instructions = new List<DisassembledInstruction>();
        this.coreFunctions.DisassembleCode(gcHandle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (EnumerateInstructionCallback) ((ref InstructionData result) =>
        {
          if (result.Length == 1 && result.Data[0] == (byte) 204)
            return false;
          instructions.Add(new DisassembledInstruction(ref result));
          return true;
        }));
        return (IReadOnlyList<DisassembledInstruction>) instructions;
      }
      finally
      {
        if (gcHandle.IsAllocated)
          gcHandle.Free();
      }
    }

    public DisassembledInstruction RemoteGetPreviousInstruction(
      IRemoteMemoryReader process,
      IntPtr address)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) process.ReadRemoteMemory(address - 90, 105), GCHandleType.Pinned);
      try
      {
        IntPtr targetBufferAddress = gcHandle.AddrOfPinnedObject() + 90;
        InstructionData instruction = new InstructionData();
        int[] numArray = new int[18]
        {
          90,
          60,
          30,
          15,
          14,
          13,
          12,
          11,
          10,
          9,
          8,
          7,
          6,
          5,
          4,
          3,
          2,
          1
        };
        foreach (int num in numArray)
        {
          IntPtr currentAddress = targetBufferAddress - num;
          this.coreFunctions.DisassembleCode(currentAddress, num + 1, address - num, false, (EnumerateInstructionCallback) ((ref InstructionData data) =>
          {
            IntPtr lhs = currentAddress + data.Length;
            if (lhs.CompareTo(targetBufferAddress) > 0)
              return false;
            instruction = data;
            currentAddress = lhs;
            return true;
          }));
          if (currentAddress == targetBufferAddress)
            return new DisassembledInstruction(ref instruction);
        }
        return (DisassembledInstruction) null;
      }
      finally
      {
        if (gcHandle.IsAllocated)
          gcHandle.Free();
      }
    }

    public IntPtr RemoteGetFunctionStartAddress(IRemoteMemoryReader process, IntPtr address)
    {
      byte[] buffer = new byte[517];
      for (int index1 = 1; index1 <= 10 && process.ReadRemoteMemoryIntoBuffer(address - index1 * 512 - 2, ref buffer); ++index1)
      {
        for (int index2 = 516; index2 > 0; --index2)
        {
          if (buffer[index2] == (byte) 204 && buffer[index2 - 1] == (byte) 204)
          {
            IntPtr num1 = address - index1 * 512 + index2 - 1;
            DisassembledInstruction previousInstruction1 = this.RemoteGetPreviousInstruction(process, num1);
            if (previousInstruction1.Length == 1 && previousInstruction1.Data[0] == (byte) 204)
            {
              DisassembledInstruction previousInstruction2 = this.RemoteGetPreviousInstruction(process, num1 - 1);
              if (previousInstruction2.Length == 1 && previousInstruction2.Data[0] == (byte) 204)
              {
                int num2 = this.RemoteDisassembleCode(process, num1, address.Sub(num1).ToInt32()).Sum<DisassembledInstruction>((Func<DisassembledInstruction, int>) (ins => ins.Length));
                if (num1 + num2 == address)
                  return num1;
              }
              else
                index2 -= previousInstruction2.Length;
            }
            else
              index2 -= previousInstruction1.Length;
          }
        }
      }
      return IntPtr.Zero;
    }
  }
}
