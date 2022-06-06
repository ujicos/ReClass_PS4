// Decompiled with JetBrains decompiler
// Type: ReClassNET.Native.NativeMethodsContract
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Drawing;

namespace ReClassNET.Native
{
  internal abstract class NativeMethodsContract : INativeMethods
  {
    public IntPtr LoadLibrary(string fileName)
    {
      throw new NotImplementedException();
    }

    public IntPtr GetProcAddress(IntPtr handle, string name)
    {
      throw new NotImplementedException();
    }

    public void FreeLibrary(IntPtr handle)
    {
      throw new NotImplementedException();
    }

    public Icon GetIconForFile(string path)
    {
      throw new NotImplementedException();
    }

    public void EnableDebugPrivileges()
    {
      throw new NotImplementedException();
    }

    public string UndecorateSymbolName(string name)
    {
      throw new NotImplementedException();
    }

    public void SetProcessDpiAwareness()
    {
      throw new NotImplementedException();
    }

    public bool RegisterExtension(
      string fileExtension,
      string extensionId,
      string applicationPath,
      string applicationName)
    {
      throw new NotImplementedException();
    }

    public void UnregisterExtension(string fileExtension, string extensionId)
    {
      throw new NotImplementedException();
    }
  }
}
