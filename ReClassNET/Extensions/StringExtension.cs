// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.StringExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace ReClassNET.Extensions
{
  public static class StringExtension
  {
    private static readonly Regex hexadecimalValueRegex = new Regex("^(0x|h)?([0-9A-F]+)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    [DebuggerStepThrough]
    public static bool IsPrintable(this char c)
    {
      return (' ' <= c && c <= '~' || '¡' <= c && c <= 'ÿ') && c != '�';
    }

    [DebuggerStepThrough]
    public static IEnumerable<char> InterpretAsSingleByteCharacter(
      this IEnumerable<byte> source)
    {
      return source.Select<byte, char>((Func<byte, char>) (b => (char) b));
    }

    [DebuggerStepThrough]
    public static IEnumerable<char> InterpretAsDoubleByteCharacter(
      this IEnumerable<byte> source)
    {
      byte[] array = source.ToArray<byte>();
      char[] chArray = new char[array.Length / 2];
      Buffer.BlockCopy((Array) array, 0, (Array) chArray, 0, array.Length);
      return (IEnumerable<char>) chArray;
    }

    [DebuggerStepThrough]
    public static bool IsPrintableData(this IEnumerable<char> source)
    {
      return (double) source.CalculatePrintableDataThreshold() >= 1.0;
    }

    [DebuggerStepThrough]
    public static bool IsLikelyPrintableData(this IEnumerable<char> source)
    {
      return (double) source.CalculatePrintableDataThreshold() >= 0.75;
    }

    [DebuggerStepThrough]
    public static float CalculatePrintableDataThreshold(this IEnumerable<char> source)
    {
      bool flag = true;
      int num1 = 0;
      int num2 = 0;
      foreach (char c in source)
      {
        ++num2;
        if (flag)
        {
          if (c.IsPrintable())
            ++num1;
          else
            flag = false;
        }
      }
      return num2 == 0 ? 0.0f : (float) num1 / (float) num2;
    }

    [DebuggerStepThrough]
    public static string LimitLength(this string s, int length)
    {
      return s.Length <= length ? s : s.Substring(0, length);
    }

    public static bool TryGetHexString(this string s, out string value)
    {
      Match match = StringExtension.hexadecimalValueRegex.Match(s);
      value = match.Success ? match.Groups[2].Value : (string) null;
      return match.Success;
    }
  }
}
