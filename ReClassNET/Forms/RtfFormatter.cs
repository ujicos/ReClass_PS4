// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.RtfFormatter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ColorCode;
using ColorCode.Parsing;
using ReClassNET.Util.Rtf;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReClassNET.Forms
{
  internal class RtfFormatter : IFormatter
  {
    private readonly RtfBuilder builder = new RtfBuilder(RtfFont.Consolas, 20f);

    public void Write(
      string parsedSourceCode,
      IList<Scope> scopes,
      IStyleSheet styleSheet,
      TextWriter textWriter)
    {
      if (scopes.Any<Scope>())
        this.builder.SetForeColor(styleSheet.Styles[scopes.First<Scope>().Name].Foreground).Append(parsedSourceCode);
      else
        this.builder.Append(parsedSourceCode);
    }

    public void WriteHeader(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
    {
    }

    public void WriteFooter(IStyleSheet styleSheet, ILanguage language, TextWriter textWriter)
    {
      textWriter.Write(this.builder.ToString());
    }
  }
}
