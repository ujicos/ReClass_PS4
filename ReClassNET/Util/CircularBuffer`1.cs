// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.CircularBuffer`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections;
using System.Collections.Generic;

namespace ReClassNET.Util
{
  public class CircularBuffer<T> : IEnumerable<T>, IEnumerable
  {
    private readonly T[] buffer;
    private int head;
    private int tail;

    public CircularBuffer(int capacity)
    {
      if (capacity < 0)
        throw new ArgumentOutOfRangeException(nameof (capacity));
      this.buffer = new T[capacity];
      this.head = capacity - 1;
    }

    public int Count { get; private set; }

    public int Capacity
    {
      get
      {
        return this.buffer.Length;
      }
    }

    public T Head
    {
      get
      {
        return this.buffer[this.head];
      }
    }

    public T Tail
    {
      get
      {
        return this.buffer[this.tail];
      }
    }

    public T Enqueue(T item)
    {
      this.head = (this.head + 1) % this.Capacity;
      T obj = this.buffer[this.head];
      this.buffer[this.head] = item;
      if (this.Count == this.Capacity)
      {
        this.tail = (this.tail + 1) % this.Capacity;
        return obj;
      }
      ++this.Count;
      return obj;
    }

    public T Dequeue()
    {
      if (this.Count == 0)
        throw new InvalidOperationException();
      T obj = this.buffer[this.head];
      this.buffer[this.head] = default (T);
      this.head = this.head != 0 ? (this.head - 1) % this.Capacity : this.Capacity - 1;
      --this.Count;
      return obj;
    }

    public void Clear()
    {
      this.head = this.Capacity - 1;
      this.tail = 0;
      this.Count = 0;
    }

    public T this[int index]
    {
      get
      {
        if (index < 0 || index >= this.Count)
          throw new ArgumentOutOfRangeException(nameof (index));
        return this.buffer[(this.tail + index) % this.Capacity];
      }
      set
      {
        if (index < 0 || index >= this.Count)
          throw new ArgumentOutOfRangeException(nameof (index));
        this.buffer[(this.tail + index) % this.Capacity] = value;
      }
    }

    public int IndexOf(T item)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if (object.Equals((object) item, (object) this[index]))
          return index;
      }
      return -1;
    }

    public void Insert(int index, T item)
    {
      if (index < 0 || index > this.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (this.Count == index)
      {
        this.Enqueue(item);
      }
      else
      {
        T obj = this[this.Count - 1];
        for (int index1 = index; index1 < this.Count - 2; ++index1)
          this[index1 + 1] = this[index1];
        this[index] = item;
        this.Enqueue(obj);
      }
    }

    public void RemoveAt(int index)
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      for (int index1 = index; index1 > 0; --index1)
        this[index1] = this[index1 - 1];
      this.Dequeue();
    }

    public IEnumerator<T> GetEnumerator()
    {
      if (this.Count != 0 && this.Capacity != 0)
      {
        for (int i = 0; i < this.Count; ++i)
          yield return this[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
