// Decompiled with JetBrains decompiler
// Type: ReClassNET.Core.InternalCoreFunctions
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Util;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReClassNET.Core
{
  internal class InternalCoreFunctions : NativeCoreWrapper, IInternalCoreFunctions, IDisposable
  {
    private static readonly Keys[] empty = new Keys[0];
    private const string CoreFunctionsModuleWindows = "NativeCore.dll";
    private const string CoreFunctionsModuleUnix = "NativeCore.so";
    private readonly IntPtr handle;
    private readonly InternalCoreFunctions.DisassembleCodeDelegate disassembleCodeDelegate;
    private readonly InternalCoreFunctions.InitializeInputDelegate initializeInputDelegate;
    private readonly InternalCoreFunctions.GetPressedKeysDelegate getPressedKeysDelegate;
    private readonly InternalCoreFunctions.ReleaseInputDelegate releaseInputDelegate;

    private InternalCoreFunctions(IntPtr handle)
      : base(handle)
    {
      this.handle = handle;
      this.disassembleCodeDelegate = NativeCoreWrapper.GetFunctionDelegate<InternalCoreFunctions.DisassembleCodeDelegate>(handle, "DisassembleCode");
      this.initializeInputDelegate = NativeCoreWrapper.GetFunctionDelegate<InternalCoreFunctions.InitializeInputDelegate>(handle, "InitializeInput");
      this.getPressedKeysDelegate = NativeCoreWrapper.GetFunctionDelegate<InternalCoreFunctions.GetPressedKeysDelegate>(handle, "GetPressedKeys");
      this.releaseInputDelegate = NativeCoreWrapper.GetFunctionDelegate<InternalCoreFunctions.ReleaseInputDelegate>(handle, "ReleaseInput");
    }

    public static InternalCoreFunctions Create()
    {
      string name = Path.Combine(PathUtil.ExecutableFolderPath, ReClassNET.Native.NativeMethods.IsUnix() ? "NativeCore.so" : "NativeCore.dll");
      IntPtr num = ReClassNET.Native.NativeMethods.LoadLibrary(name);
      if (num.IsNull())
        throw new FileNotFoundException("Failed to load native core functions! Couldnt find at location " + name);
      return new InternalCoreFunctions(num);
    }

    ~InternalCoreFunctions()
    {
      this.ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
      ReClassNET.Native.NativeMethods.FreeLibrary(this.handle);
    }

    public void Dispose()
    {
      this.ReleaseUnmanagedResources();
      GC.SuppressFinalize((object) this);
    }

    public bool DisassembleCode(
      IntPtr address,
      int length,
      IntPtr virtualAddress,
      bool determineStaticInstructionBytes,
      EnumerateInstructionCallback callback)
    {
      return this.disassembleCodeDelegate(address, (IntPtr) length, virtualAddress, determineStaticInstructionBytes, callback);
    }

    public IntPtr InitializeInput()
    {
      return this.initializeInputDelegate();
    }

    public Keys[] GetPressedKeys(IntPtr handle)
    {
      IntPtr pressedKeysArrayPtr;
      int length;
      if (!this.getPressedKeysDelegate(handle, out pressedKeysArrayPtr, out length) || length == 0)
        return InternalCoreFunctions.empty;
      int[] destination = new int[length];
      Marshal.Copy(pressedKeysArrayPtr, destination, 0, length);
      return (Keys[]) destination;
    }

    public void ReleaseInput(IntPtr handle)
    {
      this.releaseInputDelegate(handle);
    }

    [return: MarshalAs(UnmanagedType.I1)]
    private delegate bool DisassembleCodeDelegate(
      IntPtr address,
      IntPtr length,
      IntPtr virtualAddress,
      [MarshalAs(UnmanagedType.I1)] bool determineStaticInstructionBytes,
      [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateInstructionCallback callback);

    private delegate IntPtr InitializeInputDelegate();

    private delegate bool GetPressedKeysDelegate(
      IntPtr handle,
      out IntPtr pressedKeysArrayPtr,
      out int length);

    private delegate void ReleaseInputDelegate(IntPtr handle);
  }
}
