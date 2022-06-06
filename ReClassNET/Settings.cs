// Decompiled with JetBrains decompiler
// Type: ReClassNET.Settings
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Util;
using System.Drawing;
using System.Text;

namespace ReClassNET
{
  public class Settings
  {
    public string LastProcess { get; set; } = string.Empty;

    public bool StayOnTop { get; set; }

    public bool RunAsAdmin { get; set; }

    public bool RandomizeWindowTitle { get; set; }

    public bool ShowNodeAddress { get; set; } = true;

    public bool ShowNodeOffset { get; set; } = true;

    public bool ShowNodeText { get; set; } = true;

    public bool HighlightChangedValues { get; set; } = true;

    public Encoding RawDataEncoding { get; set; } = Encoding.GetEncoding(1252);

    public bool ShowCommentFloat { get; set; } = true;

    public bool ShowCommentInteger { get; set; } = true;

    public bool ShowCommentPointer { get; set; } = true;

    public bool ShowCommentRtti { get; set; } = true;

    public bool ShowCommentSymbol { get; set; } = true;

    public bool ShowCommentString { get; set; } = true;

    public bool ShowCommentPluginInfo { get; set; } = true;

    public Color BackgroundColor { get; set; } = Color.FromArgb(69, 73, 74);

    public Color SelectedColor { get; set; } = Color.FromArgb(82, 90, 95);

    public Color HiddenColor { get; set; } = Color.FromArgb(240, 240, 240);

    public Color OffsetColor { get; set; } = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);

    public Color AddressColor { get; set; } = Color.FromArgb((int) byte.MaxValue, 64, 64);

    public Color HexColor { get; set; } = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);

    public Color TypeColor { get; set; } = Color.FromArgb(61, 205, 231);

    public Color NameColor { get; set; } = Color.FromArgb(155, 146, 239);

    public Color ValueColor { get; set; } = Color.FromArgb((int) byte.MaxValue, 128, 0);

    public Color IndexColor { get; set; } = Color.FromArgb(32, 200, 200);

    public Color CommentColor { get; set; } = Color.FromArgb(0, 200, 0);

    public Color TextColor { get; set; } = Color.FromArgb((int) byte.MaxValue, 128, 0);

    public Color VTableColor { get; set; } = Color.FromArgb(0, (int) byte.MaxValue, 0);

    public Color PluginColor { get; set; } = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue);

    public CustomDataMap CustomData { get; } = new CustomDataMap();

    public Settings Clone()
    {
      return this.MemberwiseClone() as Settings;
    }
  }
}
