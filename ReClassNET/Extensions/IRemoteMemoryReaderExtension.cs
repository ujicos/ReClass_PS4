// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.IRemoteMemoryReaderExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReClassNET.Extensions
{
  public static class IRemoteMemoryReaderExtension
  {
    public static sbyte ReadRemoteInt8(this IRemoteMemoryReader reader, IntPtr address)
    {
      return (sbyte) reader.ReadRemoteMemory(address, 1)[0];
    }

    public static byte ReadRemoteUInt8(this IRemoteMemoryReader reader, IntPtr address)
    {
      return reader.ReadRemoteMemory(address, 1)[0];
    }

    public static short ReadRemoteInt16(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 2);
      return reader.BitConverter.ToInt16(numArray, 0);
    }

    public static ushort ReadRemoteUInt16(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 2);
      return reader.BitConverter.ToUInt16(numArray, 0);
    }

    public static int ReadRemoteInt32(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 4);
      return reader.BitConverter.ToInt32(numArray, 0);
    }

    public static uint ReadRemoteUInt32(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 4);
      return reader.BitConverter.ToUInt32(numArray, 0);
    }

    public static long ReadRemoteInt64(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 8);
      return reader.BitConverter.ToInt64(numArray, 0);
    }

    public static ulong ReadRemoteUInt64(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 8);
      return reader.BitConverter.ToUInt64(numArray, 0);
    }

    public static float ReadRemoteFloat(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 4);
      return reader.BitConverter.ToSingle(numArray, 0);
    }

    public static double ReadRemoteDouble(this IRemoteMemoryReader reader, IntPtr address)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, 8);
      return reader.BitConverter.ToDouble(numArray, 0);
    }

    public static IntPtr ReadRemoteIntPtr(this IRemoteMemoryReader reader, IntPtr address)
    {
      return (IntPtr) reader.ReadRemoteInt64(address);
    }

    public static string ReadRemoteString(
      this IRemoteMemoryReader reader,
      IntPtr address,
      Encoding encoding,
      int length)
    {
      byte[] bytes = reader.ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());
      try
      {
        StringBuilder stringBuilder = new StringBuilder(encoding.GetString(bytes));
        for (int index = 0; index < stringBuilder.Length; ++index)
        {
          if (stringBuilder[index] == char.MinValue)
          {
            stringBuilder.Length = index;
            break;
          }
          if (!stringBuilder[index].IsPrintable())
            stringBuilder[index] = '.';
        }
        return stringBuilder.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }

    public static string ReadRemoteStringUntilFirstNullCharacter(
      this IRemoteMemoryReader reader,
      IntPtr address,
      Encoding encoding,
      int length)
    {
      byte[] numArray = reader.ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());
      int val1 = PatternScanner.FindPattern(BytePattern.From((IEnumerable<byte>) new byte[encoding.GuessByteCountPerChar()]), numArray);
      if (val1 == -1)
        val1 = numArray.Length;
      try
      {
        return encoding.GetString(numArray, 0, Math.Min(val1, numArray.Length));
      }
      catch
      {
        return string.Empty;
      }
    }
  }
}
