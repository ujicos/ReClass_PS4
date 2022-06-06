// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Scanner
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReClassNET.MemoryScanner
{
  public class Scanner : IDisposable
  {
    private readonly RemoteProcess process;
    private readonly CircularBuffer<ScanResultStore> stores;
    private bool isFirstScan;

    public ScanSettings Settings { get; }

    private ScanResultStore CurrentStore
    {
      get
      {
        return this.stores.Head;
      }
    }

    public int TotalResultCount
    {
      get
      {
        ScanResultStore currentStore = this.CurrentStore;
        return currentStore == null ? 0 : currentStore.TotalResultCount;
      }
    }

    public bool CanUndoLastScan
    {
      get
      {
        return this.stores.Count > 1;
      }
    }

    public Scanner(RemoteProcess process, ScanSettings settings)
    {
      this.stores = new CircularBuffer<ScanResultStore>(3);
      this.process = process;
      this.Settings = settings;
      this.isFirstScan = true;
    }

    public void Dispose()
    {
      foreach (ScanResultStore store in this.stores)
        store?.Dispose();
      this.stores.Clear();
    }

    public IEnumerable<ScanResult> GetResults()
    {
      return this.CurrentStore == null ? Enumerable.Empty<ScanResult>() : this.CurrentStore.GetResultBlocks().SelectMany<ScanResultBlock, ScanResult>((Func<ScanResultBlock, IEnumerable<ScanResult>>) (rb => rb.Results.Select<ScanResult, ScanResult>((Func<ScanResult, ScanResult>) (r =>
      {
        ScanResult scanResult = r.Clone();
        scanResult.Address = scanResult.Address.Add(rb.Start);
        return scanResult;
      }))));
    }

    public void UndoLastScan()
    {
      if (!this.CanUndoLastScan)
        throw new InvalidOperationException();
      this.stores.Dequeue()?.Dispose();
    }

    private ScanResultStore CreateStore()
    {
      return new ScanResultStore(this.Settings.ValueType, Path.GetTempPath());
    }

    private IList<Section> GetSearchableSections()
    {
      return (IList<Section>) this.process.Sections.Where<Section>((Func<Section, bool>) (s => !s.Protection.HasFlag((Enum) SectionProtection.Guard))).Where<Section>((Func<Section, bool>) (s => s.Start.IsInRange(this.Settings.StartAddress, this.Settings.StopAddress) || this.Settings.StartAddress.IsInRange(s.Start, s.End) || this.Settings.StopAddress.IsInRange(s.Start, s.End))).Where<Section>((Func<Section, bool>) (s =>
      {
        bool flag;
        switch (s.Type)
        {
          case SectionType.Private:
            flag = this.Settings.ScanPrivateMemory;
            break;
          case SectionType.Mapped:
            flag = this.Settings.ScanMappedMemory;
            break;
          case SectionType.Image:
            flag = this.Settings.ScanImageMemory;
            break;
          default:
            flag = false;
            break;
        }
        return flag;
      })).Where<Section>((Func<Section, bool>) (s =>
      {
        bool flag1 = s.Protection.HasFlag((Enum) SectionProtection.Write);
        bool flag2;
        switch (this.Settings.ScanWritableMemory)
        {
          case SettingState.Yes:
            flag2 = flag1;
            break;
          case SettingState.No:
            flag2 = !flag1;
            break;
          default:
            flag2 = true;
            break;
        }
        return flag2;
      })).Where<Section>((Func<Section, bool>) (s =>
      {
        bool flag1 = s.Protection.HasFlag((Enum) SectionProtection.Execute);
        bool flag2;
        switch (this.Settings.ScanExecutableMemory)
        {
          case SettingState.Yes:
            flag2 = flag1;
            break;
          case SettingState.No:
            flag2 = !flag1;
            break;
          default:
            flag2 = true;
            break;
        }
        return flag2;
      })).Where<Section>((Func<Section, bool>) (s =>
      {
        bool flag1 = s.Protection.HasFlag((Enum) SectionProtection.CopyOnWrite);
        bool flag2;
        switch (this.Settings.ScanCopyOnWriteMemory)
        {
          case SettingState.Yes:
            flag2 = flag1;
            break;
          case SettingState.No:
            flag2 = !flag1;
            break;
          default:
            flag2 = true;
            break;
        }
        return flag2;
      })).ToList<Section>();
    }

    public Task<bool> Search(
      IScanComparer comparer,
      IProgress<int> progress,
      CancellationToken ct)
    {
      return !this.isFirstScan ? this.NextScan(comparer, progress, ct) : this.FirstScan(comparer, progress, ct);
    }

    private Task<bool> FirstScan(
      IScanComparer comparer,
      IProgress<int> progress,
      CancellationToken ct)
    {
      ScanResultStore store = this.CreateStore();
      IList<Section> searchableSections = this.GetSearchableSections();
      if (searchableSections.Count == 0)
        return Task.FromResult<bool>(true);
      List<Scanner.ConsolidatedMemoryRegion> regions = Scanner.ConsolidateSections(searchableSections);
      int initialBufferSize = (int) (regions.Average<Scanner.ConsolidatedMemoryRegion>((Func<Scanner.ConsolidatedMemoryRegion, int>) (s => s.Size)) + 1.0);
      progress?.Report(0);
      int counter = 0;
      float totalSectionCount = (float) regions.Count;
      return Task.Run<bool>((Func<bool>) (() =>
      {
        ParallelLoopResult parallelLoopResult = Parallel.ForEach<Scanner.ConsolidatedMemoryRegion, ScannerContext>((IEnumerable<Scanner.ConsolidatedMemoryRegion>) regions, (Func<ScannerContext>) (() => new ScannerContext(Scanner.CreateWorker(this.Settings, comparer), initialBufferSize)), (Func<Scanner.ConsolidatedMemoryRegion, ParallelLoopState, long, ScannerContext, ScannerContext>) ((s, state, _, context) =>
        {
          if (!ct.IsCancellationRequested)
          {
            IntPtr num1 = s.Address;
            IntPtr num2 = s.Address + s.Size;
            int size = s.Size;
            if (this.Settings.StartAddress.IsInRange(num1, num2))
            {
              size -= this.Settings.StartAddress.Sub(num1).ToInt32();
              num1 = this.Settings.StartAddress;
            }
            if (this.Settings.StopAddress.IsInRange(num1, num2))
              size -= num2.Sub(this.Settings.StopAddress).ToInt32();
            context.EnsureBufferSize(size);
            byte[] buffer = context.Buffer;
            if (this.process.ReadRemoteMemoryIntoBuffer(num1, ref buffer, 0, size))
            {
              List<ScanResult> list = context.Worker.Search(buffer, size, ct).OrderBy<ScanResult, IntPtr>((Func<ScanResult, IntPtr>) (r => r.Address), (IComparer<IntPtr>) IntPtrComparer.Instance).ToList<ScanResult>();
              if (list.Count > 0)
                store.AddBlock(Scanner.CreateResultBlock((IReadOnlyList<ScanResult>) list, num1));
            }
            progress?.Report((int) ((double) Interlocked.Increment(ref counter) / (double) totalSectionCount * 100.0));
          }
          else
            state.Stop();
          return context;
        }), (Action<ScannerContext>) (w => {}));
        store.Finish();
        this.stores.Enqueue(store)?.Dispose();
        this.isFirstScan = false;
        return parallelLoopResult.IsCompleted;
      }), ct);
    }

    private Task<bool> NextScan(
      IScanComparer comparer,
      IProgress<int> progress,
      CancellationToken ct)
    {
      ScanResultStore store = this.CreateStore();
      progress?.Report(0);
      int counter = 0;
      float totalResultCount = (float) this.CurrentStore.TotalResultCount;
      return Task.Run<bool>((Func<bool>) (() =>
      {
        ParallelLoopResult parallelLoopResult = Parallel.ForEach<ScanResultBlock, ScannerContext>(this.CurrentStore.GetResultBlocks(), (Func<ScannerContext>) (() => new ScannerContext(Scanner.CreateWorker(this.Settings, comparer), 0)), (Func<ScanResultBlock, ParallelLoopState, long, ScannerContext, ScannerContext>) ((b, state, _, context) =>
        {
          if (!ct.IsCancellationRequested)
          {
            context.EnsureBufferSize(b.Size);
            byte[] buffer = context.Buffer;
            if (this.process.ReadRemoteMemoryIntoBuffer(b.Start, ref buffer, 0, b.Size))
            {
              List<ScanResult> list = context.Worker.Search(buffer, buffer.Length, (IEnumerable<ScanResult>) b.Results, ct).OrderBy<ScanResult, IntPtr>((Func<ScanResult, IntPtr>) (r => r.Address), (IComparer<IntPtr>) IntPtrComparer.Instance).ToList<ScanResult>();
              if (list.Count > 0)
                store.AddBlock(Scanner.CreateResultBlock((IReadOnlyList<ScanResult>) list, b.Start));
            }
            progress?.Report((int) ((double) Interlocked.Add(ref counter, b.Results.Count) / (double) totalResultCount * 100.0));
          }
          else
            state.Stop();
          return context;
        }), (Action<ScannerContext>) (w => {}));
        store.Finish();
        this.stores.Enqueue(store)?.Dispose();
        return parallelLoopResult.IsCompleted;
      }), ct);
    }

    private static List<Scanner.ConsolidatedMemoryRegion> ConsolidateSections(
      IList<Section> sections)
    {
      List<Scanner.ConsolidatedMemoryRegion> consolidatedMemoryRegionList = new List<Scanner.ConsolidatedMemoryRegion>();
      if (sections.Count > 0)
      {
        IntPtr start = sections[0].Start;
        IntPtr size = sections[0].Size;
        int num1 = size.ToInt32();
        for (int index = 1; index < sections.Count; ++index)
        {
          Section section = sections[index];
          if (start + num1 != section.Start)
          {
            consolidatedMemoryRegionList.Add(new Scanner.ConsolidatedMemoryRegion()
            {
              Address = start,
              Size = num1
            });
            start = section.Start;
            size = section.Size;
            num1 = size.ToInt32();
          }
          else
          {
            int num2 = num1;
            size = section.Size;
            int int32 = size.ToInt32();
            num1 = num2 + int32;
          }
        }
        consolidatedMemoryRegionList.Add(new Scanner.ConsolidatedMemoryRegion()
        {
          Address = start,
          Size = num1
        });
      }
      return consolidatedMemoryRegionList;
    }

    private static ScanResultBlock CreateResultBlock(
      IReadOnlyList<ScanResult> results,
      IntPtr previousStartAddress)
    {
      ScanResult scanResult1 = results.First<ScanResult>();
      ScanResult scanResult2 = results.Last<ScanResult>();
      IntPtr start = scanResult1.Address.Add(previousStartAddress);
      IntPtr end = scanResult2.Address.Add(previousStartAddress) + scanResult2.ValueSize;
      IntPtr address = scanResult1.Address;
      foreach (ScanResult result in (IEnumerable<ScanResult>) results)
        result.Address = result.Address.Sub(address);
      return new ScanResultBlock(start, end, results);
    }

    private static IScannerWorker CreateWorker(
      ScanSettings settings,
      IScanComparer comparer)
    {
      switch (comparer)
      {
        case ISimpleScanComparer comparer1:
          return (IScannerWorker) new SimpleScannerWorker(settings, comparer1);
        case IComplexScanComparer comparer1:
          return (IScannerWorker) new ComplexScannerWorker(settings, comparer1);
        default:
          throw new Exception();
      }
    }

    private class ConsolidatedMemoryRegion
    {
      public IntPtr Address { get; set; }

      public int Size { get; set; }
    }
  }
}
