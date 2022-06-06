// Decompiled with JetBrains decompiler
// Type: ReClassNET.Native.INativeMethods
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Drawing;

namespace ReClassNET.Native
{
  internal interface INativeMethods
  {
    IntPtr LoadLibrary(string fileName);

    IntPtr GetProcAddress(IntPtr handle, string name);

    void FreeLibrary(IntPtr handle);

    Icon GetIconForFile(string path);

    void EnableDebugPrivileges();

    string UndecorateSymbolName(string name);

    void SetProcessDpiAwareness();

    bool RegisterExtension(
      string fileExtension,
      string extensionId,
      string applicationPath,
      string applicationName);

    void UnregisterExtension(string fileExtension, string extensionId);
  }
}
