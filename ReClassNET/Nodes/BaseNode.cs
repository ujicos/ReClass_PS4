// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Properties;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ReClassNET.Nodes
{
  [System.Diagnostics.DebuggerDisplay("{DebuggerDisplay,nq}")]
  public abstract class BaseNode
  {
    internal static readonly List<INodeInfoReader> NodeInfoReader = new List<INodeInfoReader>();
    protected static readonly int HiddenHeight = 0;
    private static int nodeIndex = 0;
    private string name = string.Empty;
    private string comment = string.Empty;

    private string DebuggerDisplay
    {
      get
      {
        return string.Format("Type: {0} Name: {1} Offset: 0x{2:X}", (object) this.GetType().Name, (object) this.Name, (object) this.Offset);
      }
    }

    public int Offset { get; set; }

    public virtual string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        if (value == null || !(this.name != value))
          return;
        this.name = value;
        NodeEventHandler nameChanged = this.NameChanged;
        if (nameChanged == null)
          return;
        nameChanged(this);
      }
    }

    public string Comment
    {
      get
      {
        return this.comment;
      }
      set
      {
        if (value == null || !(this.comment != value))
          return;
        this.comment = value;
        NodeEventHandler commentChanged = this.CommentChanged;
        if (commentChanged == null)
          return;
        commentChanged(this);
      }
    }

    public BaseNode ParentNode { get; internal set; }

    public bool IsWrapped
    {
      get
      {
        return this.ParentNode is BaseWrapperNode;
      }
    }

    public bool IsHidden { get; set; }

    public bool IsSelected { get; set; }

    public abstract int MemorySize { get; }

    public event NodeEventHandler NameChanged;

    public event NodeEventHandler CommentChanged;

    protected GrowingList<bool> LevelsOpen { get; } = new GrowingList<bool>(false);

    private void ObjectInvariants()
    {
    }

    public static BaseNode CreateInstanceFromType(Type nodeType)
    {
      return BaseNode.CreateInstanceFromType(nodeType, true);
    }

    public static BaseNode CreateInstanceFromType(Type nodeType, bool callInitialize)
    {
      BaseNode instance = Activator.CreateInstance(nodeType) as BaseNode;
      if (callInitialize && instance != null)
        instance.Initialize();
      return instance;
    }

    protected BaseNode()
    {
      this.Name = string.Format("N{0:X08}", (object) BaseNode.nodeIndex++);
      this.Comment = string.Empty;
      this.LevelsOpen[0] = true;
    }

    public abstract void GetUserInterfaceInfo(out string name, out Image icon);

    public virtual bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
    {
      address = IntPtr.Zero;
      return false;
    }

    public virtual string GetToolTipText(HotSpot spot)
    {
      return (string) null;
    }

    public virtual void Initialize()
    {
    }

    public virtual void CopyFromNode(BaseNode node)
    {
      this.Name = node.Name;
      this.Comment = node.Comment;
      this.Offset = node.Offset;
    }

    public BaseContainerNode GetParentContainer()
    {
      for (BaseNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
      {
        if (parentNode is BaseContainerNode baseContainerNode)
          return baseContainerNode;
      }
      return this is BaseContainerNode baseContainerNode ? baseContainerNode : (BaseContainerNode) null;
    }

    public ClassNode GetParentClass()
    {
      for (BaseNode parentNode = this.ParentNode; parentNode != null; parentNode = parentNode.ParentNode)
      {
        if (parentNode is ClassNode classNode)
          return classNode;
      }
      return (ClassNode) null;
    }

    public BaseWrapperNode GetRootWrapperNode()
    {
      BaseWrapperNode baseWrapperNode1 = (BaseWrapperNode) null;
      for (BaseNode parentNode = this.ParentNode; parentNode is BaseWrapperNode baseWrapperNode2; parentNode = parentNode.ParentNode)
        baseWrapperNode1 = baseWrapperNode2;
      return baseWrapperNode1 == null && this is BaseWrapperNode baseWrapperNode2 ? baseWrapperNode2 : baseWrapperNode1;
    }

    public virtual void ClearSelection()
    {
      this.IsSelected = false;
    }

    public abstract Size Draw(DrawContext context, int x, int y);

    public abstract int CalculateDrawnHeight(DrawContext context);

    public virtual void Update(HotSpot spot)
    {
      if (spot.Id == 101)
      {
        this.Name = spot.Text;
      }
      else
      {
        if (spot.Id != 102)
          return;
        this.Comment = spot.Text;
      }
    }

    internal void ToggleLevelOpen(int level)
    {
      this.LevelsOpen[level] = !this.LevelsOpen[level];
    }

    internal void SetLevelOpen(int level, bool open)
    {
      this.LevelsOpen[level] = open;
    }

    protected void AddHotSpot(
      DrawContext context,
      Rectangle spot,
      string text,
      int id,
      HotSpotType type)
    {
      if (spot.Top > context.ClientArea.Bottom || spot.Bottom < 0)
        return;
      context.HotSpots.Add(new HotSpot()
      {
        Rect = spot,
        Text = text,
        Address = context.Address + this.Offset,
        Id = id,
        Type = type,
        Node = this,
        Level = context.Level,
        Process = context.Process,
        Memory = context.Memory
      });
    }

    protected int AddText(
      DrawContext context,
      int x,
      int y,
      Color color,
      int hitId,
      string text)
    {
      int width = Math.Max(text.Length, hitId != -1 ? 1 : 0) * context.Font.Width;
      if (y >= -context.Font.Height && y + context.Font.Height <= context.ClientArea.Bottom + context.Font.Height)
      {
        if (hitId != -1)
        {
          Rectangle spot = new Rectangle(x, y, width, context.Font.Height);
          this.AddHotSpot(context, spot, text, hitId, HotSpotType.Edit);
        }
        context.Graphics.DrawStringEx(text, context.Font.Font, color, x, y);
      }
      return x + width;
    }

    protected int AddAddressOffset(DrawContext context, int x, int y)
    {
      if (context.Settings.ShowNodeOffset)
        x = this.AddText(context, x, y, context.Settings.OffsetColor, -1, string.Format("{0:X04}", (object) this.Offset)) + context.Font.Width;
      if (context.Settings.ShowNodeAddress)
        x = this.AddText(context, x, y, context.Settings.AddressColor, 100, (context.Address + this.Offset).ToString("X016")) + context.Font.Width;
      return x;
    }

    protected void AddSelection(DrawContext context, int x, int y, int height)
    {
      if (y > context.ClientArea.Bottom || y + height < 0 || this.IsWrapped)
        return;
      if (this.IsSelected)
      {
        using (SolidBrush solidBrush = new SolidBrush(context.Settings.SelectedColor))
          context.Graphics.FillRectangle((Brush) solidBrush, 0, y, context.ClientArea.Right, height);
      }
      this.AddHotSpot(context, new Rectangle(0, y, context.ClientArea.Right - (this.IsSelected ? 16 : 0), height), string.Empty, -1, HotSpotType.Select);
    }

    protected int AddIconPadding(DrawContext view, int x)
    {
      return x + view.IconProvider.Dimensions;
    }

    protected int AddIcon(
      DrawContext context,
      int x,
      int y,
      Image icon,
      int id,
      HotSpotType type)
    {
      int dimensions = context.IconProvider.Dimensions;
      if (y > context.ClientArea.Bottom || y + dimensions < 0)
        return x + dimensions;
      context.Graphics.DrawImage(icon, x + 2, y, dimensions, dimensions);
      if (id != -1)
        this.AddHotSpot(context, new Rectangle(x, y, dimensions, dimensions), string.Empty, id, type);
      return x + dimensions;
    }

    protected int AddOpenCloseIcon(DrawContext context, int x, int y)
    {
      if (y > context.ClientArea.Bottom || y + context.IconProvider.Dimensions < 0)
        return x + context.IconProvider.Dimensions;
      Image icon = this.LevelsOpen[context.Level] ? context.IconProvider.OpenCloseOpen : context.IconProvider.OpenCloseClosed;
      return this.AddIcon(context, x, y, icon, 0, HotSpotType.OpenClose);
    }

    protected void AddContextDropDownIcon(DrawContext context, int y)
    {
      if (context.MultipleNodesSelected || (y > context.ClientArea.Bottom || y + context.IconProvider.Dimensions < 0 || (this.IsWrapped || !this.IsSelected)))
        return;
      this.AddIcon(context, 0, y, context.IconProvider.DropArrow, 0, HotSpotType.Context);
    }

    protected void AddDeleteIcon(DrawContext context, int y)
    {
      if (y > context.ClientArea.Bottom || y + context.IconProvider.Dimensions < 0 || (this.IsWrapped || !this.IsSelected))
        return;
      this.AddIcon(context, context.ClientArea.Right - context.IconProvider.Dimensions, y, context.IconProvider.Delete, 0, HotSpotType.Delete);
    }

    protected virtual int AddComment(DrawContext context, int x, int y)
    {
      x = this.AddText(context, x, y, context.Settings.CommentColor, -1, "//");
      x = this.AddText(context, x, y, context.Settings.CommentColor, 102, this.Comment) + context.Font.Width;
      return x;
    }

    protected Size DrawHidden(DrawContext context, int x, int y)
    {
      using (SolidBrush solidBrush = new SolidBrush(this.IsSelected ? context.Settings.SelectedColor : context.Settings.HiddenColor))
        context.Graphics.FillRectangle((Brush) solidBrush, 0, y, context.ClientArea.Right, 1);
      return new Size(0, BaseNode.HiddenHeight);
    }

    protected void DrawInvalidMemoryIndicatorIcon(DrawContext context, int y)
    {
      if (context.Memory.ContainsValidData)
        return;
      this.AddIcon(context, 0, y, (Image) Resources.B16x16_Error, -1, HotSpotType.None);
    }
  }
}
