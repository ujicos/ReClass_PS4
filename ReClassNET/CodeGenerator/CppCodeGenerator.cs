// Decompiled with JetBrains decompiler
// Type: ReClassNET.CodeGenerator.CppCodeGenerator
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace ReClassNET.CodeGenerator
{
  public class CppCodeGenerator : ICodeGenerator
  {
    private static readonly ISet<CustomCppCodeGenerator> customGenerators = (ISet<CustomCppCodeGenerator>) new HashSet<CustomCppCodeGenerator>();
    private readonly Dictionary<Type, string> nodeTypeToTypeDefinationMap;

    public static void Add(CustomCppCodeGenerator generator)
    {
      CppCodeGenerator.customGenerators.Add(generator);
    }

    public static void Remove(CustomCppCodeGenerator generator)
    {
      CppCodeGenerator.customGenerators.Remove(generator);
    }

    private static CustomCppCodeGenerator GetCustomCodeGeneratorForNode(
      BaseNode node)
    {
      return CppCodeGenerator.customGenerators.FirstOrDefault<CustomCppCodeGenerator>((Func<CustomCppCodeGenerator, bool>) (g => g.CanHandle(node)));
    }

    public Language Language
    {
      get
      {
        return Language.Cpp;
      }
    }

    public CppCodeGenerator(CppTypeMapping typeMapping)
    {
      this.nodeTypeToTypeDefinationMap = new Dictionary<Type, string>()
      {
        [typeof (BoolNode)] = typeMapping.TypeBool,
        [typeof (DoubleNode)] = typeMapping.TypeDouble,
        [typeof (FloatNode)] = typeMapping.TypeFloat,
        [typeof (FunctionPtrNode)] = typeMapping.TypeFunctionPtr,
        [typeof (Int8Node)] = typeMapping.TypeInt8,
        [typeof (Int16Node)] = typeMapping.TypeInt16,
        [typeof (Int32Node)] = typeMapping.TypeInt32,
        [typeof (Int64Node)] = typeMapping.TypeInt64,
        [typeof (NIntNode)] = typeMapping.TypeNInt,
        [typeof (Matrix3x3Node)] = typeMapping.TypeMatrix3x3,
        [typeof (Matrix3x4Node)] = typeMapping.TypeMatrix3x4,
        [typeof (Matrix4x4Node)] = typeMapping.TypeMatrix4x4,
        [typeof (UInt8Node)] = typeMapping.TypeUInt8,
        [typeof (UInt16Node)] = typeMapping.TypeUInt16,
        [typeof (UInt32Node)] = typeMapping.TypeUInt32,
        [typeof (UInt64Node)] = typeMapping.TypeUInt64,
        [typeof (NUIntNode)] = typeMapping.TypeNUInt,
        [typeof (CppCodeGenerator.Utf8CharacterNode)] = typeMapping.TypeUtf8Text,
        [typeof (CppCodeGenerator.Utf16CharacterNode)] = typeMapping.TypeUtf16Text,
        [typeof (CppCodeGenerator.Utf32CharacterNode)] = typeMapping.TypeUtf32Text,
        [typeof (Vector2Node)] = typeMapping.TypeVector2,
        [typeof (Vector3Node)] = typeMapping.TypeVector3,
        [typeof (Vector4Node)] = typeMapping.TypeVector4
      };
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
          using (IEnumerator<EnumDescription> enumerator = enums.GetEnumerator())
          {
            if (enumerator.MoveNext())
            {
              this.WriteEnum(writer, enumerator.Current);
              while (enumerator.MoveNext())
              {
                writer.WriteLine();
                this.WriteEnum(writer, enumerator.Current);
              }
              writer.WriteLine();
            }
          }
          HashSet<ClassNode> alreadySeen = new HashSet<ClassNode>();
          using (IEnumerator<ClassNode> enumerator = classes.Where<ClassNode>((Func<ClassNode, bool>) (c => c.Nodes.None<BaseNode>((Func<BaseNode, bool>) (n => n is FunctionNode)))).SelectMany<ClassNode, ClassNode>(new Func<ClassNode, IEnumerable<ClassNode>>(GetReversedClassHierarchy)).Distinct<ClassNode>().GetEnumerator())
          {
            if (enumerator.MoveNext())
            {
              this.WriteClass(writer, enumerator.Current, (IEnumerable<ClassNode>) classes, logger);
              while (enumerator.MoveNext())
              {
                writer.WriteLine();
                this.WriteClass(writer, enumerator.Current, (IEnumerable<ClassNode>) classes, logger);
              }
            }
          }
          return stringWriter.ToString();

          IEnumerable<ClassNode> GetReversedClassHierarchy(ClassNode node)
          {
            // ISSUE: method pointer
            return !alreadySeen.Add(node) ? Enumerable.Empty<ClassNode>() : node.Nodes.OfType<BaseWrapperNode>().Where<BaseWrapperNode>((Func<BaseWrapperNode, bool>) (w => !w.IsNodePresentInChain<PointerNode>())).Select<BaseWrapperNode, ClassNode>((Func<BaseWrapperNode, ClassNode>) (w => w.ResolveMostInnerNode() as ClassNode)).Where<ClassNode>((Func<ClassNode, bool>) (n => n != null)).SelectMany<ClassNode, ClassNode>(new Func<ClassNode, IEnumerable<ClassNode>>((object) this, __methodptr(\u003CGenerateCode\u003Eg__GetReversedClassHierarchy\u007C0))).Append<ClassNode>(node);
          }
        }
      }
    }

    private void WriteEnum(IndentedTextWriter writer, EnumDescription @enum)
    {
      writer.Write("enum class " + @enum.Name + " : ");
      switch (@enum.Size)
      {
        case EnumDescription.UnderlyingTypeSize.OneByte:
          writer.WriteLine(this.nodeTypeToTypeDefinationMap[typeof (Int8Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.TwoBytes:
          writer.WriteLine(this.nodeTypeToTypeDefinationMap[typeof (Int16Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.FourBytes:
          writer.WriteLine(this.nodeTypeToTypeDefinationMap[typeof (Int32Node)]);
          break;
        case EnumDescription.UnderlyingTypeSize.EightBytes:
          writer.WriteLine(this.nodeTypeToTypeDefinationMap[typeof (Int64Node)]);
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

    private void WriteClass(
      IndentedTextWriter writer,
      ClassNode @class,
      IEnumerable<ClassNode> classes,
      ILogger logger)
    {
      writer.Write("class ");
      writer.Write(@class.Name);
      bool flag = false;
      if (@class.Nodes.FirstOrDefault<BaseNode>() is ClassInstanceNode classInstanceNode)
      {
        flag = true;
        writer.Write(" : public ");
        writer.Write(classInstanceNode.InnerNode.Name);
      }
      if (!string.IsNullOrEmpty(@class.Comment))
      {
        writer.Write(" // ");
        writer.Write(@class.Comment);
      }
      writer.WriteLine();
      writer.WriteLine("{");
      writer.WriteLine("public:");
      ++writer.Indent;
      IEnumerable<BaseNode> nodes = @class.Nodes.Skip<BaseNode>(flag ? 1 : 0).WhereNot<BaseNode>((Func<BaseNode, bool>) (n => n is FunctionNode));
      this.WriteNodes(writer, nodes, logger);
      List<VirtualMethodTableNode> list1 = @class.Nodes.OfType<VirtualMethodTableNode>().ToList<VirtualMethodTableNode>();
      if (list1.Any<VirtualMethodTableNode>())
      {
        writer.WriteLine();
        foreach (VirtualMethodNode virtualMethodNode in list1.SelectMany<VirtualMethodTableNode, BaseNode>((Func<VirtualMethodTableNode, IEnumerable<BaseNode>>) (vt => (IEnumerable<BaseNode>) vt.Nodes)).OfType<VirtualMethodNode>())
        {
          writer.Write("virtual void ");
          writer.Write(virtualMethodNode.MethodName);
          writer.WriteLine("();");
        }
      }
      List<FunctionNode> list2 = classes.SelectMany<ClassNode, BaseNode>((Func<ClassNode, IEnumerable<BaseNode>>) (c2 => (IEnumerable<BaseNode>) c2.Nodes)).OfType<FunctionNode>().Where<FunctionNode>((Func<FunctionNode, bool>) (f => f.BelongsToClass == @class)).ToList<FunctionNode>();
      if (list2.Any<FunctionNode>())
      {
        writer.WriteLine();
        foreach (FunctionNode functionNode in list2)
        {
          writer.Write(functionNode.Signature);
          writer.WriteLine("{ }");
        }
      }
      --writer.Indent;
      writer.Write("}; //Size: 0x");
      writer.WriteLine(string.Format("{0:X04}", (object) @class.MemorySize));
      writer.WriteLine(string.Format("static_assert(sizeof({0}) == 0x{1:X});", (object) @class.Name, (object) @class.MemorySize));
    }

    private void WriteNodes(IndentedTextWriter writer, IEnumerable<BaseNode> nodes, ILogger logger)
    {
      int count = 0;
      int offset = 0;
      foreach (BaseNode node in nodes.WhereNot<BaseNode>((Func<BaseNode, bool>) (m => m is VirtualMethodTableNode)))
      {
        if (node is BaseHexNode)
        {
          if (count == 0)
            offset = node.Offset;
          count += node.MemorySize;
        }
        else
        {
          if (count != 0)
          {
            this.WriteNode(writer, CreatePaddingMember(offset, count), logger);
            count = 0;
          }
          this.WriteNode(writer, node, logger);
        }
      }
      if (count == 0)
        return;
      this.WriteNode(writer, CreatePaddingMember(offset, count), logger);

      BaseNode CreatePaddingMember(int offset, int count)
      {
        ArrayNode arrayNode = new ArrayNode();
        arrayNode.Offset = offset;
        arrayNode.Count = count;
        arrayNode.Name = string.Format("pad_{0:X04}", (object) offset);
        arrayNode.ChangeInnerNode((BaseNode) new CppCodeGenerator.Utf8CharacterNode());
        return (BaseNode) arrayNode;
      }
    }

    private void WriteNode(IndentedTextWriter writer, BaseNode node, ILogger logger)
    {
      CustomCppCodeGenerator generatorForNode = CppCodeGenerator.GetCustomCodeGeneratorForNode(node);
      if (generatorForNode != null && generatorForNode.WriteNode(writer, node, new WriteNodeFunc(this.WriteNode), logger))
        return;
      node = CppCodeGenerator.TransformNode(node);
      string typeDefinition = this.GetTypeDefinition(node, logger);
      if (typeDefinition != null)
      {
        writer.Write(typeDefinition);
        writer.Write(" ");
        writer.Write(node.Name);
        writer.Write("; //0x");
        writer.Write(string.Format("{0:X04}", (object) node.Offset));
        if (!string.IsNullOrEmpty(node.Comment))
        {
          writer.Write(" ");
          writer.Write(node.Comment);
        }
        writer.WriteLine();
      }
      else
      {
        switch (node)
        {
          case BaseWrapperNode _:
            writer.Write(this.ResolveWrappedType(node, false, logger));
            writer.Write("; //0x");
            writer.Write(string.Format("{0:X04}", (object) node.Offset));
            if (!string.IsNullOrEmpty(node.Comment))
            {
              writer.Write(" ");
              writer.Write(node.Comment);
            }
            writer.WriteLine();
            break;
          case UnionNode unionNode:
            writer.Write("union //0x");
            writer.Write(string.Format("{0:X04}", (object) node.Offset));
            if (!string.IsNullOrEmpty(node.Comment))
            {
              writer.Write(" ");
              writer.Write(node.Comment);
            }
            writer.WriteLine();
            writer.WriteLine("{");
            ++writer.Indent;
            this.WriteNodes(writer, (IEnumerable<BaseNode>) unionNode.Nodes, logger);
            --writer.Indent;
            writer.WriteLine("};");
            break;
          default:
            logger.Log(ReClassNET.Logger.LogLevel.Error, string.Format("Skipping node with unhandled type: {0}", (object) node.GetType()));
            break;
        }
      }
    }

    private static BaseNode TransformNode(BaseNode node)
    {
      CustomCppCodeGenerator generatorForNode = CppCodeGenerator.GetCustomCodeGeneratorForNode(node);
      if (generatorForNode != null)
        return generatorForNode.TransformNode(node);
      switch (node)
      {
        case BaseTextNode baseTextNode:
          ArrayNode arrayNode1 = new ArrayNode();
          arrayNode1.Count = baseTextNode.Length;
          arrayNode1.CopyFromNode(node);
          arrayNode1.ChangeInnerNode(GetCharacterNodeForEncoding(baseTextNode.Encoding));
          return (BaseNode) arrayNode1;
        case BaseTextPtrNode baseTextPtrNode:
          PointerNode pointerNode = new PointerNode();
          pointerNode.CopyFromNode(node);
          pointerNode.ChangeInnerNode(GetCharacterNodeForEncoding(baseTextPtrNode.Encoding));
          return (BaseNode) pointerNode;
        case BitFieldNode bitFieldNode:
          BaseNumericNode underlayingNode = bitFieldNode.GetUnderlayingNode();
          underlayingNode.CopyFromNode(node);
          return (BaseNode) underlayingNode;
        case BaseHexNode baseHexNode:
          ArrayNode arrayNode2 = new ArrayNode();
          arrayNode2.Count = baseHexNode.MemorySize;
          arrayNode2.CopyFromNode(node);
          arrayNode2.ChangeInnerNode((BaseNode) new CppCodeGenerator.Utf8CharacterNode());
          return (BaseNode) arrayNode2;
        default:
          return node;
      }

      BaseNode GetCharacterNodeForEncoding(Encoding encoding)
      {
        if (encoding.IsSameCodePage(Encoding.Unicode))
          return (BaseNode) new CppCodeGenerator.Utf16CharacterNode();
        return encoding.IsSameCodePage(Encoding.UTF32) ? (BaseNode) new CppCodeGenerator.Utf32CharacterNode() : (BaseNode) new CppCodeGenerator.Utf8CharacterNode();
      }
    }

    private string GetTypeDefinition(BaseNode node, ILogger logger)
    {
      CustomCppCodeGenerator generatorForNode = CppCodeGenerator.GetCustomCodeGeneratorForNode(node);
      if (generatorForNode != null)
        return generatorForNode.GetTypeDefinition(node, new GetTypeDefinitionFunc(this.GetTypeDefinition), new ResolveWrappedTypeFunc(this.ResolveWrappedType), logger);
      string str;
      if (this.nodeTypeToTypeDefinationMap.TryGetValue(node.GetType(), out str))
        return str;
      switch (node)
      {
        case ClassInstanceNode classInstanceNode:
          return "class " + classInstanceNode.InnerNode.Name;
        case EnumNode enumNode:
          return enumNode.Enum.Name;
        default:
          return (string) null;
      }
    }

    private string ResolveWrappedType(BaseNode node, bool isAnonymousExpression, ILogger logger)
    {
      StringBuilder sb = new StringBuilder();
      if (!isAnonymousExpression)
        sb.Append(node.Name);
      BaseNode baseNode = (BaseNode) null;
      BaseNode node1 = node;
      BaseNode node2;
      while (true)
      {
        node2 = CppCodeGenerator.TransformNode(node1);
        switch (node2)
        {
          case PointerNode pointerNode:
            sb.Prepend('*');
            if (pointerNode.InnerNode != null)
            {
              baseNode = (BaseNode) pointerNode;
              node1 = pointerNode.InnerNode;
              continue;
            }
            goto label_5;
          case ArrayNode arrayNode:
            if (baseNode is PointerNode)
            {
              sb.Prepend('(');
              sb.Append(')');
            }
            sb.Append(string.Format("[{0}]", (object) arrayNode.Count));
            baseNode = (BaseNode) arrayNode;
            node1 = arrayNode.InnerNode;
            continue;
          default:
            goto label_12;
        }
      }
label_5:
      if (!isAnonymousExpression)
        sb.Prepend(' ');
      sb.Prepend("void");
      goto label_15;
label_12:
      string typeDefinition = this.GetTypeDefinition(node2, logger);
      if (!isAnonymousExpression)
        sb.Prepend(' ');
      sb.Prepend(typeDefinition);
label_15:
      return sb.ToString().Trim();
    }

    private class Utf8CharacterNode : BaseNode
    {
      public override int MemorySize
      {
        get
        {
          throw new NotImplementedException();
        }
      }

      public override void GetUserInterfaceInfo(out string name, out Image icon)
      {
        throw new NotImplementedException();
      }

      public override Size Draw(DrawContext context, int x, int y)
      {
        throw new NotImplementedException();
      }

      public override int CalculateDrawnHeight(DrawContext context)
      {
        throw new NotImplementedException();
      }
    }

    private class Utf16CharacterNode : BaseNode
    {
      public override int MemorySize
      {
        get
        {
          throw new NotImplementedException();
        }
      }

      public override void GetUserInterfaceInfo(out string name, out Image icon)
      {
        throw new NotImplementedException();
      }

      public override Size Draw(DrawContext context, int x, int y)
      {
        throw new NotImplementedException();
      }

      public override int CalculateDrawnHeight(DrawContext context)
      {
        throw new NotImplementedException();
      }
    }

    private class Utf32CharacterNode : BaseNode
    {
      public override int MemorySize
      {
        get
        {
          throw new NotImplementedException();
        }
      }

      public override void GetUserInterfaceInfo(out string name, out Image icon)
      {
        throw new NotImplementedException();
      }

      public override Size Draw(DrawContext context, int x, int y)
      {
        throw new NotImplementedException();
      }

      public override int CalculateDrawnHeight(DrawContext context)
      {
        throw new NotImplementedException();
      }
    }
  }
}
