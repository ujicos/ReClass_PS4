// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.HexadecimalFormatter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.Util
{
  public static class HexadecimalFormatter
  {
    private static readonly uint[] lookup = HexadecimalFormatter.CreateHexLookup();

    private static uint[] CreateHexLookup()
    {
      uint[] numArray = new uint[256];
      for (int index = 0; index < 256; ++index)
      {
        string str = index.ToString("X2");
        numArray[index] = (uint) str[0] + ((uint) str[1] << 16);
      }
      return numArray;
    }

    public static string ToString(byte[] data)
    {
      if (data.Length == 0)
        return string.Empty;
      char[] chArray = new char[data.Length * 2 + data.Length - 1];
      uint num1 = HexadecimalFormatter.lookup[(int) data[0]];
      chArray[0] = (char) num1;
      chArray[1] = (char) (num1 >> 16);
      for (int index = 1; index < data.Length; ++index)
      {
        uint num2 = HexadecimalFormatter.lookup[(int) data[index]];
        chArray[3 * index - 1] = ' ';
        chArray[3 * index] = (char) num2;
        chArray[3 * index + 1] = (char) (num2 >> 16);
      }
      return new string(chArray);
    }
  }
}
