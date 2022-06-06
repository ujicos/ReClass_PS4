// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.Interpreter
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;

namespace ReClassNET.AddressParser
{
  public class Interpreter : IExecutor
  {
    public IntPtr Execute(IExpression expression, IProcessReader processReader)
    {
      switch (expression)
      {
        case ConstantExpression constantExpression:
          return IntPtrExtension.From(constantExpression.Value);
        case NegateExpression negateExpression:
          return this.Execute(negateExpression.Expression, processReader).Negate();
        case ModuleExpression moduleExpression:
          Module moduleByName = processReader.GetModuleByName(moduleExpression.Name);
          return moduleByName != null ? moduleByName.Start : IntPtr.Zero;
        case AddExpression addExpression:
          return this.Execute(addExpression.Lhs, processReader).Add(this.Execute(addExpression.Rhs, processReader));
        case SubtractExpression subtractExpression:
          return this.Execute(subtractExpression.Lhs, processReader).Sub(this.Execute(subtractExpression.Rhs, processReader));
        case MultiplyExpression multiplyExpression:
          return this.Execute(multiplyExpression.Lhs, processReader).Mul(this.Execute(multiplyExpression.Rhs, processReader));
        case DivideExpression divideExpression:
          return this.Execute(divideExpression.Lhs, processReader).Div(this.Execute(divideExpression.Rhs, processReader));
        case ReadMemoryExpression memoryExpression:
          IntPtr address = this.Execute(memoryExpression.Expression, processReader);
          return memoryExpression.ByteCount == 4 ? IntPtrExtension.From(processReader.ReadRemoteInt32(address)) : IntPtrExtension.From(processReader.ReadRemoteInt64(address));
        default:
          throw new ArgumentException("Unsupported operation '" + expression.GetType().FullName + "'.");
      }
    }
  }
}
