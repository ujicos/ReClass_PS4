// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.XAttributeExtensions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Xml.Linq;

namespace ReClassNET.Extensions
{
  public static class XAttributeExtensions
  {
    public static TEnum GetEnumValue<TEnum>(this XAttribute attribute) where TEnum : struct
    {
      TEnum result = default (TEnum);
      if (attribute != null)
        Enum.TryParse<TEnum>(attribute.Value, out result);
      return result;
    }
  }
}
