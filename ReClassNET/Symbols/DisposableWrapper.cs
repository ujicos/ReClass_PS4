// Decompiled with JetBrains decompiler
// Type: ReClassNET.Symbols.DisposableWrapper
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Symbols
{
  internal class DisposableWrapper : IDisposable
  {
    protected object Object;

    private void ObjectInvariants()
    {
    }

    public DisposableWrapper(object obj)
    {
      this.Object = obj;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      Marshal.ReleaseComObject(this.Object);
    }

    ~DisposableWrapper()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }
  }
}
