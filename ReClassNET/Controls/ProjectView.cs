// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.ProjectView
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Extensions;
using ReClassNET.Nodes;
using ReClassNET.Project;
using ReClassNET.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  public class ProjectView : UserControl
  {
    private readonly TreeNode enumsRootNode;
    private readonly TreeNode classesRootNode;
    private ClassNode selectedClass;
    private bool autoExpandClassNodes;
    private bool enableClassHierarchyView;
    private IContainer components;
    private TreeView projectTreeView;

    public event ProjectView.SelectionChangedEvent SelectionChanged;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ClassNode SelectedClass
    {
      get
      {
        return this.selectedClass;
      }
      set
      {
        if (this.selectedClass == value)
          return;
        this.selectedClass = value;
        if (this.selectedClass != null)
          this.projectTreeView.SelectedNode = (TreeNode) this.FindMainClassTreeNode(this.selectedClass);
        ProjectView.SelectionChangedEvent selectionChanged = this.SelectionChanged;
        if (selectionChanged == null)
          return;
        selectionChanged((object) this, this.selectedClass);
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EnumDescription SelectedEnum { get; private set; }

    [DefaultValue(false)]
    public bool AutoExpandClassNodes
    {
      get
      {
        return this.autoExpandClassNodes;
      }
      set
      {
        if (this.autoExpandClassNodes == value)
          return;
        this.autoExpandClassNodes = value;
        if (!this.autoExpandClassNodes)
          return;
        this.ExpandAllClassNodes();
      }
    }

    [DefaultValue(false)]
    public bool EnableClassHierarchyView
    {
      get
      {
        return this.enableClassHierarchyView;
      }
      set
      {
        if (this.enableClassHierarchyView == value)
          return;
        this.enableClassHierarchyView = value;
        List<ClassNode> list = this.classesRootNode.Nodes.Cast<ProjectView.ClassTreeNode>().Select<ProjectView.ClassTreeNode, ClassNode>((Func<ProjectView.ClassTreeNode, ClassNode>) (t => t.ClassNode)).ToList<ClassNode>();
        this.classesRootNode.Nodes.Clear();
        this.AddClasses((IEnumerable<ClassNode>) list);
      }
    }

    public ContextMenuStrip ClassesContextMenuStrip { get; set; }

    public ContextMenuStrip ClassContextMenuStrip { get; set; }

    public ContextMenuStrip EnumsContextMenuStrip { get; set; }

    public ContextMenuStrip EnumContextMenuStrip { get; set; }

    public ProjectView()
    {
      this.InitializeComponent();
      this.DoubleBuffered = true;
      this.projectTreeView.TreeViewNodeSorter = (IComparer) new ProjectView.NodeSorter();
      this.projectTreeView.ImageList = new ImageList();
      this.projectTreeView.ImageList.Images.Add((Image) Resources.B16x16_Text_List_Bullets);
      this.projectTreeView.ImageList.Images.Add((Image) Resources.B16x16_Button_Class_Instance);
      this.projectTreeView.ImageList.Images.Add((Image) Resources.B16x16_Category);
      this.projectTreeView.ImageList.Images.Add((Image) Resources.B16x16_Enum_Type);
      this.classesRootNode = new TreeNode()
      {
        Text = "Classes",
        ImageIndex = 0,
        SelectedImageIndex = 0,
        Tag = (object) 0
      };
      this.projectTreeView.Nodes.Add(this.classesRootNode);
      this.enumsRootNode = new TreeNode()
      {
        Text = "Enums",
        ImageIndex = 2,
        SelectedImageIndex = 2,
        Tag = (object) 1
      };
      this.projectTreeView.Nodes.Add(this.enumsRootNode);
    }

    private void projectTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Level == 0)
        return;
      if (e.Node is ProjectView.ClassTreeNode node)
      {
        if (this.selectedClass == node.ClassNode)
          return;
        this.SelectedClass = node.ClassNode;
      }
      else
      {
        if (!(e.Node is ProjectView.EnumTreeNode node))
          return;
        this.SelectedEnum = node.Enum;
      }
    }

    private void projectTreeView_MouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      TreeNode nodeAt = this.projectTreeView.GetNodeAt(e.X, e.Y);
      if (nodeAt == null)
        return;
      ContextMenuStrip contextMenuStrip = (ContextMenuStrip) null;
      if (nodeAt == this.classesRootNode)
        contextMenuStrip = this.ClassesContextMenuStrip;
      else if (nodeAt is ProjectView.ClassTreeNode)
      {
        contextMenuStrip = this.ClassContextMenuStrip;
        this.projectTreeView.SelectedNode = nodeAt;
      }
      else if (nodeAt == this.enumsRootNode)
        contextMenuStrip = this.EnumsContextMenuStrip;
      else if (nodeAt is ProjectView.EnumTreeNode)
      {
        contextMenuStrip = this.EnumContextMenuStrip;
        this.projectTreeView.SelectedNode = nodeAt;
      }
      contextMenuStrip?.Show((Control) this.projectTreeView, e.Location);
    }

    private void projectTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      bool flag1 = e.Node is ProjectView.ClassTreeNode;
      bool flag2 = e.Node is ProjectView.EnumTreeNode;
      e.CancelEdit = !flag1 && !flag2;
    }

    private void projectTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
      if (string.IsNullOrEmpty(e.Label))
        return;
      if (e.Node is ProjectView.ClassTreeNode node)
      {
        node.ClassNode.Name = e.Label;
      }
      else
      {
        if (!(e.Node is ProjectView.EnumTreeNode node))
          return;
        node.Enum.Name = e.Label;
      }
    }

    public void ExpandAllClassNodes()
    {
      this.classesRootNode.ExpandAll();
    }

    public void CollapseAllClassNodes()
    {
      foreach (TreeNode treeNode in this.classesRootNode.Nodes.Cast<TreeNode>())
        treeNode.Collapse();
    }

    public void Clear()
    {
      this.Clear(true, true);
    }

    public void Clear(bool clearClasses, bool clearEnums)
    {
      if (clearClasses)
        this.classesRootNode.Nodes.Clear();
      if (!clearEnums)
        return;
      this.enumsRootNode.Nodes.Clear();
    }

    public void AddClass(ClassNode @class)
    {
      this.AddClasses((IEnumerable<ClassNode>) new ClassNode[1]
      {
        @class
      });
    }

    public void AddClasses(IEnumerable<ClassNode> classes)
    {
      this.projectTreeView.BeginUpdate();
      foreach (ClassNode node in classes)
        this.classesRootNode.Nodes.Add((TreeNode) new ProjectView.ClassTreeNode(node, this));
      this.classesRootNode.Expand();
      this.projectTreeView.Sort();
      this.projectTreeView.EndUpdate();
    }

    public void RemoveClass(ClassNode node)
    {
      foreach (TreeNode classTreeNode in this.FindClassTreeNodes(node))
        classTreeNode.Remove();
      if (this.selectedClass != node)
        return;
      if (this.classesRootNode.Nodes.Count > 0)
        this.projectTreeView.SelectedNode = this.classesRootNode.Nodes[0];
      else
        this.SelectedClass = (ClassNode) null;
    }

    private ProjectView.ClassTreeNode FindMainClassTreeNode(ClassNode node)
    {
      return this.classesRootNode.Nodes.Cast<ProjectView.ClassTreeNode>().FirstOrDefault<ProjectView.ClassTreeNode>((Func<ProjectView.ClassTreeNode, bool>) (t => t.ClassNode == node));
    }

    private IEnumerable<ProjectView.ClassTreeNode> FindClassTreeNodes(
      ClassNode node)
    {
      return this.classesRootNode.Nodes.Cast<ProjectView.ClassTreeNode>().Traverse<ProjectView.ClassTreeNode>((Func<ProjectView.ClassTreeNode, IEnumerable<ProjectView.ClassTreeNode>>) (t => t.Nodes.Cast<ProjectView.ClassTreeNode>())).Where<ProjectView.ClassTreeNode>((Func<ProjectView.ClassTreeNode, bool>) (n => n.ClassNode == node));
    }

    public void UpdateClassNode(ClassNode @class)
    {
      this.projectTreeView.BeginUpdate();
      foreach (ProjectView.ClassTreeNode classTreeNode in this.FindClassTreeNodes(@class))
        classTreeNode.Update();
      this.projectTreeView.Sort();
      this.projectTreeView.EndUpdate();
    }

    public void AddEnum(EnumDescription @enum)
    {
      this.AddEnums((IEnumerable<EnumDescription>) new EnumDescription[1]
      {
        @enum
      });
    }

    public void AddEnums(IEnumerable<EnumDescription> enums)
    {
      this.projectTreeView.BeginUpdate();
      foreach (EnumDescription @enum in enums)
        this.enumsRootNode.Nodes.Add((TreeNode) new ProjectView.EnumTreeNode(@enum));
      this.enumsRootNode.ExpandAll();
      this.projectTreeView.Sort();
      this.projectTreeView.EndUpdate();
    }

    public void UpdateEnumNode(EnumDescription @enum)
    {
      this.projectTreeView.BeginUpdate();
      ProjectView.EnumTreeNode enumTreeNode = this.enumsRootNode.Nodes.Cast<ProjectView.EnumTreeNode>().FirstOrDefault<ProjectView.EnumTreeNode>((Func<ProjectView.EnumTreeNode, bool>) (n => n.Enum == @enum));
      if (enumTreeNode != null)
        enumTreeNode.Update();
      else
        this.AddEnum(@enum);
      this.projectTreeView.Sort();
      this.projectTreeView.EndUpdate();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.projectTreeView = new TreeView();
      this.SuspendLayout();
      this.projectTreeView.BackColor = Color.FromArgb(69, 73, 74);
      this.projectTreeView.BorderStyle = BorderStyle.FixedSingle;
      this.projectTreeView.Dock = DockStyle.Fill;
      this.projectTreeView.ForeColor = Color.White;
      this.projectTreeView.HideSelection = false;
      this.projectTreeView.LabelEdit = true;
      this.projectTreeView.LineColor = Color.White;
      this.projectTreeView.Location = new Point(0, 0);
      this.projectTreeView.Name = "projectTreeView";
      this.projectTreeView.ShowRootLines = false;
      this.projectTreeView.Size = new Size(150, 150);
      this.projectTreeView.TabIndex = 0;
      this.projectTreeView.BeforeLabelEdit += new NodeLabelEditEventHandler(this.projectTreeView_BeforeLabelEdit);
      this.projectTreeView.AfterLabelEdit += new NodeLabelEditEventHandler(this.projectTreeView_AfterLabelEdit);
      this.projectTreeView.AfterSelect += new TreeViewEventHandler(this.projectTreeView_AfterSelect);
      this.projectTreeView.MouseUp += new MouseEventHandler(this.projectTreeView_MouseUp);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(69, 73, 74);
      this.Controls.Add((Control) this.projectTreeView);
      this.ForeColor = Color.FromArgb(69, 73, 74);
      this.Name = nameof (ProjectView);
      this.ResumeLayout(false);
    }

    private class ClassTreeNode : TreeNode
    {
      private readonly ProjectView control;

      public ClassNode ClassNode { get; }

      public ClassTreeNode(ClassNode node, ProjectView control)
        : this(node, control, (HashSet<ClassNode>) null)
      {
      }

      private ClassTreeNode(ClassNode node, ProjectView control, HashSet<ClassNode> seen)
      {
        this.ClassNode = node;
        this.control = control;
        this.Text = node.Name;
        this.ImageIndex = 1;
        this.SelectedImageIndex = 1;
        HashSet<ClassNode> seen1 = seen;
        if (seen1 == null)
          seen1 = new HashSet<ClassNode>()
          {
            this.ClassNode
          };
        this.RebuildClassHierarchy(seen1);
      }

      public void Update()
      {
        this.Text = this.ClassNode.Name;
        this.RebuildClassHierarchy(new HashSet<ClassNode>()
        {
          this.ClassNode
        });
      }

      private void RebuildClassHierarchy(HashSet<ClassNode> seen)
      {
        if (!this.control.EnableClassHierarchyView)
          return;
        List<ClassNode> list = this.ClassNode.Nodes.OfType<BaseWrapperNode>().Select<BaseWrapperNode, BaseNode>((Func<BaseWrapperNode, BaseNode>) (w => w.ResolveMostInnerNode())).OfType<ClassNode>().Distinct<ClassNode>().ToList<ClassNode>();
        if (list.IsEquivalentTo<ClassNode>(this.Nodes.Cast<ProjectView.ClassTreeNode>().Select<ProjectView.ClassTreeNode, ClassNode>((Func<ProjectView.ClassTreeNode, ClassNode>) (t => t.ClassNode))))
          return;
        this.Nodes.Clear();
        foreach (ClassNode node in list)
        {
          HashSet<ClassNode> seen1 = new HashSet<ClassNode>((IEnumerable<ClassNode>) seen);
          if (seen1.Add(node))
            this.Nodes.Add((TreeNode) new ProjectView.ClassTreeNode(node, this.control, seen1));
        }
        if (!this.control.AutoExpandClassNodes)
          return;
        this.Expand();
      }
    }

    public class EnumTreeNode : TreeNode
    {
      public EnumDescription Enum { get; }

      public EnumTreeNode(EnumDescription @enum)
      {
        this.Enum = @enum;
        this.ImageIndex = 3;
        this.SelectedImageIndex = 3;
        this.Update();
      }

      public void Update()
      {
        this.Text = this.Enum.Name;
      }
    }

    private class NodeSorter : IComparer
    {
      public int Compare(object x, object y)
      {
        CompareInfo compareInfo = Application.CurrentCulture.CompareInfo;
        switch (x)
        {
          case ProjectView.ClassTreeNode classTreeNode when y is ProjectView.ClassTreeNode classTreeNode:
            return compareInfo.Compare(classTreeNode.Text, classTreeNode.Text);
          case ProjectView.EnumTreeNode enumTreeNode when y is ProjectView.EnumTreeNode enumTreeNode:
            return compareInfo.Compare(enumTreeNode.Text, enumTreeNode.Text);
          case TreeNode treeNode when treeNode.Parent == null && (y is TreeNode treeNode && treeNode.Parent == null):
            return (int) treeNode.Tag - (int) treeNode.Tag;
          default:
            return 0;
        }
      }
    }

    public delegate void SelectionChangedEvent(object sender, ClassNode node);
  }
}
