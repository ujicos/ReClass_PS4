// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.CustomDataMap
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace ReClassNET.Util
{
  public class CustomDataMap : IEnumerable<KeyValuePair<string, string>>, IEnumerable
  {
    private readonly Dictionary<string, string> data = new Dictionary<string, string>();

    internal XElement Serialize(string name)
    {
      return XElementSerializer.ToXml(name, this.data);
    }

    internal void Deserialize(XElement element)
    {
      this.data.Clear();
      foreach (KeyValuePair<string, string> keyValuePair in XElementSerializer.ToDictionary((XContainer) element))
        this.data[keyValuePair.Key] = keyValuePair.Value;
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<string, string>>) this.data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public string this[string key]
    {
      get
      {
        return this.GetString(key);
      }
      set
      {
        this.SetString(key, value);
      }
    }

    public void RemoveValue(string key)
    {
      CustomDataMap.ValidateKey(key);
      this.data.Remove(key);
    }

    public void SetString(string key, string value)
    {
      CustomDataMap.ValidateKey(key);
      this.data[key] = value;
    }

    public void SetBool(string key, bool value)
    {
      this.SetString(key, Convert.ToString(value));
    }

    public void SetLong(string key, long value)
    {
      this.SetString(key, value.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
    }

    public void SetULong(string key, ulong value)
    {
      this.SetString(key, value.ToString((IFormatProvider) NumberFormatInfo.InvariantInfo));
    }

    public void SetXElement(string key, XElement value)
    {
      this.SetString(key, value?.ToString());
    }

    public string GetString(string key)
    {
      return this.GetString(key, (string) null);
    }

    public string GetString(string key, string def)
    {
      CustomDataMap.ValidateKey(key);
      string str;
      return this.data.TryGetValue(key, out str) ? str : def;
    }

    public bool GetBool(string key, bool def)
    {
      string str = this.GetString(key, (string) null);
      return string.IsNullOrEmpty(str) ? def : Convert.ToBoolean(str);
    }

    public long GetLong(string key, long def)
    {
      string s = this.GetString(key, (string) null);
      long result;
      return string.IsNullOrEmpty(s) || !long.TryParse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? def : result;
    }

    public ulong GetULong(string key, ulong def)
    {
      string s = this.GetString(key, (string) null);
      ulong result;
      return string.IsNullOrEmpty(s) || !ulong.TryParse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) ? def : result;
    }

    public XElement GetXElement(string key, XElement def)
    {
      string text = this.GetString(key, (string) null);
      return string.IsNullOrEmpty(text) ? def : XElement.Parse(text);
    }

    private static void ValidateKey(string key)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
    }
  }
}
