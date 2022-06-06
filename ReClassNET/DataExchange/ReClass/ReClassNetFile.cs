// Decompiled with JetBrains decompiler
// Type: ReClassNET.DataExchange.ReClass.ReClassNetFile
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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.ReClass
{
  public class ReClassNetFile : IReClassImport, IReClassExport
  {
    private static readonly Dictionary<string, Type> buildInStringToTypeMap = ((IEnumerable<Type>) new Type[38]
    {
      typeof (BoolNode),
      typeof (BitFieldNode),
      typeof (EnumNode),
      typeof (ClassInstanceNode),
      typeof (DoubleNode),
      typeof (FloatNode),
      typeof (FunctionNode),
      typeof (FunctionPtrNode),
      typeof (Hex8Node),
      typeof (Hex16Node),
      typeof (Hex32Node),
      typeof (Hex64Node),
      typeof (Int8Node),
      typeof (Int16Node),
      typeof (Int32Node),
      typeof (Int64Node),
      typeof (NIntNode),
      typeof (Matrix3x3Node),
      typeof (Matrix3x4Node),
      typeof (Matrix4x4Node),
      typeof (UInt8Node),
      typeof (UInt16Node),
      typeof (UInt32Node),
      typeof (UInt64Node),
      typeof (NUIntNode),
      typeof (Utf8TextNode),
      typeof (Utf8TextPtrNode),
      typeof (Utf16TextNode),
      typeof (Utf16TextPtrNode),
      typeof (Utf32TextNode),
      typeof (Utf32TextPtrNode),
      typeof (Vector2Node),
      typeof (Vector3Node),
      typeof (Vector4Node),
      typeof (VirtualMethodTableNode),
      typeof (ArrayNode),
      typeof (PointerNode),
      typeof (UnionNode)
    }).ToDictionary<Type, string, Type>((Func<Type, string>) (t => t.Name), (Func<Type, Type>) (t => t));
    private static readonly Dictionary<Type, string> buildInTypeToStringMap = ReClassNetFile.buildInStringToTypeMap.ToDictionary<KeyValuePair<string, Type>, Type, string>((Func<KeyValuePair<string, Type>, Type>) (kv => kv.Value), (Func<KeyValuePair<string, Type>, string>) (kv => kv.Key));
    public const string FormatName = "ReClass.NET File";
    public const string FileExtension = ".rcnet";
    public const string FileExtensionId = "rcnetfile";
    private const uint FileVersion = 65537;
    private const uint FileVersionCriticalMask = 4294901760;
    private const string DataFileName = "Data.xml";
    private const string SerializationClassName = "__Serialization_Class__";
    public const string XmlRootElement = "reclass";
    public const string XmlCustomDataElement = "custom_data";
    public const string XmlTypeMappingElement = "type_mapping";
    public const string XmlEnumsElement = "enums";
    public const string XmlEnumElement = "enum";
    public const string XmlClassesElement = "classes";
    public const string XmlClassElement = "class";
    public const string XmlNodeElement = "node";
    public const string XmlMethodElement = "method";
    public const string XmlVersionAttribute = "version";
    public const string XmlPlatformAttribute = "type";
    public const string XmlUuidAttribute = "uuid";
    public const string XmlNameAttribute = "name";
    public const string XmlCommentAttribute = "comment";
    public const string XmlHiddenAttribute = "hidden";
    public const string XmlAddressAttribute = "address";
    public const string XmlTypeAttribute = "type";
    public const string XmlReferenceAttribute = "reference";
    public const string XmlCountAttribute = "count";
    public const string XmlBitsAttribute = "bits";
    public const string XmlLengthAttribute = "length";
    public const string XmlSizeAttribute = "size";
    public const string XmlSignatureAttribute = "signature";
    public const string XmlFlagsAttribute = "flags";
    public const string XmlItemElement = "item";
    public const string XmlValueAttribute = "value";
    private readonly ReClassNetProject project;

    public void Load(string filePath, ILogger logger)
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        this.Load((Stream) fileStream, logger);
    }

    public void Load(Stream input, ILogger logger)
    {
      // ISSUE: unable to decompile the method.
    }

    private BaseNode CreateNodeFromElement(
      XElement element,
      BaseNode parent,
      ILogger logger)
    {
      BaseNode parent1 = CreateNode();
      if (parent1 == null)
      {
        logger.Log(ReClassNET.Logger.LogLevel.Error, "Could not create node.");
        return (BaseNode) null;
      }
      parent1.ParentNode = parent;
      parent1.Name = element.Attribute((XName) "name")?.Value ?? string.Empty;
      parent1.Comment = element.Attribute((XName) "comment")?.Value ?? string.Empty;
      bool result;
      parent1.IsHidden = bool.TryParse(element.Attribute((XName) "hidden")?.Value, out result) & result;
      if (parent1 is BaseWrapperNode baseWrapperNode)
      {
        if (parent1 is ClassPointerNode || parent1 is ClassInstanceArrayNode || parent1 is ClassPointerArrayNode)
        {
          ClassNode elementReference = GetClassNodeFromElementReference();
          if (elementReference == null)
            return (BaseNode) null;
          BaseNode equivalentNode;
          switch (parent1)
          {
            case BaseClassArrayNode baseClassArrayNode:
              equivalentNode = baseClassArrayNode.GetEquivalentNode(0, elementReference);
              break;
            case ClassPointerNode classPointerNode:
              equivalentNode = classPointerNode.GetEquivalentNode(elementReference);
              break;
            default:
              throw new InvalidOperationException();
          }
          parent1 = equivalentNode;
        }
        else
        {
          BaseNode node = (BaseNode) null;
          if (parent1 is BaseClassWrapperNode)
          {
            node = (BaseNode) GetClassNodeFromElementReference();
            if (node == null)
              return (BaseNode) null;
          }
          else
          {
            XElement element1 = element.Elements().FirstOrDefault<XElement>();
            if (element1 != null)
              node = this.CreateNodeFromElement(element1, parent1, logger);
          }
          if (baseWrapperNode.CanChangeInnerNodeTo(node))
          {
            BaseWrapperNode rootWrapperNode = parent1.GetRootWrapperNode();
            if (rootWrapperNode.ShouldPerformCycleCheckForInnerNode() && node is ClassNode classToCheck && ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent1.GetParentClass(), classToCheck, (IEnumerable<ClassNode>) this.project.Classes))
            {
              logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with cyclic class reference: " + parent1.GetParentClass().Name + "->" + rootWrapperNode.Name);
              return (BaseNode) null;
            }
            baseWrapperNode.ChangeInnerNode(node);
          }
          else
            logger.Log(ReClassNET.Logger.LogLevel.Error, string.Format("The node {0} is not a valid child for {1}.", (object) node, (object) parent1));
        }
      }
      if (!(parent1 is VirtualMethodTableNode virtualMethodTableNode))
      {
        UnionNode unionNode = parent1 as UnionNode;
        if (unionNode == null)
        {
          switch (parent1)
          {
            case BaseWrapperArrayNode wrapperArrayNode:
              int valueOrDefault1 = ((int?) element.Attribute((XName) "count")).GetValueOrDefault();
              wrapperArrayNode.Count = valueOrDefault1;
              break;
            case BaseTextNode baseTextNode:
              int valueOrDefault2 = ((int?) element.Attribute((XName) "length")).GetValueOrDefault();
              baseTextNode.Length = valueOrDefault2;
              break;
            case BitFieldNode bitFieldNode:
              int valueOrDefault3 = ((int?) element.Attribute((XName) "bits")).GetValueOrDefault();
              bitFieldNode.Bits = valueOrDefault3;
              break;
            case FunctionNode functionNode:
              functionNode.Signature = element.Attribute((XName) "signature")?.Value ?? string.Empty;
              Guid uuid = ReClassNetFile.ParseUuid(element.Attribute((XName) "reference")?.Value);
              if (this.project.ContainsClass(uuid))
              {
                functionNode.BelongsToClass = this.project.GetClassByUuid(uuid);
                break;
              }
              break;
            case EnumNode enumNode:
              string enumName = element.Attribute((XName) "reference")?.Value ?? string.Empty;
              EnumDescription @enum = this.project.Enums.FirstOrDefault<EnumDescription>((Func<EnumDescription, bool>) (e => e.Name == enumName)) ?? EnumDescription.Default;
              enumNode.ChangeEnum(@enum);
              break;
          }
        }
        else
        {
          IEnumerable<BaseNode> nodes = element.Elements().Select<XElement, BaseNode>((Func<XElement, BaseNode>) (e => this.CreateNodeFromElement(e, (BaseNode) unionNode, logger)));
          unionNode.AddNodes(nodes);
        }
      }
      else
      {
        IEnumerable<VirtualMethodNode> virtualMethodNodes = element.Elements((XName) "method").Select<XElement, VirtualMethodNode>((Func<XElement, VirtualMethodNode>) (e =>
        {
          return new VirtualMethodNode()
          {
            Name = e.Attribute((XName) "name")?.Value ?? string.Empty,
            Comment = e.Attribute((XName) "comment")?.Value ?? string.Empty,
            IsHidden = ((bool?) e.Attribute((XName) "hidden")).GetValueOrDefault()
          };
        }));
        virtualMethodTableNode.AddNodes((IEnumerable<BaseNode>) virtualMethodNodes);
      }
      return parent1;

      BaseNode CreateNode()
      {
        ICustomNodeSerializer readConverter = CustomNodeSerializer.GetReadConverter(element);
        if (readConverter != null)
          return readConverter.CreateNodeFromElement(element, parent, (IEnumerable<ClassNode>) this.project.Classes, logger, new CreateNodeFromElementHandler(this.CreateNodeFromElement));
        Type nodeType;
        if (ReClassNetFile.buildInStringToTypeMap.TryGetValue(element.Attribute((XName) "type")?.Value ?? string.Empty, out nodeType))
          return BaseNode.CreateInstanceFromType(nodeType, false);
        logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown type: " + element.Attribute((XName) "type")?.Value);
        logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
        return (BaseNode) null;
      }

      ClassNode GetClassNodeFromElementReference()
      {
        Guid uuid = ReClassNetFile.ParseUuid(element.Attribute((XName) "reference")?.Value);
        if (this.project.ContainsClass(uuid))
          return this.project.GetClassByUuid(uuid);
        logger.Log(ReClassNET.Logger.LogLevel.Error, string.Format("Skipping node with unknown reference: {0}", (object) uuid));
        logger.Log(ReClassNET.Logger.LogLevel.Warning, element.ToString());
        return (ClassNode) null;
      }
    }

    private static Guid ParseUuid(string raw)
    {
      if (raw == null)
        throw new ArgumentNullException();
      return raw.Length != 24 ? Guid.Parse(raw) : new Guid(Convert.FromBase64String(raw));
    }

    public static Tuple<List<ClassNode>, List<BaseNode>> DeserializeNodesFromStream(
      Stream input,
      ReClassNetProject templateProject,
      ILogger logger)
    {
      using (ReClassNetProject project = new ReClassNetProject())
      {
        ReClassNetProject reClassNetProject = templateProject;
        if (reClassNetProject != null)
          reClassNetProject.Classes.ForEach<ClassNode>(new Action<ClassNode>(project.AddClass));
        new ReClassNetFile(project).Load(input, logger);
        IEnumerable<ClassNode> source1 = project.Classes.Where<ClassNode>((Func<ClassNode, bool>) (c => c.Name != "__Serialization_Class__"));
        if (templateProject != null)
          source1 = source1.Where<ClassNode>((Func<ClassNode, bool>) (c => !templateProject.ContainsClass(c.Uuid)));
        IEnumerable<BaseNode> source2 = project.Classes.Where<ClassNode>((Func<ClassNode, bool>) (c => c.Name == "__Serialization_Class__")).SelectMany<ClassNode, BaseNode>((Func<ClassNode, IEnumerable<BaseNode>>) (c => (IEnumerable<BaseNode>) c.Nodes));
        return Tuple.Create<List<ClassNode>, List<BaseNode>>(source1.ToList<ClassNode>(), source2.ToList<BaseNode>());
      }
    }

    public void Save(string filePath, ILogger logger)
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        this.Save((Stream) fileStream, logger);
    }

    public void Save(Stream output, ILogger logger)
    {
      using (ZipArchive zipArchive = new ZipArchive(output, ZipArchiveMode.Create))
      {
        using (Stream stream = zipArchive.CreateEntry("Data.xml").Open())
          new XDocument(new object[3]
          {
            (object) new XComment("ReClass.NET PS4 Port by MrReeko"),
            (object) new XComment("Website: https://github.com/MrReekoFTWxD"),
            (object) new XElement((XName) "reclass", new object[6]
            {
              (object) new XAttribute((XName) "version", (object) 65537U),
              (object) new XAttribute((XName) "type", (object) "x64"),
              (object) this.project.CustomData.Serialize("custom_data"),
              (object) this.project.TypeMapping.Serialize("type_mapping"),
              (object) new XElement((XName) "enums", (object) ReClassNetFile.CreateEnumElements((IEnumerable<EnumDescription>) this.project.Enums)),
              (object) new XElement((XName) "classes", (object) ReClassNetFile.CreateClassElements((IEnumerable<ClassNode>) this.project.Classes, logger))
            })
          }).Save(stream);
      }
    }

    private static IEnumerable<XElement> CreateEnumElements(
      IEnumerable<EnumDescription> enums)
    {
      return enums.Select<EnumDescription, XElement>((Func<EnumDescription, XElement>) (e => new XElement((XName) "enum", new object[4]
      {
        (object) new XAttribute((XName) "name", (object) e.Name),
        (object) new XAttribute((XName) "size", (object) e.Size),
        (object) new XAttribute((XName) "flags", (object) e.UseFlagsMode),
        (object) e.Values.Select<KeyValuePair<string, long>, XElement>((Func<KeyValuePair<string, long>, XElement>) (kv => new XElement((XName) "item", new object[2]
        {
          (object) new XAttribute((XName) "name", (object) kv.Key),
          (object) new XAttribute((XName) "value", (object) kv.Value)
        })))
      })));
    }

    private static IEnumerable<XElement> CreateClassElements(
      IEnumerable<ClassNode> classes,
      ILogger logger)
    {
      return classes.Select<ClassNode, XElement>((Func<ClassNode, XElement>) (c => new XElement((XName) "class", new object[5]
      {
        (object) new XAttribute((XName) "uuid", (object) c.Uuid),
        (object) new XAttribute((XName) "name", (object) (c.Name ?? string.Empty)),
        (object) new XAttribute((XName) "comment", (object) (c.Comment ?? string.Empty)),
        (object) new XAttribute((XName) "address", (object) (c.AddressFormula ?? string.Empty)),
        (object) c.Nodes.Select<BaseNode, XElement>((Func<BaseNode, XElement>) (n => ReClassNetFile.CreateElementFromNode(n, logger))).Where<XElement>((Func<XElement, bool>) (e => e != null))
      })));
    }

    private static XElement CreateElementFromNode(BaseNode node, ILogger logger)
    {
      XElement element = CreateElement();
      if (element == null)
      {
        logger.Log(ReClassNET.Logger.LogLevel.Error, "Could not create element.");
        return (XElement) null;
      }
      element.SetAttributeValue((XName) "name", (object) (node.Name ?? string.Empty));
      element.SetAttributeValue((XName) "comment", (object) (node.Comment ?? string.Empty));
      element.SetAttributeValue((XName) "hidden", (object) node.IsHidden);
      if (node is BaseWrapperNode baseWrapperNode)
      {
        if (node is BaseClassWrapperNode classWrapperNode)
          element.SetAttributeValue((XName) "reference", (object) ((ClassNode) classWrapperNode.InnerNode).Uuid);
        else if (baseWrapperNode.InnerNode != null)
          element.Add((object) ReClassNetFile.CreateElementFromNode(baseWrapperNode.InnerNode, logger));
      }
      if (!(node is VirtualMethodTableNode virtualMethodTableNode))
      {
        if (!(node is UnionNode unionNode))
        {
          if (!(node is BaseWrapperArrayNode wrapperArrayNode))
          {
            if (!(node is BaseTextNode baseTextNode))
            {
              if (!(node is BitFieldNode bitFieldNode))
              {
                if (!(node is FunctionNode functionNode))
                {
                  if (node is EnumNode enumNode)
                    element.SetAttributeValue((XName) "reference", (object) enumNode.Enum.Name);
                }
                else
                {
                  ClassNode belongsToClass = functionNode.BelongsToClass;
                  Guid guid = belongsToClass != null ? belongsToClass.Uuid : Guid.Empty;
                  element.SetAttributeValue((XName) "reference", (object) guid);
                  element.SetAttributeValue((XName) "signature", (object) functionNode.Signature);
                }
              }
              else
                element.SetAttributeValue((XName) "bits", (object) bitFieldNode.Bits);
            }
            else
              element.SetAttributeValue((XName) "length", (object) baseTextNode.Length);
          }
          else
            element.SetAttributeValue((XName) "count", (object) wrapperArrayNode.Count);
        }
        else
          element.Add((object) unionNode.Nodes.Select<BaseNode, XElement>((Func<BaseNode, XElement>) (n => ReClassNetFile.CreateElementFromNode(n, logger))));
      }
      else
        element.Add((object) virtualMethodTableNode.Nodes.Select<BaseNode, XElement>((Func<BaseNode, XElement>) (n => new XElement((XName) "method", new object[3]
        {
          (object) new XAttribute((XName) "name", (object) (n.Name ?? string.Empty)),
          (object) new XAttribute((XName) "comment", (object) (n.Comment ?? string.Empty)),
          (object) new XAttribute((XName) "hidden", (object) n.IsHidden)
        }))));
      return element;

      XElement CreateElement()
      {
        ICustomNodeSerializer writeConverter = CustomNodeSerializer.GetWriteConverter(node);
        if (writeConverter != null)
          return writeConverter.CreateElementFromNode(node, logger, new CreateElementFromNodeHandler(ReClassNetFile.CreateElementFromNode));
        string str;
        if (ReClassNetFile.buildInTypeToStringMap.TryGetValue(node.GetType(), out str))
          return new XElement((XName) "node", (object) new XAttribute((XName) "type", (object) str));
        logger.Log(ReClassNET.Logger.LogLevel.Error, "Skipping node with unknown type: " + node.Name);
        logger.Log(ReClassNET.Logger.LogLevel.Warning, node.GetType().ToString());
        return (XElement) null;
      }
    }

    public static void SerializeNodesToStream(
      Stream output,
      IEnumerable<BaseNode> nodes,
      ILogger logger)
    {
      using (ReClassNetProject project = new ReClassNetProject())
      {
        ClassNode classNode1 = new ClassNode(false);
        classNode1.Name = "__Serialization_Class__";
        ClassNode node1 = classNode1;
        bool flag = true;
        foreach (BaseNode node2 in nodes)
        {
          RecursiveAddClasses(node2);
          if (!(node2 is ClassNode))
          {
            if (flag)
            {
              flag = false;
              project.AddClass(node1);
            }
            node1.AddNode(node2);
          }
        }
        new ReClassNetFile(project).Save(output, logger);

        void RecursiveAddClasses(BaseNode node)
        {
          ClassNode node1 = (ClassNode) null;
          BaseNode baseNode = node;
          if (!(baseNode is ClassNode classNode))
          {
            if (baseNode is BaseWrapperNode baseWrapperNode && baseWrapperNode.ResolveMostInnerNode() is ClassNode classNode)
              node1 = classNode;
          }
          else
            node1 = classNode;
          if (node1 == null || project.ContainsClass(node1.Uuid))
            return;
          project.AddClass(node1);
          foreach (BaseNode node2 in node1.Nodes.OfType<BaseWrapperNode>())
            RecursiveAddClasses(node2);
        }
      }
    }

    public ReClassNetFile(ReClassNetProject project)
    {
      this.project = project;
    }

    static ReClassNetFile()
    {
      ReClassNetFile.buildInStringToTypeMap["UTF8TextNode"] = typeof (Utf8TextNode);
      ReClassNetFile.buildInStringToTypeMap["UTF8TextPtrNode"] = typeof (Utf8TextPtrNode);
      ReClassNetFile.buildInStringToTypeMap["UTF16TextNode"] = typeof (Utf16TextNode);
      ReClassNetFile.buildInStringToTypeMap["UTF16TextPtrNode"] = typeof (Utf16TextPtrNode);
      ReClassNetFile.buildInStringToTypeMap["UTF32TextNode"] = typeof (Utf32TextNode);
      ReClassNetFile.buildInStringToTypeMap["UTF32TextPtrNode"] = typeof (Utf32TextPtrNode);
      ReClassNetFile.buildInStringToTypeMap["VTableNode"] = typeof (VirtualMethodTableNode);
      ReClassNetFile.buildInStringToTypeMap["ClassInstanceArrayNode"] = typeof (ClassInstanceArrayNode);
      ReClassNetFile.buildInStringToTypeMap["ClassPtrArrayNode"] = typeof (ClassPointerArrayNode);
      ReClassNetFile.buildInStringToTypeMap["ClassPtrNode"] = typeof (ClassPointerNode);
    }
  }
}
