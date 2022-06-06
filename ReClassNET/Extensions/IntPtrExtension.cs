// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.IntPtrExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Diagnostics;

namespace ReClassNET.Extensions
{
  public static class IntPtrExtension
  {
    [DebuggerStepThrough]
    public static bool IsNull(this IntPtr ptr)
    {
      return ptr == IntPtr.Zero;
    }

    [DebuggerStepThrough]
    public static bool MayBeValid(this IntPtr ptr)
    {
      return ptr.IsInRange((IntPtr) 65536, (IntPtr) long.MaxValue);
    }

    [DebuggerStepThrough]
    public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
    {
      return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
    }

    [DebuggerStepThrough]
    public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
    {
      return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
    }

    [DebuggerStepThrough]
    public static IntPtr Mul(this IntPtr lhs, IntPtr rhs)
    {
      return new IntPtr(lhs.ToInt64() * rhs.ToInt64());
    }

    [DebuggerStepThrough]
    public static IntPtr Div(this IntPtr lhs, IntPtr rhs)
    {
      return new IntPtr(lhs.ToInt64() / rhs.ToInt64());
    }

    [DebuggerStepThrough]
    public static int Mod(this IntPtr lhs, int mod)
    {
      return (int) (lhs.ToInt64() % (long) mod);
    }

    [DebuggerStepThrough]
    public static IntPtr Negate(this IntPtr ptr)
    {
      return new IntPtr(-ptr.ToInt64());
    }

    [DebuggerStepThrough]
    public static bool IsInRange(this IntPtr address, IntPtr start, IntPtr end)
    {
      ulong int64 = (ulong) address.ToInt64();
      return (ulong) start.ToInt64() <= int64 && int64 <= (ulong) end.ToInt64();
    }

    [DebuggerStepThrough]
    public static int CompareTo(this IntPtr lhs, IntPtr rhs)
    {
      return ((ulong) lhs.ToInt64()).CompareTo((ulong) rhs.ToInt64());
    }

    [DebuggerStepThrough]
    public static int CompareToRange(this IntPtr address, IntPtr start, IntPtr end)
    {
      return address.IsInRange(start, end) ? 0 : address.CompareTo(start);
    }

    [DebuggerStepThrough]
    public static long ToInt64Bits(this IntPtr ptr)
    {
      return ptr.ToInt64();
    }

    [DebuggerStepThrough]
    public static IntPtr From(int value)
    {
      return (IntPtr) value;
    }

    [DebuggerStepThrough]
    public static IntPtr From(long value)
    {
      return (IntPtr) value;
    }
  }
}
