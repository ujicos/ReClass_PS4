// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.MemoryRecord
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Util;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace ReClassNET.MemoryScanner
{
  public class MemoryRecord : INotifyPropertyChanged
  {
    private IntPtr addressOrOffset;
    private string moduleName;

    public MemoryRecordAddressMode AddressMode { get; set; }

    public IntPtr AddressOrOffset
    {
      get
      {
        return this.addressOrOffset;
      }
      set
      {
        this.addressOrOffset = value;
        this.AddressMode = MemoryRecordAddressMode.Unknown;
      }
    }

    public IntPtr RealAddress { get; private set; }

    public string AddressStr
    {
      get
      {
        return this.RealAddress.ToString("X016");
      }
    }

    public string ModuleName
    {
      get
      {
        return this.moduleName;
      }
      set
      {
        this.moduleName = value;
        this.AddressMode = MemoryRecordAddressMode.Relative;
      }
    }

    public bool IsRelativeAddress
    {
      get
      {
        return !string.IsNullOrEmpty(this.ModuleName);
      }
    }

    public string Description { get; set; } = string.Empty;

    public ScanValueType ValueType { get; set; }

    public string ValueStr { get; private set; }

    public string PreviousValueStr { get; }

    public bool HasChangedValue { get; private set; }

    public int ValueLength { get; set; }

    public Encoding Encoding { get; set; }

    public bool ShowValueHexadecimal { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public MemoryRecord()
    {
    }

    public MemoryRecord(ScanResult result)
    {
      this.addressOrOffset = result.Address;
      this.AddressMode = MemoryRecordAddressMode.Unknown;
      this.ValueType = result.ValueType;
      switch (this.ValueType)
      {
        case ScanValueType.Byte:
          this.ValueStr = MemoryRecord.FormatValue(((ByteScanResult) result).Value, false);
          break;
        case ScanValueType.Short:
          this.ValueStr = MemoryRecord.FormatValue(((ShortScanResult) result).Value, false);
          break;
        case ScanValueType.Integer:
          this.ValueStr = MemoryRecord.FormatValue(((IntegerScanResult) result).Value, false);
          break;
        case ScanValueType.Long:
          this.ValueStr = MemoryRecord.FormatValue(((LongScanResult) result).Value, false);
          break;
        case ScanValueType.Float:
          this.ValueStr = MemoryRecord.FormatValue(((FloatScanResult) result).Value);
          break;
        case ScanValueType.Double:
          this.ValueStr = MemoryRecord.FormatValue(((DoubleScanResult) result).Value);
          break;
        case ScanValueType.ArrayOfBytes:
          byte[] numArray = ((ArrayOfBytesScanResult) result).Value;
          this.ValueLength = numArray.Length;
          this.ValueStr = MemoryRecord.FormatValue(numArray);
          break;
        case ScanValueType.String:
        case ScanValueType.Regex:
          StringScanResult stringScanResult = (StringScanResult) result;
          this.ValueLength = stringScanResult.Value.Length;
          this.Encoding = stringScanResult.Encoding;
          this.ValueStr = MemoryRecord.FormatValue(stringScanResult.Value);
          break;
        default:
          throw new InvalidOperationException();
      }
      this.PreviousValueStr = this.ValueStr;
    }

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public void ResolveAddress(RemoteProcess process)
    {
      if (this.AddressMode == MemoryRecordAddressMode.Unknown)
      {
        this.RealAddress = this.addressOrOffset;
        Module moduleToPointer = process.GetModuleToPointer(this.addressOrOffset);
        if (moduleToPointer != null)
        {
          this.addressOrOffset = this.addressOrOffset.Sub(moduleToPointer.Start);
          this.ModuleName = moduleToPointer.Name;
          this.AddressMode = MemoryRecordAddressMode.Relative;
        }
        else
          this.AddressMode = MemoryRecordAddressMode.Absolute;
      }
      else
      {
        if (this.AddressMode != MemoryRecordAddressMode.Relative)
          return;
        Module moduleByName = process.GetModuleByName(this.ModuleName);
        if (moduleByName == null)
          return;
        this.RealAddress = moduleByName.Start.Add(this.addressOrOffset);
      }
    }

    public void RefreshValue(RemoteProcess process)
    {
      byte[] buffer;
      switch (this.ValueType)
      {
        case ScanValueType.Byte:
          buffer = new byte[1];
          break;
        case ScanValueType.Short:
          buffer = new byte[2];
          break;
        case ScanValueType.Integer:
        case ScanValueType.Float:
          buffer = new byte[4];
          break;
        case ScanValueType.Long:
        case ScanValueType.Double:
          buffer = new byte[8];
          break;
        case ScanValueType.ArrayOfBytes:
          buffer = new byte[this.ValueLength];
          break;
        case ScanValueType.String:
        case ScanValueType.Regex:
          buffer = new byte[this.ValueLength * this.Encoding.GuessByteCountPerChar()];
          break;
        default:
          throw new InvalidOperationException();
      }
      if (process.ReadRemoteMemoryIntoBuffer(this.RealAddress, ref buffer))
      {
        switch (this.ValueType)
        {
          case ScanValueType.Byte:
            this.ValueStr = MemoryRecord.FormatValue(buffer[0], this.ShowValueHexadecimal);
            break;
          case ScanValueType.Short:
            this.ValueStr = MemoryRecord.FormatValue(process.BitConverter.ToInt16(buffer, 0), this.ShowValueHexadecimal);
            break;
          case ScanValueType.Integer:
            this.ValueStr = MemoryRecord.FormatValue(process.BitConverter.ToInt32(buffer, 0), this.ShowValueHexadecimal);
            break;
          case ScanValueType.Long:
            this.ValueStr = MemoryRecord.FormatValue(process.BitConverter.ToInt64(buffer, 0), this.ShowValueHexadecimal);
            break;
          case ScanValueType.Float:
            this.ValueStr = MemoryRecord.FormatValue(process.BitConverter.ToSingle(buffer, 0));
            break;
          case ScanValueType.Double:
            this.ValueStr = MemoryRecord.FormatValue(process.BitConverter.ToDouble(buffer, 0));
            break;
          case ScanValueType.ArrayOfBytes:
            this.ValueStr = MemoryRecord.FormatValue(buffer);
            break;
          case ScanValueType.String:
          case ScanValueType.Regex:
            this.ValueStr = MemoryRecord.FormatValue(this.Encoding.GetString(buffer));
            break;
        }
      }
      else
        this.ValueStr = "???";
      this.HasChangedValue = this.ValueStr != this.PreviousValueStr;
      this.NotifyPropertyChanged("ValueStr");
    }

    public void SetValue(RemoteProcess process, string input, bool isHex)
    {
      byte[] data = (byte[]) null;
      if (this.ValueType == ScanValueType.Byte || this.ValueType == ScanValueType.Short || (this.ValueType == ScanValueType.Integer || this.ValueType == ScanValueType.Long))
      {
        NumberStyles style = isHex ? NumberStyles.HexNumber : NumberStyles.Integer;
        long result;
        long.TryParse(input, style, (IFormatProvider) null, out result);
        switch (this.ValueType)
        {
          case ScanValueType.Byte:
            data = process.BitConverter.GetBytes((short) (byte) result);
            break;
          case ScanValueType.Short:
            data = process.BitConverter.GetBytes((short) result);
            break;
          case ScanValueType.Integer:
            data = process.BitConverter.GetBytes((int) result);
            break;
          case ScanValueType.Long:
            data = process.BitConverter.GetBytes(result);
            break;
        }
      }
      else if (this.ValueType == ScanValueType.Float || this.ValueType == ScanValueType.Double)
      {
        NumberFormatInfo numberFormatInfo = NumberFormat.GuessNumberFormat(input);
        double result;
        double.TryParse(input, NumberStyles.Float, (IFormatProvider) numberFormatInfo, out result);
        switch (this.ValueType)
        {
          case ScanValueType.Float:
            data = process.BitConverter.GetBytes((float) result);
            break;
          case ScanValueType.Double:
            data = process.BitConverter.GetBytes(result);
            break;
        }
      }
      if (data == null)
        return;
      process.WriteRemoteMemory(this.RealAddress, data);
      this.RefreshValue(process);
    }

    private static string FormatValue(byte value, bool showAsHex)
    {
      return !showAsHex ? value.ToString() : value.ToString("X");
    }

    private static string FormatValue(short value, bool showAsHex)
    {
      return !showAsHex ? value.ToString() : value.ToString("X");
    }

    private static string FormatValue(int value, bool showAsHex)
    {
      return !showAsHex ? value.ToString() : value.ToString("X");
    }

    private static string FormatValue(long value, bool showAsHex)
    {
      return !showAsHex ? value.ToString() : value.ToString("X");
    }

    private static string FormatValue(float value)
    {
      return value.ToString("0.0000");
    }

    private static string FormatValue(double value)
    {
      return value.ToString("0.0000");
    }

    private static string FormatValue(byte[] value)
    {
      return HexadecimalFormatter.ToString(value);
    }

    private static string FormatValue(string value)
    {
      return value;
    }
  }
}
