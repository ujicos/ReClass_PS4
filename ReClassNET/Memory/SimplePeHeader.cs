// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.SimplePeHeader
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;

namespace ReClassNET.Memory
{
  public class SimplePeHeader
  {
    private readonly byte[] data;

    private int e_lfanew
    {
      get
      {
        return BitConverter.ToInt32(this.data, 60);
      }
    }

    private int FileHeader
    {
      get
      {
        return this.e_lfanew + 4;
      }
    }

    public int NumberOfSections
    {
      get
      {
        return (int) BitConverter.ToInt16(this.data, this.FileHeader + 2);
      }
    }

    private int SizeOfOptionalHeader
    {
      get
      {
        return (int) BitConverter.ToInt16(this.data, this.FileHeader + 16);
      }
    }

    private int FirstSectionOffset
    {
      get
      {
        return this.e_lfanew + 24 + this.SizeOfOptionalHeader;
      }
    }

    public int SectionOffset(int index)
    {
      return this.FirstSectionOffset + index * 40;
    }

    private SimplePeHeader(byte[] data)
    {
      this.data = data;
    }

    public static void FixSectionHeaders(byte[] data)
    {
      SimplePeHeader simplePeHeader = new SimplePeHeader(data);
      using (MemoryStream memoryStream = new MemoryStream(data))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          for (int index = 0; index < simplePeHeader.NumberOfSections; ++index)
          {
            int num = simplePeHeader.SectionOffset(index);
            binaryWriter.Seek(num + 16, SeekOrigin.Begin);
            binaryWriter.Write(BitConverter.ToUInt32(data, num + 8));
            binaryWriter.Write(BitConverter.ToUInt32(data, num + 12));
          }
        }
      }
    }
  }
}
