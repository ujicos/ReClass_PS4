// Decompiled with JetBrains decompiler
// Type: ReClassNET.Debugger.ExceptionDebugInfo
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Debugger
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public struct ExceptionDebugInfo
  {
    public IntPtr ExceptionCode;
    public IntPtr ExceptionFlags;
    public IntPtr ExceptionAddress;
    public HardwareBreakpointRegister CausedBy;
    public ExceptionDebugInfo.RegisterInfo Registers;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RegisterInfo
    {
      public IntPtr Rax;
      public IntPtr Rbx;
      public IntPtr Rcx;
      public IntPtr Rdx;
      public IntPtr Rdi;
      public IntPtr Rsi;
      public IntPtr Rsp;
      public IntPtr Rbp;
      public IntPtr Rip;
      public IntPtr R8;
      public IntPtr R9;
      public IntPtr R10;
      public IntPtr R11;
      public IntPtr R12;
      public IntPtr R13;
      public IntPtr R14;
      public IntPtr R15;
    }
  }
}
