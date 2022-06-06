// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.BitString
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Text;

namespace ReClassNET.Util
{
  public static class BitString
  {
    public static string ToString(byte value)
    {
      return BitString.AddPaddingAndBuildBlocks(8, Convert.ToString(value, 2));
    }

    public static string ToString(short value)
    {
      return BitString.AddPaddingAndBuildBlocks(16, Convert.ToString(value, 2));
    }

    public static string ToString(int value)
    {
      return BitString.AddPaddingAndBuildBlocks(32, Convert.ToString(value, 2));
    }

    public static string ToString(long value)
    {
      return BitString.AddPaddingAndBuildBlocks(64, Convert.ToString(value, 2));
    }

    private static string AddPaddingAndBuildBlocks(int bits, string value)
    {
      StringBuilder stringBuilder = new StringBuilder(bits);
      int num;
      for (num = bits - value.Length; num > 4; num -= 4)
        stringBuilder.Append("0000 ");
      if (num > 0)
      {
        for (int index = 0; index < num; ++index)
          stringBuilder.Append('0');
        stringBuilder.Append(value, 0, 4 - num);
        if (value.Length > num)
          stringBuilder.Append(' ');
      }
      for (int startIndex = num == 0 ? 0 : 4 - num; startIndex < value.Length; startIndex += 4)
      {
        stringBuilder.Append(value, startIndex, 4);
        if (startIndex < value.Length - 4)
          stringBuilder.Append(' ');
      }
      return stringBuilder.ToString();
    }
  }
}
