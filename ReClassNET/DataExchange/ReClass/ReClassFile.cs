// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.ReClassFile
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.DataExchange.ReClass.Legacy;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.ReClass
{
  public class ReClassFile : IReClassImport
  {
    private static readonly Type[] typeMap2013 = new Type[31]
    {
      null,
      typeof (ClassInstanceNode),
      null,
      null,
      typeof (Hex32Node),
      typeof (Hex16Node),
      typeof (Hex8Node),
      typeof (ClassPointerNode),
      typeof (Int32Node),
      typeof (Int16Node),
      typeof (Int8Node),
      typeof (FloatNode),
      typeof (UInt32Node),
      typeof (UInt16Node),
      typeof (UInt8Node),
      typeof (Utf8TextNode),
      typeof (FunctionPtrNode),
      typeof (CustomNode),
      typeof (Vector2Node),
      typeof (Vector3Node),
      typeof (Vector4Node),
      typeof (Matrix4x4Node),
      typeof (VirtualMethodTableNode),
      typeof (ClassInstanceArrayNode),
      null,
      null,
      null,
      typeof (Int64Node),
      typeof (DoubleNode),
      typeof (Utf16TextNode),
      typeof (ClassPointerArrayNode)
    };
    private static readonly Type[] typeMap2016 = new Type[34]
    {
      null,
      typeof (ClassInstanceNode),
      null,
      null,
      typeof (Hex32Node),
      typeof (Hex64Node),
      typeof (Hex16Node),
      typeof (Hex8Node),
      typeof (ClassPointerNode),
      typeof (Int64Node),
      typeof (Int32Node),
      typeof (Int16Node),
      typeof (Int8Node),
      typeof (FloatNode),
      typeof (DoubleNode),
      typeof (UInt32Node),
      typeof (UInt16Node),
      typeof (UInt8Node),
      typeof (Utf8TextNode),
      typeof (Utf16TextNode),
      typeof (FunctionPtrNode),
      typeof (CustomNode),
      typeof (Vector2Node),
      typeof (Vector3Node),
      typeof (Vector4Node),
      typeof (Matrix4x4Node),
      typeof (VirtualMethodTableNode),
      typeof (ClassInstanceArrayNode),
      null,
      typeof (Utf8TextPtrNode),
      typeof (Utf16TextPtrNode),
      typeof (BitFieldNode),
      typeof (UInt64Node),
      typeof (FunctionNode)
    };
    public const string FormatName = "ReClass File";
    public const string FileExtension = ".reclass";
    private readonly ReClassNetProject project;

    public ReClassFile(ReClassNetProject project)
    {
      this.project = project;
    }

    public void Load(string filePath, ILogger logger)
    {
      XDocument xdocument = XDocument.Load(filePath);
      if (xdocument.Root == null)
        return;
      Type[] typeMap = (Type[]) null;
      if (xdocument.Root.FirstNode is XComment firstNode)
      {
        string lower = firstNode.Value.Substring(0, 12).ToLower();
        if (!(lower == "reclass 2011") && !(lower == "reclass 2013"))
        {
          if (lower == "reclass 2015" || lower == "reclass 2016")
          {
            typeMap = ReClassFile.typeMap2016;
          }
          else
          {
            logger.Log(ReClassNET.Logger.LogLevel.Warning, "Unknown file version: " + firstNode.Value);
            logger.Log(ReClassNET.Logger.LogLevel.Warning, "Defaulting to ReClass 2016.");
            typeMap = ReClassFile.typeMap2016;
          }
        }
        else
          typeMap = ReClassFile.typeMap2013;
      }
      List<Tuple<XElement, ClassNode>> source = new List<Tuple<XElement, ClassNode>>();
      foreach (XElement xelement in xdocument.Root.Elements((XName) "Class").DistinctBy<XElement, string>((Func<XElement, string>) (e => e.Attribute((XName) "Name")?.Value)))
      {
        ClassNode classNode = new ClassNode(false);
        classNode.Name = xelement.Attribute((XName) "Name")?.Value ?? string.Empty;
        classNode.AddressFormula = ReClassFile.TransformAddressString(xelement.Attribute((XName) "strOffset")?.Value ?? string.Empty);
        ClassNode node = classNode;
        this.project.AddClass(node);
        source.Add(Tuple.Create<XElement, ClassNode>(xelement, node));
      }
      Dictionary<string, ClassNode> dictionary = source.ToDictionary<Tuple<XElement, ClassNode>, string, ClassNode>((Func<Tuple<XElement, ClassNode>, string>) (t => t.Item2.Name), (Func<Tuple<XElement, ClassNode>, ClassNode>) (t => t.Item2));
      foreach (Tuple<XElement, ClassNode> tuple in source)
      {
        XElement xelement1;
        ClassNode classNode;
        tuple.Deconstruct<XElement, ClassNode>(out xelement1, out classNode);
        XElement xelement2 = xelement1;
        ClassNode parent = classNode;
        this.ReadNodeElements(xelement2.Elements((XName) "Node"), parent, (IReadOnlyDictionary<string, ClassNode>) dictionary, typeMap, logger).ForEach<BaseNode>(new Action<BaseNode>(((BaseContainerNode) parent).AddNode));
      }
    }

    private static string TransformAddressString(string address)
    {
      string[] array = ((IEnumerable<string>) address.Split('+')).Select<string, string>((Func<string, string>) (s => s.Trim().ToLower().Replace("\"", string.Empty))).Where<string>((Func<string, bool>) (s => s != string.Empty)).ToArray<string>();
      for (int index = 0; index < array.Length; ++index)
      {
        string str = array[index];
        int num = str.Contains(".exe") ? 1 : (str.Contains(".dll") ? 1 : 0);
        bool flag = false;
        if (str.StartsWith("*"))
        {
          flag = true;
          str = str.Substring(1);
        }
        if (num != 0)
          str = "<" + str + ">";
        if (flag)
          str = "[" + str + "]";
        array[index] = str;
      }
      return string.Join(" + ", array);
    }

    private IEnumerable<BaseNode> ReadNodeElements(
      IEnumerable<XElement> elements,
      ClassNode parent,
      IReadOnlyDictionary<string, ClassNode> classes,
      Type[] typeMap,
      ILogger logger)
    {
      foreach (XElement element in elements)
      {
        Type nodeType = (Type) null;
        int result1;
        if (int.TryParse(element.Attribute((XName) "Type")?.Value, out result1) && result1 >= 0 && result1 < typeMap.Length)
          nodeType = typeMap[result1];
        if (nodeType == (Type) null)
        {
          logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown type: " + element.Attribute((XName) "Type")?.Value);
          logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
        }
        else
        {
          BaseNode baseNode1 = BaseNode.CreateInstanceFromType(nodeType, false);
          if (baseNode1 == null)
          {
            logger.Log(ReClassNET.Logger.LogLevel.Error, string.Format("Could not create node of type: {0}", (object) nodeType));
          }
          else
          {
            baseNode1.Name = element.Attribute((XName) "Name")?.Value ?? string.Empty;
            baseNode1.Comment = element.Attribute((XName) "Comment")?.Value ?? string.Empty;
            BaseNode baseNode2 = baseNode1;
            XAttribute xattribute1 = element.Attribute((XName) "bHidden");
            int num = xattribute1 != null ? (xattribute1.Value.Equals("1") ? 1 : 0) : 0;
            baseNode2.IsHidden = num != 0;
            if (baseNode1 is CustomNode customNode)
            {
              int result2;
              int.TryParse(element.Attribute((XName) "Size")?.Value, out result2);
              foreach (BaseNode equivalentNode in customNode.GetEquivalentNodes(result2))
                yield return equivalentNode;
            }
            else
            {
              if (baseNode1 is BaseWrapperNode baseWrapperNode)
              {
                int val = 0;
                string key;
                if (baseNode1 is BaseClassArrayNode)
                {
                  key = element.Element((XName) "Array")?.Attribute((XName) "Name")?.Value;
                  if (baseNode1 is ClassInstanceArrayNode)
                    ReClassFile.TryGetAttributeValue(element, "Total", out val, logger);
                  else
                    ReClassFile.TryGetAttributeValue(element, "Count", out val, logger);
                }
                else
                  key = element.Attribute((XName) "Pointer")?.Value ?? element.Attribute((XName) "Instance")?.Value;
                if (key == null || !classes.ContainsKey(key))
                {
                  logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown reference: " + key);
                  logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
                  continue;
                }
                ClassNode classNode = classes[key];
                if (baseWrapperNode.ShouldPerformCycleCheckForInnerNode() && !ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, classNode, (IEnumerable<ClassNode>) this.project.Classes))
                {
                  logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with cycle reference: " + parent.Name + "->" + baseNode1.Name);
                  continue;
                }
                switch (baseNode1)
                {
                  case BaseClassArrayNode baseClassArrayNode:
                    baseNode1 = baseClassArrayNode.GetEquivalentNode(val, classNode);
                    break;
                  case ClassPointerNode classPointerNode:
                    baseNode1 = classPointerNode.GetEquivalentNode(classNode);
                    break;
                  default:
                    baseWrapperNode.ChangeInnerNode((BaseNode) classNode);
                    break;
                }
              }
              if (!(baseNode1 is VirtualMethodTableNode virtualMethodTableNode))
              {
                if (!(baseNode1 is BaseTextNode baseTextNode))
                {
                  if (baseNode1 is BitFieldNode bitFieldNode)
                  {
                    int val;
                    ReClassFile.TryGetAttributeValue(element, "Size", out val, logger);
                    bitFieldNode.Bits = val * 8;
                  }
                }
                else
                {
                  int val;
                  ReClassFile.TryGetAttributeValue(element, "Size", out val, logger);
                  baseTextNode.Length = baseTextNode is Utf16TextNode ? val / 2 : val;
                }
              }
              else
                element.Elements((XName) "Function").Select<XElement, VirtualMethodNode>((Func<XElement, VirtualMethodNode>) (e =>
                {
                  VirtualMethodNode virtualMethodNode = new VirtualMethodNode();
                  virtualMethodNode.Name = e.Attribute((XName) "Name")?.Value ?? string.Empty;
                  virtualMethodNode.Comment = e.Attribute((XName) "Comment")?.Value ?? string.Empty;
                  XAttribute xattribute = e.Attribute((XName) "bHidden");
                  virtualMethodNode.IsHidden = xattribute != null && xattribute.Value.Equals("1");
                  return virtualMethodNode;
                })).ForEach<VirtualMethodNode>(new Action<VirtualMethodNode>(((BaseContainerNode) virtualMethodTableNode).AddNode));
              yield return baseNode1;
            }
          }
        }
      }
    }

    private static void TryGetAttributeValue(
      XElement element,
      string attribute,
      out int val,
      ILogger logger)
    {
      if (int.TryParse(element.Attribute((XName) attribute)?.Value, out val))
        return;
      val = 0;
      logger.Log(ReClassNET.Logger.LogLevel.Error, "Node is missing a valid '" + attribute + "' attribute, defaulting to 0.");
      logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
    }
  }
}
