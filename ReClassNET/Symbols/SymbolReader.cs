// Decompiled with JetBrains decompiler
// Type: ReClassNET.Symbols.SymbolReader
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using Dia2Lib;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ReClassNET.Symbols
{
  public class SymbolReader : IDisposable
  {
    private ComDisposableWrapper<DiaSource> diaSource;
    private ComDisposableWrapper<IDiaSession> diaSession;

    public SymbolReader()
    {
      this.diaSource = new ComDisposableWrapper<DiaSource>((DiaSource) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("E6756135-1E65-4D17-8576-610761398C3C"))));
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.diaSource == null)
        return;
      this.diaSource.Dispose();
      this.diaSource = (ComDisposableWrapper<DiaSource>) null;
      if (this.diaSession == null)
        return;
      this.diaSession.Dispose();
      this.diaSession = (ComDisposableWrapper<IDiaSession>) null;
    }

    ~SymbolReader()
    {
      this.Dispose(false);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public static void TryResolveSymbolsForModule(Module module, string searchPath)
    {
      using (ComDisposableWrapper<DiaSource> disposableWrapper = new ComDisposableWrapper<DiaSource>((DiaSource) Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("E6756135-1E65-4D17-8576-610761398C3C")))))
      {
        // ISSUE: reference to a compiler-generated method
        disposableWrapper.Interface.loadDataForExe(module.Path, searchPath, (object) null);
      }
    }

    public static SymbolReader FromModule(Module module, string searchPath)
    {
      SymbolReader symbolReader = new SymbolReader();
      // ISSUE: reference to a compiler-generated method
      symbolReader.diaSource.Interface.loadDataForExe(module.Path, searchPath, (object) null);
      symbolReader.CreateSession();
      return symbolReader;
    }

    public static SymbolReader FromDatabase(string path)
    {
      SymbolReader symbolReader = new SymbolReader();
      // ISSUE: reference to a compiler-generated method
      symbolReader.diaSource.Interface.loadDataFromPdb(path);
      symbolReader.CreateSession();
      return symbolReader;
    }

    private void CreateSession()
    {
      // ISSUE: variable of a compiler-generated type
      IDiaSession ppSession;
      // ISSUE: reference to a compiler-generated method
      this.diaSource.Interface.openSession(out ppSession);
      this.diaSession = new ComDisposableWrapper<IDiaSession>(ppSession);
    }

    public string GetSymbolString(IntPtr address, Module module)
    {
      // ISSUE: variable of a compiler-generated type
      IDiaSymbol ppSymbol;
      // ISSUE: reference to a compiler-generated method
      this.diaSession.Interface.findSymbolByRVA((uint) address.Sub(module.Start).ToInt32(), SymTagEnum.SymTagNull, out ppSymbol);
      if (ppSymbol == null)
        return (string) null;
      using (ComDisposableWrapper<IDiaSymbol> disposableWrapper = new ComDisposableWrapper<IDiaSymbol>(ppSymbol))
      {
        StringBuilder sb = new StringBuilder();
        this.ReadSymbol(disposableWrapper.Interface, sb);
        return sb.ToString();
      }
    }

    private void ReadSymbol(IDiaSymbol symbol, StringBuilder sb)
    {
      this.ReadName(symbol, sb);
    }

    private void ReadSymbolType(IDiaSymbol symbol, StringBuilder sb)
    {
      if (symbol.type == null)
        return;
      using (ComDisposableWrapper<IDiaSymbol> disposableWrapper = new ComDisposableWrapper<IDiaSymbol>(symbol.type))
        this.ReadType(disposableWrapper.Interface, sb);
    }

    private void ReadType(IDiaSymbol symbol, StringBuilder sb)
    {
      throw new NotImplementedException();
    }

    private void ReadName(IDiaSymbol symbol, StringBuilder sb)
    {
      if (string.IsNullOrEmpty(symbol.name))
        return;
      if (!string.IsNullOrEmpty(symbol.undecoratedName))
      {
        if (symbol.name == symbol.undecoratedName)
        {
          string name = symbol.name;
          if (name.StartsWith("@ILT+"))
          {
            int num = name.IndexOf('(');
            if (num != -1)
              name = name.Substring(num + 1, name.Length - 1 - num - 1);
          }
          else if (!name.StartsWith("?"))
            name = "?" + name;
          sb.Append(ReClassNET.Native.NativeMethods.UndecorateSymbolName(name).TrimStart('?', ' '));
        }
        else
          sb.Append(symbol.undecoratedName);
      }
      else
        sb.Append(symbol.name);
    }

    private void ReadData(IDiaSymbol symbol, StringBuilder sb)
    {
      throw new NotImplementedException();
    }
  }
}
