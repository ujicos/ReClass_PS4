// Decompiled with JetBrains decompiler
// Type: ReClassNET.Input.KeyboardInput
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Windows.Forms;

namespace ReClassNET.Input
{
  public class KeyboardInput : IDisposable
  {
    private readonly IntPtr handle;

    public KeyboardInput()
    {
      this.handle = Program.CoreFunctions.InitializeInput();
    }

    public void Dispose()
    {
      this.ReleaseUnmanagedResources();
      GC.SuppressFinalize((object) this);
    }

    ~KeyboardInput()
    {
      this.ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
      Program.CoreFunctions.ReleaseInput(this.handle);
    }

    public Keys[] GetPressedKeys()
    {
      return Program.CoreFunctions.GetPressedKeys(this.handle);
    }
  }
}
