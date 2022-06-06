// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.EncodingExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Text;

namespace ReClassNET.Extensions
{
  public static class EncodingExtension
  {
    public static int GuessByteCountPerChar(this Encoding encoding)
    {
      if (encoding.IsSameCodePage(Encoding.UTF8) || encoding.CodePage == 1252 || encoding.IsSameCodePage(Encoding.ASCII))
        return 1;
      if (encoding.IsSameCodePage(Encoding.Unicode) || encoding.IsSameCodePage(Encoding.BigEndianUnicode))
        return 2;
      if (encoding.IsSameCodePage(Encoding.UTF32))
        return 4;
      throw new NotImplementedException();
    }

    public static bool IsSameCodePage(this Encoding encoding, Encoding other)
    {
      return encoding.CodePage == other.CodePage;
    }
  }
}
