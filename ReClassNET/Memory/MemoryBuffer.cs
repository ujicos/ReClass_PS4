// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.MemoryBuffer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Forms;
using ReClassNET.Util.Conversion;
using System;
using System.Text;

namespace ReClassNET.Memory
{
  public class MemoryBuffer
  {
    private byte[] data;
    private byte[] historyData;
    private bool hasHistory;

    public byte[] RawData
    {
      get
      {
        return this.data;
      }
    }

    public EndianBitConverter BitConverter { get; set; } = EndianBitConverter.System;

    public int Size
    {
      get
      {
        return this.data.Length;
      }
      set
      {
        if (value < 0 || value == this.data.Length)
          return;
        this.data = new byte[value];
        this.historyData = new byte[value];
        this.hasHistory = false;
        this.ContainsValidData = false;
      }
    }

    public int Offset { get; set; }

    public bool ContainsValidData { get; private set; }

    private void ObjectInvariants()
    {
    }

    public MemoryBuffer()
    {
      this.data = Array.Empty<byte>();
      this.historyData = Array.Empty<byte>();
    }

    public MemoryBuffer Clone()
    {
      return new MemoryBuffer()
      {
        data = this.data,
        historyData = this.historyData,
        hasHistory = this.hasHistory,
        BitConverter = this.BitConverter,
        ContainsValidData = this.ContainsValidData,
        Offset = this.Offset
      };
    }

    public void UpdateFrom(IRemoteMemoryReader reader, IntPtr address, string process)
    {
      if (reader == null)
      {
        this.data.FillWithZero();
        this.hasHistory = false;
      }
      else
      {
        Array.Copy((Array) this.data, (Array) this.historyData, this.data.Length);
        this.hasHistory = false;
        if (MainForm.PS4.IsConnected && process != null && MainForm.ProcList.FindProcess(process, false) != null)
        {
          int pid = MainForm.ProcList.FindProcess(process, false).pid;
          this.data = MainForm.PS4.ReadMemory(pid, (ulong) (long) address, this.data.Length);
          this.ContainsValidData = true;
        }
        else
          this.ContainsValidData = false;
        if (this.ContainsValidData)
          return;
        this.data.FillWithZero();
        this.hasHistory = false;
      }
    }

    public byte[] ReadBytes(int offset, int length)
    {
      byte[] buffer = new byte[length];
      this.ReadBytes(offset, buffer);
      return buffer;
    }

    public void ReadBytes(int offset, byte[] buffer)
    {
      offset = this.Offset + offset;
      if (offset + buffer.Length > this.data.Length)
        return;
      Array.Copy((Array) this.data, offset, (Array) buffer, 0, buffer.Length);
    }

    public sbyte ReadInt8(int offset)
    {
      offset = this.Offset + offset;
      return offset + 1 > this.data.Length ? (sbyte) 0 : (sbyte) this.data[offset];
    }

    public byte ReadUInt8(int offset)
    {
      offset = this.Offset + offset;
      return offset + 1 > this.data.Length ? (byte) 0 : this.data[offset];
    }

    public short ReadInt16(int offset)
    {
      offset = this.Offset + offset;
      return offset + 2 > this.data.Length ? (short) 0 : this.BitConverter.ToInt16(this.data, offset);
    }

    public ushort ReadUInt16(int offset)
    {
      offset = this.Offset + offset;
      return offset + 2 > this.data.Length ? (ushort) 0 : this.BitConverter.ToUInt16(this.data, offset);
    }

    public int ReadInt32(int offset)
    {
      offset = this.Offset + offset;
      return offset + 4 > this.data.Length ? 0 : this.BitConverter.ToInt32(this.data, offset);
    }

    public uint ReadUInt32(int offset)
    {
      offset = this.Offset + offset;
      return offset + 4 > this.data.Length ? 0U : this.BitConverter.ToUInt32(this.data, offset);
    }

    public long ReadInt64(int offset)
    {
      offset = this.Offset + offset;
      return offset + 8 > this.data.Length ? 0L : this.BitConverter.ToInt64(this.data, offset);
    }

    public ulong ReadUInt64(int offset)
    {
      offset = this.Offset + offset;
      return offset + 8 > this.data.Length ? 0UL : this.BitConverter.ToUInt64(this.data, offset);
    }

    public float ReadFloat(int offset)
    {
      offset = this.Offset + offset;
      return offset + 4 > this.data.Length ? 0.0f : this.BitConverter.ToSingle(this.data, offset);
    }

    public double ReadDouble(int offset)
    {
      offset = this.Offset + offset;
      return offset + 8 > this.data.Length ? 0.0 : this.BitConverter.ToDouble(this.data, offset);
    }

    public IntPtr ReadIntPtr(int offset)
    {
      return (IntPtr) this.ReadInt64(offset);
    }

    public UIntPtr ReadUIntPtr(int offset)
    {
      return (UIntPtr) this.ReadUInt64(offset);
    }

    public string ReadString(Encoding encoding, int offset, int length)
    {
      if (this.Offset + offset + length > this.data.Length)
        length = Math.Max(this.data.Length - this.Offset - offset, 0);
      if (length <= 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(encoding.GetString(this.data, this.Offset + offset, length));
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        if (!stringBuilder[index].IsPrintable())
          stringBuilder[index] = '.';
      }
      return stringBuilder.ToString();
    }

    public bool HasChanged(int offset, int length)
    {
      if (!this.hasHistory || this.Offset + offset + length > this.data.Length)
        return false;
      int num = this.Offset + offset + length;
      for (int index = this.Offset + offset; index < num; ++index)
      {
        if ((int) this.data[index] != (int) this.historyData[index])
          return true;
      }
      return false;
    }
  }
}
