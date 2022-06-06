// Decompiled with JetBrains decompiler
// Type: ReClassNET.Nodes.BaseTextNode
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;
using System.Drawing;
using System.Text;

namespace ReClassNET.Nodes
{
  public abstract class BaseTextNode : BaseNode
  {
    public int Length { get; set; }

    public override int MemorySize
    {
      get
      {
        return this.Length * this.CharacterSize;
      }
    }

    public abstract Encoding Encoding { get; }

    private int CharacterSize
    {
      get
      {
        return this.Encoding.GuessByteCountPerChar();
      }
    }

    public override void CopyFromNode(BaseNode node)
    {
      this.Length = node.MemorySize / this.CharacterSize;
    }

    protected Size DrawText(DrawContext context, int x, int y, string type)
    {
      if (this.IsHidden && !this.IsWrapped)
        return this.DrawHidden(context, x, y);
      int num1 = this.MemorySize / this.CharacterSize;
      string s = this.ReadValueFromMemory(context.Memory);
      int num2 = x;
      this.AddSelection(context, x, y, context.Font.Height);
      x = this.AddIconPadding(context, x);
      x = this.AddIcon(context, x, y, context.IconProvider.Text, -1, HotSpotType.None);
      x = this.AddAddressOffset(context, x, y);
      x = this.AddText(context, x, y, context.Settings.TypeColor, -1, type) + context.Font.Width;
      if (!this.IsWrapped)
        x = this.AddText(context, x, y, context.Settings.NameColor, 101, this.Name);
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, "[");
      x = this.AddText(context, x, y, context.Settings.IndexColor, 0, num1.ToString());
      x = this.AddText(context, x, y, context.Settings.IndexColor, -1, "]") + context.Font.Width;
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, "= '");
      x = this.AddText(context, x, y, context.Settings.TextColor, 1, s.LimitLength(150));
      x = this.AddText(context, x, y, context.Settings.TextColor, -1, "'") + context.Font.Width;
      x = this.AddComment(context, x, y);
      this.DrawInvalidMemoryIndicatorIcon(context, y);
      this.AddContextDropDownIcon(context, y);
      this.AddDeleteIcon(context, y);
      return new Size(x - num2, context.Font.Height);
    }

    public override int CalculateDrawnHeight(DrawContext context)
    {
      return !this.IsHidden || this.IsWrapped ? context.Font.Height : BaseNode.HiddenHeight;
    }

    public override void Update(HotSpot spot)
    {
      base.Update(spot);
      if (spot.Id == 0)
      {
        int result;
        if (!int.TryParse(spot.Text, out result) || result <= 0)
          return;
        this.Length = result;
        this.GetParentContainer()?.ChildHasChanged((BaseNode) this);
      }
      else
      {
        if (spot.Id != 1)
          return;
        byte[] bytes = this.Encoding.GetBytes(spot.Text);
        spot.Process.WriteRemoteMemory(spot.Address, bytes);
      }
    }

    public string ReadValueFromMemory(MemoryBuffer memory)
    {
      return memory.ReadString(this.Encoding, this.Offset, this.MemorySize);
    }
  }
}
