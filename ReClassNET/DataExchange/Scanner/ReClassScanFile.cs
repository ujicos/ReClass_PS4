// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.Scanner.ReClassScanFile
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.MemoryScanner;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.Scanner
{
  public class ReClassScanFile : IScannerImport, IScannerExport
  {
    public const string FormatName = "ReClass.NET Scanner File";
    public const string FileExtension = ".rcnetscan";
    private const string Version1 = "1";
    private const string DataFileName = "Data.xml";
    public const string XmlRootElement = "records";
    public const string XmlRecordElement = "record";
    public const string XmlVersionAttribute = "version";
    public const string XmlPlatformAttribute = "platform";
    public const string XmlValueTypeAttribute = "type";
    public const string XmlAddressAttribute = "address";
    public const string XmlModuleAttribute = "module";
    public const string XmlDescriptionAttribute = "description";
    public const string XmlValueLengthAttribute = "length";
    public const string XmlEncodingAttribute = "encoding";

    public IEnumerable<MemoryRecord> Load(string filePath, ILogger logger)
    {
      bool flag;
      using (FileStream fs = new FileStream(filePath, FileMode.Open))
      {
        using (ZipArchive archive = new ZipArchive((Stream) fs, ZipArchiveMode.Read))
        {
          ZipArchiveEntry entry = archive.GetEntry("Data.xml");
          if (entry == null)
            throw new FormatException();
          using (Stream entryStream = entry.Open())
          {
            XDocument xdocument = XDocument.Load(entryStream);
            if (xdocument.Root == null)
            {
              logger.Log(ReClassNET.Logger.LogLevel.Error, "File has not the correct format.");
              flag = false;
            }
            else
            {
              string str1 = xdocument.Root.Attribute((XName) "platform")?.Value;
              if (str1 != "x64")
                logger.Log(ReClassNET.Logger.LogLevel.Warning, "The platform of the file (" + str1 + ") doesn't match the program platform (x64).");
              foreach (XElement element in xdocument.Root.Elements((XName) "record"))
              {
                string str2 = element.Attribute((XName) "type")?.Value ?? string.Empty;
                ScanValueType result1;
                if (!Enum.TryParse<ScanValueType>(str2, out result1))
                {
                  logger?.Log(ReClassNET.Logger.LogLevel.Warning, "Unknown value type: " + str2);
                }
                else
                {
                  string str3 = element.Attribute((XName) "description")?.Value ?? string.Empty;
                  string s = element.Attribute((XName) "address")?.Value ?? string.Empty;
                  string str4 = element.Attribute((XName) "module")?.Value ?? string.Empty;
                  long result2;
                  long.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result2);
                  MemoryRecord memoryRecord = new MemoryRecord()
                  {
                    Description = str3,
                    AddressOrOffset = (IntPtr) result2,
                    ValueType = result1
                  };
                  if (!string.IsNullOrEmpty(str4))
                    memoryRecord.ModuleName = str4;
                  if (result1 == ScanValueType.ArrayOfBytes || result1 == ScanValueType.String)
                  {
                    int result3;
                    int.TryParse(element.Attribute((XName) "length")?.Value ?? string.Empty, NumberStyles.Integer, (IFormatProvider) null, out result3);
                    memoryRecord.ValueLength = Math.Max(1, result3);
                    if (result1 == ScanValueType.String)
                    {
                      string str5 = element.Attribute((XName) "encoding")?.Value ?? string.Empty;
                      memoryRecord.Encoding = str5 == "UTF16" ? Encoding.Unicode : (str5 == "UTF32" ? Encoding.UTF32 : Encoding.UTF8);
                    }
                  }
                  yield return memoryRecord;
                }
              }
              flag = false;
            }
          }
        }
      }
      return flag;
    }

    public void Save(IEnumerable<MemoryRecord> records, string filePath, ILogger logger)
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
      {
        using (ZipArchive zipArchive = new ZipArchive((Stream) fileStream, ZipArchiveMode.Create))
        {
          using (Stream stream = zipArchive.CreateEntry("Data.xml").Open())
            new XDocument(new object[3]
            {
              (object) new XComment("ReClass.NET Scanner PS4 Port by MrReeko"),
              (object) new XComment("Website: https://github.com/MrReekoFTWxD"),
              (object) new XElement((XName) nameof (records), new object[3]
              {
                (object) new XAttribute((XName) "version", (object) "1"),
                (object) new XAttribute((XName) "platform", (object) "x64"),
                (object) records.Select<MemoryRecord, XElement>((Func<MemoryRecord, XElement>) (r =>
                {
                  XElement xelement = new XElement((XName) "record", new object[3]
                  {
                    (object) new XAttribute((XName) "type", (object) r.ValueType.ToString()),
                    (object) new XAttribute((XName) "description", (object) (r.Description ?? string.Empty)),
                    (object) new XAttribute((XName) "address", (object) r.AddressOrOffset.ToString("X016"))
                  });
                  if (r.IsRelativeAddress)
                    xelement.SetAttributeValue((XName) "module", (object) r.ModuleName);
                  if (r.ValueType == ScanValueType.ArrayOfBytes || r.ValueType == ScanValueType.String)
                  {
                    xelement.SetAttributeValue((XName) "length", (object) r.ValueLength);
                    if (r.ValueType == ScanValueType.String)
                      xelement.SetAttributeValue((XName) "encoding", r.Encoding.IsSameCodePage(Encoding.UTF8) ? (object) "UTF8" : (r.Encoding.IsSameCodePage(Encoding.Unicode) ? (object) "UTF16" : (object) "UTF32"));
                  }
                  return xelement;
                }))
              })
            }).Save(stream);
        }
      }
    }
  }
}
