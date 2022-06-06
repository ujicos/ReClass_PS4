// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScanResultStore
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ReClassNET.MemoryScanner
{
  internal class ScanResultStore : IDisposable
  {
    private readonly List<ScanResultBlock> store = new List<ScanResultBlock>();
    private const int MaximumMemoryResultsCount = 10000000;
    private readonly string storePath;
    private FileStream fileStream;
    private ScanResultStore.StorageMode mode;
    private readonly ScanValueType valueType;

    public int TotalResultCount { get; private set; }

    public ScanResultStore(ScanValueType valueType, string storePath)
    {
      this.valueType = valueType;
      this.storePath = Path.Combine(storePath, string.Format("ReClass.NET_MemoryScanner_{0}.tmp", (object) Guid.NewGuid()));
    }

    public void Dispose()
    {
      this.Finish();
      this.store.Clear();
      try
      {
        if (!File.Exists(this.storePath))
          return;
        File.Delete(this.storePath);
      }
      catch
      {
      }
    }

    public void Finish()
    {
      if (this.mode != ScanResultStore.StorageMode.File)
        return;
      this.fileStream?.Dispose();
      this.fileStream = (FileStream) null;
    }

    public IEnumerable<ScanResultBlock> GetResultBlocks()
    {
      return this.mode != ScanResultStore.StorageMode.Memory ? this.ReadBlocksFromFile() : (IEnumerable<ScanResultBlock>) this.store;
    }

    public void AddBlock(ScanResultBlock block)
    {
      lock (this.store)
      {
        this.TotalResultCount += block.Results.Count;
        if (this.mode == ScanResultStore.StorageMode.Memory)
        {
          if (this.TotalResultCount > 10000000)
          {
            this.mode = ScanResultStore.StorageMode.File;
            this.fileStream = File.OpenWrite(this.storePath);
            foreach (ScanResultBlock block1 in this.store)
              this.AppendBlockToFile(block1);
            this.store.Clear();
            this.store.TrimExcess();
            this.AppendBlockToFile(block);
          }
          else
            this.store.Add(block);
        }
        else
          this.AppendBlockToFile(block);
      }
    }

    private void AppendBlockToFile(ScanResultBlock block)
    {
      using (BinaryWriter bw = new BinaryWriter((Stream) this.fileStream, Encoding.Unicode, true))
      {
        bw.Write(block.Start);
        bw.Write(block.End);
        bw.Write(block.Results.Count);
        foreach (ScanResult result in (IEnumerable<ScanResult>) block.Results)
          ScanResultStore.WriteSearchResult(bw, result);
      }
    }

    private IEnumerable<ScanResultBlock> ReadBlocksFromFile()
    {
      using (FileStream stream = File.OpenRead(this.storePath))
      {
        using (BinaryReader br = new BinaryReader((Stream) stream, Encoding.Unicode))
        {
          long length = stream.Length;
          while (stream.Position < length)
          {
            IntPtr start = br.ReadIntPtr();
            IntPtr end = br.ReadIntPtr();
            int capacity = br.ReadInt32();
            List<ScanResult> scanResultList = new List<ScanResult>(capacity);
            for (int index = 0; index < capacity; ++index)
              scanResultList.Add(this.ReadScanResult(br));
            yield return new ScanResultBlock(start, end, (IReadOnlyList<ScanResult>) scanResultList);
          }
        }
      }
    }

    private ScanResult ReadScanResult(BinaryReader br)
    {
      IntPtr num = br.ReadIntPtr();
      ScanResult scanResult;
      switch (this.valueType)
      {
        case ScanValueType.Byte:
          scanResult = (ScanResult) new ByteScanResult(br.ReadByte());
          break;
        case ScanValueType.Short:
          scanResult = (ScanResult) new ShortScanResult(br.ReadInt16());
          break;
        case ScanValueType.Integer:
          scanResult = (ScanResult) new IntegerScanResult(br.ReadInt32());
          break;
        case ScanValueType.Long:
          scanResult = (ScanResult) new LongScanResult(br.ReadInt64());
          break;
        case ScanValueType.Float:
          scanResult = (ScanResult) new FloatScanResult(br.ReadSingle());
          break;
        case ScanValueType.Double:
          scanResult = (ScanResult) new DoubleScanResult(br.ReadDouble());
          break;
        case ScanValueType.ArrayOfBytes:
          scanResult = (ScanResult) new ArrayOfBytesScanResult(br.ReadBytes(br.ReadInt32()));
          break;
        case ScanValueType.String:
        case ScanValueType.Regex:
          Encoding encoding1;
          switch (br.ReadInt32())
          {
            case 0:
              encoding1 = Encoding.UTF8;
              break;
            case 1:
              encoding1 = Encoding.Unicode;
              break;
            default:
              encoding1 = Encoding.UTF32;
              break;
          }
          Encoding encoding2 = encoding1;
          string str = br.ReadString();
          scanResult = this.valueType == ScanValueType.String ? (ScanResult) new StringScanResult(str, encoding2) : (ScanResult) new RegexStringScanResult(str, encoding2);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      scanResult.Address = num;
      return scanResult;
    }

    private static void WriteSearchResult(BinaryWriter bw, ScanResult result)
    {
      bw.Write(result.Address);
      switch (result)
      {
        case ByteScanResult byteScanResult:
          bw.Write(byteScanResult.Value);
          break;
        case ShortScanResult shortScanResult:
          bw.Write(shortScanResult.Value);
          break;
        case IntegerScanResult integerScanResult:
          bw.Write(integerScanResult.Value);
          break;
        case LongScanResult longScanResult:
          bw.Write(longScanResult.Value);
          break;
        case FloatScanResult floatScanResult:
          bw.Write(floatScanResult.Value);
          break;
        case DoubleScanResult doubleScanResult:
          bw.Write(doubleScanResult.Value);
          break;
        case ArrayOfBytesScanResult ofBytesScanResult:
          bw.Write(ofBytesScanResult.Value.Length);
          bw.Write(ofBytesScanResult.Value);
          break;
        case StringScanResult stringScanResult:
          bw.Write(stringScanResult.Encoding.IsSameCodePage(Encoding.UTF8) ? 0 : (stringScanResult.Encoding.IsSameCodePage(Encoding.Unicode) ? 1 : 2));
          bw.Write(stringScanResult.Value);
          break;
      }
    }

    private enum StorageMode
    {
      Memory,
      File,
    }
  }
}
