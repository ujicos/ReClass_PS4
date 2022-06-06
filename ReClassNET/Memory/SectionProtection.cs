// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.SectionProtection
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Memory
{
  [Flags]
  public enum SectionProtection
  {
    NoAccess = 0,
    Read = 1,
    Write = 2,
    CopyOnWrite = 4,
    Execute = 8,
    Guard = 16, // 0x00000010
  }
}
