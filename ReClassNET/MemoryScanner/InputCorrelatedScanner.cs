// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.InputCorrelatedScanner
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Input;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.MemoryScanner
{
  public class InputCorrelatedScanner : Scanner
  {
    private readonly RemoteProcess process;
    private readonly KeyboardInput input;
    private readonly List<KeyboardHotkey> hotkeys;
    private bool shouldHaveChangedSinceLastScan;

    public int ScanCount { get; private set; }

    public InputCorrelatedScanner(
      RemoteProcess process,
      KeyboardInput input,
      IEnumerable<KeyboardHotkey> hotkeys,
      ScanValueType valueType)
      : base(process, InputCorrelatedScanner.CreateScanSettings(valueType))
    {
      this.process = process;
      this.input = input;
      this.hotkeys = hotkeys.ToList<KeyboardHotkey>();
    }

    private static ScanSettings CreateScanSettings(ScanValueType valueType)
    {
      ScanSettings scanSettings = ScanSettings.Default;
      scanSettings.ValueType = valueType;
      return scanSettings;
    }

    private IScanComparer CreateScanComparer(ScanCompareType compareType)
    {
      switch (this.Settings.ValueType)
      {
        case ScanValueType.Byte:
          return (IScanComparer) new ByteMemoryComparer(compareType, (byte) 0, (byte) 0);
        case ScanValueType.Short:
          return (IScanComparer) new ShortMemoryComparer(compareType, (short) 0, (short) 0, this.process.BitConverter);
        case ScanValueType.Integer:
          return (IScanComparer) new IntegerMemoryComparer(compareType, 0, 0, this.process.BitConverter);
        case ScanValueType.Long:
          return (IScanComparer) new LongMemoryComparer(compareType, 0L, 0L, this.process.BitConverter);
        case ScanValueType.Float:
          return (IScanComparer) new FloatMemoryComparer(compareType, ScanRoundMode.Normal, 2, 0.0f, 0.0f, this.process.BitConverter);
        case ScanValueType.Double:
          return (IScanComparer) new DoubleMemoryComparer(compareType, ScanRoundMode.Normal, 2, 0.0, 0.0, this.process.BitConverter);
        default:
          throw new InvalidOperationException();
      }
    }

    public Task Initialize()
    {
      return (Task) this.Search(this.CreateScanComparer(ScanCompareType.Unknown), (IProgress<int>) null, CancellationToken.None);
    }

    public void CorrelateInput()
    {
      if (this.shouldHaveChangedSinceLastScan)
        return;
      Keys[] keys = ((IEnumerable<Keys>) this.input.GetPressedKeys()).Select<Keys, Keys>((Func<Keys, Keys>) (k => k & Keys.KeyCode)).Where<Keys>((Func<Keys, bool>) (k => (uint) k > 0U)).ToArray<Keys>();
      if (keys.Length == 0 || !this.hotkeys.Any<KeyboardHotkey>((Func<KeyboardHotkey, bool>) (h => h.Matches(keys))))
        return;
      this.shouldHaveChangedSinceLastScan = true;
    }

    public async Task RefineResults(CancellationToken ct, IProgress<int> progress)
    {
      InputCorrelatedScanner correlatedScanner = this;
      ScanCompareType compareType = correlatedScanner.shouldHaveChangedSinceLastScan ? ScanCompareType.Changed : ScanCompareType.NotChanged;
      if (compareType == ScanCompareType.Changed)
        await Task.Delay(TimeSpan.FromMilliseconds(200.0), ct);
      int num = await correlatedScanner.Search(correlatedScanner.CreateScanComparer(compareType), progress, ct) ? 1 : 0;
      correlatedScanner.shouldHaveChangedSinceLastScan = false;
      correlatedScanner.ScanCount++;
    }
  }
}
