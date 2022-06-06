// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.MemoryViewControl
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class MemoryViewControl : UserControl
  {
    private readonly List<HotSpot> hotSpots = new List<HotSpot>();
    private readonly List<HotSpot> selectedNodes = new List<HotSpot>();
    private IContainer components;
    private Timer repaintTimer;
    private HotSpotTextBox hotSpotEditBox;
    private ToolTip nodeInfoToolTip;
    private HotSpot selectionCaret;
    private HotSpot selectionAnchor;
    private readonly FontEx font;
    private readonly MemoryPreviewPopUp memoryPreviewPopUp;
    private Point toolTipPosition;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.repaintTimer = new Timer(this.components);
      this.hotSpotEditBox = new HotSpotTextBox();
      this.nodeInfoToolTip = new ToolTip(this.components);
      this.SuspendLayout();
      this.repaintTimer.Enabled = true;
      this.repaintTimer.Interval = 250;
      this.repaintTimer.Tick += new EventHandler(this.repaintTimer_Tick);
      this.hotSpotEditBox.BorderStyle = BorderStyle.None;
      this.hotSpotEditBox.Location = new Point(0, 0);
      this.hotSpotEditBox.Name = "hotSpotEditBox";
      this.hotSpotEditBox.Size = new Size(100, 13);
      this.hotSpotEditBox.TabIndex = 1;
      this.hotSpotEditBox.TabStop = false;
      this.hotSpotEditBox.Visible = false;
      this.hotSpotEditBox.Committed += new HotSpotTextBoxCommitEventHandler(this.editBox_Committed);
      this.nodeInfoToolTip.ShowAlways = true;
      this.Controls.Add((Control) this.hotSpotEditBox);
      this.DoubleBuffered = true;
      this.Name = nameof (MemoryViewControl);
      this.Size = new Size(150, 162);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public ContextMenuStrip NodeContextMenuStrip { get; set; }

    public event DrawContextRequestEventHandler DrawContextRequested;

    public event EventHandler SelectionChanged;

    public event NodeClickEventHandler ChangeClassTypeClick;

    public event NodeClickEventHandler ChangeWrappedTypeClick;

    public event NodeClickEventHandler ChangeEnumTypeClick;

    public MemoryViewControl()
    {
      this.InitializeComponent();
      if (Program.DesignMode)
        return;
      this.AutoScroll = true;
      this.font = Program.MonoSpaceFont;
      this.hotSpotEditBox.Font = this.font;
      this.memoryPreviewPopUp = new MemoryPreviewPopUp(this.font);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (this.DesignMode)
      {
        e.Graphics.FillRectangle(Brushes.White, this.ClientRectangle);
      }
      else
      {
        DrawContextRequestEventArgs args = new DrawContextRequestEventArgs();
        DrawContextRequestEventHandler contextRequested = this.DrawContextRequested;
        if (contextRequested != null)
          contextRequested((object) this, args);
        this.hotSpots.Clear();
        using (SolidBrush solidBrush = new SolidBrush(Program.Settings.BackgroundColor))
          e.Graphics.FillRectangle((Brush) solidBrush, this.ClientRectangle);
        if (args.Process == null || args.Memory == null || args.Node == null)
          return;
        if (this.memoryPreviewPopUp.Visible)
          this.memoryPreviewPopUp.UpdateMemory();
        DrawContext context = new DrawContext()
        {
          Settings = args.Settings,
          Graphics = e.Graphics,
          Font = this.font,
          IconProvider = args.IconProvider,
          Process = args.Process,
          Memory = args.Memory,
          CurrentTime = args.CurrentTime,
          ClientArea = this.ClientRectangle,
          HotSpots = this.hotSpots,
          Address = args.BaseAddress,
          Level = 0,
          MultipleNodesSelected = this.selectedNodes.Count > 1
        };
        Point autoScrollPosition = this.AutoScrollPosition;
        Size size = args.Node.Draw(context, autoScrollPosition.X, autoScrollPosition.Y);
        size.Width += 10;
        int width1 = size.Width;
        Size clientSize = this.ClientSize;
        int width2 = clientSize.Width;
        int width3 = Math.Max(width1, width2);
        int height1 = size.Height;
        clientSize = this.ClientSize;
        int height2 = clientSize.Height;
        int height3 = Math.Max(height1, height2);
        this.AutoScrollMinSize = new Size(width3, height3);
        this.AutoScrollPosition = new Point(-autoScrollPosition.X, -autoScrollPosition.Y);
      }
    }

    private void OnSelectionChanged()
    {
      EventHandler selectionChanged = this.SelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, EventArgs.Empty);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
      this.hotSpotEditBox.Hide();
      bool flag = false;
      foreach (HotSpot hotSpot1 in this.hotSpots)
      {
        if (hotSpot1.Rect.Contains(e.Location))
        {
          BaseNode hitObject = hotSpot1.Node;
          if (hotSpot1.Type == HotSpotType.OpenClose)
          {
            hitObject.ToggleLevelOpen(hotSpot1.Level);
            flag = true;
            break;
          }
          if (hotSpot1.Type == HotSpotType.Click)
          {
            hitObject.Update(hotSpot1);
            flag = true;
            break;
          }
          if (hotSpot1.Type == HotSpotType.Select)
          {
            if (e.Button == MouseButtons.Left)
            {
              switch (Control.ModifierKeys)
              {
                case Keys.None:
                  this.ClearSelection();
                  hitObject.IsSelected = true;
                  this.selectedNodes.Add(hotSpot1);
                  this.OnSelectionChanged();
                  this.selectionAnchor = this.selectionCaret = hotSpot1;
                  break;
                case Keys.Shift:
                  if (this.selectedNodes.Count > 0)
                  {
                    BaseNode node = this.selectedNodes[0].Node;
                    if ((hitObject.GetParentContainer() == null || node.GetParentContainer() == hitObject.GetParentContainer()) && !(hotSpot1.Node is BaseContainerNode))
                    {
                      HotSpot first = Utils.Min<HotSpot, int>(this.selectedNodes[0], hotSpot1, (Func<HotSpot, int>) (h => h.Node.Offset));
                      HotSpot last = first == hotSpot1 ? this.selectedNodes[0] : hotSpot1;
                      this.ClearSelection();
                      BaseContainerNode containerNode = node.GetParentContainer();
                      foreach (HotSpot hotSpot2 in containerNode.Nodes.SkipWhile<BaseNode>((Func<BaseNode, bool>) (n => n != first.Node)).TakeWhileInclusive<BaseNode>((Func<BaseNode, bool>) (n => n != last.Node)).Select<BaseNode, HotSpot>((Func<BaseNode, HotSpot>) (n => new HotSpot()
                      {
                        Address = (IntPtr) (containerNode.Offset + n.Offset),
                        Node = n,
                        Process = first.Process,
                        Memory = first.Memory,
                        Level = first.Level
                      })))
                      {
                        hotSpot2.Node.IsSelected = true;
                        this.selectedNodes.Add(hotSpot2);
                      }
                      this.OnSelectionChanged();
                      this.selectionAnchor = first;
                      this.selectionCaret = last;
                      break;
                    }
                    continue;
                  }
                  break;
                case Keys.Control:
                  hitObject.IsSelected = !hitObject.IsSelected;
                  if (hitObject.IsSelected)
                    this.selectedNodes.Add(hotSpot1);
                  else
                    this.selectedNodes.Remove(this.selectedNodes.FirstOrDefault<HotSpot>((Func<HotSpot, bool>) (c => c.Node == hitObject)));
                  this.OnSelectionChanged();
                  break;
              }
            }
            else if (e.Button == MouseButtons.Right)
            {
              if (this.selectedNodes.Count <= 1)
              {
                this.ClearSelection();
                hitObject.IsSelected = true;
                this.selectedNodes.Add(hotSpot1);
                this.OnSelectionChanged();
                this.selectionAnchor = this.selectionCaret = hotSpot1;
              }
              this.ShowNodeContextMenu(e.Location);
            }
            flag = true;
          }
          else
          {
            if (hotSpot1.Type == HotSpotType.Context)
            {
              this.ShowNodeContextMenu(e.Location);
              break;
            }
            if (hotSpot1.Type == HotSpotType.Delete)
            {
              hotSpot1.Node.GetParentContainer().RemoveNode(hotSpot1.Node);
              flag = true;
              break;
            }
            if (hotSpot1.Type == HotSpotType.ChangeClassType || hotSpot1.Type == HotSpotType.ChangeWrappedType || hotSpot1.Type == HotSpotType.ChangeEnumType)
            {
              NodeClickEventHandler clickEventHandler1;
              switch (hotSpot1.Type)
              {
                case HotSpotType.ChangeClassType:
                  clickEventHandler1 = this.ChangeClassTypeClick;
                  break;
                case HotSpotType.ChangeWrappedType:
                  clickEventHandler1 = this.ChangeWrappedTypeClick;
                  break;
                case HotSpotType.ChangeEnumType:
                  clickEventHandler1 = this.ChangeEnumTypeClick;
                  break;
                default:
                  throw new InvalidOperationException();
              }
              NodeClickEventHandler clickEventHandler2 = clickEventHandler1;
              if (clickEventHandler2 != null)
              {
                clickEventHandler2((object) this, new NodeClickEventArgs(hitObject, hotSpot1.Address, hotSpot1.Memory, e.Button, e.Location));
                break;
              }
              break;
            }
          }
        }
      }
      if (flag)
        this.Invalidate();
      base.OnMouseClick(e);
    }

    protected override void OnMouseDoubleClick(MouseEventArgs e)
    {
      this.hotSpotEditBox.Hide();
      bool flag = false;
      foreach (HotSpot hotSpot in this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.DoubleClick)).Concat<HotSpot>(this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Click))).Concat<HotSpot>(this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Edit))).Concat<HotSpot>(this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Select))))
      {
        if (hotSpot.Rect.Contains(e.Location))
        {
          if (hotSpot.Type == HotSpotType.DoubleClick || hotSpot.Type == HotSpotType.Click)
          {
            hotSpot.Node.Update(hotSpot);
            flag = true;
            break;
          }
          if (hotSpot.Type == HotSpotType.Edit)
          {
            this.hotSpotEditBox.ShowOnHotSpot(hotSpot);
            break;
          }
          if (hotSpot.Type == HotSpotType.Select)
          {
            hotSpot.Node.ToggleLevelOpen(hotSpot.Level);
            flag = true;
            break;
          }
        }
      }
      if (flag)
        this.Invalidate();
      base.OnMouseDoubleClick(e);
    }

    protected override void OnMouseHover(EventArgs e)
    {
      base.OnMouseHover(e);
      if (this.selectedNodes.Count > 1)
      {
        this.nodeInfoToolTip.Show(string.Format("{0} Nodes selected, {1} bytes", (object) this.selectedNodes.Count, (object) this.selectedNodes.Sum<HotSpot>((Func<HotSpot, int>) (h => h.Node.MemorySize))), (IWin32Window) this, this.toolTipPosition.Relocate(16, 16));
      }
      else
      {
        foreach (HotSpot spot in this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Select)))
        {
          if (spot.Rect.Contains(this.toolTipPosition))
          {
            IntPtr address;
            if (spot.Node.UseMemoryPreviewToolTip(spot, out address))
            {
              this.memoryPreviewPopUp.InitializeMemory(spot.Process, address);
              this.memoryPreviewPopUp.Show((Control) this, this.toolTipPosition.Relocate(16, 16));
              break;
            }
            string toolTipText = spot.Node.GetToolTipText(spot);
            if (string.IsNullOrEmpty(toolTipText))
              break;
            this.nodeInfoToolTip.Show(toolTipText, (IWin32Window) this, this.toolTipPosition.Relocate(16, 16));
            break;
          }
        }
      }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (!(e.Location != this.toolTipPosition))
        return;
      this.toolTipPosition = e.Location;
      this.nodeInfoToolTip.Hide((IWin32Window) this);
      if (this.memoryPreviewPopUp.Visible)
      {
        this.memoryPreviewPopUp.Close();
        this.Invalidate();
      }
      this.ResetMouseEventArgs();
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
      this.hotSpotEditBox.Hide();
      if (this.memoryPreviewPopUp.Visible)
        this.memoryPreviewPopUp.HandleMouseWheelEvent(e);
      else
        base.OnMouseWheel(e);
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (!this.hotSpotEditBox.Visible)
      {
        Keys keys1 = keyData & Keys.KeyCode;
        Keys keys2 = keyData & Keys.Modifiers;
        if (this.selectedNodes.Count > 0)
        {
          if (keys1 == Keys.Menu)
          {
            this.ShowNodeContextMenu(new Point(10, 10));
            return true;
          }
          if ((keys1 == Keys.Down || keys1 == Keys.Up) && (this.selectionCaret != null && this.selectionAnchor != null))
          {
            IEnumerable<HotSpot> source = this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Select)).Where<HotSpot>((Func<HotSpot, bool>) (h => h.Node.GetParentContainer() == this.selectionCaret.Node.GetParentContainer()));
            bool flag;
            HotSpot toSelect;
            if (keys1 == Keys.Down)
            {
              List<HotSpot> list = source.SkipWhile<HotSpot>((Func<HotSpot, bool>) (h => h.Node != this.selectionCaret.Node)).Skip<HotSpot>(1).ToList<HotSpot>();
              toSelect = list.FirstOrDefault<HotSpot>();
              flag = toSelect != null && toSelect == list.LastOrDefault<HotSpot>();
            }
            else
            {
              List<HotSpot> list = source.TakeWhile<HotSpot>((Func<HotSpot, bool>) (h => h.Node != this.selectionCaret.Node)).ToList<HotSpot>();
              toSelect = list.LastOrDefault<HotSpot>();
              flag = toSelect != null && toSelect == list.FirstOrDefault<HotSpot>();
            }
            if (toSelect != null && !(toSelect.Node is ClassNode))
            {
              if (keys2 != Keys.Shift)
                this.selectionAnchor = this.selectionCaret = toSelect;
              else
                this.selectionCaret = toSelect;
              HotSpot first = Utils.Min<HotSpot, int>(this.selectionAnchor, this.selectionCaret, (Func<HotSpot, int>) (h => h.Node.Offset));
              HotSpot last = first == this.selectionAnchor ? this.selectionCaret : this.selectionAnchor;
              this.selectedNodes.ForEach((Action<HotSpot>) (h => h.Node.ClearSelection()));
              this.selectedNodes.Clear();
              BaseContainerNode containerNode = toSelect.Node.GetParentContainer();
              foreach (HotSpot hotSpot in containerNode.Nodes.SkipWhile<BaseNode>((Func<BaseNode, bool>) (n => n != first.Node)).TakeWhileInclusive<BaseNode>((Func<BaseNode, bool>) (n => n != last.Node)).Select<BaseNode, HotSpot>((Func<BaseNode, HotSpot>) (n => new HotSpot()
              {
                Address = (IntPtr) (containerNode.Offset + n.Offset),
                Node = n,
                Process = toSelect.Process,
                Memory = toSelect.Memory,
                Level = toSelect.Level
              })))
              {
                hotSpot.Node.IsSelected = true;
                this.selectedNodes.Add(hotSpot);
              }
              this.OnSelectionChanged();
              if (flag)
              {
                Point autoScrollPosition = this.AutoScrollPosition;
                this.AutoScrollPosition = new Point(-autoScrollPosition.X, -autoScrollPosition.Y + (keys1 == Keys.Down ? 3 : -3) * this.font.Height);
              }
              this.Invalidate();
              return true;
            }
          }
          else if ((keys1 == Keys.Left || keys1 == Keys.Right) && this.selectedNodes.Count == 1)
          {
            HotSpot selectedNode = this.selectedNodes[0];
            selectedNode.Node.SetLevelOpen(selectedNode.Level, keys1 == Keys.Right);
          }
        }
        else if (keys1 == Keys.Down || keys1 == Keys.Up)
        {
          HotSpot hotSpot = this.hotSpots.Where<HotSpot>((Func<HotSpot, bool>) (h => h.Type == HotSpotType.Select)).WhereNot<HotSpot>((Func<HotSpot, bool>) (h => h.Node is ClassNode)).FirstOrDefault<HotSpot>();
          if (hotSpot != null)
          {
            this.selectionAnchor = this.selectionCaret = hotSpot;
            hotSpot.Node.IsSelected = true;
            this.selectedNodes.Add(hotSpot);
            this.OnSelectionChanged();
            return true;
          }
        }
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      this.Invalidate();
    }

    private void repaintTimer_Tick(object sender, EventArgs e)
    {
      if (this.DesignMode)
        return;
      this.Invalidate(false);
    }

    private void editBox_Committed(object sender, HotSpotTextBoxCommitEventArgs e)
    {
      HotSpot hotSpot = e.HotSpot;
      if (hotSpot != null)
      {
        try
        {
          hotSpot.Node.Update(hotSpot);
        }
        catch (Exception ex)
        {
          Program.Logger.Log(ex);
        }
        this.Invalidate();
      }
      this.Focus();
    }

    public IReadOnlyList<MemoryViewControl.SelectedNodeInfo> GetSelectedNodes()
    {
      return (IReadOnlyList<MemoryViewControl.SelectedNodeInfo>) this.selectedNodes.Select<HotSpot, MemoryViewControl.SelectedNodeInfo>((Func<HotSpot, MemoryViewControl.SelectedNodeInfo>) (h => new MemoryViewControl.SelectedNodeInfo(h.Node, h.Process, h.Memory, h.Address, h.Level))).ToList<MemoryViewControl.SelectedNodeInfo>();
    }

    public void SetSelectedNodes(
      IEnumerable<MemoryViewControl.SelectedNodeInfo> nodes)
    {
      this.selectedNodes.ForEach((Action<HotSpot>) (h => h.Node.ClearSelection()));
      this.selectedNodes.Clear();
      this.selectedNodes.AddRange(nodes.Select<MemoryViewControl.SelectedNodeInfo, HotSpot>((Func<MemoryViewControl.SelectedNodeInfo, HotSpot>) (i => new HotSpot()
      {
        Type = HotSpotType.Select,
        Node = i.Node,
        Process = i.Process,
        Memory = i.Memory,
        Address = i.Address,
        Level = i.Level
      })));
      this.selectedNodes.ForEach((Action<HotSpot>) (h => h.Node.IsSelected = true));
      this.OnSelectionChanged();
    }

    private void ShowNodeContextMenu(Point location)
    {
      this.NodeContextMenuStrip?.Show((Control) this, location);
    }

    public void ShowNodeNameEditBox(BaseNode node)
    {
      if (node == null || node is BaseHexNode)
        return;
      HotSpot hotSpot = this.hotSpots.FirstOrDefault<HotSpot>((Func<HotSpot, bool>) (s => s.Node == node && s.Type == HotSpotType.Edit && s.Id == 101));
      if (hotSpot == null)
        return;
      this.hotSpotEditBox.ShowOnHotSpot(hotSpot);
    }

    public void ClearSelection()
    {
      this.selectionAnchor = this.selectionCaret = (HotSpot) null;
      this.selectedNodes.ForEach((Action<HotSpot>) (h => h.Node.ClearSelection()));
      this.selectedNodes.Clear();
      this.OnSelectionChanged();
    }

    public void Reset()
    {
      this.ClearSelection();
      this.hotSpotEditBox.Hide();
      this.VerticalScroll.Value = this.VerticalScroll.Minimum;
    }

    public class SelectedNodeInfo
    {
      public BaseNode Node { get; }

      public RemoteProcess Process { get; }

      public MemoryBuffer Memory { get; }

      public IntPtr Address { get; }

      public int Level { get; }

      public SelectedNodeInfo(
        BaseNode node,
        RemoteProcess process,
        MemoryBuffer memory,
        IntPtr address,
        int level)
      {
        this.Node = node;
        this.Process = process;
        this.Memory = memory;
        this.Address = address;
        this.Level = level;
      }
    }
  }
}
