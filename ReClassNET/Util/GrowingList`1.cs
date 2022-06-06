// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.GrowingList`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Collections.Generic;

namespace ReClassNET.Util
{
  public class GrowingList<T>
  {
    private readonly List<T> list;

    public T DefaultValue { get; set; }

    public int Count
    {
      get
      {
        return this.list.Count;
      }
    }

    public GrowingList()
    {
      this.list = new List<T>();
    }

    public GrowingList(T defaultValue)
      : this()
    {
      this.DefaultValue = defaultValue;
    }

    private void GrowToSize(int size)
    {
      this.list.Capacity = size;
      for (int count = this.list.Count; count <= size; ++count)
        this.list.Add(this.DefaultValue);
    }

    private void CheckIndex(int index)
    {
      if (index < this.list.Count)
        return;
      this.GrowToSize(index);
    }

    public T this[int index]
    {
      get
      {
        this.CheckIndex(index);
        return this.list[index];
      }
      set
      {
        this.CheckIndex(index);
        this.list[index] = value;
      }
    }
  }
}
