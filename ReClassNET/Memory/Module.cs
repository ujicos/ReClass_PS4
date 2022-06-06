// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.Module
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Memory
{
  public class Module
  {
    public IntPtr Start { get; set; }

    public IntPtr End { get; set; }

    public IntPtr Size { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }
  }
}
