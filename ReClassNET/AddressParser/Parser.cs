// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.Parser
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;

namespace ReClassNET.AddressParser
{
  public class Parser
  {
    private readonly ITokenizer tokenizer;

    public Parser(ITokenizer tokenizer)
    {
      this.tokenizer = tokenizer;
    }

    public IExpression ParseExpression()
    {
      IExpression addSubtract = this.ParseAddSubtract();
      if (this.tokenizer.Token == Token.None)
        return addSubtract;
      throw new ParseException("Unexpected characters at end of expression");
    }

    private IExpression ParseAddSubtract()
    {
      IExpression lhs = this.ParseMultiplyDivide();
      while (this.tokenizer.Token == Token.Add || this.tokenizer.Token == Token.Subtract)
      {
        int token = (int) this.tokenizer.Token;
        this.tokenizer.ReadNextToken();
        IExpression multiplyDivide = this.ParseMultiplyDivide();
        lhs = token != 1 ? (IExpression) new SubtractExpression(lhs, multiplyDivide) : (IExpression) new AddExpression(lhs, multiplyDivide);
      }
      return lhs;
    }

    private IExpression ParseMultiplyDivide()
    {
      IExpression lhs = this.ParseUnary();
      while (this.tokenizer.Token == Token.Multiply || this.tokenizer.Token == Token.Divide)
      {
        int token = (int) this.tokenizer.Token;
        this.tokenizer.ReadNextToken();
        IExpression unary = this.ParseUnary();
        lhs = token != 3 ? (IExpression) new DivideExpression(lhs, unary) : (IExpression) new MultiplyExpression(lhs, unary);
      }
      return lhs;
    }

    private IExpression ParseUnary()
    {
      while (this.tokenizer.Token == Token.Add)
        this.tokenizer.ReadNextToken();
      if (this.tokenizer.Token != Token.Subtract)
        return this.ParseLeaf();
      this.tokenizer.ReadNextToken();
      return (IExpression) new NegateExpression(this.ParseUnary());
    }

    private IExpression ParseLeaf()
    {
      switch (this.tokenizer.Token)
      {
        case Token.OpenParenthesis:
          this.tokenizer.ReadNextToken();
          IExpression addSubtract1 = this.ParseAddSubtract();
          if (this.tokenizer.Token != Token.CloseParenthesis)
            throw new ParseException("Missing close parenthesis");
          this.tokenizer.ReadNextToken();
          return addSubtract1;
        case Token.OpenBrackets:
          this.tokenizer.ReadNextToken();
          IExpression addSubtract2 = this.ParseAddSubtract();
          int num = IntPtr.Size;
          if (this.tokenizer.Token == Token.Comma)
          {
            this.tokenizer.ReadNextToken();
            if (this.tokenizer.Token != Token.Number)
              throw new ParseException("Missing read byte count");
            if (this.tokenizer.Number != 4L && this.tokenizer.Number != 8L)
              throw new ParseException("The byte count must be 4 or 8.");
            num = (int) this.tokenizer.Number;
            this.tokenizer.ReadNextToken();
          }
          if (this.tokenizer.Token != Token.CloseBrackets)
            throw new ParseException("Missing close bracket");
          this.tokenizer.ReadNextToken();
          int byteCount = num;
          return (IExpression) new ReadMemoryExpression(addSubtract2, byteCount);
        case Token.Number:
          ConstantExpression constantExpression = new ConstantExpression(this.tokenizer.Number);
          this.tokenizer.ReadNextToken();
          return (IExpression) constantExpression;
        case Token.Identifier:
          ModuleExpression moduleExpression = new ModuleExpression(this.tokenizer.Identifier);
          this.tokenizer.ReadNextToken();
          return (IExpression) moduleExpression;
        default:
          throw new ParseException(string.Format("Unexpect token: {0}", (object) this.tokenizer.Token));
      }
    }

    public static IExpression Parse(string str)
    {
      using (StringReader stringReader = new StringReader(str))
        return Parser.Parse((ITokenizer) new Tokenizer((TextReader) stringReader));
    }

    private static IExpression Parse(ITokenizer tokenizer)
    {
      return new Parser(tokenizer).ParseExpression();
    }
  }
}
