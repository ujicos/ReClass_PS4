// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.IInternalCoreFunctions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Windows.Forms;

namespace ReClassNET.Core
{
  public interface IInternalCoreFunctions
  {
    bool DisassembleCode(
      IntPtr address,
      int length,
      IntPtr virtualAddress,
      bool determineStaticInstructionBytes,
      EnumerateInstructionCallback callback);

    IntPtr InitializeInput();

    Keys[] GetPressedKeys(IntPtr handle);

    void ReleaseInput(IntPtr handle);
  }
}
