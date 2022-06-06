// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.BytePattern
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReClassNET.MemoryScanner
{
  public class BytePattern
  {
    private readonly List<BytePattern.IPatternByte> pattern = new List<BytePattern.IPatternByte>();

    public int Length
    {
      get
      {
        return this.pattern.Count;
      }
    }

    public bool HasWildcards
    {
      get
      {
        return this.pattern.Any<BytePattern.IPatternByte>((Func<BytePattern.IPatternByte, bool>) (pb => pb is BytePattern.PatternByte patternByte && patternByte.HasWildcard));
      }
    }

    private BytePattern()
    {
    }

    public static BytePattern Parse(string value)
    {
      BytePattern bytePattern = new BytePattern();
      using (StringReader sr = new StringReader(value))
      {
        while (true)
        {
          BytePattern.PatternByte patternByte = new BytePattern.PatternByte();
          if (patternByte.TryRead(sr))
          {
            if (!patternByte.HasWildcard)
              bytePattern.pattern.Add((BytePattern.IPatternByte) new BytePattern.SimplePatternByte(patternByte.ToByte()));
            else
              bytePattern.pattern.Add((BytePattern.IPatternByte) patternByte);
          }
          else
            break;
        }
        if (sr.Peek() != -1)
          throw new ArgumentException("'" + value + "' is not a valid byte pattern.");
        return bytePattern;
      }
    }

    public static BytePattern From(IEnumerable<byte> data)
    {
      BytePattern bytePattern = new BytePattern();
      bytePattern.pattern.AddRange((IEnumerable<BytePattern.IPatternByte>) data.Select<byte, BytePattern.SimplePatternByte>((Func<byte, BytePattern.SimplePatternByte>) (b => new BytePattern.SimplePatternByte(b))));
      return bytePattern;
    }

    public static BytePattern From(IEnumerable<Tuple<byte, bool>> data)
    {
      BytePattern bytePattern = new BytePattern();
      foreach (Tuple<byte, bool> tuple in data)
      {
        byte num1;
        bool flag;
        tuple.Deconstruct<byte, bool>(out num1, out flag);
        byte num2 = num1;
        BytePattern.IPatternByte patternByte = flag ? (BytePattern.IPatternByte) BytePattern.PatternByte.NewWildcardByte() : (BytePattern.IPatternByte) new BytePattern.SimplePatternByte(num2);
        bytePattern.pattern.Add(patternByte);
      }
      return bytePattern;
    }

    public bool Equals(byte[] data, int index)
    {
      for (int index1 = 0; index1 < this.pattern.Count; ++index1)
      {
        if (!this.pattern[index1].Equals(data[index + index1]))
          return false;
      }
      return true;
    }

    public byte[] ToByteArray()
    {
      if (this.HasWildcards)
        throw new InvalidOperationException();
      return this.pattern.Select<BytePattern.IPatternByte, byte>((Func<BytePattern.IPatternByte, byte>) (pb => pb.ToByte())).ToArray<byte>();
    }

    public Tuple<string, string> ToString(PatternMaskFormat format)
    {
      if (format == PatternMaskFormat.Combined)
        return Tuple.Create<string, string>(string.Join(" ", this.pattern.Select<BytePattern.IPatternByte, string>((Func<BytePattern.IPatternByte, string>) (p => p.ToString(PatternMaskFormat.Combined).Item1))), (string) null);
      if (format != PatternMaskFormat.Separated)
        throw new ArgumentOutOfRangeException(nameof (format), (object) format, (string) null);
      StringBuilder sb1 = new StringBuilder();
      StringBuilder sb2 = new StringBuilder();
      this.pattern.Select<BytePattern.IPatternByte, Tuple<string, string>>((Func<BytePattern.IPatternByte, Tuple<string, string>>) (p => p.ToString(PatternMaskFormat.Separated))).ForEach<Tuple<string, string>>((Action<Tuple<string, string>>) (t =>
      {
        sb1.Append(t.Item1);
        sb2.Append(t.Item2);
      }));
      return Tuple.Create<string, string>(sb1.ToString(), sb2.ToString());
    }

    public override string ToString()
    {
      return this.ToString(PatternMaskFormat.Combined).Item1;
    }

    private interface IPatternByte
    {
      byte ToByte();

      bool Equals(byte b);

      Tuple<string, string> ToString(PatternMaskFormat format);
    }

    private class PatternByte : BytePattern.IPatternByte
    {
      private BytePattern.PatternByte.Nibble nibble1;
      private BytePattern.PatternByte.Nibble nibble2;

      public bool HasWildcard
      {
        get
        {
          return this.nibble1.IsWildcard || this.nibble2.IsWildcard;
        }
      }

      public byte ToByte()
      {
        if (this.HasWildcard)
          throw new InvalidOperationException();
        return (byte) ((this.nibble1.Value << 4) + this.nibble2.Value);
      }

      public static BytePattern.PatternByte NewWildcardByte()
      {
        return new BytePattern.PatternByte()
        {
          nibble1 = {
            IsWildcard = true
          },
          nibble2 = {
            IsWildcard = true
          }
        };
      }

      private static bool IsHexValue(char c)
      {
        if ('0' <= c && c <= '9' || 'A' <= c && c <= 'F')
          return true;
        return 'a' <= c && c <= 'f';
      }

      private static int HexToInt(char c)
      {
        if ('0' <= c && c <= '9')
          return (int) c - 48;
        return 'A' <= c && c <= 'F' ? (int) c - 65 + 10 : (int) c - 97 + 10;
      }

      public bool TryRead(StringReader sr)
      {
        int num1 = sr.ReadSkipWhitespaces();
        if (num1 == -1 || !BytePattern.PatternByte.IsHexValue((char) num1) && (ushort) num1 != (ushort) 63)
          return false;
        this.nibble1.Value = BytePattern.PatternByte.HexToInt((char) num1) & 15;
        this.nibble1.IsWildcard = (ushort) num1 == (ushort) 63;
        int num2 = sr.Read();
        if (num2 == -1 || char.IsWhiteSpace((char) num2) || (ushort) num2 == (ushort) 63)
        {
          this.nibble2.IsWildcard = true;
          return true;
        }
        if (!BytePattern.PatternByte.IsHexValue((char) num2))
          return false;
        this.nibble2.Value = BytePattern.PatternByte.HexToInt((char) num2) & 15;
        this.nibble2.IsWildcard = false;
        return true;
      }

      public bool Equals(byte b)
      {
        return (this.nibble1.IsWildcard || ((int) b >> 4 & 15) == this.nibble1.Value) && (this.nibble2.IsWildcard || ((int) b & 15) == this.nibble2.Value);
      }

      public Tuple<string, string> ToString(PatternMaskFormat format)
      {
        if (format != PatternMaskFormat.Combined)
        {
          if (format != PatternMaskFormat.Separated)
            throw new ArgumentOutOfRangeException(nameof (format), (object) format, (string) null);
          return !this.HasWildcard ? Tuple.Create<string, string>(string.Format("\\x{0:X02}", (object) this.ToByte()), "x") : Tuple.Create<string, string>("\\x00", "?");
        }
        StringBuilder stringBuilder = new StringBuilder();
        if (this.nibble1.IsWildcard)
          stringBuilder.Append('?');
        else
          stringBuilder.AppendFormat("{0:X}", (object) this.nibble1.Value);
        if (this.nibble2.IsWildcard)
          stringBuilder.Append('?');
        else
          stringBuilder.AppendFormat("{0:X}", (object) this.nibble2.Value);
        return Tuple.Create<string, string>(stringBuilder.ToString(), (string) null);
      }

      public override string ToString()
      {
        return this.ToString(PatternMaskFormat.Combined).Item1;
      }

      private struct Nibble
      {
        public int Value;
        public bool IsWildcard;
      }
    }

    private class SimplePatternByte : BytePattern.IPatternByte
    {
      private readonly byte value;

      public SimplePatternByte(byte value)
      {
        this.value = value;
      }

      public byte ToByte()
      {
        return this.value;
      }

      public bool Equals(byte b)
      {
        return (int) this.value == (int) b;
      }

      public Tuple<string, string> ToString(PatternMaskFormat format)
      {
        if (format == PatternMaskFormat.Combined)
          return Tuple.Create<string, string>(string.Format("{0:X02}", (object) this.ToByte()), (string) null);
        if (format == PatternMaskFormat.Separated)
          return Tuple.Create<string, string>(string.Format("\\x{0:X02}", (object) this.ToByte()), "x");
        throw new ArgumentOutOfRangeException(nameof (format), (object) format, (string) null);
      }
    }
  }
}
