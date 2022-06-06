// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.BindingDisplayWrapper`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.UI
{
  public class BindingDisplayWrapper<T>
  {
    private readonly Func<T, string> toString;

    public T Value { get; }

    public BindingDisplayWrapper(T value, Func<T, string> toString)
    {
      this.Value = value;
      this.toString = toString;
    }

    public override string ToString()
    {
      return this.toString(this.Value);
    }
  }
}
