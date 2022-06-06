// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.Dumper
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;

namespace ReClassNET.Memory
{
  public static class Dumper
  {
    public static void DumpRaw(
      IRemoteMemoryReader reader,
      IntPtr address,
      int size,
      Stream stream)
    {
      byte[] buffer = reader.ReadRemoteMemory(address, size);
      stream.Write(buffer, 0, buffer.Length);
    }

    public static void DumpSection(IRemoteMemoryReader reader, Section section, Stream stream)
    {
      Dumper.DumpRaw(reader, section.Start, section.Size.ToInt32(), stream);
    }

    public static void DumpModule(IRemoteMemoryReader reader, Module module, Stream stream)
    {
      byte[] numArray = reader.ReadRemoteMemory(module.Start, module.Size.ToInt32());
      SimplePeHeader.FixSectionHeaders(numArray);
      stream.Write(numArray, 0, numArray.Length);
    }
  }
}
