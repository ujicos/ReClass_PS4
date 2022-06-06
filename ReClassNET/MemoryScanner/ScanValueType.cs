// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScanValueType
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.ComponentModel;

namespace ReClassNET.MemoryScanner
{
  public enum ScanValueType
  {
    [Description("Byte")] Byte,
    [Description("Short (2 Bytes)")] Short,
    [Description("Integer (4 Bytes)")] Integer,
    [Description("Long (8 Bytes)")] Long,
    [Description("Float (4 Bytes)")] Float,
    [Description("Double (8 Bytes)")] Double,
    [Description("Array of Bytes")] ArrayOfBytes,
    [Description("String")] String,
    [Description("Regular Expression")] Regex,
  }
}
