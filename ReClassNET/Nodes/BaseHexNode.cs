// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseHexNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace ReClassNET.Nodes
{
  public abstract class BaseHexNode : BaseNode
  {
    private static readonly Random highlightRandom = new Random();
    private static readonly Color[] highlightColors = new Color[8]
    {
      Color.Aqua,
      Color.Aquamarine,
      Color.Blue,
      Color.BlueViolet,
      Color.Chartreuse,
      Color.Crimson,
      Color.LawnGreen,
      Color.Magenta
    };
    private static readonly TimeSpan hightlightDuration = TimeSpan.FromSeconds(1.0);
    private static readonly Dictionary<IntPtr, ValueTypeWrapper<DateTime>> highlightTimer = new Dictionary<IntPtr, ValueTypeWrapper<DateTime>>();
    private readonly byte[] buffer;

    private static Color GetRandomHighlightColor()
    {
      return BaseHexNode.highlightColors[BaseHexNode.highlightRandom.Next(BaseHexNode.highlightColors.Length)];
    }

    protected BaseHexNode()
    {
      this.buffer = new byte[this.MemorySize];
    }

    protected Size Draw(DrawContext context, int x, int y, string text, int length)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIconPadding(context, x);
      x = this.AddAddressOffset(context, x, y);
      if (!string.IsNullOrEmpty(text))
        x = this.AddText(context, x, y, context.Settings.TextColor, -1, text);
      context.Memory.ReadBytes(this.Offset, this.buffer);
      Color color = context.Settings.HexColor;
      if (context.Settings.HighlightChangedValues)
      {
        IntPtr key = context.Address + this.Offset;
        BaseHexNode.highlightTimer.RemoveWhere<IntPtr, ValueTypeWrapper<DateTime>>((Func<KeyValuePair<IntPtr, ValueTypeWrapper<DateTime>>, bool>) (kv => kv.Value.Value < context.CurrentTime));
        ValueTypeWrapper<DateTime> valueTypeWrapper;
        if (BaseHexNode.highlightTimer.TryGetValue(key, out valueTypeWrapper))
        {
          if (valueTypeWrapper.Value >= context.CurrentTime)
          {
            color = BaseHexNode.GetRandomHighlightColor();
            if (context.Memory.HasChanged(this.Offset, this.MemorySize))
              valueTypeWrapper.Value = context.CurrentTime.Add(BaseHexNode.hightlightDuration);
          }
        }
        else if (context.Memory.HasChanged(this.Offset, this.MemorySize))
        {
          BaseHexNode.highlightTimer.Add(key, (ValueTypeWrapper<DateTime>) context.CurrentTime.Add(BaseHexNode.hightlightDuration));
          color = BaseHexNode.GetRandomHighlightColor();
        }
      }
      for (int hitId = 0; hitId < length; ++hitId)
        x = this.AddText(context, x, y, color, hitId, string.Format("{0:X02}", (object) this.buffer[hitId])) + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      return new Size(x - num, context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      return !this.IsHidden || this.IsWrapped ? context.Font.Height : BaseNode.HiddenHeight;
    }

    public void Update(HotSpot spot, int maxId)
    {
      this.Update(spot);
      byte result;
      if (spot.Id < 0 || spot.Id >= maxId || !byte.TryParse(spot.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
        return;
      spot.Process.WriteRemoteMemory(spot.Address + spot.Id, result);
    }

    public byte[] ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadBytes(this.Offset, this.MemorySize);
    }
  }
}
