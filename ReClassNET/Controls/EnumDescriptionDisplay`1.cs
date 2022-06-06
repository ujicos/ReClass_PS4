// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.EnumDescriptionDisplay`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ReClassNET.Controls
{
  public class EnumDescriptionDisplay<TEnum> where TEnum : struct
  {
    public TEnum Value { get; internal set; }

    public string Description { get; internal set; }

    public static List<EnumDescriptionDisplay<TEnum>> Create()
    {
      return EnumDescriptionDisplay<TEnum>.CreateExact(Enum.GetValues(typeof (TEnum)).Cast<TEnum>());
    }

    public static List<EnumDescriptionDisplay<TEnum>> CreateExact(
      IEnumerable<TEnum> include)
    {
      return include.Select<TEnum, EnumDescriptionDisplay<TEnum>>((Func<TEnum, EnumDescriptionDisplay<TEnum>>) (value => new EnumDescriptionDisplay<TEnum>()
      {
        Description = EnumDescriptionDisplay<TEnum>.GetDescription(value),
        Value = value
      })).OrderBy<EnumDescriptionDisplay<TEnum>, TEnum>((Func<EnumDescriptionDisplay<TEnum>, TEnum>) (item => item.Value)).ToList<EnumDescriptionDisplay<TEnum>>();
    }

    public static List<EnumDescriptionDisplay<TEnum>> CreateExclude(
      IEnumerable<TEnum> exclude)
    {
      return Enum.GetValues(typeof (TEnum)).Cast<TEnum>().Except<TEnum>(exclude).Select<TEnum, EnumDescriptionDisplay<TEnum>>((Func<TEnum, EnumDescriptionDisplay<TEnum>>) (value => new EnumDescriptionDisplay<TEnum>()
      {
        Description = EnumDescriptionDisplay<TEnum>.GetDescription(value),
        Value = value
      })).OrderBy<EnumDescriptionDisplay<TEnum>, TEnum>((Func<EnumDescriptionDisplay<TEnum>, TEnum>) (item => item.Value)).ToList<EnumDescriptionDisplay<TEnum>>();
    }

    private static string GetDescription(TEnum value)
    {
      return value.GetType().GetField(value.ToString()).GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }
  }
}
