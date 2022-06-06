// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.BinaryReaderWriterExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;

namespace ReClassNET.Extensions
{
  public static class BinaryReaderWriterExtension
  {
    public static IntPtr ReadIntPtr(this BinaryReader br)
    {
      return (IntPtr) br.ReadInt64();
    }

    public static void Write(this BinaryWriter bw, IntPtr value)
    {
      bw.Write(value.ToInt64());
    }
  }
}
