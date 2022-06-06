// Decompiled with JetBrains decompiler
// Type: ReClassNET.Symbols.DiaUtil
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Dia2Lib;
using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Symbols
{
  internal class DiaUtil : IDisposable
  {
    public readonly IDiaDataSource DiaDataSource;
    public readonly IDiaSession DiaSession;
    private bool isDisposed;

    public DiaUtil(string pdbName)
    {
      this.DiaDataSource = (IDiaDataSource) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("E6756135-1E65-4D17-8576-610761398C3C")));
      // ISSUE: reference to a compiler-generated method
      this.DiaDataSource.loadDataFromPdb(pdbName);
      // ISSUE: reference to a compiler-generated method
      this.DiaDataSource.openSession(out this.DiaSession);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.isDisposed)
        return;
      Marshal.ReleaseComObject((object) this.DiaSession);
      Marshal.ReleaseComObject((object) this.DiaDataSource);
      this.isDisposed = true;
    }

    ~DiaUtil()
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
