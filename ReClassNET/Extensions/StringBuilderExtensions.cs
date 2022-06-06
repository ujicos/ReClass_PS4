// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.StringBuilderExtensions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Text;

namespace ReClassNET.Extensions
{
  public static class StringBuilderExtensions
  {
    public static StringBuilder Prepend(this StringBuilder sb, char value)
    {
      return sb.Insert(0, value);
    }

    public static StringBuilder Prepend(this StringBuilder sb, string value)
    {
      return sb.Insert(0, value);
    }
  }
}
