// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.XElementSerializer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.Util
{
  public static class XElementSerializer
  {
    public static bool TryRead(XContainer element, string name, Action<XElement> iff)
    {
      XElement xelement = element.Element((XName) name);
      if (xelement == null)
        return false;
      iff(xelement);
      return true;
    }

    public static bool ToBool(XElement value)
    {
      return ((bool?) value).GetValueOrDefault();
    }

    public static int ToInt(XElement value)
    {
      return ((int?) value).GetValueOrDefault();
    }

    public static string ToString(XElement value)
    {
      return value.Value;
    }

    public static Color ToColor(XElement value)
    {
      return Color.FromArgb((int) (4278190080L | (long) int.Parse(value.Value, NumberStyles.HexNumber)));
    }

    public static Dictionary<string, string> ToDictionary(XContainer value)
    {
      return value.Elements().ToDictionary<XElement, string, string>((Func<XElement, string>) (e => e.Name.ToString()), (Func<XElement, string>) (e => e.Value));
    }

    public static XElement ToXml(string name, bool value)
    {
      return new XElement((XName) name, (object) value);
    }

    public static XElement ToXml(string name, int value)
    {
      return new XElement((XName) name, (object) value);
    }

    public static XElement ToXml(string name, string value)
    {
      return new XElement((XName) name, (object) value);
    }

    public static XElement ToXml(string name, Color value)
    {
      return new XElement((XName) name, (object) string.Format("{0:X6}", (object) value.ToRgb()));
    }

    public static XElement ToXml(string name, Dictionary<string, string> value)
    {
      return new XElement((XName) name, (object) value.Select<KeyValuePair<string, string>, XElement>((Func<KeyValuePair<string, string>, XElement>) (kv => new XElement((XName) kv.Key, (object) kv.Value))));
    }
  }
}
