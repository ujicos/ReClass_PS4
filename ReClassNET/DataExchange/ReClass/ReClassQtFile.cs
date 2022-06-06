// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.ReClassQtFile
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
  public class ReClassQtFile : IReClassImport
  {
    private readonly Type[] typeMap = new Type[22]
    {
      null,
      null,
      typeof (ClassPointerNode),
      typeof (ClassInstanceNode),
      typeof (Hex64Node),
      typeof (Hex32Node),
      typeof (Hex16Node),
      typeof (Hex8Node),
      typeof (Int64Node),
      typeof (Int32Node),
      typeof (Int16Node),
      typeof (Int8Node),
      typeof (UInt32Node),
      null,
      null,
      typeof (UInt32Node),
      null,
      typeof (FloatNode),
      typeof (DoubleNode),
      typeof (Vector4Node),
      typeof (Vector3Node),
      typeof (Vector2Node)
    };
    public const string FormatName = "ReClassQt File";
    public const string FileExtension = ".reclassqt";
    private readonly ReClassNetProject project;

    public ReClassQtFile(ReClassNetProject project)
    {
      this.project = project;
    }

    public void Load(string filePath, ILogger logger)
    {
      XDocument xdocument = XDocument.Load(filePath);
      if (xdocument.Root == null)
        return;
      List<Tuple<XElement, ClassNode>> source = new List<Tuple<XElement, ClassNode>>();
      foreach (XElement element in xdocument.Root.Elements((XName) "Namespace").SelectMany<XElement, XElement>((Func<XElement, IEnumerable<XElement>>) (ns => ns.Elements((XName) "Class"))).DistinctBy<XElement, string>((Func<XElement, string>) (e => e.Attribute((XName) "ClassId")?.Value)))
      {
        ClassNode classNode = new ClassNode(false);
        classNode.Name = element.Attribute((XName) "Name")?.Value ?? string.Empty;
        classNode.AddressFormula = ReClassQtFile.ParseAddressString(element);
        ClassNode node = classNode;
        this.project.AddClass(node);
        source.Add(Tuple.Create<XElement, ClassNode>(element, node));
      }
      Dictionary<string, ClassNode> dictionary = source.ToDictionary<Tuple<XElement, ClassNode>, string, ClassNode>((Func<Tuple<XElement, ClassNode>, string>) (c => c.Item1.Attribute((XName) "ClassId")?.Value), (Func<Tuple<XElement, ClassNode>, ClassNode>) (c => c.Item2));
      foreach (Tuple<XElement, ClassNode> tuple in source)
      {
        XElement xelement1;
        ClassNode classNode;
        tuple.Deconstruct<XElement, ClassNode>(out xelement1, out classNode);
        XElement xelement2 = xelement1;
        ClassNode parent = classNode;
        this.ReadNodeElements(xelement2.Elements((XName) "Node"), parent, (IReadOnlyDictionary<string, ClassNode>) dictionary, logger).ForEach<BaseNode>(new Action<BaseNode>(((BaseContainerNode) parent).AddNode));
      }
    }

    private static string ParseAddressString(XElement element)
    {
      string str = element.Attribute((XName) "Address")?.Value;
      if (string.IsNullOrEmpty(str))
        return string.Empty;
      if (element.Attribute((XName) "DerefTwice")?.Value == "1")
        str = "[" + str + "]";
      return str;
    }

    private IEnumerable<BaseNode> ReadNodeElements(
      IEnumerable<XElement> elements,
      ClassNode parent,
      IReadOnlyDictionary<string, ClassNode> classes,
      ILogger logger)
    {
      foreach (XElement element in elements)
      {
        Type nodeType = (Type) null;
        int result;
        if (int.TryParse(element.Attribute((XName) "Type")?.Value, out result) && result >= 0 && result < this.typeMap.Length)
          nodeType = this.typeMap[result];
        if (nodeType == (Type) null)
        {
          logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown type: " + element.Attribute((XName) "Type")?.Value);
          logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
        }
        else
        {
          BaseNode baseNode = BaseNode.CreateInstanceFromType(nodeType, false);
          if (baseNode == null)
          {
            logger.Log(ReClassNET.Logger.LogLevel.Error, string.Format("Could not create node of type: {0}", (object) nodeType));
          }
          else
          {
            baseNode.Name = element.Attribute((XName) "Name")?.Value ?? string.Empty;
            baseNode.Comment = element.Attribute((XName) "Comments")?.Value ?? string.Empty;
            if (baseNode is BaseWrapperNode baseWrapperNode)
            {
              string key = element.Attribute((XName) "PointToClass")?.Value;
              if (key == null || !classes.ContainsKey(key))
              {
                logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown reference: " + key);
                logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
                continue;
              }
              ClassNode classNode = classes[key];
              if (baseWrapperNode.ShouldPerformCycleCheckForInnerNode() && !ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, classNode, (IEnumerable<ClassNode>) this.project.Classes))
              {
                logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with cycle reference: " + parent.Name + "->" + baseNode.Name);
                continue;
              }
              if (baseNode is ClassPointerNode classPointerNode)
                baseNode = classPointerNode.GetEquivalentNode(classNode);
              else
                baseWrapperNode.ChangeInnerNode((BaseNode) classNode);
            }
            yield return baseNode;
          }
        }
      }
    }
  }
}
