// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.ValueTypeWrapper`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.Util
{
  public class ValueTypeWrapper<T> where T : struct
  {
    public ValueTypeWrapper(T value)
    {
      this.Value = value;
    }

    public T Value { get; set; }

    public static implicit operator ValueTypeWrapper<T>(T value)
    {
      return new ValueTypeWrapper<T>(value);
    }
  }
}
