// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.ScannerContext
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.MemoryScanner
{
  internal class ScannerContext
  {
    public byte[] Buffer { get; private set; }

    public IScannerWorker Worker { get; }

    public ScannerContext(IScannerWorker worker, int bufferSize)
    {
      this.EnsureBufferSize(bufferSize);
      this.Worker = worker;
    }

    public void EnsureBufferSize(int size)
    {
      if (this.Buffer != null && this.Buffer.Length >= size)
        return;
      this.Buffer = new byte[size];
    }
  }
}
