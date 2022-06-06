// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.DictionaryExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Extensions
{
  public static class DictionaryExtension
  {
    public static void RemoveWhere<TKey, TValue>(
      this IDictionary<TKey, TValue> source,
      Func<KeyValuePair<TKey, TValue>, bool> selector)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in source.Where<KeyValuePair<TKey, TValue>>(selector).ToList<KeyValuePair<TKey, TValue>>())
        source.Remove(keyValuePair.Key);
    }
  }
}
