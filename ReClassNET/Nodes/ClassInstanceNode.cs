// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.ClassInstanceNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public class ClassInstanceNode : BaseClassWrapperNode
  {
    public override int MemorySize
    {
      get
      {
        return this.InnerNode.MemorySize;
      }
    }

    protected override bool PerformCycleCheck
    {
      get
      {
        return true;
      }
    }

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Class Instance";
      icon = (Image) Resources.B16x16_Button_Class_Instance;
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddOpenCloseIcon(context, x, y);
      x = this.AddIcon(context, x, y, context.IconProvider.Class, -1, HotSpotType.None);
      int x1 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Instance") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, -1, "<" + this.InnerNode.Name + ">") + context.Font.Width;
      x = this.AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeClassType) + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      y += context.Font.Height;
      Size size1 = new Size(x - num1, y - num2);
      if (this.LevelsOpen[context.Level])
      {
        DrawContext context1 = context.Clone();
        context1.Address = context.Address + this.Offset;
        context1.Memory = context.Memory.Clone();
        context1.Memory.Offset += this.Offset;
        Size size2 = this.InnerNode.Draw(context1, x1, y);
        size1.Width = Math.Max(size1.Width, size2.Width + x1 - num1);
        size1.Height += size2.Height;
      }
      return size1;
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.InnerNode.CalculateDrawnHeight(context);
      return height;
    }
  }
}
