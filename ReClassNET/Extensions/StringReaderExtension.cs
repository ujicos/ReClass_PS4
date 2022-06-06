// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.StringReaderExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.IO;

namespace ReClassNET.Extensions
{
  public static class StringReaderExtension
  {
    public static int ReadSkipWhitespaces(this StringReader sr)
    {
      int num;
      do
      {
        num = sr.Read();
      }
      while (num != -1 && char.IsWhiteSpace((char) num));
      return num;
    }
  }
}
