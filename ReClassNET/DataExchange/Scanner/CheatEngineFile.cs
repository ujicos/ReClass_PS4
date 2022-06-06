// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.Scanner.CheatEngineFile
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Logger;
using ReClassNET.MemoryScanner;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.Scanner
{
  public class CheatEngineFile : IScannerImport
  {
    public const string FormatName = "Cheat Engine Tables";
    public const string FileExtension = ".ct";
    private const string Version26 = "26";
    public const string XmlVersionElement = "CheatEngineTableVersion";
    public const string XmlEntriesElement = "CheatEntries";
    public const string XmlEntryElement = "CheatEntry";
    public const string XmlDescriptionElement = "Description";
    public const string XmlValueTypeElement = "VariableType";
    public const string XmlAddressElement = "Address";
    public const string XmlUnicodeElement = "Unicode";
    public const string XmlLengthElement = "Length";

    public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
    {
      using (FileStream stream = File.OpenRead(filePath))
      {
        XDocument xdocument = XDocument.Load((Stream) stream);
        if (xdocument.Root != null && string.Compare(xdocument.Root.Attribute((XName) "CheatEngineTableVersion")?.Value, "26", StringComparison.Ordinal) >= 0)
        {
          XElement xelement = xdocument.Root.Element((XName) "CheatEntries");
          if (xelement != null)
          {
            foreach (XElement element in xelement.Elements((XName) "CheatEntry"))
            {
              string str = element.Element((XName) "Description")?.Value.Trim() ?? string.Empty;
              if (str == "\"No description\"")
                str = string.Empty;
              ScanValueType scanValueType = CheatEngineFile.Parse(element.Element((XName) "VariableType")?.Value.Trim() ?? string.Empty, logger);
              MemoryRecord memoryRecord = new MemoryRecord()
              {
                Description = str,
                ValueType = scanValueType
              };
              string s = element.Element((XName) "Address")?.Value.Trim() ?? string.Empty;
              string[] strArray = s.Split('+');
              if (strArray.Length == 2)
              {
                long result;
                long.TryParse(strArray[1], NumberStyles.HexNumber, (IFormatProvider) null, out result);
                memoryRecord.AddressOrOffset = (IntPtr) result;
                memoryRecord.ModuleName = strArray[0].Trim();
              }
              else
              {
                long result;
                long.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result);
                memoryRecord.AddressOrOffset = (IntPtr) result;
              }
              if (scanValueType == ScanValueType.ArrayOfBytes || scanValueType == ScanValueType.String)
              {
                int result;
                int.TryParse(element.Element((XName) "Length")?.Value ?? string.Empty, NumberStyles.Integer, (IFormatProvider) null, out result);
                memoryRecord.ValueLength = Math.Max(1, result);
                if (scanValueType == ScanValueType.String)
                {
                  bool flag = (element.Element((XName) "Unicode")?.Value ?? string.Empty) == "1";
                  memoryRecord.Encoding = flag ? Encoding.Unicode : Encoding.UTF8;
                }
              }
              yield return memoryRecord;
            }
          }
        }
      }
    }

    private static ScanValueType Parse(string value, ILogger logger)
    {
      switch (value)
      {
        case "2 Bytes":
          return ScanValueType.Short;
        case "4 Bytes":
          return ScanValueType.Integer;
        case "8 Bytes":
          return ScanValueType.Long;
        case "Array of byte":
          return ScanValueType.ArrayOfBytes;
        case "Byte":
          return ScanValueType.Byte;
        case "Double":
          return ScanValueType.Double;
        case "Float":
          return ScanValueType.Float;
        case "String":
          return ScanValueType.String;
        default:
          logger?.Log(ReClassNET.Logger.LogLevel.Warning, "Unknown value type: " + value);
          return ScanValueType.Integer;
      }
    }
  }
}
