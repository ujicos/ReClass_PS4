// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScanSettings
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner
{
  public class ScanSettings
  {
    public IntPtr StartAddress { get; set; } = IntPtr.Zero;

    public IntPtr StopAddress { get; set; } = (IntPtr) long.MaxValue;

    public SettingState ScanWritableMemory { get; set; }

    public SettingState ScanExecutableMemory { get; set; } = SettingState.Indeterminate;

    public SettingState ScanCopyOnWriteMemory { get; set; } = SettingState.No;

    public bool ScanPrivateMemory { get; set; } = true;

    public bool ScanImageMemory { get; set; } = true;

    public bool ScanMappedMemory { get; set; }

    public bool EnableFastScan { get; set; } = true;

    public int FastScanAlignment { get; set; } = 4;

    public ScanValueType ValueType { get; set; } = ScanValueType.Integer;

    public static ScanSettings Default
    {
      get
      {
        return new ScanSettings();
      }
    }
  }
}
