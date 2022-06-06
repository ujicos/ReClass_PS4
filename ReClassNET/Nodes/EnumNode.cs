// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.EnumNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Project;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ReClassNET.Nodes
{
  public class EnumNode : BaseNode
  {
    public override int MemorySize
    {
      get
      {
        return (int) this.Enum.Size;
      }
    }

    public EnumDescription Enum { get; private set; } = EnumDescription.Default;

    public override void GetUserInterfaceInfo(out string name, out Image icon)
    {
      name = "Enum";
      icon = (Image) Resources.B16x16_Button_Enum;
    }

    public void ChangeEnum(EnumDescription @enum)
    {
      this.Enum = @enum;
      this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
    }

    private string GetTextRepresentation(MemoryBuffer memory)
    {
      return !this.Enum.UseFlagsMode ? this.GetStringRepresentation(memory) : this.GetFlagsStringRepresentation(memory);
    }

    private long ReadSignedValueFromMemory(MemoryBuffer memory)
    {
      switch (this.Enum.Size)
      {
        case EnumDescription.UnderlyingTypeSize.OneByte:
          return (long) memory.ReadInt8(this.Offset);
        case EnumDescription.UnderlyingTypeSize.TwoBytes:
          return (long) memory.ReadInt16(this.Offset);
        case EnumDescription.UnderlyingTypeSize.FourBytes:
          return (long) memory.ReadInt32(this.Offset);
        case EnumDescription.UnderlyingTypeSize.EightBytes:
          return memory.ReadInt64(this.Offset);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private string GetStringRepresentation(MemoryBuffer memory)
    {
      long value = this.ReadSignedValueFromMemory(memory);
      int index = this.Enum.Values.FindIndex<KeyValuePair<string, long>>((Func<KeyValuePair<string, long>, bool>) (kv => kv.Value == value));
      return index == -1 ? value.ToString() : this.Enum.Values[index].Key;
    }

    private ulong ReadUnsignedValueFromMemory(MemoryBuffer memory)
    {
      switch (this.Enum.Size)
      {
        case EnumDescription.UnderlyingTypeSize.OneByte:
          return (ulong) memory.ReadUInt8(this.Offset);
        case EnumDescription.UnderlyingTypeSize.TwoBytes:
          return (ulong) memory.ReadUInt16(this.Offset);
        case EnumDescription.UnderlyingTypeSize.FourBytes:
          return (ulong) memory.ReadUInt32(this.Offset);
        case EnumDescription.UnderlyingTypeSize.EightBytes:
          return memory.ReadUInt64(this.Offset);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private string GetFlagsStringRepresentation(MemoryBuffer memory)
    {
      ulong num1 = this.ReadUnsignedValueFromMemory(memory);
      ulong num2 = num1;
      IReadOnlyList<KeyValuePair<string, long>> values = this.Enum.Values;
      int index = values.Count - 1;
      StringBuilder sb1 = new StringBuilder();
      bool flag = true;
      ulong num3 = num2;
      KeyValuePair<string, long> keyValuePair;
      for (; index >= 0; --index)
      {
        keyValuePair = values[index];
        ulong num4 = (ulong) keyValuePair.Value;
        if (index != 0 || num4 != 0UL)
        {
          if (((long) num2 & (long) num4) == (long) num4)
          {
            num2 -= num4;
            if (!flag)
              sb1.Prepend(" | ");
            StringBuilder sb2 = sb1;
            keyValuePair = values[index];
            string key = keyValuePair.Key;
            sb2.Prepend(key);
            flag = false;
          }
        }
        else
          break;
      }
      if (num2 != 0UL)
        return num1.ToString();
      if (num3 != 0UL)
        return sb1.ToString();
      if (values.Count > 0)
      {
        keyValuePair = values[0];
        if (keyValuePair.Value == 0L)
        {
          keyValuePair = values[0];
          return keyValuePair.Key;
        }
      }
      return "0";
    }

    public override Size Draw(DrawContext context, int x, int y)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Enum, -1, HotSpotType.None);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, "Enum") + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.ValueColor, -1, "<" + this.Enum.Name + ">") + context.Font.Width;
      x = this.AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeEnumType) + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, "=") + context.Font.Width;
      string textRepresentation = this.GetTextRepresentation(context.Memory);
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, textRepresentation) + context.Font.Width;
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
  }
}
