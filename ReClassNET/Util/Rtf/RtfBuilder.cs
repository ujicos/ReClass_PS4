// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.Rtf.RtfBuilder
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ReClassNET.Util.Rtf
{
  public class RtfBuilder
  {
    private static readonly char[] slashable = new char[3]
    {
      '{',
      '}',
      '\\'
    };
    private readonly Color defaultForeColor = Color.White;
    private readonly Color defaultBackColor = Color.Empty;
    private readonly List<Color> usedColors = new List<Color>();
    private readonly List<string> usedFonts = new List<string>();
    private readonly StringBuilder buffer;
    private readonly float defaultFontSize;
    private Color foreColor;
    private Color backColor;
    private int fontIndex;
    private float fontSize;
    private FontStyle fontStyle;

    public RtfBuilder()
      : this(RtfFont.Calibri, 22f)
    {
    }

    public RtfBuilder(RtfFont defaultFont, float defaultFontSize)
    {
      this.buffer = new StringBuilder();
      this.fontIndex = this.IndexOfFont(defaultFont);
      this.defaultFontSize = defaultFontSize;
      this.fontSize = defaultFontSize;
      this.usedColors.Add(this.defaultForeColor);
      this.usedColors.Add(this.defaultBackColor);
      this.fontStyle = FontStyle.Regular;
      this.foreColor = this.defaultForeColor;
      this.backColor = this.defaultBackColor;
    }

    public RtfBuilder Append(char value)
    {
      return this.Append(value.ToString());
    }

    public RtfBuilder Append(string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        using (new RtfBuilder.RtfFormatWrapper(this))
        {
          value = RtfBuilder.EscapeString(value);
          if (value.IndexOf(Environment.NewLine, StringComparison.Ordinal) >= 0)
            this.buffer.Append(string.Join("\\line ", value.Split(new string[1]
            {
              Environment.NewLine
            }, StringSplitOptions.None)));
          else
            this.buffer.Append(value);
        }
      }
      return this;
    }

    public RtfBuilder AppendLevel(int level)
    {
      this.buffer.AppendFormat("\\level{0} ", (object) level);
      return this;
    }

    public RtfBuilder AppendLine()
    {
      this.buffer.AppendLine("\\line");
      return this;
    }

    public RtfBuilder AppendLine(string value)
    {
      this.Append(value);
      return this.AppendLine();
    }

    public RtfBuilder AppendParagraph()
    {
      this.buffer.AppendLine("\\par");
      return this;
    }

    public RtfBuilder AppendPage()
    {
      this.buffer.AppendLine("\\page");
      return this;
    }

    public RtfBuilder SetForeColor(Color color)
    {
      this.foreColor = color;
      return this;
    }

    public RtfBuilder SetBackColor(Color color)
    {
      this.backColor = color;
      return this;
    }

    public RtfBuilder SetFont(RtfFont font)
    {
      this.fontIndex = this.IndexOfFont(font);
      return this;
    }

    public RtfBuilder SetFontSize(float size)
    {
      this.fontSize = size;
      return this;
    }

    public RtfBuilder SetFontStyle(FontStyle style)
    {
      this.fontStyle = style;
      return this;
    }

    protected int IndexOfColor(Color color)
    {
      if (!this.usedColors.Contains(color))
        this.usedColors.Add(color);
      return this.usedColors.IndexOf(color) + 1;
    }

    private int IndexOfFont(RtfFont font)
    {
      return this.IndexOfRawFont(RtfBuilder.GetKnownFontString(font));
    }

    private int IndexOfRawFont(string font)
    {
      if (string.IsNullOrEmpty(font))
        return 0;
      int num = this.usedFonts.IndexOf(font);
      if (num >= 0)
        return num;
      this.usedFonts.Add(font);
      return this.usedFonts.Count - 1;
    }

    private static string GetKnownFontString(RtfFont font)
    {
      switch (font)
      {
        case RtfFont.Arial:
          return "{{\\f{0}\\fswiss\\fprq2\\fcharset0 Arial;}}";
        case RtfFont.Calibri:
          return "{{\\f{0}\\fnil\\fcharset0 Calibri;}}";
        case RtfFont.Consolas:
          return "{{\\f{0}\\fmodern\\fprq1\\fcharset0 Consolas;}}";
        case RtfFont.CourierNew:
          return "{{\\f{0}\\fmodern\\fprq1\\fcharset0 Courier New;}}";
        case RtfFont.Impact:
          return "{{\\f{0}\\fswiss\\fprq2\\fcharset0 Impact;}}";
        case RtfFont.LucidaConsole:
          return "{{\\f{0}\\fmodern\\fprq1\\fcharset0 Lucida Console;}}";
        case RtfFont.Symbol:
          return "{{\\f{0}\\ftech\\fcharset0 Symbol;}}";
        case RtfFont.MSSansSerif:
          return "{{\\f{0}\\fswiss\\fprq2\\fcharset0 MS Reference Sans Serif;}}";
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public RtfBuilder Reset()
    {
      this.buffer.AppendLine("\\pard");
      return this;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang3081");
      stringBuilder.Append("{\\fonttbl");
      for (int index = 0; index < this.usedFonts.Count; ++index)
        stringBuilder.AppendFormat(this.usedFonts[index], (object) index);
      stringBuilder.AppendLine("}");
      stringBuilder.Append("{\\colortbl ;");
      foreach (Color usedColor in this.usedColors)
        stringBuilder.Append(string.Format("\\red{0}\\green{1}\\blue{2};", (object) usedColor.R, (object) usedColor.G, (object) usedColor.B));
      stringBuilder.AppendLine("}");
      stringBuilder.Append("\\viewkind4\\uc1\\pard\\plain\\f0");
      stringBuilder.AppendFormat("\\fs{0} ", (object) this.defaultFontSize);
      stringBuilder.AppendLine();
      stringBuilder.Append((object) this.buffer);
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    private static string EscapeString(string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        if (value.IndexOfAny(RtfBuilder.slashable) >= 0)
          value = value.Replace("\\", "\\\\").Replace("{", "\\{").Replace("}", "\\}");
        if (value.Any<char>((Func<char, bool>) (c => c > 'ÿ')))
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (char ch in value)
          {
            if (ch <= 'ÿ')
              stringBuilder.Append(ch);
            else if (ch == '\t')
            {
              stringBuilder.Append("\\tab");
            }
            else
            {
              stringBuilder.Append("\\u");
              stringBuilder.Append((int) ch);
              stringBuilder.Append("?");
            }
          }
          value = stringBuilder.ToString();
        }
      }
      return value;
    }

    private class RtfFormatWrapper : IDisposable
    {
      private readonly RtfBuilder builder;

      public RtfFormatWrapper(RtfBuilder builder)
      {
        this.builder = builder;
        StringBuilder buffer = builder.buffer;
        int length = buffer.Length;
        if ((builder.fontStyle & FontStyle.Bold) == FontStyle.Bold)
          buffer.Append("\\b");
        if ((builder.fontStyle & FontStyle.Italic) == FontStyle.Italic)
          buffer.Append("\\i");
        if ((builder.fontStyle & FontStyle.Underline) == FontStyle.Underline)
          buffer.Append("\\ul");
        if ((builder.fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
          buffer.Append("\\strike");
        if ((double) builder.fontSize != (double) builder.defaultFontSize)
          buffer.AppendFormat("\\fs{0}", (object) builder.fontSize);
        if (builder.fontIndex != 0)
          buffer.AppendFormat("\\f{0}", (object) builder.fontIndex);
        if (builder.foreColor != builder.defaultForeColor)
          buffer.AppendFormat("\\cf{0}", (object) builder.IndexOfColor(builder.foreColor));
        if (builder.backColor != builder.defaultBackColor)
          buffer.AppendFormat("\\highlight{0}", (object) builder.IndexOfColor(builder.backColor));
        if (buffer.Length <= length)
          return;
        buffer.Append(" ");
      }

      public void Dispose()
      {
        StringBuilder buffer = this.builder.buffer;
        int length = buffer.Length;
        if ((this.builder.fontStyle & FontStyle.Bold) == FontStyle.Bold)
          buffer.Append("\\b0");
        if ((this.builder.fontStyle & FontStyle.Italic) == FontStyle.Italic)
          buffer.Append("\\i0");
        if ((this.builder.fontStyle & FontStyle.Underline) == FontStyle.Underline)
          buffer.Append("\\ulnone");
        if ((this.builder.fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
          buffer.Append("\\strike0");
        this.builder.fontStyle = FontStyle.Regular;
        if ((double) this.builder.fontSize != (double) this.builder.defaultFontSize)
        {
          this.builder.fontSize = this.builder.defaultFontSize;
          buffer.AppendFormat("\\fs{0} ", (object) this.builder.defaultFontSize);
        }
        if (this.builder.fontIndex != 0)
        {
          buffer.Append("\\f0");
          this.builder.fontIndex = 0;
        }
        if (this.builder.foreColor != this.builder.defaultForeColor)
        {
          this.builder.foreColor = this.builder.defaultForeColor;
          buffer.Append("\\cf0");
        }
        if (this.builder.backColor != this.builder.defaultBackColor)
        {
          this.builder.backColor = this.builder.defaultBackColor;
          buffer.Append("\\highlight0");
        }
        if (buffer.Length <= length)
          return;
        buffer.Append(" ");
      }
    }
  }
}
