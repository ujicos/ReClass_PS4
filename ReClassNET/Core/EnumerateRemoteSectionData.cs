// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.EnumerateRemoteSectionData
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Core
{
  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
  public struct EnumerateRemoteSectionData
  {
    public IntPtr BaseAddress;
    public IntPtr Size;
    public SectionType Type;
    public SectionCategory Category;
    public SectionProtection Protection;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string Name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string ModulePath;
  }
}
