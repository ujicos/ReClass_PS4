// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.Conversion.EndianBitConverter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Util.Conversion
{
  public abstract class EndianBitConverter
  {
    public static LittleEndianBitConverter Little { get; } = new LittleEndianBitConverter();

    public static BigEndianBitConverter Big { get; } = new BigEndianBitConverter();

    public static EndianBitConverter System { get; } = BitConverter.IsLittleEndian ? (EndianBitConverter) EndianBitConverter.Little : (EndianBitConverter) EndianBitConverter.Big;

    public bool ToBoolean(byte[] value, int startIndex)
    {
      return BitConverter.ToBoolean(value, startIndex);
    }

    public char ToChar(byte[] value, int startIndex)
    {
      return (char) this.FromBytes(value, startIndex, 2);
    }

    public double ToDouble(byte[] value, int startIndex)
    {
      return BitConverter.Int64BitsToDouble(this.ToInt64(value, startIndex));
    }

    public float ToSingle(byte[] value, int startIndex)
    {
      return new EndianBitConverter.Int32FloatUnion(this.ToInt32(value, startIndex)).FloatValue;
    }

    public short ToInt16(byte[] value, int startIndex)
    {
      return (short) this.FromBytes(value, startIndex, 2);
    }

    public int ToInt32(byte[] value, int startIndex)
    {
      return (int) this.FromBytes(value, startIndex, 4);
    }

    public long ToInt64(byte[] value, int startIndex)
    {
      return this.FromBytes(value, startIndex, 8);
    }

    public ushort ToUInt16(byte[] value, int startIndex)
    {
      return (ushort) this.FromBytes(value, startIndex, 2);
    }

    public uint ToUInt32(byte[] value, int startIndex)
    {
      return (uint) this.FromBytes(value, startIndex, 4);
    }

    public ulong ToUInt64(byte[] value, int startIndex)
    {
      return (ulong) this.FromBytes(value, startIndex, 8);
    }

    protected abstract long FromBytes(byte[] value, int index, int bytesToConvert);

    public byte[] GetBytes(bool value)
    {
      return BitConverter.GetBytes(value);
    }

    public byte[] GetBytes(char value)
    {
      return this.ToBytes((long) value, 2);
    }

    public byte[] GetBytes(double value)
    {
      return this.ToBytes(BitConverter.DoubleToInt64Bits(value), 8);
    }

    public byte[] GetBytes(short value)
    {
      return this.ToBytes((long) value, 2);
    }

    public byte[] GetBytes(int value)
    {
      return this.ToBytes((long) value, 4);
    }

    public byte[] GetBytes(long value)
    {
      return this.ToBytes(value, 8);
    }

    public byte[] GetBytes(float value)
    {
      return this.ToBytes((long) new EndianBitConverter.Int32FloatUnion(value).IntValue, 4);
    }

    public byte[] GetBytes(ushort value)
    {
      return this.ToBytes((long) value, 2);
    }

    public byte[] GetBytes(uint value)
    {
      return this.ToBytes((long) value, 4);
    }

    public byte[] GetBytes(ulong value)
    {
      return this.ToBytes((long) value, 8);
    }

    protected abstract byte[] ToBytes(long value, int bytes);

    [StructLayout(LayoutKind.Explicit)]
    private readonly struct Int32FloatUnion
    {
      [FieldOffset(0)]
      public readonly int IntValue;
      [FieldOffset(0)]
      public readonly float FloatValue;

      internal Int32FloatUnion(int value)
      {
        this.FloatValue = 0.0f;
        this.IntValue = value;
      }

      internal Int32FloatUnion(float value)
      {
        this.IntValue = 0;
        this.FloatValue = value;
      }
    }
  }
}
