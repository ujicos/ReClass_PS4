// Decompiled with JetBrains decompiler
// Type: ReClassNET.CodeGenerator.CSharpCodeGenerator
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReClassNET.CodeGenerator
{
  public class CSharpCodeGenerator : ICodeGenerator
  {
    private static readonly Dictionary<Type, string> nodeTypeToTypeDefinationMap = new Dictionary<Type, string>()
    {
      [typeof (DoubleNode)] = "double",
      [typeof (FloatNode)] = "float",
      [typeof (BoolNode)] = "bool",
      [typeof (Int8Node)] = "sbyte",
      [typeof (Int16Node)] = "short",
      [typeof (Int32Node)] = "int",
      [typeof (Int64Node)] = "long",
      [typeof (NIntNode)] = "IntPtr",
      [typeof (UInt8Node)] = "byte",
      [typeof (UInt16Node)] = "ushort",
      [typeof (UInt32Node)] = "uint",
      [typeof (UInt64Node)] = "ulong",
      [typeof (NUIntNode)] = "UIntPtr",
      [typeof (FunctionPtrNode)] = "IntPtr",
      [typeof (Utf8TextPtrNode)] = "IntPtr",
      [typeof (Utf16TextPtrNode)] = "IntPtr",
      [typeof (Utf32TextPtrNode)] = "IntPtr",
      [typeof (PointerNode)] = "IntPtr",
      [typeof (VirtualMethodTableNode)] = "IntPtr",
      [typeof (Vector2Node)] = "Vector2",
      [typeof (Vector3Node)] = "Vector3",
      [typeof (Vector4Node)] = "Vector4"
    };

    public Language Language
    {
      get
      {
        return Language.CSharp;
      }
    }

    public string GenerateCode(
      IReadOnlyList<ClassNode> classes,
      IReadOnlyList<EnumDescription> enums,
      ILogger logger)
    {
      using (StringWriter stringWriter = new StringWriter())
      {
        using (IndentedTextWriter writer = new IndentedTextWriter((TextWriter) stringWriter, "\t"))
        {
          writer.WriteLine("// Created with ReClass.NET PS4 Port by MrReeko");
          writer.WriteLine();
          writer.WriteLine("// Warning: The C# code generator doesn't support all node types!");
          writer.WriteLine();
          writer.WriteLine("using System.Runtime.InteropServices;");
          writer.WriteLine("// optional namespace, only for vectors");
          writer.WriteLine("using System.Numerics;");
          writer.WriteLine();
          using (IEnumerator<EnumDescription> enumerator = enums.GetEnumerator())
          {
            if (enumerator.MoveNext())
            {
              CSharpCodeGenerator.WriteEnum(writer, enumerator.Current);
              while (enumerator.MoveNext())
              {
                writer.WriteLine();
                CSharpCodeGenerator.WriteEnum(writer, enumerator.Current);
              }
              writer.WriteLine();
            }
          }
          IEnumerable<ClassNode> classNodes = classes.Where<ClassNode>((Func<ClassNode, bool>) (c => c.Nodes.None<BaseNode>((Func<BaseNode, bool>) (n => n is FunctionNode)))).Distinct<ClassNode>();
          HashSet<int> unicodeStringClassLengthsToGenerate = new HashSet<int>();
          using (IEnumerator<ClassNode> enumerator = classNodes.GetEnumerator())
          {
            if (enumerator.MoveNext())
            {
              FindUnicodeStringClasses((IEnumerable<BaseNode>) enumerator.Current.Nodes);
              CSharpCodeGenerator.WriteClass(writer, enumerator.Current, logger);
              while (enumerator.MoveNext())
              {
                writer.WriteLine();
                FindUnicodeStringClasses((IEnumerable<BaseNode>) enumerator.Current.Nodes);
                CSharpCodeGenerator.WriteClass(writer, enumerator.Current, logger);
              }
            }
          }
          if (unicodeStringClassLengthsToGenerate.Any<int>())
          {
            foreach (int length in unicodeStringClassLengthsToGenerate)
            {
              writer.WriteLine();
              CSharpCodeGenerator.WriteUnicodeStringClass(writer, length);
            }
          }
          return stringWriter.ToString();

          void FindUnicodeStringClasses(IEnumerable<BaseNode> nodes)
          {
            unicodeStringClassLengthsToGenerate.UnionWith(nodes.OfType<Utf16TextNode>().Select<Utf16TextNode, int>((Func<Utf16TextNode, int>) (n => n.Length)));
          }
        }
      }
    }

    private static void WriteEnum(IndentedTextWriter writer, EnumDescription @enum)
    {
      writer.Write("enum " + @enum.Name + " : ");
      switch (@enum.Size)
      {
        case EnumDescription.UnderlyingTypeSize.OneByte:
          writer.WriteLine(CSharpCodeGenerator.nodeTypeToTypeDefinationMap[typeof (Int8Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.TwoBytes:
          writer.WriteLine(CSharpCodeGenerator.nodeTypeToTypeDefinationMap[typeof (Int16Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.FourBytes:
          writer.WriteLine(CSharpCodeGenerator.nodeTypeToTypeDefinationMap[typeof (Int32Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.EightBytes:
          writer.WriteLine(CSharpCodeGenerator.nodeTypeToTypeDefinationMap[typeof (Int64Node)]);
          break;
      }
      writer.WriteLine("{");
      ++writer.Indent;
      for (int index = 0; index < @enum.Values.Count; ++index)
      {
        KeyValuePair<string, long> keyValuePair = @enum.Values[index];
        writer.Write(keyValuePair.Key);
        writer.Write(" = ");
        writer.Write(keyValuePair.Value);
        if (index < @enum.Values.Count - 1)
          writer.Write(",");
        writer.WriteLine();
      }
      --writer.Indent;
      writer.WriteLine("};");
    }

    private static void WriteClass(IndentedTextWriter writer, ClassNode @class, ILogger logger)
    {
      writer.WriteLine("[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]");
      writer.Write("public struct ");
      writer.Write(@class.Name);
      if (!string.IsNullOrEmpty(@class.Comment))
      {
        writer.Write(" // ");
        writer.Write(@class.Comment);
      }
      writer.WriteLine();
      writer.WriteLine("{");
      ++writer.Indent;
      foreach (BaseNode node in @class.Nodes.WhereNot<BaseNode>((Func<BaseNode, bool>) (n => n is FunctionNode || n is BaseHexNode)))
      {
        (string typeName2, string attribute2) = CSharpCodeGenerator.GetTypeDefinition(node);
        if (typeName2 != null)
        {
          if (attribute2 != null)
            writer.WriteLine(attribute2);
          writer.WriteLine(string.Format("[FieldOffset(0x{0:X})]", (object) node.Offset));
          writer.Write("public readonly " + typeName2 + " " + node.Name + ";");
          if (!string.IsNullOrEmpty(node.Comment))
          {
            writer.Write(" //");
            writer.Write(node.Comment);
          }
          writer.WriteLine();
        }
        else
          logger.Log(ReClassNET.Logger.LogLevel.Warning, string.Format("Skipping node with unhandled type: {0}", (object) node.GetType()));
      }
      --writer.Indent;
      writer.WriteLine("}");
    }

    private static (string typeName, string attribute) GetTypeDefinition(BaseNode node)
    {
      if (node is BitFieldNode bitFieldNode)
      {
        BaseNumericNode underlayingNode = bitFieldNode.GetUnderlayingNode();
        underlayingNode.CopyFromNode(node);
        node = (BaseNode) underlayingNode;
      }
      string str;
      if (CSharpCodeGenerator.nodeTypeToTypeDefinationMap.TryGetValue(node.GetType(), out str))
        return (str, (string) null);
      (string, string) valueTuple;
      switch (node)
      {
        case EnumNode enumNode:
          valueTuple = (enumNode.Enum.Name, (string) null);
          break;
        case Utf8TextNode utf8TextNode:
          valueTuple = ("string", string.Format("[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {0})]", (object) utf8TextNode.Length));
          break;
        case Utf16TextNode utf16TextNode:
          valueTuple = (CSharpCodeGenerator.GetUnicodeStringClassName(utf16TextNode.Length), "[MarshalAs(UnmanagedType.Struct)]");
          break;
        default:
          valueTuple = ((string) null, (string) null);
          break;
      }
      return valueTuple;
    }

    private static string GetUnicodeStringClassName(int length)
    {
      return string.Format("__UnicodeString{0}", (object) length);
    }

    private static void WriteUnicodeStringClass(IndentedTextWriter writer, int length)
    {
      string unicodeStringClassName = CSharpCodeGenerator.GetUnicodeStringClassName(length);
      writer.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]");
      writer.WriteLine("public struct " + unicodeStringClassName);
      writer.WriteLine("{");
      ++writer.Indent;
      writer.WriteLine(string.Format("[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {0})]", (object) length));
      writer.WriteLine("public string Value;");
      writer.WriteLine();
      writer.WriteLine("public static implicit operator string(" + unicodeStringClassName + " value) => value.Value;");
      --writer.Indent;
      writer.WriteLine("}");
    }
  }
}
