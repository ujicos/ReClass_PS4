// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.UInt32FloatData
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Memory
{
  [StructLayout(LayoutKind.Explicit)]
  public struct UInt32FloatData
  {
    [FieldOffset(0)]
    public int Raw;
    [FieldOffset(0)]
    public int IntValue;
    [FieldOffset(0)]
    public uint UIntValue;
    [FieldOffset(0)]
    public float FloatValue;

    public IntPtr IntPtr
    {
      get
      {
        return (IntPtr) this.IntValue;
      }
    }

    public UIntPtr UIntPtr
    {
      get
      {
        return (UIntPtr) this.UIntValue;
      }
    }
  }
}
