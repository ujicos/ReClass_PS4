// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.EnumerableExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ReClassNET.Extensions
{
  public static class EnumerableExtension
  {
    [DebuggerStepThrough]
    public static bool None<TSource>(this IEnumerable<TSource> source)
    {
      return !source.Any<TSource>();
    }

    [DebuggerStepThrough]
    public static bool None<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return !source.Any<TSource>(predicate);
    }

    [DebuggerStepThrough]
    public static IEnumerable<TSource> WhereNot<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      return source.Where<TSource>((Func<TSource, bool>) (item => !predicate(item)));
    }

    [DebuggerStepThrough]
    public static int FindIndex<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      int num = 0;
      foreach (TSource source1 in source)
      {
        if (predicate(source1))
          return num;
        ++num;
      }
      return -1;
    }

    [DebuggerStepThrough]
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
    {
      foreach (TSource source1 in source)
        func(source1);
    }

    [DebuggerStepThrough]
    public static IEnumerable<TSource> Traverse<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, IEnumerable<TSource>> childSelector)
    {
      Queue<TSource> queue = new Queue<TSource>(source);
      while (queue.Count > 0)
      {
        TSource next = queue.Dequeue();
        yield return next;
        foreach (TSource source1 in childSelector(next))
          queue.Enqueue(source1);
        next = default (TSource);
      }
    }

    [DebuggerStepThrough]
    public static IEnumerable<TSource> TakeWhileInclusive<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      foreach (TSource source1 in source)
      {
        TSource item = source1;
        yield return item;
        if (!predicate(item))
          break;
        item = default (TSource);
      }
    }

    [DebuggerStepThrough]
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
    {
      HashSet<TKey> knownKeys = new HashSet<TKey>();
      foreach (TSource source1 in source)
      {
        if (knownKeys.Add(keySelector(source1)))
          yield return source1;
      }
    }

    [DebuggerStepThrough]
    public static bool IsEquivalentTo<T>(this IEnumerable<T> source, IEnumerable<T> other)
    {
      List<T> expected = new List<T>(source);
      return !other.Any<T>((Func<T, bool>) (item => !expected.Remove(item))) && expected.Count == 0;
    }

    public static TSource PredicateOrFirst<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, bool> predicate)
    {
      TSource source1 = default (TSource);
      bool flag = true;
      foreach (TSource source2 in source)
      {
        if (predicate(source2))
          return source2;
        if (flag)
        {
          source1 = source2;
          flag = false;
        }
      }
      if (flag)
        throw new InvalidOperationException("Sequence contains no elements");
      return source1;
    }

    public static IEnumerable<IEnumerable<T>> GroupWhile<T>(
      this IEnumerable<T> source,
      Func<T, T, bool> condition)
    {
      using (IEnumerator<T> it = source.GetEnumerator())
      {
        if (it.MoveNext())
        {
          T obj = it.Current;
          List<T> objList = new List<T>() { obj };
          while (it.MoveNext())
          {
            T item = it.Current;
            if (!condition(obj, item))
            {
              yield return (IEnumerable<T>) objList;
              objList = new List<T>();
            }
            objList.Add(item);
            obj = item;
            item = default (T);
          }
          yield return (IEnumerable<T>) objList;
        }
      }
    }
  }
}
