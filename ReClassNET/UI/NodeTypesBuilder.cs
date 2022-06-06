// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.NodeTypesBuilder
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.UI
{
  internal static class NodeTypesBuilder
  {
    private static readonly List<Type[]> defaultNodeTypeGroupList = new List<Type[]>();
    private static readonly Dictionary<Plugin, IReadOnlyList<Type>> pluginNodeTypes = new Dictionary<Plugin, IReadOnlyList<Type>>();

    static NodeTypesBuilder()
    {
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[4]
      {
        typeof (Hex64Node),
        typeof (Hex32Node),
        typeof (Hex16Node),
        typeof (Hex8Node)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[5]
      {
        typeof (NIntNode),
        typeof (Int64Node),
        typeof (Int32Node),
        typeof (Int16Node),
        typeof (Int8Node)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[5]
      {
        typeof (NUIntNode),
        typeof (UInt64Node),
        typeof (UInt32Node),
        typeof (UInt16Node),
        typeof (UInt8Node)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[3]
      {
        typeof (BoolNode),
        typeof (BitFieldNode),
        typeof (EnumNode)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[2]
      {
        typeof (FloatNode),
        typeof (DoubleNode)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[6]
      {
        typeof (Vector4Node),
        typeof (Vector3Node),
        typeof (Vector2Node),
        typeof (Matrix4x4Node),
        typeof (Matrix3x4Node),
        typeof (Matrix3x3Node)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[4]
      {
        typeof (Utf8TextNode),
        typeof (Utf8TextPtrNode),
        typeof (Utf16TextNode),
        typeof (Utf16TextPtrNode)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[3]
      {
        typeof (PointerNode),
        typeof (ArrayNode),
        typeof (UnionNode)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[1]
      {
        typeof (ClassInstanceNode)
      });
      NodeTypesBuilder.defaultNodeTypeGroupList.Add(new Type[3]
      {
        typeof (VirtualMethodTableNode),
        typeof (FunctionNode),
        typeof (FunctionPtrNode)
      });
    }

    public static void AddPluginNodeGroup(Plugin plugin, IReadOnlyList<Type> nodeTypes)
    {
      if (NodeTypesBuilder.pluginNodeTypes.ContainsKey(plugin))
        throw new InvalidOperationException();
      NodeTypesBuilder.pluginNodeTypes.Add(plugin, nodeTypes);
    }

    public static void RemovePluginNodeGroup(Plugin plugin)
    {
      NodeTypesBuilder.pluginNodeTypes.Remove(plugin);
    }

    public static IEnumerable<ToolStripItem> CreateToolStripButtons(
      Action<Type> handler)
    {
      EventHandler clickHandler = (EventHandler) ((sender, e) =>
      {
        Action<Type> action = handler;
        Type type = sender is TypeToolStripButton typeToolStripButton ? typeToolStripButton.Value : (Type) null;
        if ((object) type == null)
          type = ((TypeToolStripMenuItem) sender).Value;
        action(type);
      });
      return NodeTypesBuilder.CreateToolStripItems((Func<Type, ToolStripItem>) (t =>
      {
        string label;
        Image icon;
        NodeTypesBuilder.GetNodeInfoFromType(t, out label, out icon);
        TypeToolStripButton typeToolStripButton = new TypeToolStripButton();
        typeToolStripButton.Value = t;
        typeToolStripButton.ToolTipText = label;
        typeToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
        typeToolStripButton.Image = icon;
        typeToolStripButton.Click += clickHandler;
        return (ToolStripItem) typeToolStripButton;
      }), (Func<Plugin, ToolStripDropDownItem>) (p =>
      {
        return (ToolStripDropDownItem) new ToolStripDropDownButton()
        {
          ToolTipText = "",
          Image = p.Icon
        };
      }), (Func<Type, ToolStripItem>) (t =>
      {
        string label;
        Image icon;
        NodeTypesBuilder.GetNodeInfoFromType(t, out label, out icon);
        TypeToolStripMenuItem toolStripMenuItem = new TypeToolStripMenuItem();
        toolStripMenuItem.Value = t;
        toolStripMenuItem.Text = label;
        toolStripMenuItem.Image = icon;
        toolStripMenuItem.Click += clickHandler;
        return (ToolStripItem) toolStripMenuItem;
      }));
    }

    public static IEnumerable<ToolStripItem> CreateToolStripMenuItems(
      Action<Type> handler,
      bool addNoneType)
    {
      EventHandler clickHandler = (EventHandler) ((sender, e) => handler(((TypeToolStripMenuItem) sender).Value));
      IEnumerable<ToolStripItem> source = NodeTypesBuilder.CreateToolStripItems((Func<Type, ToolStripItem>) (t =>
      {
        string label;
        Image icon;
        NodeTypesBuilder.GetNodeInfoFromType(t, out label, out icon);
        TypeToolStripMenuItem toolStripMenuItem = new TypeToolStripMenuItem();
        toolStripMenuItem.Value = t;
        toolStripMenuItem.Text = label;
        toolStripMenuItem.Image = icon;
        toolStripMenuItem.ForeColor = Color.White;
        toolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
        toolStripMenuItem.Click += clickHandler;
        return (ToolStripItem) toolStripMenuItem;
      }), (Func<Plugin, ToolStripDropDownItem>) (p =>
      {
        return (ToolStripDropDownItem) new ToolStripMenuItem()
        {
          Text = p.GetType().ToString(),
          Image = p.Icon,
          ForeColor = Color.White,
          BackColor = Color.FromArgb(60, 63, 65)
        };
      }));
      if (addNoneType)
      {
        TypeToolStripMenuItem toolStripMenuItem = new TypeToolStripMenuItem();
        toolStripMenuItem.Value = (Type) null;
        toolStripMenuItem.Text = "None";
        toolStripMenuItem.ForeColor = Color.White;
        toolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
        ToolStripItem element = (ToolStripItem) toolStripMenuItem;
        source = source.Prepend<ToolStripItem>((ToolStripItem) new ToolStripSeparator()).Prepend<ToolStripItem>(element);
      }
      return source;
    }

    private static IEnumerable<ToolStripItem> CreateToolStripItems(
      Func<Type, ToolStripItem> createItem,
      Func<Plugin, ToolStripDropDownItem> createPluginContainerItem)
    {
      return NodeTypesBuilder.CreateToolStripItems(createItem, createPluginContainerItem, createItem);
    }

    private static IEnumerable<ToolStripItem> CreateToolStripItems(
      Func<Type, ToolStripItem> createItem,
      Func<Plugin, ToolStripDropDownItem> createPluginContainerItem,
      Func<Type, ToolStripItem> createPluginItem)
    {
      if (!NodeTypesBuilder.defaultNodeTypeGroupList.Any<Type[]>())
        return Enumerable.Empty<ToolStripItem>();
      IEnumerable<ToolStripItem> source = NodeTypesBuilder.defaultNodeTypeGroupList.Select<Type[], IEnumerable<ToolStripItem>>((Func<Type[], IEnumerable<ToolStripItem>>) (t => ((IEnumerable<Type>) t).Select<Type, ToolStripItem>(createItem))).Aggregate<IEnumerable<ToolStripItem>>((Func<IEnumerable<ToolStripItem>, IEnumerable<ToolStripItem>, IEnumerable<ToolStripItem>>) ((l1, l2) => l1.Append<ToolStripItem>((ToolStripItem) new ToolStripSeparator()).Concat<ToolStripItem>(l2)));
      if (NodeTypesBuilder.pluginNodeTypes.Any<KeyValuePair<Plugin, IReadOnlyList<Type>>>())
      {
        foreach (KeyValuePair<Plugin, IReadOnlyList<Type>> pluginNodeType in NodeTypesBuilder.pluginNodeTypes)
        {
          ToolStripDropDownItem stripDropDownItem = createPluginContainerItem(pluginNodeType.Key);
          stripDropDownItem.Tag = (object) pluginNodeType.Key;
          stripDropDownItem.DropDownItems.AddRange(pluginNodeType.Value.Select<Type, ToolStripItem>(createPluginItem).ToArray<ToolStripItem>());
          source = source.Append<ToolStripItem>((ToolStripItem) new ToolStripSeparator()).Append<ToolStripItem>((ToolStripItem) stripDropDownItem);
        }
      }
      return source;
    }

    private static void GetNodeInfoFromType(Type nodeType, out string label, out Image icon)
    {
      BaseNode instanceFromType = BaseNode.CreateInstanceFromType(nodeType, false);
      if (instanceFromType == null)
        throw new InvalidOperationException(string.Format("'{0}' is not a valid node type.", (object) nodeType));
      instanceFromType.GetUserInterfaceInfo(out label, out icon);
    }
  }
}
