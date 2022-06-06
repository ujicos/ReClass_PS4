// Decompiled with JetBrains decompiler
// Type: Dia2Lib.IDiaDataSource
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Dia2Lib
{
  [CompilerGenerated]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("79F1BB5F-B66E-48E5-B6A9-1545C323CA3D")]
  [TypeIdentifier]
  [ComImport]
  public interface IDiaDataSource
  {
    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap1_1();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void loadDataFromPdb([MarshalAs(UnmanagedType.LPWStr), In] string pdbPath);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap2_1();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void loadDataForExe([MarshalAs(UnmanagedType.LPWStr), In] string executable, [MarshalAs(UnmanagedType.LPWStr), In] string searchPath, [MarshalAs(UnmanagedType.IUnknown), In] object pCallback);

    [SpecialName]
    [MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
    sealed extern void _VtblGap3_1();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void openSession([MarshalAs(UnmanagedType.Interface)] out IDiaSession ppSession);
  }
}
