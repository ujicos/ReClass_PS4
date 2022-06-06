// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.SettingsSerializer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;
using System.Xml.Linq;

namespace ReClassNET.Util
{
  internal sealed class SettingsSerializer
  {
    private const string XmlRootElement = "Settings";
    private const string XmlGeneralElement = "General";
    private const string XmlDisplayElement = "Display";
    private const string XmlColorsElement = "Colors";
    private const string XmlCustomDataElement = "CustomData";

    public static Settings Load()
    {
      SettingsSerializer.EnsureSettingsDirectoryAvailable();
      Settings settings = new Settings();
      try
      {
        using (StreamReader streamReader = new StreamReader(Path.Combine(PathUtil.SettingsFolderPath, "settings.xml")))
        {
          XElement root = XDocument.Load((TextReader) streamReader).Root;
          XElement xelement1 = root?.Element((XName) "General");
          if (xelement1 != null)
          {
            XElementSerializer.TryRead((XContainer) xelement1, "LastProcess", (Action<XElement>) (e => settings.LastProcess = XElementSerializer.ToString(e)));
            XElementSerializer.TryRead((XContainer) xelement1, "StayOnTop", (Action<XElement>) (e => settings.StayOnTop = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement1, "RunAsAdmin", (Action<XElement>) (e => settings.RunAsAdmin = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement1, "RandomizeWindowTitle", (Action<XElement>) (e => settings.RandomizeWindowTitle = XElementSerializer.ToBool(e)));
          }
          XElement xelement2 = root?.Element((XName) "Display");
          if (xelement2 != null)
          {
            XElementSerializer.TryRead((XContainer) xelement2, "ShowNodeAddress", (Action<XElement>) (e => settings.ShowNodeAddress = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowNodeOffset", (Action<XElement>) (e => settings.ShowNodeOffset = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowNodeText", (Action<XElement>) (e => settings.ShowNodeText = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "HighlightChangedValues", (Action<XElement>) (e => settings.HighlightChangedValues = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentFloat", (Action<XElement>) (e => settings.ShowCommentFloat = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentInteger", (Action<XElement>) (e => settings.ShowCommentInteger = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentPointer", (Action<XElement>) (e => settings.ShowCommentPointer = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentRtti", (Action<XElement>) (e => settings.ShowCommentRtti = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentSymbol", (Action<XElement>) (e => settings.ShowCommentSymbol = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentString", (Action<XElement>) (e => settings.ShowCommentString = XElementSerializer.ToBool(e)));
            XElementSerializer.TryRead((XContainer) xelement2, "ShowCommentPluginInfo", (Action<XElement>) (e => settings.ShowCommentPluginInfo = XElementSerializer.ToBool(e)));
          }
          root?.Element((XName) "Colors");
          XElement element = root?.Element((XName) "CustomData");
          if (element != null)
            settings.CustomData.Deserialize(element);
        }
      }
      catch
      {
      }
      return settings;
    }

    public static void Save(Settings settings)
    {
      SettingsSerializer.EnsureSettingsDirectoryAvailable();
      using (StreamWriter streamWriter = new StreamWriter(Path.Combine(PathUtil.SettingsFolderPath, "settings.xml")))
        new XDocument(new object[3]
        {
          (object) new XComment("ReClass.NET PS4 Port by MrReeko"),
          (object) new XComment("Website: https://github.com/MrReekoFTWxD"),
          (object) new XElement((XName) "Settings", new object[4]
          {
            (object) new XElement((XName) "General", new object[4]
            {
              (object) XElementSerializer.ToXml("LastProcess", settings.LastProcess),
              (object) XElementSerializer.ToXml("StayOnTop", settings.StayOnTop),
              (object) XElementSerializer.ToXml("RunAsAdmin", settings.RunAsAdmin),
              (object) XElementSerializer.ToXml("RandomizeWindowTitle", settings.RandomizeWindowTitle)
            }),
            (object) new XElement((XName) "Display", new object[11]
            {
              (object) XElementSerializer.ToXml("ShowNodeAddress", settings.ShowNodeAddress),
              (object) XElementSerializer.ToXml("ShowNodeOffset", settings.ShowNodeOffset),
              (object) XElementSerializer.ToXml("ShowNodeText", settings.ShowNodeText),
              (object) XElementSerializer.ToXml("HighlightChangedValues", settings.HighlightChangedValues),
              (object) XElementSerializer.ToXml("ShowCommentFloat", settings.ShowCommentFloat),
              (object) XElementSerializer.ToXml("ShowCommentInteger", settings.ShowCommentInteger),
              (object) XElementSerializer.ToXml("ShowCommentPointer", settings.ShowCommentPointer),
              (object) XElementSerializer.ToXml("ShowCommentRtti", settings.ShowCommentRtti),
              (object) XElementSerializer.ToXml("ShowCommentSymbol", settings.ShowCommentSymbol),
              (object) XElementSerializer.ToXml("ShowCommentString", settings.ShowCommentString),
              (object) XElementSerializer.ToXml("ShowCommentPluginInfo", settings.ShowCommentPluginInfo)
            }),
            (object) new XElement((XName) "Colors", new object[13]
            {
              (object) XElementSerializer.ToXml("BackgroundColor", settings.BackgroundColor),
              (object) XElementSerializer.ToXml("SelectedColor", settings.SelectedColor),
              (object) XElementSerializer.ToXml("HiddenColor", settings.HiddenColor),
              (object) XElementSerializer.ToXml("OffsetColor", settings.OffsetColor),
              (object) XElementSerializer.ToXml("AddressColor", settings.AddressColor),
              (object) XElementSerializer.ToXml("HexColor", settings.HexColor),
              (object) XElementSerializer.ToXml("TypeColor", settings.TypeColor),
              (object) XElementSerializer.ToXml("NameColor", settings.NameColor),
              (object) XElementSerializer.ToXml("ValueColor", settings.ValueColor),
              (object) XElementSerializer.ToXml("IndexColor", settings.IndexColor),
              (object) XElementSerializer.ToXml("CommentColor", settings.CommentColor),
              (object) XElementSerializer.ToXml("TextColor", settings.TextColor),
              (object) XElementSerializer.ToXml("VTableColor", settings.VTableColor)
            }),
            (object) settings.CustomData.Serialize("CustomData")
          })
        }).Save((TextWriter) streamWriter);
    }

    private static void EnsureSettingsDirectoryAvailable()
    {
      try
      {
        if (Directory.Exists(PathUtil.SettingsFolderPath))
          return;
        Directory.CreateDirectory(PathUtil.SettingsFolderPath);
      }
      catch
      {
      }
    }
  }
}
