// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseMatrixNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.UI;
using System;
using System.Drawing;

namespace ReClassNET.Nodes
{
  public abstract class BaseMatrixNode : BaseNode
  {
    public abstract int ValueTypeSize { get; }

    protected BaseMatrixNode()
    {
      this.LevelsOpen.DefaultValue = true;
    }

    protected Size DrawMatrixType(
      DrawContext context,
      int x,
      int y,
      string type,
      int rows,
      int columns)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Matrix, -1, HotSpotType.None);
      int num3 = x;
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name);
      x = this.AddOpenCloseIcon(context, x, y);
      x += context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      if (this.LevelsOpen[context.Level])
      {
        int hitId = 0;
        for (int index1 = 0; index1 < rows; ++index1)
        {
          y += context.Font.Height;
          int x1 = num3;
          int x2 = this.AddText(context, x1, y, context.Settings.NameColor, -1, "|");
          for (int index2 = 0; index2 < columns; ++index2)
          {
            float num4 = context.Memory.ReadFloat(this.Offset + hitId * 4);
            x2 = this.AddText(context, x2, y, context.Settings.ValueColor, hitId, string.Format("{0,14:0.000}", (object) num4));
            ++hitId;
          }
          x = Math.Max(this.AddText(context, x2, y, context.Settings.NameColor, -1, "|"), x);
        }
      }
      return new Size(x - num1, y - num2 + context.Font.Height);
    }

    protected Size DrawVectorType(
      DrawContext context,
      int x,
      int y,
      string type,
      int columns)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      int num1 = x;
      int num2 = y;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Vector, -1, HotSpotType.None);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name);
      x = this.AddOpenCloseIcon(context, x, y);
      if (this.LevelsOpen[context.Level])
      {
        x = this.AddText(context, x, y, context.Settings.NameColor, -1, "(");
        for (int hitId = 0; hitId < columns; ++hitId)
        {
          float num3 = context.Memory.ReadFloat(this.Offset + hitId * 4);
          x = this.AddText(context, x, y, context.Settings.ValueColor, hitId, string.Format("{0:0.000}", (object) num3));
          if (hitId < columns - 1)
            x = this.AddText(context, x, y, context.Settings.NameColor, -1, ",");
        }
        x = this.AddText(context, x, y, context.Settings.NameColor, -1, ")");
      }
      x += context.Font.Width;
      x = this.AddComment(context, x, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      return new Size(x - num1, y - num2 + context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      if (this.IsHidden && !this.IsWrapped)
        return BaseNode.HiddenHeight;
      int height = context.Font.Height;
      if (this.LevelsOpen[context.Level])
        height += this.CalculateValuesHeight(context);
      return height;
    }

    protected abstract int CalculateValuesHeight(DrawContext context);

    public void Update(HotSpot spot, int max)
    {
      this.Update(spot);
      float result;
      if (spot.Id < 0 || spot.Id >= max || !float.TryParse(spot.Text, out result))
        return;
      spot.Process.WriteRemoteMemory(spot.Address + spot.Id * this.ValueTypeSize, result);
    }

    protected delegate void DrawMatrixValues(int x, ref int maxX, ref int y);

    protected delegate void DrawVectorValues(ref int x, ref int y);
  }
}
