// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.RegexStringScanResult
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System.Text;

namespace ReClassNET.MemoryScanner
{
  public class RegexStringScanResult : StringScanResult
  {
    public override ScanValueType ValueType
    {
      get
      {
        return ScanValueType.Regex;
      }
    }

    public RegexStringScanResult(string value, Encoding encoding)
      : base(value, encoding)
    {
    }

    public override ScanResult Clone()
    {
      RegexStringScanResult stringScanResult = new RegexStringScanResult(this.Value, this.Encoding);
      stringScanResult.Address = this.Address;
      return (ScanResult) stringScanResult;
    }
  }
}
