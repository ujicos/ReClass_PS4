// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.EnumerateRemoteModuleData
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Core
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  public struct EnumerateRemoteModuleData
  {
    public IntPtr BaseAddress;
    public IntPtr Size;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string Path;
  }
}
