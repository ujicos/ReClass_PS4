// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.ClassNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ReClassNET.Nodes
{
  public class ClassNode : BaseContainerNode
  {
    public static event ClassCreatedEventHandler ClassCreated;

    public static IntPtr DefaultAddress { get; } = (IntPtr) 5368709120L;

    public static string DefaultAddressFormula { get; } = "140000000";

    public override int MemorySize
    {
      get
      {
        return this.Nodes.Sum<BaseNode>((Func<BaseNode, int>) (n => n.MemorySize));
      }
    }

    protected override bool ShouldCompensateSizeChanges
    {
      get
      {
        return true;
      }
    }

    public Guid Uuid { get; set; }

    public string AddressFormula { get; set; } = ClassNode.DefaultAddressFormula;

    public event NodeEventHandler NodesChanged;

    internal ClassNode(bool notifyClassCreated)
    {
      this.LevelsOpen.DefaultValue = true;
      this.Uuid = Guid.NewGuid();
      if (!notifyClassCreated)
        return;
      ClassCreatedEventHandler classCreated = ClassNode.ClassCreated;
      if (classCreated == null)
        return;
      classCreated(this);
    }

    public static ClassNode Create()
    {
      return new ClassNode(true);
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      throw new InvalidOperationException("The 'ClassNode' node should not be accessible from the ui.");
    }

    public override bool CanHandleChildNode(BaseNode node)
    {
      switch (node)
      {
        case null:
        case ClassNode _:
        case VirtualMethodNode _:
          return false;
        default:
          return true;
      }
    }

    public override void Initialize()
    {
      this.AddBytes(IntPtr.Size);
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      this.AddSelection(context, 0, y, context.Font.Height);
      int num1 = x;
      int num2 = y;
      x = this.AddOpenCloseIcon(context, x, y);
      int x1 = x;
      x = this.AddIcon(context, x, y, context.IconProvider.Class, -1, HotSpotType.None);
      x = this.AddText(context, x, y, context.Settings.OffsetColor, 0, this.AddressFormula) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Class") + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, -1, string.Format("[{0}]", (object) this.MemorySize)) + context.Font.Width;
      x = this.AddComment(context, x, y);
      y += context.Font.Height;
      Size baseSize1 = new Size(x - num1, y - num2);
      if (this.LevelsOpen[context.Level])
      {
        int width = x1 - num1;
        DrawContext context1 = context.Clone();
        ++context1.Level;
        foreach (BaseNode node in (IEnumerable<BaseNode>) this.Nodes)
        {
          if (context.ClientArea.Contains(x1, y))
          {
            Size baseSize2 = node.Draw(context1, x1, y);
            baseSize1 = AggregateNodeSizes(baseSize1, ExtendWidth(baseSize2, width));
            y += baseSize2.Height;
          }
          else
          {
            int drawnHeight = node.CalculateDrawnHeight(context1);
            if (new Rectangle(x1, y, 9999999, drawnHeight).IntersectsWith(context.ClientArea))
            {
              Size baseSize2 = node.Draw(context1, x1, y);
              baseSize1 = AggregateNodeSizes(baseSize1, ExtendWidth(baseSize2, width));
              y += baseSize2.Height;
            }
            else
            {
              baseSize1 = AggregateNodeSizes(baseSize1, new Size(0, drawnHeight));
              y += drawnHeight;
            }
          }
        }
      }
      return baseSize1;

      Size AggregateNodeSizes(Size baseSize, Size newSize)
      {
        return new Size(Math.Max(baseSize.Width, newSize.Width), baseSize.Height + newSize.Height);
      }

      Size ExtendWidth(Size baseSize, int width)
      {
        return new Size(baseSize.Width + width, baseSize.Height);
      }
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
      {
        DrawContext nv = context.Clone();
        ++nv.Level;
        height += this.Nodes.Sum<BaseNode>((Func<BaseNode, int>) (n => n.CalculateDrawnHeight(nv)));
      }
      return height;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      if (spot.Id != 0)
        return;
      this.AddressFormula = spot.Text;
    }

    protected internal override void ChildHasChanged(BaseNode child)
    {
      NodeEventHandler nodesChanged = this.NodesChanged;
      if (nodesChanged == null)
        return;
      nodesChanged((BaseNode) this);
    }
  }
}
