// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.Conversion.LittleEndianBitConverter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.Util.Conversion
{
  public sealed class LittleEndianBitConverter : EndianBitConverter
  {
    protected override long FromBytes(byte[] buffer, int index, int bytesToConvert)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index + bytesToConvert > buffer.Length)
        throw new ArgumentOutOfRangeException(nameof (index));
      long num = 0;
      for (int index1 = 0; index1 < bytesToConvert; ++index1)
        num = num << 8 | (long) buffer[index + bytesToConvert - 1 - index1];
      return num;
    }

    protected override byte[] ToBytes(long value, int bytes)
    {
      byte[] numArray = new byte[bytes];
      for (int index = 0; index < bytes; ++index)
      {
        numArray[index] = (byte) ((ulong) value & (ulong) byte.MaxValue);
        value >>= 8;
      }
      return numArray;
    }
  }
}
