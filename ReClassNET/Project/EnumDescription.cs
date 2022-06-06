// Decompiled with JetBrains decompiler
// Type: ReClassNET.Project.EnumDescription
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Project
{
  public class EnumDescription
  {
    public static EnumDescription Default
    {
      get
      {
        return new EnumDescription() { Name = "DummyEnum" };
      }
    }

    public string Name { get; set; } = string.Empty;

    public bool UseFlagsMode { get; private set; }

    public EnumDescription.UnderlyingTypeSize Size { get; private set; } = EnumDescription.UnderlyingTypeSize.FourBytes;

    public IReadOnlyList<KeyValuePair<string, long>> Values { get; private set; } = (IReadOnlyList<KeyValuePair<string, long>>) new Dictionary<string, long>().ToList<KeyValuePair<string, long>>();

    public void SetData(
      bool useFlagsMode,
      EnumDescription.UnderlyingTypeSize size,
      IEnumerable<KeyValuePair<string, long>> values)
    {
      List<KeyValuePair<string, long>> list = values.OrderBy<KeyValuePair<string, long>, long>((Func<KeyValuePair<string, long>, long>) (t => t.Value)).ToList<KeyValuePair<string, long>>();
      if (useFlagsMode)
      {
        ulong maxValue = ulong.MaxValue;
        switch (size)
        {
          case EnumDescription.UnderlyingTypeSize.OneByte:
            maxValue = (ulong) byte.MaxValue;
            break;
          case EnumDescription.UnderlyingTypeSize.TwoBytes:
            maxValue = (ulong) ushort.MaxValue;
            break;
          case EnumDescription.UnderlyingTypeSize.FourBytes:
            maxValue = (ulong) uint.MaxValue;
            break;
        }
        if (list.Select<KeyValuePair<string, long>, ulong>((Func<KeyValuePair<string, long>, ulong>) (kv => (ulong) kv.Value)).Max<ulong>() > maxValue)
          throw new ArgumentOutOfRangeException();
      }
      else
      {
        long minValue = long.MinValue;
        long maxValue = long.MaxValue;
        switch (size)
        {
          case EnumDescription.UnderlyingTypeSize.OneByte:
            minValue = (long) sbyte.MinValue;
            maxValue = (long) sbyte.MaxValue;
            break;
          case EnumDescription.UnderlyingTypeSize.TwoBytes:
            minValue = (long) short.MinValue;
            maxValue = (long) short.MaxValue;
            break;
          case EnumDescription.UnderlyingTypeSize.FourBytes:
            minValue = (long) int.MinValue;
            maxValue = (long) int.MaxValue;
            break;
        }
        if (list.Max<KeyValuePair<string, long>>((Func<KeyValuePair<string, long>, long>) (kv => kv.Value)) > maxValue || list.Min<KeyValuePair<string, long>>((Func<KeyValuePair<string, long>, long>) (kv => kv.Value)) < minValue)
          throw new ArgumentOutOfRangeException();
      }
      this.UseFlagsMode = useFlagsMode;
      this.Size = size;
      this.Values = (IReadOnlyList<KeyValuePair<string, long>>) list;
    }

    public enum UnderlyingTypeSize
    {
      OneByte = 1,
      TwoBytes = 2,
      FourBytes = 4,
      EightBytes = 8,
    }
  }
}
