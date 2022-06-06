// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.UInt64FloatDoubleData
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Memory
{
  [StructLayout(LayoutKind.Explicit)]
  public struct UInt64FloatDoubleData
  {
    [FieldOffset(0)]
    public int Raw1;
    [FieldOffset(4)]
    public int Raw2;
    [FieldOffset(0)]
    public long LongValue;
    [FieldOffset(0)]
    public ulong ULongValue;
    [FieldOffset(0)]
    public float FloatValue;
    [FieldOffset(0)]
    public double DoubleValue;

    public IntPtr IntPtr
    {
      get
      {
        return (IntPtr) this.LongValue;
      }
    }

    public UIntPtr UIntPtr
    {
      get
      {
        return (UIntPtr) this.ULongValue;
      }
    }
  }
}
