// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.IRemoteMemoryWriter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util.Conversion;
using System;

namespace ReClassNET.Memory
{
  public interface IRemoteMemoryWriter
  {
    EndianBitConverter BitConverter { get; set; }

    bool WriteRemoteMemory(IntPtr address, byte[] data);
  }
}
