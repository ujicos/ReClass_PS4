// Decompiled with JetBrains decompiler
// Type: ReClassNET.Symbols.ComDisposableWrapper`1
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.Symbols
{
  internal class ComDisposableWrapper<T> : DisposableWrapper
  {
    public T Interface
    {
      get
      {
        return (T) this.Object;
      }
    }

    public ComDisposableWrapper(T com)
      : base((object) com)
    {
    }
  }
}
