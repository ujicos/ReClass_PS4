// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.InstructionData
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Core
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  public struct InstructionData
  {
    public IntPtr Address;
    public int Length;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
    public byte[] Data;
    public int StaticInstructionBytes;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string Instruction;
  }
}
