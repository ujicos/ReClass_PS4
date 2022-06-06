// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.PatternScanner
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Core;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ReClassNET.MemoryScanner
{
  public class PatternScanner
  {
    public static IntPtr FindPattern(
      BytePattern pattern,
      RemoteProcess process,
      Module module)
    {
      return PatternScanner.FindPattern(pattern, process, module.Start, module.Size.ToInt32());
    }

    public static IntPtr FindPattern(
      BytePattern pattern,
      RemoteProcess process,
      Section section)
    {
      return PatternScanner.FindPattern(pattern, process, section.Start, section.Size.ToInt32());
    }

    public static IntPtr FindPattern(
      BytePattern pattern,
      RemoteProcess process,
      IntPtr start,
      int size)
    {
      byte[] data = process.ReadRemoteMemory(start, size);
      int pattern1 = PatternScanner.FindPattern(pattern, data);
      return pattern1 == -1 ? IntPtr.Zero : start + pattern1;
    }

    public static int FindPattern(BytePattern pattern, byte[] data)
    {
      int num = data.Length - pattern.Length;
      for (int index = 0; index < num; ++index)
      {
        if (pattern.Equals(data, index))
          return index;
      }
      return -1;
    }

    public static BytePattern CreatePatternFromCode(
      RemoteProcess process,
      IntPtr start,
      int size)
    {
      List<Tuple<byte, bool>> data = new List<Tuple<byte, bool>>();
      GCHandle gcHandle = GCHandle.Alloc((object) process.ReadRemoteMemory(start, size), GCHandleType.Pinned);
      try
      {
        IntPtr address = gcHandle.AddrOfPinnedObject();
        process.CoreFunctions.DisassembleCode(address, size, IntPtr.Zero, true, (EnumerateInstructionCallback) ((ref InstructionData instruction) =>
        {
          for (int index = 0; index < instruction.Length; ++index)
            data.Add(Tuple.Create<byte, bool>(instruction.Data[index], index >= instruction.StaticInstructionBytes));
          return true;
        }));
      }
      finally
      {
        if (gcHandle.IsAllocated)
          gcHandle.Free();
      }
      return BytePattern.From((IEnumerable<Tuple<byte, bool>>) data);
    }
  }
}
