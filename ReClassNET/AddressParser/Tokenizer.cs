// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.Tokenizer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReClassNET.AddressParser
{
  public class Tokenizer : ITokenizer
  {
    private readonly TextReader reader;
    private char currentCharacter;

    public Token Token { get; private set; }

    public string Identifier { get; private set; }

    public long Number { get; private set; }

    public Tokenizer(TextReader reader)
    {
      this.reader = reader;
      this.ReadNextCharacter();
      this.ReadNextToken();
    }

    public void ReadNextToken()
    {
      this.SkipWhitespaces();
      if (this.currentCharacter == char.MinValue)
      {
        this.Token = Token.None;
        this.Identifier = (string) null;
        this.Number = 0L;
      }
      else if (this.TryReadSimpleToken())
        this.ReadNextCharacter();
      else if (!this.TryReadNumberToken() && !this.TryReadIdentifierToken())
        throw new ParseException(string.Format("Invalid character '{0}'.", (object) this.currentCharacter));
    }

    private void ReadNextCharacter()
    {
      int num = this.reader.Read();
      this.currentCharacter = num < 0 ? char.MinValue : (char) num;
    }

    private void SkipWhitespaces()
    {
      while (char.IsWhiteSpace(this.currentCharacter))
        this.ReadNextCharacter();
    }

    private bool TryReadSimpleToken()
    {
      switch (this.currentCharacter)
      {
        case '(':
          this.Token = Token.OpenParenthesis;
          return true;
        case ')':
          this.Token = Token.CloseParenthesis;
          return true;
        case '*':
          this.Token = Token.Multiply;
          return true;
        case '+':
          this.Token = Token.Add;
          return true;
        case ',':
          this.Token = Token.Comma;
          return true;
        case '-':
          this.Token = Token.Subtract;
          return true;
        case '/':
          this.Token = Token.Divide;
          return true;
        case '[':
          this.Token = Token.OpenBrackets;
          return true;
        case ']':
          this.Token = Token.CloseBrackets;
          return true;
        default:
          return false;
      }
    }

    private bool TryReadNumberToken()
    {
      if (!IsHexadecimalDigit(this.currentCharacter))
        return false;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      while (IsHexadecimalDigit(this.currentCharacter) || IsHexadecimalIdentifier(this.currentCharacter) && !flag && (stringBuilder.Length == 1 && stringBuilder[0] == '0'))
      {
        stringBuilder.Append(this.currentCharacter);
        if (!flag)
          flag = IsHexadecimalIdentifier(this.currentCharacter);
        this.ReadNextCharacter();
      }
      if (flag)
        stringBuilder.Remove(0, 2);
      long result;
      if (!long.TryParse(stringBuilder.ToString(), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        throw new ParseException(string.Format("Could not parse '{0}' as number.", (object) stringBuilder));
      this.Number = result;
      this.Token = Token.Number;
      return true;

      bool IsHexadecimalDigit(char c)
      {
        if (char.IsDigit(c) || 'a' <= c && c <= 'f')
          return true;
        return 'A' <= c && c <= 'F';
      }

      bool IsHexadecimalIdentifier(char c)
      {
        return c == 'x' || c == 'X';
      }
    }

    private bool TryReadIdentifierToken()
    {
      if (this.currentCharacter != '<')
        return false;
      this.ReadNextCharacter();
      StringBuilder stringBuilder = new StringBuilder();
      while (this.currentCharacter != char.MinValue && this.currentCharacter != '>')
      {
        stringBuilder.Append(this.currentCharacter);
        this.ReadNextCharacter();
      }
      if (this.currentCharacter != '>')
        throw new ParseException("Invalid identifier, missing '>'.");
      this.ReadNextCharacter();
      this.Identifier = stringBuilder.ToString();
      this.Token = Token.Identifier;
      return true;
    }
  }
}
