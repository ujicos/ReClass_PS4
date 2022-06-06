// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.ConstantExpression
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

namespace ReClassNET.AddressParser
{
  public class ConstantExpression : IExpression
  {
    public long Value { get; }

    public ConstantExpression(long value)
    {
      this.Value = value;
    }
  }
}
