// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.Scanner.CrySearchFile
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.MemoryScanner;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.Scanner
{
  public class CrySearchFile : IScannerImport
  {
    public const string FormatName = "CrySearch Address Tables";
    public const string FileExtension = ".csat";
    private const string Version3 = "3.0";
    public const string XmlVersionElement = "CrySearchVersion";
    public const string XmlEntriesElement = "Entries";
    public const string XmlItemElement = "item";
    public const string XmlDescriptionElement = "Description";
    public const string XmlValueTypeElement = "ValueType";
    public const string XmlAddressElement = "Address";
    public const string XmlModuleNameElement = "ModuleName";
    public const string XmlIsRelativeElement = "IsRelative";
    public const string XmlSizeElement = "Size";
    public const string XmlValueAttribute = "value";

    public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
    {
      XDocument xdocument = XDocument.Load(filePath);
      if (xdocument.Root != null && string.Compare(xdocument.Root.Element((XName) "CrySearchVersion")?.Value, "3.0", StringComparison.Ordinal) >= 0)
      {
        XElement xelement = xdocument.Root.Element((XName) "Entries");
        if (xelement != null)
        {
          foreach (XElement element in xelement.Elements((XName) "item"))
          {
            string str1 = element.Element((XName) "Description")?.Value.Trim() ?? string.Empty;
            string str2 = element.Element((XName) "ValueType")?.Attribute((XName) "value")?.Value.Trim() ?? string.Empty;
            string s = element.Element((XName) "Address")?.Attribute((XName) "value")?.Value.Trim() ?? string.Empty;
            string str3 = element.Element((XName) "ModuleName")?.Value.Trim() ?? string.Empty;
            long result1;
            long.TryParse(s, NumberStyles.Number, (IFormatProvider) null, out result1);
            ScanValueType scanValueType = CrySearchFile.Parse(str2, logger);
            MemoryRecord memoryRecord = new MemoryRecord()
            {
              AddressOrOffset = (IntPtr) result1,
              Description = str1,
              ValueType = scanValueType
            };
            if ((element.Element((XName) "IsRelative")?.Attribute((XName) "value")?.Value.Trim() ?? string.Empty) == "1" && !string.IsNullOrEmpty(str3))
              memoryRecord.ModuleName = str3;
            if (scanValueType == ScanValueType.ArrayOfBytes || scanValueType == ScanValueType.String)
            {
              int result2;
              int.TryParse(element.Element((XName) "Size")?.Attribute((XName) "value")?.Value.Trim() ?? string.Empty, NumberStyles.Integer, (IFormatProvider) null, out result2);
              memoryRecord.ValueLength = Math.Max(1, result2);
              if (scanValueType == ScanValueType.String)
                memoryRecord.Encoding = str2 == "9" ? Encoding.Unicode : Encoding.UTF8;
            }
            yield return memoryRecord;
          }
        }
      }
    }

    private static ScanValueType Parse(string value, ILogger logger)
    {
      switch (value)
      {
        case "1":
          return ScanValueType.Byte;
        case "2":
          return ScanValueType.Short;
        case "3":
          return ScanValueType.Integer;
        case "4":
          return ScanValueType.Long;
        case "5":
          return ScanValueType.Float;
        case "6":
          return ScanValueType.Double;
        case "7":
          return ScanValueType.ArrayOfBytes;
        case "8":
        case "9":
          return ScanValueType.String;
        default:
          logger?.Log(ReClassNET.Logger.LogLevel.Warning, "Unknown value type: " + value);
          return ScanValueType.Integer;
      }
    }
  }
}
