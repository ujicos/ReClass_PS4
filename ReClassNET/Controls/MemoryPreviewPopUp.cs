// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.MemoryPreviewPopUp
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  [ToolboxItem(false)]
  public class MemoryPreviewPopUp : ToolStripDropDown
  {
    private const int ToolTipWidth = 1004;
    private const int ToolTipPadding = 4;
    private readonly MemoryPreviewPopUp.MemoryPreviewPanel panel;
    private IntPtr memoryAddress;

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ExStyle |= 134217728;
        return createParams;
      }
    }

    public MemoryPreviewPopUp(FontEx font)
    {
      this.AutoSize = false;
      this.AutoClose = false;
      this.DoubleBuffered = true;
      this.ResizeRedraw = true;
      this.TabStop = false;
      MemoryPreviewPopUp.MemoryPreviewPanel memoryPreviewPanel = new MemoryPreviewPopUp.MemoryPreviewPanel(font);
      memoryPreviewPanel.Location = Point.Empty;
      this.panel = memoryPreviewPanel;
      ToolStripControlHost stripControlHost = new ToolStripControlHost((Control) this.panel);
      this.Padding = this.Margin = stripControlHost.Padding = stripControlHost.Margin = Padding.Empty;
      this.MinimumSize = this.panel.MinimumSize;
      this.panel.MinimumSize = this.panel.Size;
      this.MaximumSize = this.panel.MaximumSize;
      this.panel.MaximumSize = this.panel.Size;
      this.Size = this.panel.Size;
      this.panel.SizeChanged += (EventHandler) ((s, e) => this.Size = this.MinimumSize = this.MaximumSize = this.panel.Size);
      this.Items.Add((ToolStripItem) stripControlHost);
    }

    protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
    {
      this.panel.Reset();
      base.OnClosed(e);
    }

    internal void HandleMouseWheelEvent(MouseEventArgs e)
    {
      if (e.Delta == 0)
        return;
      this.panel.ChangeNodeCount(e.Delta < 0 ? 1 : -1);
      this.UpdateMemory();
      this.Invalidate();
      if (!(e is HandledMouseEventArgs handledMouseEventArgs))
        return;
      handledMouseEventArgs.Handled = true;
    }

    public void InitializeMemory(RemoteProcess process, IntPtr address)
    {
      this.memoryAddress = address;
      this.panel.DrawContext.Process = process;
      this.panel.DrawContext.Memory.UpdateFrom((IRemoteMemoryReader) process, address, MainForm.PS4PID);
    }

    public void UpdateMemory()
    {
      Thread.Sleep(120);
      this.panel.DrawContext.Memory.UpdateFrom((IRemoteMemoryReader) this.panel.DrawContext.Process, this.memoryAddress, MainForm.PS4PID);
      this.panel.Invalidate();
    }

    private class MemoryPreviewPanel : Panel
    {
      private const int MinNodeCount = 10;
      private readonly List<BaseHexNode> nodes;

      public DrawContext DrawContext { get; }

      public MemoryPreviewPanel(FontEx font)
      {
        this.DoubleBuffered = true;
        this.nodes = new List<BaseHexNode>();
        this.DrawContext = new DrawContext()
        {
          Font = font,
          IconProvider = new IconProvider(),
          Memory = new MemoryBuffer(),
          HotSpots = new List<HotSpot>()
        };
        this.SetNodeCount(10);
        this.CalculateSize();
      }

      private void SetNodeCount(int count)
      {
        if (this.nodes.Count < count)
          this.nodes.AddRange(Enumerable.Range(this.nodes.Count, count - this.nodes.Count).Select<int, BaseHexNode>(new Func<int, BaseHexNode>(CreateNode)));
        else if (this.nodes.Count > count && count >= 10)
          this.nodes.RemoveRange(count, this.nodes.Count - count);
        this.DrawContext.Memory.Size = this.nodes.Select<BaseHexNode, int>((Func<BaseHexNode, int>) (n => n.MemorySize)).Sum();

        BaseHexNode CreateNode(int index)
        {
          Hex64Node hex64Node = new Hex64Node();
          hex64Node.Offset = index * IntPtr.Size;
          return (BaseHexNode) hex64Node;
        }
      }

      public void ChangeNodeCount(int delta)
      {
        this.SetNodeCount(this.nodes.Count + delta);
        this.CalculateSize();
      }

      public void Reset()
      {
        this.SetNodeCount(10);
        this.CalculateSize();
      }

      private void CalculateSize()
      {
        Size size = new Size(1004, this.nodes.Sum<BaseHexNode>((Func<BaseHexNode, int>) (n => n.CalculateDrawnHeight(this.DrawContext))) + 4);
        this.DrawContext.ClientArea = new Rectangle(2, 2, size.Width - 4, size.Height - 4);
        this.Size = this.MinimumSize = this.MaximumSize = size;
      }

      protected override void OnPaint(PaintEventArgs e)
      {
        this.DrawContext.HotSpots.Clear();
        this.DrawContext.Settings = Program.Settings.Clone();
        this.DrawContext.Settings.ShowNodeAddress = false;
        this.DrawContext.Graphics = e.Graphics;
        using (SolidBrush solidBrush = new SolidBrush(this.DrawContext.Settings.BackgroundColor))
          e.Graphics.FillRectangle((Brush) solidBrush, this.ClientRectangle);
        using (Pen pen = new Pen(this.DrawContext.Settings.BackgroundColor.Invert(), 1f))
          e.Graphics.DrawRectangle(pen, new Rectangle(this.Bounds.X, this.Bounds.Y, this.Bounds.Width - 1, this.Bounds.Height - 1));
        int x = -24;
        int y = 2;
        foreach (BaseHexNode node in this.nodes)
          y += node.Draw(this.DrawContext, x, y).Height;
      }
    }
  }
}
