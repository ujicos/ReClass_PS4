// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseWrapperArrayNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.UI;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public abstract class BaseWrapperArrayNode : BaseWrapperNode
  {
    public override int MemorySize
    {
      get
      {
        return this.InnerNode.MemorySize * this.Count;
      }
    }

    public int CurrentIndex { get; set; }

    public int Count { get; set; } = 1;

    public bool IsReadOnly { get; protected set; }

    protected override bool PerformCycleCheck
    {
      get
      {
        return true;
      }
    }

    public override bool CanChangeInnerNodeTo(BaseNode node)
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

    protected Size Draw(DrawContext context, int x, int y, string type)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddOpenCloseIcon(context, x, y);
      x = this.AddIcon(context, x, y, context.IconProvider.Array, -1, HotSpotType.None);
      int x1 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name);
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, "[");
      x = this.AddText(context, x, y, context.Settings.IndexColor, this.IsReadOnly ? -1 : 0, this.Count.ToString());
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, "]");
      x = this.AddIcon(context, x, y, context.IconProvider.LeftArrow, 2, HotSpotType.Click);
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, "(");
      x = this.AddText(context, x, y, context.Settings.IndexColor, 1, this.CurrentIndex.ToString());
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, ")");
      x = this.AddIcon(context, x, y, context.IconProvider.RightArrow, 3, HotSpotType.Click) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, -1, string.Format("<Size={0}>", (object) this.MemorySize)) + context.Font.Width;
      x = this.AddIcon(context, x + 2, y, context.IconProvider.Change, 4, HotSpotType.ChangeWrappedType);
      x += context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      y += context.Font.Height;
      Size size1 = new Size(x - num, context.Font.Height);
      if (this.LevelsOpen[context.Level])
      {
        Size size2 = this.DrawChild(context, x1, y);
        size1.Width = Math.Max(size1.Width, size2.Width + x1 - num);
        size1.Height += size2.Height;
      }
      return size1;
    }

    protected abstract Size DrawChild(DrawContext context, int x, int y);

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.InnerNode.CalculateDrawnHeight(context);
      return height;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      if (spot.Id == 0 || spot.Id == 1)
      {
        int result;
        if (!int.TryParse(spot.Text, out result))
          return;
        if (spot.Id == 0 && !this.IsReadOnly)
        {
          if (result == 0)
            return;
          this.Count = result;
          this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
        }
        else
        {
          if (result >= this.Count)
            return;
          this.CurrentIndex = result;
        }
      }
      else if (spot.Id == 2)
      {
        if (this.CurrentIndex <= 0)
          return;
        --this.CurrentIndex;
      }
      else
      {
        if (spot.Id != 3 || this.CurrentIndex >= this.Count - 1)
          return;
        ++this.CurrentIndex;
      }
    }
  }
}
