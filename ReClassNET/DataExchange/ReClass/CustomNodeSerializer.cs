// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.CustomNodeSerializer
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.ReClass
{
  internal class CustomNodeSerializer
  {
    private static readonly List<ICustomNodeSerializer> converters = new List<ICustomNodeSerializer>();

    public static void Add(ICustomNodeSerializer serializer)
    {
      CustomNodeSerializer.converters.Add(serializer);
    }

    public static void Remove(ICustomNodeSerializer serializer)
    {
      CustomNodeSerializer.converters.Remove(serializer);
    }

    public static ICustomNodeSerializer GetReadConverter(XElement element)
    {
      return CustomNodeSerializer.converters.FirstOrDefault<ICustomNodeSerializer>((Func<ICustomNodeSerializer, bool>) (c => c.CanHandleElement(element)));
    }

    public static ICustomNodeSerializer GetWriteConverter(BaseNode node)
    {
      return CustomNodeSerializer.converters.FirstOrDefault<ICustomNodeSerializer>((Func<ICustomNodeSerializer, bool>) (c => c.CanHandleNode(node)));
    }
  }
}
