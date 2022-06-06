// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.Utils
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Util
{
  public static class Utils
  {
    public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
    {
      return Utils.Min<T, U>(item1, item2, keySelector, (IComparer<U>) Comparer<U>.Default);
    }

    public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector, IComparer<U> comparer)
    {
      return comparer.Compare(keySelector(item1), keySelector(item2)) < 0 ? item1 : item2;
    }

    public static T Max<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
    {
      return Utils.Max<T, U>(item1, item2, keySelector, (IComparer<U>) Comparer<U>.Default);
    }

    public static T1 Max<T1, T2>(
      T1 item1,
      T1 item2,
      Func<T1, T2> keySelector,
      IComparer<T2> comparer)
    {
      return comparer.Compare(keySelector(item1), keySelector(item2)) > 0 ? item1 : item2;
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
      T obj = lhs;
      lhs = rhs;
      rhs = obj;
    }

    public static string RandomString(int length)
    {
      return new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>) (s => s[Program.GlobalRandom.Next(s.Length)])).ToArray<char>());
    }
  }
}
