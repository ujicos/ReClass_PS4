// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.IRemoteMemoryWriterExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using System;
using System.Text;

namespace ReClassNET.Extensions
{
  public static class IRemoteMemoryWriterExtension
  {
    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      sbyte value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes((short) value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      byte value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes((short) value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      short value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      ushort value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      int value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      uint value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      long value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      ulong value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      float value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      double value)
    {
      writer.WriteRemoteMemory(address, writer.BitConverter.GetBytes(value));
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      IntPtr value)
    {
      writer.WriteRemoteMemory(address, value.ToInt64());
    }

    public static void WriteRemoteMemory(
      this IRemoteMemoryWriter writer,
      IntPtr address,
      string value,
      Encoding encoding)
    {
      writer.WriteRemoteMemory(address, encoding.GetBytes(value));
    }
  }
}
