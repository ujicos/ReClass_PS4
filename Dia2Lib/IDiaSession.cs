// Decompiled with JetBrains decompiler
// Type: Dia2Lib.IDiaSession
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Dia2Lib
{
  [CompilerGenerated]
  [Guid("2F609EE1-D1C8-4E24-8288-3326BADCD211")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [TypeIdentifier]
  [ComImport]
  public interface IDiaSession
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_11();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void findSymbolByRVA([In] uint rva, [In] SymTagEnum symTag, [MarshalAs(UnmanagedType.Interface)] out IDiaSymbol ppSymbol);
  }
}
