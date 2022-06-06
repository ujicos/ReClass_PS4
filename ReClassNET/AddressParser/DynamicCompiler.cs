// Decompiled with JetBrains decompiler
// Type: ReClassNET.AddressParser.DynamicCompiler
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ReClassNET.AddressParser
{
  public class DynamicCompiler : IExecutor
  {
    public IntPtr Execute(IExpression expression, IProcessReader processReader)
    {
      return DynamicCompiler.CompileExpression(expression)(processReader);
    }

    public static Func<IProcessReader, IntPtr> CompileExpression(
      IExpression expression)
    {
      ParameterExpression parameterExpression;
      return Expression.Lambda<Func<IProcessReader, IntPtr>>(DynamicCompiler.GenerateMethodBody(expression, (Expression) parameterExpression), parameterExpression).Compile();
    }

    private static Expression GenerateMethodBody(
      IExpression expression,
      Expression processParameter)
    {
      switch (expression)
      {
        case ConstantExpression constantExpression2:
          return (Expression) Expression.Call((Expression) null, typeof (IntPtrExtension).GetRuntimeMethod("From", new Type[1]
          {
            typeof (long)
          }), (Expression) Expression.Constant((object) constantExpression2.Value));
        case NegateExpression negateExpression:
          return (Expression) Expression.Call((Expression) null, typeof (IntPtrExtension).GetRuntimeMethod("Negate", new Type[1]
          {
            typeof (IntPtr)
          }), DynamicCompiler.GenerateMethodBody(negateExpression.Expression, processParameter));
        case AddExpression addExpression:
          return (Expression) Expression.Call((Expression) null, GetIntPtrExtension("Add"), DynamicCompiler.GenerateMethodBody(addExpression.Lhs, processParameter), DynamicCompiler.GenerateMethodBody(addExpression.Rhs, processParameter));
        case SubtractExpression subtractExpression:
          return (Expression) Expression.Call((Expression) null, GetIntPtrExtension("Sub"), DynamicCompiler.GenerateMethodBody(subtractExpression.Lhs, processParameter), DynamicCompiler.GenerateMethodBody(subtractExpression.Rhs, processParameter));
        case MultiplyExpression multiplyExpression:
          return (Expression) Expression.Call((Expression) null, GetIntPtrExtension("Mul"), DynamicCompiler.GenerateMethodBody(multiplyExpression.Lhs, processParameter), DynamicCompiler.GenerateMethodBody(multiplyExpression.Rhs, processParameter));
        case DivideExpression divideExpression:
          return (Expression) Expression.Call((Expression) null, GetIntPtrExtension("Div"), DynamicCompiler.GenerateMethodBody(divideExpression.Lhs, processParameter), DynamicCompiler.GenerateMethodBody(divideExpression.Rhs, processParameter));
        case ModuleExpression moduleExpression:
          MethodInfo runtimeMethod = typeof (IProcessReader).GetRuntimeMethod("GetModuleByName", new Type[1]
          {
            typeof (string)
          });
          System.Linq.Expressions.ConstantExpression constantExpression1 = Expression.Constant((object) moduleExpression.Name);
          ParameterExpression parameterExpression = Expression.Variable(typeof (ReClassNET.Memory.Module));
          System.Linq.Expressions.BinaryExpression binaryExpression = Expression.Assign((Expression) parameterExpression, (Expression) Expression.Call(processParameter, runtimeMethod, (Expression) constantExpression1));
          return (Expression) Expression.Block((IEnumerable<ParameterExpression>) new ParameterExpression[1]
          {
            parameterExpression
          }, (Expression) binaryExpression, (Expression) Expression.Condition((Expression) Expression.Equal((Expression) parameterExpression, (Expression) Expression.Constant((object) null)), (Expression) Expression.Constant((object) IntPtr.Zero), (Expression) Expression.MakeMemberAccess((Expression) parameterExpression, (MemberInfo) typeof (ReClassNET.Memory.Module).GetProperty("Start"))));
        case ReadMemoryExpression memoryExpression:
          Expression methodBody = DynamicCompiler.GenerateMethodBody(memoryExpression.Expression, processParameter);
          MethodCallExpression methodCallExpression = Expression.Call((Expression) null, typeof (IRemoteMemoryReaderExtension).GetRuntimeMethod(memoryExpression.ByteCount == 4 ? "ReadRemoteInt32" : "ReadRemoteInt64", new Type[2]
          {
            typeof (IRemoteMemoryReader),
            typeof (IntPtr)
          }), processParameter, methodBody);
          return (Expression) Expression.Call((Expression) null, typeof (IntPtrExtension).GetRuntimeMethod("From", new Type[1]
          {
            memoryExpression.ByteCount == 4 ? typeof (int) : typeof (long)
          }), (Expression) methodCallExpression);
        default:
          throw new ArgumentException("Unsupported operation '" + expression.GetType().FullName + "'.");
      }

      MethodInfo GetIntPtrExtension(string name)
      {
        return typeof (IntPtrExtension).GetRuntimeMethod(name, new Type[2]
        {
          typeof (IntPtr),
          typeof (IntPtr)
        });
      }
    }
  }
}
