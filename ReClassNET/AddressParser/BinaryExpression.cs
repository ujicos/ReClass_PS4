// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.BinaryExpression
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.AddressParser
{
  public abstract class BinaryExpression : IExpression
  {
    public IExpression Lhs { get; }

    public IExpression Rhs { get; }

    protected BinaryExpression(IExpression lhs, IExpression rhs)
    {
      this.Lhs = lhs;
      this.Rhs = rhs;
    }
  }
}
