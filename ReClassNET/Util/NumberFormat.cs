// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.NumberFormat
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Globalization;

namespace ReClassNET.Util
{
  public static class NumberFormat
  {
    public static NumberFormatInfo GuessNumberFormat(string input)
    {
      if (input.IndexOf(',') > input.IndexOf('.'))
        return new NumberFormatInfo()
        {
          NumberDecimalSeparator = ",",
          NumberGroupSeparator = "."
        };
      return new NumberFormatInfo()
      {
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ","
      };
    }
  }
}
