// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.RegexStringMemoryComparer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class RegexStringMemoryComparer : IComplexScanComparer, IScanComparer
  {
    public ScanCompareType CompareType
    {
      get
      {
        return ScanCompareType.Equal;
      }
    }

    public Regex Pattern { get; }

    public Encoding Encoding { get; }

    public RegexStringMemoryComparer(string pattern, Encoding encoding, bool caseSensitive)
    {
      RegexOptions options = RegexOptions.Compiled | RegexOptions.Singleline;
      if (!caseSensitive)
        options |= RegexOptions.IgnoreCase;
      this.Pattern = new Regex(pattern, options);
      this.Encoding = encoding;
    }

    public IEnumerable<ScanResult> Compare(byte[] data, int size)
    {
      string input = this.Encoding.GetString(data, 0, size);
      char[] bufferArray = input.ToCharArray();
      int lastIndex = 0;
      int lastOffset = 0;
      for (Match match = this.Pattern.Match(input); match.Success; match = match.NextMatch())
      {
        int num = this.Encoding.GetByteCount(bufferArray, lastIndex, match.Index - lastIndex) + lastOffset;
        lastIndex = match.Index;
        lastOffset = num;
        RegexStringScanResult stringScanResult = new RegexStringScanResult(match.Value, this.Encoding);
        stringScanResult.Address = (IntPtr) num;
        yield return (ScanResult) stringScanResult;
      }
    }

    public bool CompareWithPrevious(
      byte[] data,
      int size,
      ScanResult previous,
      out ScanResult result)
    {
      result = (ScanResult) null;
      int int32 = previous.Address.ToInt32();
      if (int32 >= size)
        return false;
      Match match = this.Pattern.Match(this.Encoding.GetString(data, int32, size - int32));
      if (!match.Success)
        return false;
      ref ScanResult local = ref result;
      RegexStringScanResult stringScanResult = new RegexStringScanResult(match.Value, this.Encoding);
      stringScanResult.Address = (IntPtr) int32;
      local = (ScanResult) stringScanResult;
      return true;
    }
  }
}
