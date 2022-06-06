// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.UInt16Data
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Runtime.InteropServices;

namespace ReClassNET.Memory
{
  [StructLayout(LayoutKind.Explicit)]
  public struct UInt16Data
  {
    [FieldOffset(0)]
    public short ShortValue;
    [FieldOffset(0)]
    public ushort UShortValue;
  }
}
