// Decompiled with JetBrains decompiler
// Type: ReClassNET.MemoryScanner.Comparer.InvalidCompareTypeException
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;

namespace ReClassNET.MemoryScanner.Comparer
{
  public class InvalidCompareTypeException : Exception
  {
    public InvalidCompareTypeException(ScanCompareType type)
      : base(string.Format("{0} is not valid in the current state.", (object) type))
    {
    }
  }
}
