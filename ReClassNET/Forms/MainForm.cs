// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.MainForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using libdebug;
using ReClassNET.AddressParser;
using ReClassNET.CodeGenerator;
using ReClassNET.Controls;
using ReClassNET.Core;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Project;
using ReClassNET.Properties;
using ReClassNET.UI;
using ReClassNET.Util;
using ReClassNET.Util.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class MainForm : IconForm
  {
    private readonly IconProvider iconProvider = new IconProvider();
    private readonly MemoryBuffer memoryViewBuffer = new MemoryBuffer();
    private readonly IniFile cfg = new IniFile(Application.StartupPath + "\\ps4info.ini");
    private readonly PluginManager pluginManager;
    private ReClassNetProject currentProject;
    private ClassNode currentClassNode;
    private Task updateProcessInformationsTask;
    private Task loadSymbolsTask;
    private CancellationTokenSource loadSymbolsTaskToken;
    public static PS4DBG PS4;
    public static string PS4PID;
    public static ProcessList ProcList;
    private IContainer components;
    private MemoryViewControl memoryViewControl;
    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem clearProjectToolStripMenuItem;
    private ToolStripMenuItem openProjectToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem saveAsToolStripMenuItem;
    private ToolStripMenuItem quitToolStripMenuItem;
    private ToolStripMenuItem helpToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private ToolStrip toolStrip;
    private SplitContainer splitContainer;
    private ToolStripButton saveToolStripButton;
    private ToolStripSeparator toolStripSeparator7;
    private ToolStripButton newClassToolStripButton;
    private ToolStripDropDownButton addBytesToolStripDropDownButton;
    private IntegerToolStripMenuItem add4BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add8BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add64BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add256BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add1024BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add2048BytesToolStripMenuItem;
    private IntegerToolStripMenuItem add4096BytesToolStripMenuItem;
    private ToolStripDropDownButton insertBytesToolStripDropDownButton;
    private IntegerToolStripMenuItem insert4BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert8BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert64BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert256BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert1024BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert2048BytesToolStripMenuItem;
    private IntegerToolStripMenuItem insert4096BytesToolStripMenuItem;
    private ToolStripMenuItem addXBytesToolStripMenuItem;
    private ToolStripMenuItem insertXBytesToolStripMenuItem;
    private ToolStripSeparator nodeTypesToolStripSeparator;
    private ProjectView projectView;
    private ToolStripMenuItem projectToolStripMenuItem;
    private ToolStripMenuItem cleanUnusedClassesToolStripMenuItem;
    private ToolStripMenuItem generateCppCodeToolStripMenuItem;
    private ToolStripMenuItem generateCSharpCodeToolStripMenuItem;
    private System.Windows.Forms.Timer processUpdateTimer;
    private ToolStripButton openProjectToolStripButton;
    private ToolStripMenuItem mergeWithProjectToolStripMenuItem;
    private ContextMenuStrip selectedNodeContextMenuStrip;
    private ToolStripMenuItem changeTypeToolStripMenuItem;
    private ToolStripMenuItem addBytesToolStripMenuItem;
    private IntegerToolStripMenuItem integerToolStripMenuItem1;
    private IntegerToolStripMenuItem integerToolStripMenuItem2;
    private IntegerToolStripMenuItem integerToolStripMenuItem3;
    private IntegerToolStripMenuItem integerToolStripMenuItem4;
    private IntegerToolStripMenuItem integerToolStripMenuItem5;
    private IntegerToolStripMenuItem integerToolStripMenuItem6;
    private IntegerToolStripMenuItem integerToolStripMenuItem7;
    private ToolStripMenuItem insertBytesToolStripMenuItem;
    private IntegerToolStripMenuItem integerToolStripMenuItem8;
    private IntegerToolStripMenuItem integerToolStripMenuItem9;
    private IntegerToolStripMenuItem integerToolStripMenuItem10;
    private IntegerToolStripMenuItem integerToolStripMenuItem11;
    private IntegerToolStripMenuItem integerToolStripMenuItem12;
    private IntegerToolStripMenuItem integerToolStripMenuItem13;
    private IntegerToolStripMenuItem integerToolStripMenuItem14;
    private ToolStripMenuItem createClassFromNodesToolStripMenuItem;
    private ToolStripMenuItem dissectNodesToolStripMenuItem;
    private ToolStripMenuItem copyNodeToolStripMenuItem;
    private ToolStripMenuItem pasteNodesToolStripMenuItem;
    private ToolStripMenuItem removeToolStripMenuItem;
    private ToolStripMenuItem hideNodesToolStripMenuItem;
    private ToolStripMenuItem unhideNodesToolStripMenuItem;
    private ToolStripMenuItem unhideChildNodesToolStripMenuItem;
    private ToolStripMenuItem unhideNodesAboveToolStripMenuItem;
    private ToolStripMenuItem unhideNodesBelowToolStripMenuItem;
    private ToolStripMenuItem copyAddressToolStripMenuItem;
    private ToolStripMenuItem showCodeOfClassToolStripMenuItem;
    private ToolStripMenuItem shrinkClassToolStripMenuItem;
    private ToolStripMenuItem toolStripMenuItem1;
    private ToolStripMenuItem toolStripMenuItem2;
    private ToolStripMenuItem goToClassToolStripMenuItem;
    private ContextMenuStrip projectClassContextMenuStrip;
    private ToolStripMenuItem deleteClassToolStripMenuItem;
    private ToolStripMenuItem removeUnusedClassesToolStripMenuItem;
    private ToolStripMenuItem showCodeOfClassToolStripMenuItem2;
    private ContextMenuStrip projectClassesContextMenuStrip;
    private ToolStripMenuItem enableHierarchyViewToolStripMenuItem;
    private ToolStripMenuItem autoExpandHierarchyViewToolStripMenuItem;
    private ToolStripMenuItem expandAllClassesToolStripMenuItem;
    private ToolStripMenuItem collapseAllClassesToolStripMenuItem;
    private ToolStripMenuItem addNewClassToolStripMenuItem;
    private ContextMenuStrip projectEnumContextMenuStrip;
    private ToolStripMenuItem editEnumToolStripMenuItem;
    private ContextMenuStrip projectEnumsContextMenuStrip;
    private ToolStripMenuItem editEnumsToolStripMenuItem;
    private ToolStripMenuItem showEnumsToolStripMenuItem;
    private ToolStripTextBox toolStripTextBox1;
    private ToolStripComboBox toolStripComboBox1;
    private ToolStripMenuItem toolStripMenuItem3;
    private ToolStripMenuItem socketListenerToolStripMenuItem;

    public void ShowPartialCodeGeneratorForm(IReadOnlyList<ClassNode> partialClasses)
    {
      this.ShowCodeGeneratorForm(partialClasses, (IReadOnlyList<EnumDescription>) new EnumDescription[0], (ICodeGenerator) new CppCodeGenerator(this.currentProject.TypeMapping));
    }

    public void ShowCodeGeneratorForm(ICodeGenerator generator)
    {
      this.ShowCodeGeneratorForm(this.currentProject.Classes, this.currentProject.Enums, generator);
    }

    public void ShowCodeGeneratorForm(
      IReadOnlyList<ClassNode> classes,
      IReadOnlyList<EnumDescription> enums,
      ICodeGenerator generator)
    {
      new CodeForm(generator, classes, enums, Program.Logger).Show();
    }

    public void AttachToProcess(string processName)
    {
      ReClassNET.Memory.ProcessInfo info = Program.CoreFunctions.EnumerateProcesses().FirstOrDefault<ReClassNET.Memory.ProcessInfo>((Func<ReClassNET.Memory.ProcessInfo, bool>) (p => string.Equals(p.Name, processName, StringComparison.OrdinalIgnoreCase)));
      if (info == null)
      {
        int num = (int) MessageBox.Show("Process '" + processName + "' could not be found.", "ReClass.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Program.Settings.LastProcess = string.Empty;
      }
      else
        this.AttachToProcess(info);
    }

    public void AttachToProcess(ReClassNET.Memory.ProcessInfo info)
    {
      Program.RemoteProcess.Close();
      Program.RemoteProcess.Open(info);
      Program.RemoteProcess.UpdateProcessInformations();
      Program.Settings.LastProcess = Program.RemoteProcess.UnderlayingProcess.Name;
    }

    public void SetProject(ReClassNetProject newProject)
    {
      if (this.currentProject == newProject)
        return;
      if (this.currentProject != null)
        ClassNode.ClassCreated -= new ClassCreatedEventHandler(this.currentProject.AddClass);
      this.currentProject = newProject;
      this.currentProject.ClassAdded += (ReClassNetProject.ClassesChangedEvent) (c =>
      {
        this.projectView.AddClass(c);
        // ISSUE: method pointer
        c.NodesChanged += new NodeEventHandler((object) this, __methodptr(\u003CSetProject\u003Eg__UpdateClassNodes\u007C5_0));
        // ISSUE: method pointer
        c.NameChanged += new NodeEventHandler((object) this, __methodptr(\u003CSetProject\u003Eg__UpdateClassNodes\u007C5_0));
      });
      this.currentProject.ClassRemoved += (ReClassNetProject.ClassesChangedEvent) (c =>
      {
        this.projectView.RemoveClass(c);
        // ISSUE: method pointer
        c.NodesChanged -= new NodeEventHandler((object) this, __methodptr(\u003CSetProject\u003Eg__UpdateClassNodes\u007C5_0));
        // ISSUE: method pointer
        c.NameChanged -= new NodeEventHandler((object) this, __methodptr(\u003CSetProject\u003Eg__UpdateClassNodes\u007C5_0));
      });
      this.currentProject.EnumAdded += (ReClassNetProject.EnumsChangedEvent) (e => this.projectView.AddEnum(e));
      ClassNode.ClassCreated += new ClassCreatedEventHandler(this.currentProject.AddClass);
      this.projectView.Clear();
      this.projectView.AddEnums((IEnumerable<EnumDescription>) this.currentProject.Enums);
      this.projectView.AddClasses((IEnumerable<ClassNode>) this.currentProject.Classes);
      this.CurrentClassNode = this.currentProject.Classes.FirstOrDefault<ClassNode>();
    }

    private void AskAddOrInsertBytes(string title, Action<int> callback)
    {
      ClassNode currentClassNode = this.CurrentClassNode;
      if (currentClassNode == null)
        return;
      InputBytesForm inputBytesForm1 = new InputBytesForm(currentClassNode.MemorySize);
      inputBytesForm1.Text = title;
      using (InputBytesForm inputBytesForm2 = inputBytesForm1)
      {
        if (inputBytesForm2.ShowDialog() != DialogResult.OK)
          return;
        callback(inputBytesForm2.Bytes);
      }
    }

    public void AddBytesToClass(int bytes)
    {
      BaseNode baseNode = this.memoryViewControl.GetSelectedNodes().Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (h => h.Node)).FirstOrDefault<BaseNode>();
      if (baseNode == null)
        return;
      if (!(baseNode is BaseContainerNode baseContainerNode))
        baseContainerNode = baseNode.GetParentContainer();
      baseContainerNode?.AddBytes(bytes);
      this.Invalidate();
    }

    public void InsertBytesInClass(int bytes)
    {
      BaseNode position = this.memoryViewControl.GetSelectedNodes().Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (h => h.Node)).FirstOrDefault<BaseNode>();
      if (position == null)
        return;
      if (!(position is BaseContainerNode baseContainerNode))
        baseContainerNode = position.GetParentContainer();
      baseContainerNode?.InsertBytes(position, bytes);
      this.Invalidate();
    }

    public void ClearSelection()
    {
      this.memoryViewControl.ClearSelection();
    }

    public static string ShowOpenProjectFileDialog()
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "All ReClass Types |*.rcnet;*.reclass;*.reclassqt|ReClass.NET File (*.rcnet)|*.rcnet|ReClass File (*.reclass)|*.reclass|ReClassQt File (*.reclassqt)|*.reclassqt";
      using (OpenFileDialog openFileDialog2 = openFileDialog1)
        return openFileDialog2.ShowDialog() == DialogResult.OK ? openFileDialog2.FileName : (string) null;
    }

    public void LoadProjectFromPath(string path)
    {
      ReClassNetProject project = new ReClassNetProject();
      MainForm.LoadProjectFromPath(path, ref project);
      if (Path.GetExtension(path) == ".rcnet")
        project.Path = path;
      this.SetProject(project);
    }

    private static void LoadProjectFromPath(string path, ref ReClassNetProject project)
    {
      string lower = Path.GetExtension(path)?.ToLower();
      IReClassImport reClassImport;
      if (!(lower == ".rcnet"))
      {
        if (!(lower == ".reclassqt"))
        {
          if (lower == ".reclass")
          {
            reClassImport = (IReClassImport) new ReClassFile(project);
          }
          else
          {
            Program.Logger.Log(ReClassNET.Logger.LogLevel.Error, "The file '" + path + "' has an unknown type.");
            return;
          }
        }
        else
          reClassImport = (IReClassImport) new ReClassQtFile(project);
      }
      else
        reClassImport = (IReClassImport) new ReClassNetFile(project);
      reClassImport.Load(path, Program.Logger);
    }

    private void LoadAllSymbolsForCurrentProcess()
    {
      if (this.loadSymbolsTask != null && !this.loadSymbolsTask.IsCompleted)
        return;
      Progress<Tuple<Module, IReadOnlyList<Module>>> progress = new Progress<Tuple<Module, IReadOnlyList<Module>>>((Action<Tuple<Module, IReadOnlyList<Module>>>) (report => {}));
      this.loadSymbolsTaskToken = new CancellationTokenSource();
    }

    public void ReplaceSelectedNodesWithType(Type type)
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      List<MemoryViewControl.SelectedNodeInfo> selectedNodeInfoList = new List<MemoryViewControl.SelectedNodeInfo>(selectedNodes.Count);
      foreach (var data in selectedNodes.WhereNot<MemoryViewControl.SelectedNodeInfo>((Func<MemoryViewControl.SelectedNodeInfo, bool>) (s => s.Node is ClassNode)).GroupBy<MemoryViewControl.SelectedNodeInfo, BaseContainerNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseContainerNode>) (s => s.Node.GetParentContainer())).Select(g => new
      {
        Container = g.Key,
        Partitions = g.OrderBy<MemoryViewControl.SelectedNodeInfo, int>((Func<MemoryViewControl.SelectedNodeInfo, int>) (s => s.Node.Offset)).GroupWhile<MemoryViewControl.SelectedNodeInfo>((Func<MemoryViewControl.SelectedNodeInfo, MemoryViewControl.SelectedNodeInfo, bool>) ((s1, s2) => s1.Node.Offset + s1.Node.MemorySize == s2.Node.Offset))
      }))
      {
        data.Container.BeginUpdate();
        foreach (IEnumerable<MemoryViewControl.SelectedNodeInfo> partition in data.Partitions)
        {
          Queue<MemoryViewControl.SelectedNodeInfo> selectedNodeInfoQueue = new Queue<MemoryViewControl.SelectedNodeInfo>(partition);
          while (selectedNodeInfoQueue.Count > 0)
          {
            MemoryViewControl.SelectedNodeInfo selectedNodeInfo1 = selectedNodeInfoQueue.Dequeue();
            BaseNode instanceFromType = BaseNode.CreateInstanceFromType(type);
            List<BaseNode> additionalCreatedNodes = new List<BaseNode>();
            data.Container.ReplaceChildNode(selectedNodeInfo1.Node, instanceFromType, ref additionalCreatedNodes);
            instanceFromType.IsSelected = true;
            MemoryViewControl.SelectedNodeInfo selectedNodeInfo2 = new MemoryViewControl.SelectedNodeInfo(instanceFromType, selectedNodeInfo1.Process, selectedNodeInfo1.Memory, selectedNodeInfo1.Address, selectedNodeInfo1.Level);
            selectedNodeInfoList.Add(selectedNodeInfo2);
            if (selectedNodes.Count > 1)
            {
              foreach (BaseNode node in additionalCreatedNodes)
                selectedNodeInfoQueue.Enqueue(new MemoryViewControl.SelectedNodeInfo(node, selectedNodeInfo1.Process, selectedNodeInfo1.Memory, selectedNodeInfo1.Address + node.Offset - instanceFromType.Offset, selectedNodeInfo1.Level));
            }
          }
        }
        data.Container.EndUpdate();
      }
      this.memoryViewControl.ClearSelection();
      if (selectedNodeInfoList.Count <= 0)
        return;
      this.memoryViewControl.SetSelectedNodes((IEnumerable<MemoryViewControl.SelectedNodeInfo>) selectedNodeInfoList);
    }

    private void FindWhatInteractsWithSelectedNode(bool writeOnly)
    {
      MemoryViewControl.SelectedNodeInfo selectedNodeInfo = this.memoryViewControl.GetSelectedNodes().FirstOrDefault<MemoryViewControl.SelectedNodeInfo>();
      if (selectedNodeInfo == null)
        return;
      LinkedWindowFeatures.FindWhatInteractsWithAddress(selectedNodeInfo.Address, selectedNodeInfo.Node.MemorySize, writeOnly);
    }

    private void CopySelectedNodesToClipboard()
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count <= 0)
        return;
      ReClassClipboard.Copy(selectedNodes.Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (h => h.Node)), Program.Logger);
    }

    private void PasteNodeFromClipboardToSelection()
    {
      List<ClassNode> classNodeList1;
      List<BaseNode> baseNodeList1;
      ReClassClipboard.Paste(this.CurrentProject, Program.Logger).Deconstruct<List<ClassNode>, List<BaseNode>>(out classNodeList1, out baseNodeList1);
      List<ClassNode> classNodeList2 = classNodeList1;
      List<BaseNode> baseNodeList2 = baseNodeList1;
      foreach (ClassNode node in classNodeList2)
      {
        if (!this.CurrentProject.ContainsClass(node.Uuid))
          this.CurrentProject.AddClass(node);
      }
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count != 1)
        return;
      BaseNode node1 = selectedNodes[0].Node;
      BaseContainerNode parentContainer = node1.GetParentContainer();
      ClassNode parentClass = node1.GetParentClass();
      if (parentContainer == null || parentClass == null)
        return;
      parentContainer.BeginUpdate();
      foreach (BaseNode node2 in baseNodeList2)
      {
        if (node2 is BaseWrapperNode)
        {
          BaseWrapperNode rootWrapperNode = node2.GetRootWrapperNode();
          if (rootWrapperNode.ShouldPerformCycleCheckForInnerNode() && rootWrapperNode.ResolveMostInnerNode() is ClassNode node2 && !this.IsCycleFree(parentClass, node2))
            continue;
        }
        parentContainer.InsertNode(node1, node2);
      }
      parentContainer.EndUpdate();
    }

    private void EditSelectedNodeName()
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count != 1)
        return;
      this.memoryViewControl.ShowNodeNameEditBox(selectedNodes[0].Node);
    }

    private void RemoveSelectedNodes()
    {
      this.memoryViewControl.GetSelectedNodes().WhereNot<MemoryViewControl.SelectedNodeInfo>((Func<MemoryViewControl.SelectedNodeInfo, bool>) (h => h.Node is ClassNode)).ForEach<MemoryViewControl.SelectedNodeInfo>((Action<MemoryViewControl.SelectedNodeInfo>) (h => h.Node.GetParentContainer().RemoveNode(h.Node)));
      this.ClearSelection();
    }

    private void HideSelectedNodes()
    {
      foreach (MemoryViewControl.SelectedNodeInfo selectedNode in (IEnumerable<MemoryViewControl.SelectedNodeInfo>) this.memoryViewControl.GetSelectedNodes())
        selectedNode.Node.IsHidden = true;
      this.ClearSelection();
    }

    private void UnhideChildNodes()
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count != 1 || !(selectedNodes[0].Node is BaseContainerNode node))
        return;
      foreach (BaseNode node in (IEnumerable<BaseNode>) node.Nodes)
      {
        node.IsHidden = false;
        node.IsSelected = false;
      }
      node.IsSelected = false;
      this.ClearSelection();
    }

    private void UnhideNodesBelow()
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count != 1)
        return;
      BaseNode node1 = selectedNodes[0].Node;
      BaseContainerNode parentContainer = node1.GetParentContainer();
      if (parentContainer == null)
        return;
      int num = parentContainer.FindNodeIndex(node1) + 1;
      if (num >= parentContainer.Nodes.Count)
        return;
      for (int index = num; index < parentContainer.Nodes.Count; ++index)
      {
        BaseNode node2 = parentContainer.Nodes[index];
        if (node2.IsHidden)
        {
          node2.IsHidden = false;
          node2.IsSelected = false;
        }
        else
          break;
      }
      node1.IsSelected = false;
      this.ClearSelection();
    }

    private void UnhideNodesAbove()
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count != 1)
        return;
      BaseNode node1 = selectedNodes[0].Node;
      BaseContainerNode parentContainer = node1.GetParentContainer();
      if (parentContainer == null)
        return;
      int num = parentContainer.FindNodeIndex(node1) - 1;
      if (num < 0)
        return;
      for (int index = num; index > -1; --index)
      {
        BaseNode node2 = parentContainer.Nodes[index];
        if (node2.IsHidden)
        {
          node2.IsHidden = false;
          node2.IsSelected = false;
        }
        else
          break;
      }
      node1.IsSelected = false;
      this.ClearSelection();
    }

    private bool IsCycleFree(ClassNode parent, ClassNode node)
    {
      if (!ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, node, (IEnumerable<ClassNode>) this.CurrentProject.Classes))
        return true;
      int num = (int) MessageBox.Show("Invalid operation because this would create a class cycle.", "Cycle Detected", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    public ReClassNetProject CurrentProject
    {
      get
      {
        return this.currentProject;
      }
    }

    public ProjectView ProjectView
    {
      get
      {
        return this.projectView;
      }
    }

    public MenuStrip MainMenu
    {
      get
      {
        return this.mainMenuStrip;
      }
    }

    public ClassNode CurrentClassNode
    {
      get
      {
        return this.currentClassNode;
      }
      set
      {
        this.currentClassNode = value;
        this.projectView.SelectedClass = value;
        this.memoryViewControl.Reset();
        this.memoryViewControl.Invalidate();
      }
    }

    private void UpdateWindowTitle(string extra = null)
    {
      string str = !MainForm.PS4.IsConnected || !(this.toolStripComboBox1.Text != "Select Process") ? (Program.Settings.RandomizeWindowTitle ? Utils.RandomString(Program.GlobalRandom.Next(15, 20)) : "ReClass.NET") + " (x64)" : (Program.Settings.RandomizeWindowTitle ? Utils.RandomString(Program.GlobalRandom.Next(15, 20)) : "ReClass.NET") + " (x64) Process: " + this.toolStripComboBox1.SelectedItem.ToString();
      if (!string.IsNullOrEmpty(extra))
        str = str + " - " + extra;
      this.Text = str;
    }

    public MainForm()
    {
      this.InitializeComponent();
      this.toolStripTextBox1.Text = this.cfg.IniReadValue(nameof (PS4), "IP");
      MainForm.PS4 = new PS4DBG(this.toolStripTextBox1.Text);
      this.UpdateWindowTitle((string) null);
      this.mainMenuStrip.Renderer = (ToolStripRenderer) new CustomToolStripProfessionalRenderer(true, true);
      this.toolStrip.Renderer = (ToolStripRenderer) new CustomToolStripProfessionalRenderer(true, false);
      Program.RemoteProcess.ProcessAttached += (RemoteProcessEvent) (sender => this.UpdateWindowTitle(sender.UnderlayingProcess.Name + " (ID: " + sender.UnderlayingProcess.Id.ToString() + ")"));
      Program.RemoteProcess.ProcessClosed += (RemoteProcessEvent) (sender => this.UpdateWindowTitle((string) null));
      this.pluginManager = new PluginManager((IPluginHost) new DefaultPluginHost(this, Program.RemoteProcess, Program.Logger));
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      GlobalWindowManager.AddWindow((Form) this);
      this.toolStrip.Items.AddRange(NodeTypesBuilder.CreateToolStripButtons(new Action<Type>(this.ReplaceSelectedNodesWithType)).ToArray<ToolStripItem>());
      this.changeTypeToolStripMenuItem.DropDownItems.AddRange(NodeTypesBuilder.CreateToolStripMenuItems(new Action<Type>(this.ReplaceSelectedNodesWithType), false).ToArray<ToolStripItem>());
      bool flag = true;
      if (Program.CommandLineArgs.FileName != null)
      {
        try
        {
          this.LoadProjectFromPath(Program.CommandLineArgs.FileName);
          flag = false;
        }
        catch (Exception ex)
        {
          Program.Logger.Log(ex);
        }
      }
      if (flag)
      {
        this.SetProject(new ReClassNetProject());
        LinkedWindowFeatures.CreateDefaultClass();
      }
      if (Program.CommandLineArgs["attachto"] == null)
        return;
      this.AttachToProcess(Program.CommandLineArgs["attachto"]);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      this.pluginManager.UnloadAllPlugins();
      GlobalWindowManager.RemoveWindow((Form) this);
      base.OnFormClosed(e);
    }

    private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      MainForm mainForm = this;
      mainForm.processUpdateTimer.Stop();
      mainForm.cfg.IniWriteValue("PS4", "IP", mainForm.toolStripTextBox1.Text);
      if (mainForm.loadSymbolsTask == null && mainForm.updateProcessInformationsTask == null)
        return;
      e.Cancel = true;
      mainForm.Hide();
      if (mainForm.loadSymbolsTask != null)
      {
        mainForm.loadSymbolsTaskToken.Cancel();
        try
        {
          await mainForm.loadSymbolsTask;
        }
        catch
        {
        }
        mainForm.loadSymbolsTask = (Task) null;
      }
      if (mainForm.updateProcessInformationsTask != null)
      {
        try
        {
          await mainForm.updateProcessInformationsTask;
        }
        catch
        {
        }
        mainForm.updateProcessInformationsTask = (Task) null;
      }
      mainForm.Close();
    }

    private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
    {
      string.IsNullOrEmpty(Program.Settings.LastProcess);
    }

    private void reattachToProcessToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string lastProcess = Program.Settings.LastProcess;
      if (string.IsNullOrEmpty(lastProcess))
        return;
      this.AttachToProcess(lastProcess);
    }

    private void detachToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Program.RemoteProcess.Close();
    }

    private void newClassToolStripButton_Click(object sender, EventArgs e)
    {
      LinkedWindowFeatures.CreateDefaultClass();
    }

    private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        string path = MainForm.ShowOpenProjectFileDialog();
        if (path == null)
          return;
        this.LoadProjectFromPath(path);
      }
      catch (Exception ex)
      {
        Program.Logger.Log(ex);
      }
    }

    private void mergeWithProjectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        string path = MainForm.ShowOpenProjectFileDialog();
        if (path == null)
          return;
        MainForm.LoadProjectFromPath(path, ref this.currentProject);
      }
      catch (Exception ex)
      {
        Program.Logger.Log(ex);
      }
    }

    private void goToClassToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (ClassSelectionForm classSelectionForm = new ClassSelectionForm((IEnumerable<ClassNode>) this.currentProject.Classes.OrderBy<ClassNode, string>((Func<ClassNode, string>) (c => c.Name))))
      {
        if (classSelectionForm.ShowDialog() != DialogResult.OK)
          return;
        ClassNode selectedClass = classSelectionForm.SelectedClass;
        if (selectedClass == null)
          return;
        this.projectView.SelectedClass = selectedClass;
      }
    }

    private void clearProjectToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.SetProject(new ReClassNetProject());
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.currentProject.Classes.Any<ClassNode>())
        return;
      if (string.IsNullOrEmpty(this.currentProject.Path))
        this.saveAsToolStripMenuItem_Click(sender, e);
      else
        new ReClassNetFile(this.currentProject).Save(this.currentProject.Path, Program.Logger);
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!this.currentProject.Classes.Any<ClassNode>())
        return;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.DefaultExt = ".rcnet";
      saveFileDialog1.Filter = "ReClass.NET File (*.rcnet)|*.rcnet";
      using (SaveFileDialog saveFileDialog2 = saveFileDialog1)
      {
        if (saveFileDialog2.ShowDialog() != DialogResult.OK)
          return;
        this.currentProject.Path = saveFileDialog2.FileName;
        this.saveToolStripMenuItem_Click(sender, e);
      }
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (SettingsForm settingsForm = new SettingsForm(Program.Settings, this.CurrentProject.TypeMapping))
      {
        int num = (int) settingsForm.ShowDialog();
      }
    }

    private void pluginsToolStripButton_Click(object sender, EventArgs e)
    {
      using (PluginForm pluginForm = new PluginForm(this.pluginManager))
      {
        int num = (int) pluginForm.ShowDialog();
      }
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void memoryViewerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new ProcessInfoForm((IProcessReader) Program.RemoteProcess).Show();
    }

    private void memorySearcherToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new ScannerForm(Program.RemoteProcess).Show();
    }

    private void namedAddressesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new NamedAddressesForm(Program.RemoteProcess).Show();
    }

    private void isLittleEndianToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void loadSymbolToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "Program Debug Database (*.pdb)|*.pdb|All Files (*.*)|*.*";
      using (OpenFileDialog openFileDialog2 = openFileDialog1)
      {
        if (openFileDialog2.ShowDialog() != DialogResult.OK)
          return;
        try
        {
          Program.RemoteProcess.Symbols.LoadSymbolsFromPDB(openFileDialog2.FileName);
        }
        catch (Exception ex)
        {
          Program.Logger.Log(ex);
        }
      }
    }

    private void loadSymbolsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.LoadAllSymbolsForCurrentProcess();
    }

    private void ControlRemoteProcessToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!Program.RemoteProcess.IsValid)
        return;
      Program.RemoteProcess.ControlRemoteProcess(ControlRemoteProcessAction.Terminate);
    }

    private void cleanUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.currentProject.RemoveUnusedClasses();
    }

    private void generateCppCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ShowCodeGeneratorForm((ICodeGenerator) new CppCodeGenerator(this.currentProject.TypeMapping));
    }

    private void generateCSharpCodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.ShowCodeGeneratorForm((ICodeGenerator) new CSharpCodeGenerator());
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (AboutForm aboutForm = new AboutForm())
      {
        int num = (int) aboutForm.ShowDialog();
      }
    }

    private void attachToProcessToolStripSplitButton_ButtonClick(object sender, EventArgs e)
    {
      using (ProcessBrowserForm processBrowserForm = new ProcessBrowserForm(Program.Settings.LastProcess))
      {
        if (processBrowserForm.ShowDialog() != DialogResult.OK || processBrowserForm.SelectedProcess == null)
          return;
        this.AttachToProcess(processBrowserForm.SelectedProcess);
        if (!processBrowserForm.LoadSymbols)
          return;
        this.LoadAllSymbolsForCurrentProcess();
      }
    }

    private void attachToProcessToolStripSplitButton_DropDownClosed(object sender, EventArgs e)
    {
    }

    private void attachToProcessToolStripSplitButton_DropDownOpening(object sender, EventArgs e)
    {
    }

    private void selectedNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      int count = selectedNodes.Count;
      BaseNode node = selectedNodes.Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (s => s.Node)).FirstOrDefault<BaseNode>();
      BaseContainerNode parentContainer = node?.GetParentContainer();
      bool flag1 = node is ClassNode;
      bool flag2;
      switch (node)
      {
        case BaseHexNode _:
          flag2 = true;
          break;
        case FloatNode _:
          flag2 = true;
          break;
        case DoubleNode _:
          flag2 = true;
          break;
        case Int8Node _:
          flag2 = true;
          break;
        case UInt8Node _:
          flag2 = true;
          break;
        case Int16Node _:
          flag2 = true;
          break;
        case UInt16Node _:
          flag2 = true;
          break;
        case Int32Node _:
          flag2 = true;
          break;
        case UInt32Node _:
          flag2 = true;
          break;
        case Int64Node _:
          flag2 = true;
          break;
        case UInt64Node _:
          flag2 = true;
          break;
        case NIntNode _:
          flag2 = true;
          break;
        case NUIntNode _:
          flag2 = true;
          break;
        case Utf8TextNode _:
          flag2 = true;
          break;
        case Utf16TextNode _:
          flag2 = true;
          break;
        case Utf32TextNode _:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      this.addBytesToolStripMenuItem.Enabled = parentContainer != null | flag1;
      this.insertBytesToolStripMenuItem.Enabled = count == 1 && parentContainer != null;
      this.changeTypeToolStripMenuItem.Enabled = count > 0 && !flag1;
      this.createClassFromNodesToolStripMenuItem.Enabled = count > 0 && !flag1;
      this.dissectNodesToolStripMenuItem.Enabled = count > 0 && !flag1;
      this.pasteNodesToolStripMenuItem.Enabled = count == 1 && ReClassClipboard.ContainsNodes;
      this.removeToolStripMenuItem.Enabled = !flag1;
      this.copyAddressToolStripMenuItem.Enabled = !flag1;
      this.showCodeOfClassToolStripMenuItem.Enabled = flag1;
      this.shrinkClassToolStripMenuItem.Enabled = flag1;
      this.hideNodesToolStripMenuItem.Enabled = selectedNodes.All<MemoryViewControl.SelectedNodeInfo>((Func<MemoryViewControl.SelectedNodeInfo, bool>) (h => !(h.Node is ClassNode)));
      this.unhideChildNodesToolStripMenuItem.Enabled = count == 1 && node is BaseContainerNode baseContainerNode && baseContainerNode.Nodes.Any<BaseNode>((Func<BaseNode, bool>) (n => n.IsHidden));
      BaseNode predecessor;
      this.unhideNodesAboveToolStripMenuItem.Enabled = count == 1 && parentContainer != null && parentContainer.TryGetPredecessor(node, out predecessor) && predecessor.IsHidden;
      BaseNode successor;
      this.unhideNodesBelowToolStripMenuItem.Enabled = count == 1 && parentContainer != null && parentContainer.TryGetSuccessor(node, out successor) && successor.IsHidden;
    }

    private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(sender is IntegerToolStripMenuItem toolStripMenuItem))
        return;
      this.AddBytesToClass(toolStripMenuItem.Value);
    }

    private void addXBytesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.AskAddOrInsertBytes("Add Bytes", new Action<int>(this.AddBytesToClass));
    }

    private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(sender is IntegerToolStripMenuItem toolStripMenuItem))
        return;
      this.InsertBytesInClass(toolStripMenuItem.Value);
    }

    private void insertXBytesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.AskAddOrInsertBytes("Insert Bytes", new Action<int>(this.InsertBytesInClass));
    }

    private void createClassFromNodesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count <= 0 || selectedNodes[0].Node is ClassNode)
        return;
      ClassNode parentNode = selectedNodes[0].Node.GetParentContainer() as ClassNode;
      if (parentNode == null)
        return;
      ClassNode classNode = ClassNode.Create();
      selectedNodes.Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (h => h.Node)).ForEach<BaseNode>(new Action<BaseNode>(((BaseContainerNode) classNode).AddNode));
      ClassInstanceNode classInstanceNode = new ClassInstanceNode();
      classInstanceNode.ChangeInnerNode((BaseNode) classNode);
      parentNode.InsertNode(selectedNodes[0].Node, (BaseNode) classInstanceNode);
      selectedNodes.Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (h => h.Node)).ForEach<BaseNode>((Action<BaseNode>) (c => parentNode.RemoveNode(c)));
      this.ClearSelection();
    }

    private void dissectNodesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      List<MemoryViewControl.SelectedNodeInfo> list = this.memoryViewControl.GetSelectedNodes().Where<MemoryViewControl.SelectedNodeInfo>((Func<MemoryViewControl.SelectedNodeInfo, bool>) (h => h.Node is BaseHexNode)).ToList<MemoryViewControl.SelectedNodeInfo>();
      if (!list.Any<MemoryViewControl.SelectedNodeInfo>())
        return;
      foreach (IGrouping<BaseContainerNode, MemoryViewControl.SelectedNodeInfo> source in list.GroupBy<MemoryViewControl.SelectedNodeInfo, BaseContainerNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseContainerNode>) (n => n.Node.GetParentContainer())))
        NodeDissector.DissectNodes(source.Select<MemoryViewControl.SelectedNodeInfo, BaseHexNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseHexNode>) (h => (BaseHexNode) h.Node)), (IProcessReader) Program.RemoteProcess, source.First<MemoryViewControl.SelectedNodeInfo>().Memory);
      this.ClearSelection();
    }

    private void searchForEqualValuesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MemoryViewControl.SelectedNodeInfo selectedNodeInfo = this.memoryViewControl.GetSelectedNodes().FirstOrDefault<MemoryViewControl.SelectedNodeInfo>();
      if (selectedNodeInfo == null)
        return;
      EndianBitConverter bitConverter = Program.RemoteProcess.BitConverter;
      IScanComparer comparer;
      switch (selectedNodeInfo.Node)
      {
        case BaseHexNode baseHexNode:
          comparer = (IScanComparer) new ArrayOfBytesMemoryComparer(baseHexNode.ReadValueFromMemory(selectedNodeInfo.Memory));
          break;
        case FloatNode floatNode:
          comparer = (IScanComparer) new FloatMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, ScanRoundMode.Normal, 2, floatNode.ReadValueFromMemory(selectedNodeInfo.Memory), 0.0f, bitConverter);
          break;
        case DoubleNode doubleNode:
          comparer = (IScanComparer) new DoubleMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, ScanRoundMode.Normal, 2, doubleNode.ReadValueFromMemory(selectedNodeInfo.Memory), 0.0, bitConverter);
          break;
        case Int8Node int8Node:
          comparer = (IScanComparer) new ByteMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, (byte) int8Node.ReadValueFromMemory(selectedNodeInfo.Memory), (byte) 0);
          break;
        case UInt8Node uint8Node:
          comparer = (IScanComparer) new ByteMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, uint8Node.ReadValueFromMemory(selectedNodeInfo.Memory), (byte) 0);
          break;
        case Int16Node int16Node:
          comparer = (IScanComparer) new ShortMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, int16Node.ReadValueFromMemory(selectedNodeInfo.Memory), (short) 0, bitConverter);
          break;
        case UInt16Node uint16Node:
          comparer = (IScanComparer) new ShortMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, (short) uint16Node.ReadValueFromMemory(selectedNodeInfo.Memory), (short) 0, bitConverter);
          break;
        case Int32Node int32Node:
          comparer = (IScanComparer) new IntegerMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, int32Node.ReadValueFromMemory(selectedNodeInfo.Memory), 0, bitConverter);
          break;
        case UInt32Node uint32Node:
          comparer = (IScanComparer) new IntegerMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, (int) uint32Node.ReadValueFromMemory(selectedNodeInfo.Memory), 0, bitConverter);
          break;
        case Int64Node int64Node:
          comparer = (IScanComparer) new LongMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, int64Node.ReadValueFromMemory(selectedNodeInfo.Memory), 0L, bitConverter);
          break;
        case UInt64Node uint64Node:
          comparer = (IScanComparer) new LongMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, (long) uint64Node.ReadValueFromMemory(selectedNodeInfo.Memory), 0L, bitConverter);
          break;
        case NIntNode nintNode:
          comparer = (IScanComparer) new LongMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, nintNode.ReadValueFromMemory(selectedNodeInfo.Memory).ToInt64(), 0L, bitConverter);
          break;
        case NUIntNode nuIntNode:
          comparer = (IScanComparer) new LongMemoryComparer(ReClassNET.MemoryScanner.ScanCompareType.Equal, (long) nuIntNode.ReadValueFromMemory(selectedNodeInfo.Memory).ToUInt64(), 0L, bitConverter);
          break;
        case Utf8TextNode utf8TextNode:
          comparer = (IScanComparer) new StringMemoryComparer(utf8TextNode.ReadValueFromMemory(selectedNodeInfo.Memory), Encoding.UTF8, true);
          break;
        case Utf16TextNode utf16TextNode:
          comparer = (IScanComparer) new StringMemoryComparer(utf16TextNode.ReadValueFromMemory(selectedNodeInfo.Memory), Encoding.Unicode, true);
          break;
        case Utf32TextNode utf32TextNode:
          comparer = (IScanComparer) new StringMemoryComparer(utf32TextNode.ReadValueFromMemory(selectedNodeInfo.Memory), Encoding.UTF32, true);
          break;
        default:
          return;
      }
      LinkedWindowFeatures.StartMemoryScan(comparer);
    }

    private void findOutWhatAccessesThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.FindWhatInteractsWithSelectedNode(false);
    }

    private void findOutWhatWritesToThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.FindWhatInteractsWithSelectedNode(true);
    }

    private void copyNodeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.CopySelectedNodesToClipboard();
    }

    private void pasteNodesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.PasteNodeFromClipboardToSelection();
    }

    private void removeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.RemoveSelectedNodes();
    }

    private void hideNodesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.HideSelectedNodes();
    }

    private void unhideChildNodesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.UnhideChildNodes();
    }

    private void unhideNodesAboveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.UnhideNodesAbove();
    }

    private void unhideNodesBelowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.UnhideNodesBelow();
    }

    private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = this.memoryViewControl.GetSelectedNodes();
      if (selectedNodes.Count <= 0)
        return;
      Clipboard.SetText(selectedNodes.First<MemoryViewControl.SelectedNodeInfo>().Address.ToString("X"));
    }

    private void showCodeOfClassToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(this.memoryViewControl.GetSelectedNodes().FirstOrDefault<MemoryViewControl.SelectedNodeInfo>()?.Node is ClassNode node))
        return;
      this.ShowPartialCodeGeneratorForm((IReadOnlyList<ClassNode>) new ClassNode[1]
      {
        node
      });
    }

    private void shrinkClassToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(this.memoryViewControl.GetSelectedNodes().Select<MemoryViewControl.SelectedNodeInfo, BaseNode>((Func<MemoryViewControl.SelectedNodeInfo, BaseNode>) (s => s.Node)).FirstOrDefault<BaseNode>() is ClassNode classNode))
        return;
      foreach (BaseNode node in classNode.Nodes.Reverse<BaseNode>().TakeWhile<BaseNode>((Func<BaseNode, bool>) (n => n is BaseHexNode)))
        classNode.RemoveNode(node);
    }

    private void MainForm_DragEnter(object sender, DragEventArgs e)
    {
      if (!e.Data.GetDataPresent(DataFormats.FileDrop) || !(e.Data.GetData(DataFormats.FileDrop) is string[] data) || !((IEnumerable<string>) data).Any<string>())
        return;
      string extension = Path.GetExtension(((IEnumerable<string>) data).First<string>());
      if (!(extension == ".rcnet") && !(extension == ".reclassqt") && !(extension == ".reclass"))
        return;
      e.Effect = DragDropEffects.Copy;
    }

    private void MainForm_DragDrop(object sender, DragEventArgs e)
    {
      if (!(e.Data.GetData(DataFormats.FileDrop) is string[] data))
        return;
      if (!((IEnumerable<string>) data).Any<string>())
        return;
      try
      {
        this.LoadProjectFromPath(((IEnumerable<string>) data).First<string>());
      }
      catch (Exception ex)
      {
        Program.Logger.Log(ex);
      }
    }

    private void processUpdateTimer_Tick(object sender, EventArgs e)
    {
      if (this.updateProcessInformationsTask != null && !this.updateProcessInformationsTask.IsCompleted)
        return;
      this.updateProcessInformationsTask = Program.RemoteProcess.UpdateProcessInformationsAsync();
    }

    private void classesView_ClassSelected(object sender, ClassNode node)
    {
      this.CurrentClassNode = node;
    }

    private void memoryViewControl_KeyDown(object sender, KeyEventArgs args)
    {
      switch (args.KeyCode)
      {
        case Keys.Delete:
          this.RemoveSelectedNodes();
          break;
        case Keys.C:
          if (!args.Control)
            break;
          this.CopySelectedNodesToClipboard();
          break;
        case Keys.V:
          if (!args.Control)
            break;
          this.PasteNodeFromClipboardToSelection();
          break;
        case Keys.F2:
          this.EditSelectedNodeName();
          break;
      }
    }

    private void memoryViewControl_SelectionChanged(object sender, EventArgs e)
    {
      if (!(sender is MemoryViewControl memoryViewControl))
        return;
      IReadOnlyList<MemoryViewControl.SelectedNodeInfo> selectedNodes = memoryViewControl.GetSelectedNodes();
      BaseNode node = selectedNodes.FirstOrDefault<MemoryViewControl.SelectedNodeInfo>()?.Node;
      BaseContainerNode parentContainer = node?.GetParentContainer();
      this.addBytesToolStripDropDownButton.Enabled = parentContainer != null || node is ClassNode;
      this.insertBytesToolStripDropDownButton.Enabled = selectedNodes.Count == 1 && parentContainer != null;
      bool enabled = selectedNodes.Count > 0 && !(node is ClassNode);
      this.toolStrip.Items.OfType<TypeToolStripButton>().ForEach<TypeToolStripButton>((Action<TypeToolStripButton>) (b => b.Enabled = enabled));
    }

    private void memoryViewControl_ChangeClassTypeClick(object sender, NodeClickEventArgs e)
    {
      IOrderedEnumerable<ClassNode> source = this.CurrentProject.Classes.OrderBy<ClassNode, string>((Func<ClassNode, string>) (c => c.Name));
      if (e.Node is FunctionNode node)
      {
        ClassNode classNode1 = new ClassNode(false);
        classNode1.Name = "None";
        ClassNode element = classNode1;
        using (ClassSelectionForm classSelectionForm = new ClassSelectionForm(source.Prepend<ClassNode>(element)))
        {
          if (classSelectionForm.ShowDialog() != DialogResult.OK)
            return;
          ClassNode classNode2 = classSelectionForm.SelectedClass;
          if (classNode2 == null)
            return;
          if (classNode2 == element)
            classNode2 = (ClassNode) null;
          node.BelongsToClass = classNode2;
        }
      }
      else
      {
        if (!(e.Node is BaseWrapperNode node))
          return;
        using (ClassSelectionForm classSelectionForm = new ClassSelectionForm((IEnumerable<ClassNode>) source))
        {
          if (classSelectionForm.ShowDialog() != DialogResult.OK)
            return;
          ClassNode selectedClass = classSelectionForm.SelectedClass;
          if (!node.CanChangeInnerNodeTo((BaseNode) selectedClass) || node.GetRootWrapperNode().ShouldPerformCycleCheckForInnerNode() && !this.IsCycleFree(e.Node.GetParentClass(), selectedClass))
            return;
          node.ChangeInnerNode((BaseNode) selectedClass);
        }
      }
    }

    private void memoryViewControl_ChangeWrappedTypeClick(object sender, NodeClickEventArgs e)
    {
      BaseWrapperNode wrapperNode = e.Node as BaseWrapperNode;
      if (wrapperNode == null)
        return;
      IEnumerable<ToolStripItem> toolStripMenuItems = NodeTypesBuilder.CreateToolStripMenuItems((Action<Type>) (t =>
      {
        BaseNode instanceFromType = BaseNode.CreateInstanceFromType(t);
        if (!wrapperNode.CanChangeInnerNodeTo(instanceFromType))
          return;
        wrapperNode.ChangeInnerNode(instanceFromType);
      }), wrapperNode.CanChangeInnerNodeTo((BaseNode) null));
      ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
      contextMenuStrip.Items.AddRange(toolStripMenuItems.ToArray<ToolStripItem>());
      contextMenuStrip.Show((Control) this, e.Location);
    }

    private void memoryViewControl_ChangeEnumTypeClick(object sender, NodeClickEventArgs e)
    {
      if (!(e.Node is EnumNode node))
        return;
      using (EnumSelectionForm enumSelectionForm = new EnumSelectionForm(this.CurrentProject))
      {
        int size1 = (int) node.Enum.Size;
        if (enumSelectionForm.ShowDialog() == DialogResult.OK)
        {
          EnumDescription selectedItem = enumSelectionForm.SelectedItem;
          if (selectedItem != null)
            node.ChangeEnum(selectedItem);
        }
        int size2 = (int) node.Enum.Size;
        if (size1 != size2)
          node.GetParentContainer()?.ChildHasChanged((BaseNode) node);
        foreach (EnumDescription @enum in (IEnumerable<EnumDescription>) this.CurrentProject.Enums)
          this.projectView.UpdateEnumNode(@enum);
      }
    }

    private void showCodeOfClassToolStripMenuItem2_Click(object sender, EventArgs e)
    {
      ClassNode selectedClass = this.projectView.SelectedClass;
      if (selectedClass == null)
        return;
      this.ShowPartialCodeGeneratorForm((IReadOnlyList<ClassNode>) new ClassNode[1]
      {
        selectedClass
      });
    }

    private void enableHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool flag = !this.enableHierarchyViewToolStripMenuItem.Checked;
      this.enableHierarchyViewToolStripMenuItem.Checked = flag;
      this.expandAllClassesToolStripMenuItem.Enabled = this.collapseAllClassesToolStripMenuItem.Enabled = flag;
      this.projectView.EnableClassHierarchyView = flag;
    }

    private void autoExpandHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      bool flag = !this.autoExpandHierarchyViewToolStripMenuItem.Checked;
      this.autoExpandHierarchyViewToolStripMenuItem.Checked = flag;
      this.projectView.AutoExpandClassNodes = flag;
    }

    private void expandAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.projectView.ExpandAllClassNodes();
    }

    private void collapseAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.projectView.CollapseAllClassNodes();
    }

    private void removeUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      this.CurrentProject.RemoveUnusedClasses();
    }

    private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ClassNode selectedClass = this.projectView.SelectedClass;
      if (selectedClass == null)
        return;
      try
      {
        this.CurrentProject.Remove(selectedClass);
      }
      catch (ClassReferencedException ex)
      {
        Program.Logger.Log((Exception) ex);
      }
    }

    private void editEnumsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (EnumListForm enumListForm = new EnumListForm(this.currentProject))
      {
        int num = (int) enumListForm.ShowDialog();
      }
    }

    private void editEnumToolStripMenuItem_Click(object sender, EventArgs e)
    {
      EnumDescription selectedEnum = this.projectView.SelectedEnum;
      if (selectedEnum == null)
        return;
      using (EnumEditorForm enumEditorForm = new EnumEditorForm(selectedEnum))
      {
        int num = (int) enumEditorForm.ShowDialog();
      }
    }

    private void showEnumsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (EnumListForm enumListForm = new EnumListForm(this.currentProject))
      {
        int num = (int) enumListForm.ShowDialog();
      }
    }

    private void memoryViewControl_DrawContextRequested(
      object sender,
      DrawContextRequestEventArgs args)
    {
      RemoteProcess remoteProcess = Program.RemoteProcess;
      ClassNode currentClassNode = this.CurrentClassNode;
      if (currentClassNode == null)
        return;
      this.memoryViewBuffer.Size = currentClassNode.MemorySize;
      IntPtr address;
      try
      {
        address = remoteProcess.ParseAddress(currentClassNode.AddressFormula);
      }
      catch (ParseException ex)
      {
        address = IntPtr.Zero;
      }
      this.memoryViewBuffer.UpdateFrom((IRemoteMemoryReader) remoteProcess, address, MainForm.PS4PID);
      args.Settings = Program.Settings;
      args.IconProvider = this.iconProvider;
      args.Process = remoteProcess;
      args.Memory = this.memoryViewBuffer;
      args.Node = (BaseNode) currentClassNode;
      args.BaseAddress = address;
    }

    private void toolStripComboBox1_DropDown(object sender, EventArgs e)
    {
      if (!MainForm.PS4.IsConnected)
        return;
      this.toolStripComboBox1.Items.Clear();
      MainForm.ProcList = MainForm.PS4.GetProcessList();
      foreach (Process process in MainForm.ProcList.processes)
        this.toolStripComboBox1.Items.Add((object) process.name);
    }

    private void toolStripTextBox1_Click(object sender, EventArgs e)
    {
    }

    private void connectToolStripMenuItem_Click(object sender, EventArgs e)
    {
    }

    private void toolStripMenuItem3_Click(object sender, EventArgs e)
    {
      MainForm.PS4 = new PS4DBG(this.toolStripTextBox1.Text);
      MainForm.PS4.Connect();
      MainForm.PS4.Notify(222, "ReClass.Net (x64) Connected!");
    }

    private void toolStripComboBox1_Click(object sender, EventArgs e)
    {
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

    private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
    {
      if (!MainForm.PS4.IsConnected)
        return;
      try
      {
        if (MainForm.ProcList.FindProcess(this.toolStripComboBox1.SelectedItem.ToString(), false).name == null)
          return;
        MainForm.PS4PID = this.toolStripComboBox1.SelectedItem.ToString();
      }
      catch
      {
      }
    }

    private void socketListenerToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new SocketListener().Show();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.processUpdateTimer = new System.Windows.Forms.Timer(this.components);
      this.splitContainer = new SplitContainer();
      this.projectView = new ProjectView();
      this.projectClassContextMenuStrip = new ContextMenuStrip(this.components);
      this.deleteClassToolStripMenuItem = new ToolStripMenuItem();
      this.removeUnusedClassesToolStripMenuItem = new ToolStripMenuItem();
      this.showCodeOfClassToolStripMenuItem2 = new ToolStripMenuItem();
      this.projectClassesContextMenuStrip = new ContextMenuStrip(this.components);
      this.enableHierarchyViewToolStripMenuItem = new ToolStripMenuItem();
      this.autoExpandHierarchyViewToolStripMenuItem = new ToolStripMenuItem();
      this.expandAllClassesToolStripMenuItem = new ToolStripMenuItem();
      this.collapseAllClassesToolStripMenuItem = new ToolStripMenuItem();
      this.addNewClassToolStripMenuItem = new ToolStripMenuItem();
      this.projectEnumContextMenuStrip = new ContextMenuStrip(this.components);
      this.editEnumToolStripMenuItem = new ToolStripMenuItem();
      this.projectEnumsContextMenuStrip = new ContextMenuStrip(this.components);
      this.editEnumsToolStripMenuItem = new ToolStripMenuItem();
      this.memoryViewControl = new MemoryViewControl();
      this.selectedNodeContextMenuStrip = new ContextMenuStrip(this.components);
      this.changeTypeToolStripMenuItem = new ToolStripMenuItem();
      this.addBytesToolStripMenuItem = new ToolStripMenuItem();
      this.integerToolStripMenuItem1 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem2 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem3 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem4 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem5 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem6 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem7 = new IntegerToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripMenuItem();
      this.insertBytesToolStripMenuItem = new ToolStripMenuItem();
      this.integerToolStripMenuItem8 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem9 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem10 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem11 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem12 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem13 = new IntegerToolStripMenuItem();
      this.integerToolStripMenuItem14 = new IntegerToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripMenuItem();
      this.createClassFromNodesToolStripMenuItem = new ToolStripMenuItem();
      this.dissectNodesToolStripMenuItem = new ToolStripMenuItem();
      this.copyNodeToolStripMenuItem = new ToolStripMenuItem();
      this.pasteNodesToolStripMenuItem = new ToolStripMenuItem();
      this.removeToolStripMenuItem = new ToolStripMenuItem();
      this.hideNodesToolStripMenuItem = new ToolStripMenuItem();
      this.unhideNodesToolStripMenuItem = new ToolStripMenuItem();
      this.unhideChildNodesToolStripMenuItem = new ToolStripMenuItem();
      this.unhideNodesAboveToolStripMenuItem = new ToolStripMenuItem();
      this.unhideNodesBelowToolStripMenuItem = new ToolStripMenuItem();
      this.copyAddressToolStripMenuItem = new ToolStripMenuItem();
      this.showCodeOfClassToolStripMenuItem = new ToolStripMenuItem();
      this.shrinkClassToolStripMenuItem = new ToolStripMenuItem();
      this.toolStrip = new ToolStrip();
      this.openProjectToolStripButton = new ToolStripButton();
      this.saveToolStripButton = new ToolStripButton();
      this.toolStripSeparator7 = new ToolStripSeparator();
      this.newClassToolStripButton = new ToolStripButton();
      this.addBytesToolStripDropDownButton = new ToolStripDropDownButton();
      this.add4BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add8BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add64BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add256BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add1024BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add2048BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.add4096BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.addXBytesToolStripMenuItem = new ToolStripMenuItem();
      this.insertBytesToolStripDropDownButton = new ToolStripDropDownButton();
      this.insert4BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert8BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert64BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert256BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert1024BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert2048BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insert4096BytesToolStripMenuItem = new IntegerToolStripMenuItem();
      this.insertXBytesToolStripMenuItem = new ToolStripMenuItem();
      this.nodeTypesToolStripSeparator = new ToolStripSeparator();
      this.mainMenuStrip = new MenuStrip();
      this.fileToolStripMenuItem = new ToolStripMenuItem();
      this.openProjectToolStripMenuItem = new ToolStripMenuItem();
      this.mergeWithProjectToolStripMenuItem = new ToolStripMenuItem();
      this.clearProjectToolStripMenuItem = new ToolStripMenuItem();
      this.saveToolStripMenuItem = new ToolStripMenuItem();
      this.saveAsToolStripMenuItem = new ToolStripMenuItem();
      this.quitToolStripMenuItem = new ToolStripMenuItem();
      this.projectToolStripMenuItem = new ToolStripMenuItem();
      this.goToClassToolStripMenuItem = new ToolStripMenuItem();
      this.cleanUnusedClassesToolStripMenuItem = new ToolStripMenuItem();
      this.showEnumsToolStripMenuItem = new ToolStripMenuItem();
      this.generateCppCodeToolStripMenuItem = new ToolStripMenuItem();
      this.generateCSharpCodeToolStripMenuItem = new ToolStripMenuItem();
      this.helpToolStripMenuItem = new ToolStripMenuItem();
      this.aboutToolStripMenuItem = new ToolStripMenuItem();
      this.socketListenerToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripMenuItem3 = new ToolStripMenuItem();
      this.toolStripTextBox1 = new ToolStripTextBox();
      this.toolStripComboBox1 = new ToolStripComboBox();
      this.splitContainer.BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      this.projectClassContextMenuStrip.SuspendLayout();
      this.projectClassesContextMenuStrip.SuspendLayout();
      this.projectEnumContextMenuStrip.SuspendLayout();
      this.projectEnumsContextMenuStrip.SuspendLayout();
      this.selectedNodeContextMenuStrip.SuspendLayout();
      this.toolStrip.SuspendLayout();
      this.mainMenuStrip.SuspendLayout();
      this.SuspendLayout();
      this.processUpdateTimer.Enabled = true;
      this.processUpdateTimer.Interval = 5000;
      this.processUpdateTimer.Tick += new EventHandler(this.processUpdateTimer_Tick);
      this.splitContainer.Dock = DockStyle.Fill;
      this.splitContainer.FixedPanel = FixedPanel.Panel1;
      this.splitContainer.Location = new Point(0, 52);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Panel1.Controls.Add((Control) this.projectView);
      this.splitContainer.Panel2.BackColor = SystemColors.Control;
      this.splitContainer.Panel2.Controls.Add((Control) this.memoryViewControl);
      this.splitContainer.Size = new Size(1141, 543);
      this.splitContainer.SplitterDistance = 201;
      this.splitContainer.TabIndex = 4;
      this.projectView.BackColor = Color.FromArgb(35, 35, 35);
      this.projectView.ClassContextMenuStrip = this.projectClassContextMenuStrip;
      this.projectView.ClassesContextMenuStrip = this.projectClassesContextMenuStrip;
      this.projectView.Dock = DockStyle.Fill;
      this.projectView.EnumContextMenuStrip = this.projectEnumContextMenuStrip;
      this.projectView.EnumsContextMenuStrip = this.projectEnumsContextMenuStrip;
      this.projectView.ForeColor = Color.FromArgb(69, 73, 74);
      this.projectView.Location = new Point(0, 0);
      this.projectView.Name = "projectView";
      this.projectView.Size = new Size(201, 543);
      this.projectView.TabIndex = 0;
      this.projectView.SelectionChanged += new ProjectView.SelectionChangedEvent(this.classesView_ClassSelected);
      this.projectClassContextMenuStrip.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.deleteClassToolStripMenuItem,
        (ToolStripItem) this.removeUnusedClassesToolStripMenuItem,
        (ToolStripItem) this.showCodeOfClassToolStripMenuItem2
      });
      this.projectClassContextMenuStrip.Name = "contextMenuStrip";
      this.projectClassContextMenuStrip.Size = new Size(206, 70);
      this.deleteClassToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.deleteClassToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Class_Remove;
      this.deleteClassToolStripMenuItem.Name = "deleteClassToolStripMenuItem";
      this.deleteClassToolStripMenuItem.Size = new Size(205, 22);
      this.deleteClassToolStripMenuItem.Text = "Delete class";
      this.deleteClassToolStripMenuItem.Click += new EventHandler(this.deleteClassToolStripMenuItem_Click);
      this.removeUnusedClassesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.removeUnusedClassesToolStripMenuItem.Image = (Image) Resources.B16x16_Chart_Delete;
      this.removeUnusedClassesToolStripMenuItem.Name = "removeUnusedClassesToolStripMenuItem";
      this.removeUnusedClassesToolStripMenuItem.Size = new Size(205, 22);
      this.removeUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
      this.removeUnusedClassesToolStripMenuItem.Click += new EventHandler(this.removeUnusedClassesToolStripMenuItem_Click);
      this.showCodeOfClassToolStripMenuItem2.BackColor = Color.FromArgb(60, 63, 65);
      this.showCodeOfClassToolStripMenuItem2.Image = (Image) Resources.B16x16_Page_Code_Cpp;
      this.showCodeOfClassToolStripMenuItem2.Name = "showCodeOfClassToolStripMenuItem2";
      this.showCodeOfClassToolStripMenuItem2.Size = new Size(205, 22);
      this.showCodeOfClassToolStripMenuItem2.Text = "Show C++ Code of Class";
      this.showCodeOfClassToolStripMenuItem2.Click += new EventHandler(this.showCodeOfClassToolStripMenuItem2_Click);
      this.projectClassesContextMenuStrip.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.enableHierarchyViewToolStripMenuItem,
        (ToolStripItem) this.autoExpandHierarchyViewToolStripMenuItem,
        (ToolStripItem) this.expandAllClassesToolStripMenuItem,
        (ToolStripItem) this.collapseAllClassesToolStripMenuItem,
        (ToolStripItem) this.addNewClassToolStripMenuItem
      });
      this.projectClassesContextMenuStrip.Name = "rootContextMenuStrip";
      this.projectClassesContextMenuStrip.Size = new Size(222, 114);
      this.enableHierarchyViewToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.enableHierarchyViewToolStripMenuItem.ForeColor = Color.White;
      this.enableHierarchyViewToolStripMenuItem.Name = "enableHierarchyViewToolStripMenuItem";
      this.enableHierarchyViewToolStripMenuItem.Size = new Size(221, 22);
      this.enableHierarchyViewToolStripMenuItem.Text = "Enable hierarchy view";
      this.enableHierarchyViewToolStripMenuItem.Click += new EventHandler(this.enableHierarchyViewToolStripMenuItem_Click);
      this.autoExpandHierarchyViewToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.autoExpandHierarchyViewToolStripMenuItem.ForeColor = Color.White;
      this.autoExpandHierarchyViewToolStripMenuItem.Name = "autoExpandHierarchyViewToolStripMenuItem";
      this.autoExpandHierarchyViewToolStripMenuItem.Size = new Size(221, 22);
      this.autoExpandHierarchyViewToolStripMenuItem.Text = "Auto expand hierarchy view";
      this.autoExpandHierarchyViewToolStripMenuItem.Click += new EventHandler(this.autoExpandHierarchyViewToolStripMenuItem_Click);
      this.expandAllClassesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.expandAllClassesToolStripMenuItem.Enabled = false;
      this.expandAllClassesToolStripMenuItem.ForeColor = Color.White;
      this.expandAllClassesToolStripMenuItem.Image = (Image) Resources.B16x16_Tree_Expand;
      this.expandAllClassesToolStripMenuItem.Name = "expandAllClassesToolStripMenuItem";
      this.expandAllClassesToolStripMenuItem.Size = new Size(221, 22);
      this.expandAllClassesToolStripMenuItem.Text = "Expand all classes";
      this.expandAllClassesToolStripMenuItem.Click += new EventHandler(this.expandAllClassesToolStripMenuItem_Click);
      this.collapseAllClassesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.collapseAllClassesToolStripMenuItem.Enabled = false;
      this.collapseAllClassesToolStripMenuItem.ForeColor = Color.White;
      this.collapseAllClassesToolStripMenuItem.Image = (Image) Resources.B16x16_Tree_Collapse;
      this.collapseAllClassesToolStripMenuItem.Name = "collapseAllClassesToolStripMenuItem";
      this.collapseAllClassesToolStripMenuItem.Size = new Size(221, 22);
      this.collapseAllClassesToolStripMenuItem.Text = "Collapse all classes";
      this.collapseAllClassesToolStripMenuItem.Click += new EventHandler(this.collapseAllClassesToolStripMenuItem_Click);
      this.addNewClassToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.addNewClassToolStripMenuItem.ForeColor = Color.White;
      this.addNewClassToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Class_Add;
      this.addNewClassToolStripMenuItem.Name = "addNewClassToolStripMenuItem";
      this.addNewClassToolStripMenuItem.Size = new Size(221, 22);
      this.addNewClassToolStripMenuItem.Text = "Add new class";
      this.addNewClassToolStripMenuItem.Click += new EventHandler(this.newClassToolStripButton_Click);
      this.projectEnumContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.editEnumToolStripMenuItem
      });
      this.projectEnumContextMenuStrip.Name = "projectEnumContextMenuStrip";
      this.projectEnumContextMenuStrip.Size = new Size(138, 26);
      this.editEnumToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.editEnumToolStripMenuItem.ForeColor = Color.White;
      this.editEnumToolStripMenuItem.Image = (Image) Resources.B16x16_Enum_Type;
      this.editEnumToolStripMenuItem.Name = "editEnumToolStripMenuItem";
      this.editEnumToolStripMenuItem.Size = new Size(137, 22);
      this.editEnumToolStripMenuItem.Text = "Edit Enum...";
      this.editEnumToolStripMenuItem.Click += new EventHandler(this.editEnumToolStripMenuItem_Click);
      this.projectEnumsContextMenuStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.editEnumsToolStripMenuItem
      });
      this.projectEnumsContextMenuStrip.Name = "projectEnumsContextMenuStrip";
      this.projectEnumsContextMenuStrip.Size = new Size(143, 26);
      this.editEnumsToolStripMenuItem.Image = (Image) Resources.B16x16_Category;
      this.editEnumsToolStripMenuItem.Name = "editEnumsToolStripMenuItem";
      this.editEnumsToolStripMenuItem.Size = new Size(142, 22);
      this.editEnumsToolStripMenuItem.Text = "Edit enums...";
      this.editEnumsToolStripMenuItem.Click += new EventHandler(this.editEnumsToolStripMenuItem_Click);
      this.memoryViewControl.BackColor = Color.FromArgb(35, 35, 35);
      this.memoryViewControl.BorderStyle = BorderStyle.FixedSingle;
      this.memoryViewControl.Dock = DockStyle.Fill;
      this.memoryViewControl.Location = new Point(0, 0);
      this.memoryViewControl.Name = "memoryViewControl";
      this.memoryViewControl.NodeContextMenuStrip = this.selectedNodeContextMenuStrip;
      this.memoryViewControl.Size = new Size(936, 543);
      this.memoryViewControl.TabIndex = 0;
      this.memoryViewControl.DrawContextRequested += new DrawContextRequestEventHandler(this.memoryViewControl_DrawContextRequested);
      this.memoryViewControl.SelectionChanged += new EventHandler(this.memoryViewControl_SelectionChanged);
      this.memoryViewControl.ChangeClassTypeClick += new NodeClickEventHandler(this.memoryViewControl_ChangeClassTypeClick);
      this.memoryViewControl.ChangeWrappedTypeClick += new NodeClickEventHandler(this.memoryViewControl_ChangeWrappedTypeClick);
      this.memoryViewControl.ChangeEnumTypeClick += new NodeClickEventHandler(this.memoryViewControl_ChangeEnumTypeClick);
      this.memoryViewControl.KeyDown += new KeyEventHandler(this.memoryViewControl_KeyDown);
      this.selectedNodeContextMenuStrip.Items.AddRange(new ToolStripItem[13]
      {
        (ToolStripItem) this.changeTypeToolStripMenuItem,
        (ToolStripItem) this.addBytesToolStripMenuItem,
        (ToolStripItem) this.insertBytesToolStripMenuItem,
        (ToolStripItem) this.createClassFromNodesToolStripMenuItem,
        (ToolStripItem) this.dissectNodesToolStripMenuItem,
        (ToolStripItem) this.copyNodeToolStripMenuItem,
        (ToolStripItem) this.pasteNodesToolStripMenuItem,
        (ToolStripItem) this.removeToolStripMenuItem,
        (ToolStripItem) this.hideNodesToolStripMenuItem,
        (ToolStripItem) this.unhideNodesToolStripMenuItem,
        (ToolStripItem) this.copyAddressToolStripMenuItem,
        (ToolStripItem) this.showCodeOfClassToolStripMenuItem,
        (ToolStripItem) this.shrinkClassToolStripMenuItem
      });
      this.selectedNodeContextMenuStrip.Name = "selectedNodeContextMenuStrip";
      this.selectedNodeContextMenuStrip.Size = new Size(206, 290);
      this.selectedNodeContextMenuStrip.Opening += new CancelEventHandler(this.selectedNodeContextMenuStrip_Opening);
      this.changeTypeToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.changeTypeToolStripMenuItem.ForeColor = Color.White;
      this.changeTypeToolStripMenuItem.Image = (Image) Resources.B16x16_Exchange_Button;
      this.changeTypeToolStripMenuItem.Name = "changeTypeToolStripMenuItem";
      this.changeTypeToolStripMenuItem.Size = new Size(205, 22);
      this.changeTypeToolStripMenuItem.Text = "Change Type";
      this.addBytesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.addBytesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.integerToolStripMenuItem1,
        (ToolStripItem) this.integerToolStripMenuItem2,
        (ToolStripItem) this.integerToolStripMenuItem3,
        (ToolStripItem) this.integerToolStripMenuItem4,
        (ToolStripItem) this.integerToolStripMenuItem5,
        (ToolStripItem) this.integerToolStripMenuItem6,
        (ToolStripItem) this.integerToolStripMenuItem7,
        (ToolStripItem) this.toolStripMenuItem1
      });
      this.addBytesToolStripMenuItem.ForeColor = Color.White;
      this.addBytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_X;
      this.addBytesToolStripMenuItem.Name = "addBytesToolStripMenuItem";
      this.addBytesToolStripMenuItem.Size = new Size(205, 22);
      this.addBytesToolStripMenuItem.Text = "Add Bytes";
      this.integerToolStripMenuItem1.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem1.ForeColor = Color.White;
      this.integerToolStripMenuItem1.Image = (Image) Resources.B16x16_Button_Add_Bytes_4;
      this.integerToolStripMenuItem1.Name = "integerToolStripMenuItem1";
      this.integerToolStripMenuItem1.Size = new Size(154, 22);
      this.integerToolStripMenuItem1.Text = "Add 4 Bytes";
      this.integerToolStripMenuItem1.Value = 4;
      this.integerToolStripMenuItem1.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem2.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem2.ForeColor = Color.White;
      this.integerToolStripMenuItem2.Image = (Image) Resources.B16x16_Button_Add_Bytes_8;
      this.integerToolStripMenuItem2.Name = "integerToolStripMenuItem2";
      this.integerToolStripMenuItem2.Size = new Size(154, 22);
      this.integerToolStripMenuItem2.Text = "Add 8 Bytes";
      this.integerToolStripMenuItem2.Value = 8;
      this.integerToolStripMenuItem2.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem3.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem3.ForeColor = Color.White;
      this.integerToolStripMenuItem3.Image = (Image) Resources.B16x16_Button_Add_Bytes_64;
      this.integerToolStripMenuItem3.Name = "integerToolStripMenuItem3";
      this.integerToolStripMenuItem3.Size = new Size(154, 22);
      this.integerToolStripMenuItem3.Text = "Add 64 Bytes";
      this.integerToolStripMenuItem3.Value = 64;
      this.integerToolStripMenuItem3.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem4.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem4.ForeColor = Color.White;
      this.integerToolStripMenuItem4.Image = (Image) Resources.B16x16_Button_Add_Bytes_256;
      this.integerToolStripMenuItem4.Name = "integerToolStripMenuItem4";
      this.integerToolStripMenuItem4.Size = new Size(154, 22);
      this.integerToolStripMenuItem4.Text = "Add 256 Bytes";
      this.integerToolStripMenuItem4.Value = 256;
      this.integerToolStripMenuItem4.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem5.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem5.ForeColor = Color.White;
      this.integerToolStripMenuItem5.Image = (Image) Resources.B16x16_Button_Add_Bytes_1024;
      this.integerToolStripMenuItem5.Name = "integerToolStripMenuItem5";
      this.integerToolStripMenuItem5.Size = new Size(154, 22);
      this.integerToolStripMenuItem5.Text = "Add 1024 Bytes";
      this.integerToolStripMenuItem5.Value = 1024;
      this.integerToolStripMenuItem5.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem6.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem6.ForeColor = Color.White;
      this.integerToolStripMenuItem6.Image = (Image) Resources.B16x16_Button_Add_Bytes_2048;
      this.integerToolStripMenuItem6.Name = "integerToolStripMenuItem6";
      this.integerToolStripMenuItem6.Size = new Size(154, 22);
      this.integerToolStripMenuItem6.Text = "Add 2048 Bytes";
      this.integerToolStripMenuItem6.Value = 2048;
      this.integerToolStripMenuItem6.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem7.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem7.ForeColor = Color.White;
      this.integerToolStripMenuItem7.Image = (Image) Resources.B16x16_Button_Add_Bytes_4096;
      this.integerToolStripMenuItem7.Name = "integerToolStripMenuItem7";
      this.integerToolStripMenuItem7.Size = new Size(154, 22);
      this.integerToolStripMenuItem7.Text = "Add 4096 Bytes";
      this.integerToolStripMenuItem7.Value = 4096;
      this.integerToolStripMenuItem7.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.toolStripMenuItem1.BackColor = Color.FromArgb(60, 63, 65);
      this.toolStripMenuItem1.ForeColor = Color.White;
      this.toolStripMenuItem1.Image = (Image) Resources.B16x16_Button_Add_Bytes_X;
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(154, 22);
      this.toolStripMenuItem1.Text = "Add ... Bytes";
      this.toolStripMenuItem1.Click += new EventHandler(this.addXBytesToolStripMenuItem_Click);
      this.insertBytesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.insertBytesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.integerToolStripMenuItem8,
        (ToolStripItem) this.integerToolStripMenuItem9,
        (ToolStripItem) this.integerToolStripMenuItem10,
        (ToolStripItem) this.integerToolStripMenuItem11,
        (ToolStripItem) this.integerToolStripMenuItem12,
        (ToolStripItem) this.integerToolStripMenuItem13,
        (ToolStripItem) this.integerToolStripMenuItem14,
        (ToolStripItem) this.toolStripMenuItem2
      });
      this.insertBytesToolStripMenuItem.ForeColor = Color.White;
      this.insertBytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_X;
      this.insertBytesToolStripMenuItem.Name = "insertBytesToolStripMenuItem";
      this.insertBytesToolStripMenuItem.Size = new Size(205, 22);
      this.insertBytesToolStripMenuItem.Text = "Insert Bytes";
      this.integerToolStripMenuItem8.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem8.ForeColor = Color.White;
      this.integerToolStripMenuItem8.Image = (Image) Resources.B16x16_Button_Insert_Bytes_4;
      this.integerToolStripMenuItem8.Name = "integerToolStripMenuItem8";
      this.integerToolStripMenuItem8.Size = new Size(161, 22);
      this.integerToolStripMenuItem8.Text = "Insert 4 Bytes";
      this.integerToolStripMenuItem8.Value = 4;
      this.integerToolStripMenuItem8.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem9.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem9.ForeColor = Color.White;
      this.integerToolStripMenuItem9.Image = (Image) Resources.B16x16_Button_Insert_Bytes_8;
      this.integerToolStripMenuItem9.Name = "integerToolStripMenuItem9";
      this.integerToolStripMenuItem9.Size = new Size(161, 22);
      this.integerToolStripMenuItem9.Text = "Insert 8 Bytes";
      this.integerToolStripMenuItem9.Value = 8;
      this.integerToolStripMenuItem9.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem10.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem10.ForeColor = Color.White;
      this.integerToolStripMenuItem10.Image = (Image) Resources.B16x16_Button_Insert_Bytes_64;
      this.integerToolStripMenuItem10.Name = "integerToolStripMenuItem10";
      this.integerToolStripMenuItem10.Size = new Size(161, 22);
      this.integerToolStripMenuItem10.Text = "Insert 64 Bytes";
      this.integerToolStripMenuItem10.Value = 64;
      this.integerToolStripMenuItem10.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem11.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem11.ForeColor = Color.White;
      this.integerToolStripMenuItem11.Image = (Image) Resources.B16x16_Button_Insert_Bytes_256;
      this.integerToolStripMenuItem11.Name = "integerToolStripMenuItem11";
      this.integerToolStripMenuItem11.Size = new Size(161, 22);
      this.integerToolStripMenuItem11.Text = "Insert 256 Bytes";
      this.integerToolStripMenuItem11.Value = 256;
      this.integerToolStripMenuItem11.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem12.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem12.ForeColor = Color.White;
      this.integerToolStripMenuItem12.Image = (Image) Resources.B16x16_Button_Insert_Bytes_1024;
      this.integerToolStripMenuItem12.Name = "integerToolStripMenuItem12";
      this.integerToolStripMenuItem12.Size = new Size(161, 22);
      this.integerToolStripMenuItem12.Text = "Insert 1024 Bytes";
      this.integerToolStripMenuItem12.Value = 1024;
      this.integerToolStripMenuItem12.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem13.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem13.ForeColor = Color.White;
      this.integerToolStripMenuItem13.Image = (Image) Resources.B16x16_Button_Insert_Bytes_2048;
      this.integerToolStripMenuItem13.Name = "integerToolStripMenuItem13";
      this.integerToolStripMenuItem13.Size = new Size(161, 22);
      this.integerToolStripMenuItem13.Text = "Insert 2048 Bytes";
      this.integerToolStripMenuItem13.Value = 2048;
      this.integerToolStripMenuItem13.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.integerToolStripMenuItem14.BackColor = Color.FromArgb(60, 63, 65);
      this.integerToolStripMenuItem14.ForeColor = Color.White;
      this.integerToolStripMenuItem14.Image = (Image) Resources.B16x16_Button_Insert_Bytes_4096;
      this.integerToolStripMenuItem14.Name = "integerToolStripMenuItem14";
      this.integerToolStripMenuItem14.Size = new Size(161, 22);
      this.integerToolStripMenuItem14.Text = "Insert 4096 Bytes";
      this.integerToolStripMenuItem14.Value = 4096;
      this.integerToolStripMenuItem14.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.toolStripMenuItem2.BackColor = Color.FromArgb(60, 63, 65);
      this.toolStripMenuItem2.ForeColor = Color.White;
      this.toolStripMenuItem2.Image = (Image) Resources.B16x16_Button_Insert_Bytes_X;
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(161, 22);
      this.toolStripMenuItem2.Text = "Insert ... Bytes";
      this.toolStripMenuItem2.Click += new EventHandler(this.insertXBytesToolStripMenuItem_Click);
      this.createClassFromNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.createClassFromNodesToolStripMenuItem.ForeColor = Color.White;
      this.createClassFromNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Class_Add;
      this.createClassFromNodesToolStripMenuItem.Name = "createClassFromNodesToolStripMenuItem";
      this.createClassFromNodesToolStripMenuItem.Size = new Size(205, 22);
      this.createClassFromNodesToolStripMenuItem.Text = "Create Class from Nodes";
      this.createClassFromNodesToolStripMenuItem.Click += new EventHandler(this.createClassFromNodesToolStripMenuItem_Click);
      this.dissectNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.dissectNodesToolStripMenuItem.ForeColor = Color.White;
      this.dissectNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Camera;
      this.dissectNodesToolStripMenuItem.Name = "dissectNodesToolStripMenuItem";
      this.dissectNodesToolStripMenuItem.Size = new Size(205, 22);
      this.dissectNodesToolStripMenuItem.Text = "Dissect Node(s)";
      this.dissectNodesToolStripMenuItem.Click += new EventHandler(this.dissectNodesToolStripMenuItem_Click);
      this.copyNodeToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.copyNodeToolStripMenuItem.ForeColor = Color.White;
      this.copyNodeToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Copy;
      this.copyNodeToolStripMenuItem.Name = "copyNodeToolStripMenuItem";
      this.copyNodeToolStripMenuItem.Size = new Size(205, 22);
      this.copyNodeToolStripMenuItem.Text = "Copy Node(s)";
      this.copyNodeToolStripMenuItem.Click += new EventHandler(this.copyNodeToolStripMenuItem_Click);
      this.pasteNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.pasteNodesToolStripMenuItem.ForeColor = Color.White;
      this.pasteNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Paste;
      this.pasteNodesToolStripMenuItem.Name = "pasteNodesToolStripMenuItem";
      this.pasteNodesToolStripMenuItem.Size = new Size(205, 22);
      this.pasteNodesToolStripMenuItem.Text = "Paste Node(s)";
      this.pasteNodesToolStripMenuItem.Click += new EventHandler(this.pasteNodesToolStripMenuItem_Click);
      this.removeToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.removeToolStripMenuItem.ForeColor = Color.White;
      this.removeToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Delete;
      this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
      this.removeToolStripMenuItem.Size = new Size(205, 22);
      this.removeToolStripMenuItem.Text = "Remove Node(s)";
      this.removeToolStripMenuItem.Click += new EventHandler(this.removeToolStripMenuItem_Click);
      this.hideNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.hideNodesToolStripMenuItem.ForeColor = Color.White;
      this.hideNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Eye;
      this.hideNodesToolStripMenuItem.Name = "hideNodesToolStripMenuItem";
      this.hideNodesToolStripMenuItem.Size = new Size(205, 22);
      this.hideNodesToolStripMenuItem.Text = "Hide selected Node(s)";
      this.hideNodesToolStripMenuItem.Click += new EventHandler(this.hideNodesToolStripMenuItem_Click);
      this.unhideNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.unhideNodesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.unhideChildNodesToolStripMenuItem,
        (ToolStripItem) this.unhideNodesAboveToolStripMenuItem,
        (ToolStripItem) this.unhideNodesBelowToolStripMenuItem
      });
      this.unhideNodesToolStripMenuItem.ForeColor = Color.White;
      this.unhideNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Eye;
      this.unhideNodesToolStripMenuItem.Name = "unhideNodesToolStripMenuItem";
      this.unhideNodesToolStripMenuItem.Size = new Size(205, 22);
      this.unhideNodesToolStripMenuItem.Text = "Unhide...";
      this.unhideChildNodesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.unhideChildNodesToolStripMenuItem.ForeColor = Color.White;
      this.unhideChildNodesToolStripMenuItem.Image = (Image) Resources.B16x16_Eye;
      this.unhideChildNodesToolStripMenuItem.Name = "unhideChildNodesToolStripMenuItem";
      this.unhideChildNodesToolStripMenuItem.Size = new Size(163, 22);
      this.unhideChildNodesToolStripMenuItem.Text = "... Child Node(s)";
      this.unhideChildNodesToolStripMenuItem.Click += new EventHandler(this.unhideChildNodesToolStripMenuItem_Click);
      this.unhideNodesAboveToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.unhideNodesAboveToolStripMenuItem.ForeColor = Color.White;
      this.unhideNodesAboveToolStripMenuItem.Image = (Image) Resources.B16x16_Eye;
      this.unhideNodesAboveToolStripMenuItem.Name = "unhideNodesAboveToolStripMenuItem";
      this.unhideNodesAboveToolStripMenuItem.Size = new Size(163, 22);
      this.unhideNodesAboveToolStripMenuItem.Text = "... Node(s) above";
      this.unhideNodesAboveToolStripMenuItem.Click += new EventHandler(this.unhideNodesAboveToolStripMenuItem_Click);
      this.unhideNodesBelowToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.unhideNodesBelowToolStripMenuItem.ForeColor = Color.White;
      this.unhideNodesBelowToolStripMenuItem.Image = (Image) Resources.B16x16_Eye;
      this.unhideNodesBelowToolStripMenuItem.Name = "unhideNodesBelowToolStripMenuItem";
      this.unhideNodesBelowToolStripMenuItem.Size = new Size(163, 22);
      this.unhideNodesBelowToolStripMenuItem.Text = "... Node(s) below";
      this.unhideNodesBelowToolStripMenuItem.Click += new EventHandler(this.unhideNodesBelowToolStripMenuItem_Click);
      this.copyAddressToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.copyAddressToolStripMenuItem.ForeColor = Color.White;
      this.copyAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Copy;
      this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
      this.copyAddressToolStripMenuItem.Size = new Size(205, 22);
      this.copyAddressToolStripMenuItem.Text = "Copy Address";
      this.copyAddressToolStripMenuItem.Click += new EventHandler(this.copyAddressToolStripMenuItem_Click);
      this.showCodeOfClassToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.showCodeOfClassToolStripMenuItem.ForeColor = Color.White;
      this.showCodeOfClassToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Code_Cpp;
      this.showCodeOfClassToolStripMenuItem.Name = "showCodeOfClassToolStripMenuItem";
      this.showCodeOfClassToolStripMenuItem.Size = new Size(205, 22);
      this.showCodeOfClassToolStripMenuItem.Text = "Show C++ Code of Class";
      this.showCodeOfClassToolStripMenuItem.Click += new EventHandler(this.showCodeOfClassToolStripMenuItem_Click);
      this.shrinkClassToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.shrinkClassToolStripMenuItem.ForeColor = Color.White;
      this.shrinkClassToolStripMenuItem.Image = (Image) Resources.B16x16_Chart_Delete;
      this.shrinkClassToolStripMenuItem.Name = "shrinkClassToolStripMenuItem";
      this.shrinkClassToolStripMenuItem.Size = new Size(205, 22);
      this.shrinkClassToolStripMenuItem.Text = "Shrink Class";
      this.shrinkClassToolStripMenuItem.Click += new EventHandler(this.shrinkClassToolStripMenuItem_Click);
      this.toolStrip.BackColor = Color.FromArgb(50, 53, 55);
      this.toolStrip.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.openProjectToolStripButton,
        (ToolStripItem) this.saveToolStripButton,
        (ToolStripItem) this.toolStripSeparator7,
        (ToolStripItem) this.newClassToolStripButton,
        (ToolStripItem) this.addBytesToolStripDropDownButton,
        (ToolStripItem) this.insertBytesToolStripDropDownButton,
        (ToolStripItem) this.nodeTypesToolStripSeparator
      });
      this.toolStrip.Location = new Point(0, 27);
      this.toolStrip.Name = "toolStrip";
      this.toolStrip.Size = new Size(1141, 25);
      this.toolStrip.TabIndex = 3;
      this.openProjectToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.openProjectToolStripButton.Image = (Image) Resources.B16x16_Folder;
      this.openProjectToolStripButton.ImageTransparentColor = Color.Magenta;
      this.openProjectToolStripButton.Name = "openProjectToolStripButton";
      this.openProjectToolStripButton.Size = new Size(23, 22);
      this.openProjectToolStripButton.ToolTipText = "Open Project...";
      this.openProjectToolStripButton.Click += new EventHandler(this.openProjectToolStripMenuItem_Click);
      this.saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.saveToolStripButton.Image = (Image) Resources.B16x16_Save;
      this.saveToolStripButton.ImageTransparentColor = Color.Magenta;
      this.saveToolStripButton.Name = "saveToolStripButton";
      this.saveToolStripButton.Size = new Size(23, 22);
      this.saveToolStripButton.ToolTipText = "Save Project";
      this.saveToolStripButton.Click += new EventHandler(this.saveToolStripMenuItem_Click);
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new Size(6, 25);
      this.newClassToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.newClassToolStripButton.Image = (Image) Resources.B16x16_Button_Class_Add;
      this.newClassToolStripButton.ImageTransparentColor = Color.Magenta;
      this.newClassToolStripButton.Name = "newClassToolStripButton";
      this.newClassToolStripButton.Size = new Size(23, 22);
      this.newClassToolStripButton.Text = "addClassToolStripButton";
      this.newClassToolStripButton.ToolTipText = "Add a new class to this project";
      this.newClassToolStripButton.Click += new EventHandler(this.newClassToolStripButton_Click);
      this.addBytesToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.addBytesToolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.add4BytesToolStripMenuItem,
        (ToolStripItem) this.add8BytesToolStripMenuItem,
        (ToolStripItem) this.add64BytesToolStripMenuItem,
        (ToolStripItem) this.add256BytesToolStripMenuItem,
        (ToolStripItem) this.add1024BytesToolStripMenuItem,
        (ToolStripItem) this.add2048BytesToolStripMenuItem,
        (ToolStripItem) this.add4096BytesToolStripMenuItem,
        (ToolStripItem) this.addXBytesToolStripMenuItem
      });
      this.addBytesToolStripDropDownButton.Image = (Image) Resources.B16x16_Button_Add_Bytes_X;
      this.addBytesToolStripDropDownButton.ImageTransparentColor = Color.Magenta;
      this.addBytesToolStripDropDownButton.Name = "addBytesToolStripDropDownButton";
      this.addBytesToolStripDropDownButton.Size = new Size(29, 22);
      this.add4BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_4;
      this.add4BytesToolStripMenuItem.Name = "add4BytesToolStripMenuItem";
      this.add4BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add4BytesToolStripMenuItem.Tag = (object) "";
      this.add4BytesToolStripMenuItem.Text = "Add 4 Bytes";
      this.add4BytesToolStripMenuItem.Value = 4;
      this.add4BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add8BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_8;
      this.add8BytesToolStripMenuItem.Name = "add8BytesToolStripMenuItem";
      this.add8BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add8BytesToolStripMenuItem.Text = "Add 8 Bytes";
      this.add8BytesToolStripMenuItem.Value = 8;
      this.add8BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add64BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_64;
      this.add64BytesToolStripMenuItem.Name = "add64BytesToolStripMenuItem";
      this.add64BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add64BytesToolStripMenuItem.Text = "Add 64 Bytes";
      this.add64BytesToolStripMenuItem.Value = 64;
      this.add64BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add256BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_256;
      this.add256BytesToolStripMenuItem.Name = "add256BytesToolStripMenuItem";
      this.add256BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add256BytesToolStripMenuItem.Text = "Add 256 Bytes";
      this.add256BytesToolStripMenuItem.Value = 256;
      this.add256BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add1024BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_1024;
      this.add1024BytesToolStripMenuItem.Name = "add1024BytesToolStripMenuItem";
      this.add1024BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add1024BytesToolStripMenuItem.Text = "Add 1024 Bytes";
      this.add1024BytesToolStripMenuItem.Value = 1024;
      this.add1024BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add2048BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_2048;
      this.add2048BytesToolStripMenuItem.Name = "add2048BytesToolStripMenuItem";
      this.add2048BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add2048BytesToolStripMenuItem.Text = "Add 2048 Bytes";
      this.add2048BytesToolStripMenuItem.Value = 2048;
      this.add2048BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.add4096BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_4096;
      this.add4096BytesToolStripMenuItem.Name = "add4096BytesToolStripMenuItem";
      this.add4096BytesToolStripMenuItem.Size = new Size(154, 22);
      this.add4096BytesToolStripMenuItem.Text = "Add 4096 Bytes";
      this.add4096BytesToolStripMenuItem.Value = 4096;
      this.add4096BytesToolStripMenuItem.Click += new EventHandler(this.addBytesToolStripMenuItem_Click);
      this.addXBytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Add_Bytes_X;
      this.addXBytesToolStripMenuItem.Name = "addXBytesToolStripMenuItem";
      this.addXBytesToolStripMenuItem.Size = new Size(154, 22);
      this.addXBytesToolStripMenuItem.Text = "Add ... Bytes";
      this.addXBytesToolStripMenuItem.Click += new EventHandler(this.addXBytesToolStripMenuItem_Click);
      this.insertBytesToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.insertBytesToolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[8]
      {
        (ToolStripItem) this.insert4BytesToolStripMenuItem,
        (ToolStripItem) this.insert8BytesToolStripMenuItem,
        (ToolStripItem) this.insert64BytesToolStripMenuItem,
        (ToolStripItem) this.insert256BytesToolStripMenuItem,
        (ToolStripItem) this.insert1024BytesToolStripMenuItem,
        (ToolStripItem) this.insert2048BytesToolStripMenuItem,
        (ToolStripItem) this.insert4096BytesToolStripMenuItem,
        (ToolStripItem) this.insertXBytesToolStripMenuItem
      });
      this.insertBytesToolStripDropDownButton.Image = (Image) Resources.B16x16_Button_Insert_Bytes_X;
      this.insertBytesToolStripDropDownButton.ImageTransparentColor = Color.Magenta;
      this.insertBytesToolStripDropDownButton.Name = "insertBytesToolStripDropDownButton";
      this.insertBytesToolStripDropDownButton.Size = new Size(29, 22);
      this.insertBytesToolStripDropDownButton.ToolTipText = "Insert bytes at selected position";
      this.insert4BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_4;
      this.insert4BytesToolStripMenuItem.Name = "insert4BytesToolStripMenuItem";
      this.insert4BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert4BytesToolStripMenuItem.Tag = (object) "";
      this.insert4BytesToolStripMenuItem.Text = "Insert 4 Bytes";
      this.insert4BytesToolStripMenuItem.Value = 4;
      this.insert4BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert8BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_8;
      this.insert8BytesToolStripMenuItem.Name = "insert8BytesToolStripMenuItem";
      this.insert8BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert8BytesToolStripMenuItem.Text = "Insert 8 Bytes";
      this.insert8BytesToolStripMenuItem.Value = 8;
      this.insert8BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert64BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_64;
      this.insert64BytesToolStripMenuItem.Name = "insert64BytesToolStripMenuItem";
      this.insert64BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert64BytesToolStripMenuItem.Text = "Insert 64 Bytes";
      this.insert64BytesToolStripMenuItem.Value = 64;
      this.insert64BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert256BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_256;
      this.insert256BytesToolStripMenuItem.Name = "insert256BytesToolStripMenuItem";
      this.insert256BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert256BytesToolStripMenuItem.Text = "Insert 256 Bytes";
      this.insert256BytesToolStripMenuItem.Value = 256;
      this.insert256BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert1024BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_1024;
      this.insert1024BytesToolStripMenuItem.Name = "insert1024BytesToolStripMenuItem";
      this.insert1024BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert1024BytesToolStripMenuItem.Text = "Insert 1024 Bytes";
      this.insert1024BytesToolStripMenuItem.Value = 1024;
      this.insert1024BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert2048BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_2048;
      this.insert2048BytesToolStripMenuItem.Name = "insert2048BytesToolStripMenuItem";
      this.insert2048BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert2048BytesToolStripMenuItem.Text = "Insert 2048 Bytes";
      this.insert2048BytesToolStripMenuItem.Value = 2048;
      this.insert2048BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insert4096BytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_4096;
      this.insert4096BytesToolStripMenuItem.Name = "insert4096BytesToolStripMenuItem";
      this.insert4096BytesToolStripMenuItem.Size = new Size(161, 22);
      this.insert4096BytesToolStripMenuItem.Text = "Insert 4096 Bytes";
      this.insert4096BytesToolStripMenuItem.Value = 4096;
      this.insert4096BytesToolStripMenuItem.Click += new EventHandler(this.insertBytesToolStripMenuItem_Click);
      this.insertXBytesToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Insert_Bytes_X;
      this.insertXBytesToolStripMenuItem.Name = "insertXBytesToolStripMenuItem";
      this.insertXBytesToolStripMenuItem.Size = new Size(161, 22);
      this.insertXBytesToolStripMenuItem.Text = "Insert ... Bytes";
      this.insertXBytesToolStripMenuItem.Click += new EventHandler(this.insertXBytesToolStripMenuItem_Click);
      this.nodeTypesToolStripSeparator.Name = "nodeTypesToolStripSeparator";
      this.nodeTypesToolStripSeparator.Size = new Size(6, 25);
      this.mainMenuStrip.BackColor = Color.FromArgb(50, 53, 55);
      this.mainMenuStrip.Items.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.fileToolStripMenuItem,
        (ToolStripItem) this.projectToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem,
        (ToolStripItem) this.toolStripMenuItem3,
        (ToolStripItem) this.toolStripTextBox1,
        (ToolStripItem) this.toolStripComboBox1
      });
      this.mainMenuStrip.Location = new Point(0, 0);
      this.mainMenuStrip.Name = "mainMenuStrip";
      this.mainMenuStrip.Size = new Size(1141, 27);
      this.mainMenuStrip.TabIndex = 2;
      this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[6]
      {
        (ToolStripItem) this.openProjectToolStripMenuItem,
        (ToolStripItem) this.mergeWithProjectToolStripMenuItem,
        (ToolStripItem) this.clearProjectToolStripMenuItem,
        (ToolStripItem) this.saveToolStripMenuItem,
        (ToolStripItem) this.saveAsToolStripMenuItem,
        (ToolStripItem) this.quitToolStripMenuItem
      });
      this.fileToolStripMenuItem.ForeColor = Color.White;
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new Size(37, 23);
      this.fileToolStripMenuItem.Text = "File";
      this.fileToolStripMenuItem.DropDownOpening += new EventHandler(this.fileToolStripMenuItem_DropDownOpening);
      this.openProjectToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.openProjectToolStripMenuItem.ForeColor = Color.White;
      this.openProjectToolStripMenuItem.Image = (Image) Resources.B16x16_Folder;
      this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
      this.openProjectToolStripMenuItem.ShortcutKeys = Keys.O | Keys.Control;
      this.openProjectToolStripMenuItem.Size = new Size(195, 22);
      this.openProjectToolStripMenuItem.Text = "Open Project...";
      this.openProjectToolStripMenuItem.Click += new EventHandler(this.openProjectToolStripMenuItem_Click);
      this.mergeWithProjectToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.mergeWithProjectToolStripMenuItem.ForeColor = Color.White;
      this.mergeWithProjectToolStripMenuItem.Image = (Image) Resources.B16x16_Folder_Add;
      this.mergeWithProjectToolStripMenuItem.Name = "mergeWithProjectToolStripMenuItem";
      this.mergeWithProjectToolStripMenuItem.Size = new Size(195, 22);
      this.mergeWithProjectToolStripMenuItem.Text = "Merge with Project...";
      this.mergeWithProjectToolStripMenuItem.Click += new EventHandler(this.mergeWithProjectToolStripMenuItem_Click);
      this.clearProjectToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.clearProjectToolStripMenuItem.ForeColor = Color.White;
      this.clearProjectToolStripMenuItem.Image = (Image) Resources.B16x16_Arrow_Refresh;
      this.clearProjectToolStripMenuItem.Name = "clearProjectToolStripMenuItem";
      this.clearProjectToolStripMenuItem.Size = new Size(195, 22);
      this.clearProjectToolStripMenuItem.Text = "Clear Project";
      this.clearProjectToolStripMenuItem.Click += new EventHandler(this.clearProjectToolStripMenuItem_Click);
      this.saveToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.saveToolStripMenuItem.ForeColor = Color.White;
      this.saveToolStripMenuItem.Image = (Image) Resources.B16x16_Save;
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.ShortcutKeys = Keys.S | Keys.Control;
      this.saveToolStripMenuItem.Size = new Size(195, 22);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
      this.saveAsToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.saveAsToolStripMenuItem.ForeColor = Color.White;
      this.saveAsToolStripMenuItem.Image = (Image) Resources.B16x16_Save_As;
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.ShortcutKeys = Keys.S | Keys.Shift | Keys.Control;
      this.saveAsToolStripMenuItem.Size = new Size(195, 22);
      this.saveAsToolStripMenuItem.Text = "Save as...";
      this.saveAsToolStripMenuItem.Click += new EventHandler(this.saveAsToolStripMenuItem_Click);
      this.quitToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.quitToolStripMenuItem.ForeColor = Color.White;
      this.quitToolStripMenuItem.Image = (Image) Resources.B16x16_Quit;
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new Size(195, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new EventHandler(this.quitToolStripMenuItem_Click);
      this.projectToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.goToClassToolStripMenuItem,
        (ToolStripItem) this.cleanUnusedClassesToolStripMenuItem,
        (ToolStripItem) this.showEnumsToolStripMenuItem,
        (ToolStripItem) this.generateCppCodeToolStripMenuItem,
        (ToolStripItem) this.generateCSharpCodeToolStripMenuItem
      });
      this.projectToolStripMenuItem.ForeColor = Color.White;
      this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
      this.projectToolStripMenuItem.Size = new Size(56, 23);
      this.projectToolStripMenuItem.Text = "Project";
      this.goToClassToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.goToClassToolStripMenuItem.ForeColor = Color.White;
      this.goToClassToolStripMenuItem.Image = (Image) Resources.B16x16_Class_Type;
      this.goToClassToolStripMenuItem.Name = "goToClassToolStripMenuItem";
      this.goToClassToolStripMenuItem.ShortcutKeys = Keys.F | Keys.Control;
      this.goToClassToolStripMenuItem.Size = new Size(198, 22);
      this.goToClassToolStripMenuItem.Text = "Go to class...";
      this.goToClassToolStripMenuItem.Click += new EventHandler(this.goToClassToolStripMenuItem_Click);
      this.cleanUnusedClassesToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.cleanUnusedClassesToolStripMenuItem.ForeColor = Color.White;
      this.cleanUnusedClassesToolStripMenuItem.Image = (Image) Resources.B16x16_Chart_Delete;
      this.cleanUnusedClassesToolStripMenuItem.Name = "cleanUnusedClassesToolStripMenuItem";
      this.cleanUnusedClassesToolStripMenuItem.Size = new Size(198, 22);
      this.cleanUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
      this.cleanUnusedClassesToolStripMenuItem.Click += new EventHandler(this.cleanUnusedClassesToolStripMenuItem_Click);
      this.showEnumsToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.showEnumsToolStripMenuItem.ForeColor = Color.White;
      this.showEnumsToolStripMenuItem.Image = (Image) Resources.B16x16_Category;
      this.showEnumsToolStripMenuItem.Name = "showEnumsToolStripMenuItem";
      this.showEnumsToolStripMenuItem.Size = new Size(198, 22);
      this.showEnumsToolStripMenuItem.Text = "Show Enums...";
      this.showEnumsToolStripMenuItem.Click += new EventHandler(this.showEnumsToolStripMenuItem_Click);
      this.generateCppCodeToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.generateCppCodeToolStripMenuItem.ForeColor = Color.White;
      this.generateCppCodeToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Code_Cpp;
      this.generateCppCodeToolStripMenuItem.Name = "generateCppCodeToolStripMenuItem";
      this.generateCppCodeToolStripMenuItem.Size = new Size(198, 22);
      this.generateCppCodeToolStripMenuItem.Text = "Generate C++ Code...";
      this.generateCppCodeToolStripMenuItem.Click += new EventHandler(this.generateCppCodeToolStripMenuItem_Click);
      this.generateCSharpCodeToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.generateCSharpCodeToolStripMenuItem.ForeColor = Color.White;
      this.generateCSharpCodeToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Code_Csharp;
      this.generateCSharpCodeToolStripMenuItem.Name = "generateCSharpCodeToolStripMenuItem";
      this.generateCSharpCodeToolStripMenuItem.Size = new Size(198, 22);
      this.generateCSharpCodeToolStripMenuItem.Text = "Generate C# Code...";
      this.generateCSharpCodeToolStripMenuItem.Click += new EventHandler(this.generateCSharpCodeToolStripMenuItem_Click);
      this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.aboutToolStripMenuItem,
        (ToolStripItem) this.socketListenerToolStripMenuItem
      });
      this.helpToolStripMenuItem.ForeColor = Color.White;
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new Size(77, 23);
      this.helpToolStripMenuItem.Text = "More Tools";
      this.aboutToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.aboutToolStripMenuItem.ForeColor = Color.White;
      this.aboutToolStripMenuItem.Image = (Image) Resources.B16x16_Information;
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new Size(180, 22);
      this.aboutToolStripMenuItem.Text = "About...";
      this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
      this.socketListenerToolStripMenuItem.BackColor = Color.FromArgb(60, 63, 65);
      this.socketListenerToolStripMenuItem.ForeColor = Color.White;
      this.socketListenerToolStripMenuItem.Name = "socketListenerToolStripMenuItem";
      this.socketListenerToolStripMenuItem.Size = new Size(180, 22);
      this.socketListenerToolStripMenuItem.Text = "Socket Listener";
      this.socketListenerToolStripMenuItem.Click += new EventHandler(this.socketListenerToolStripMenuItem_Click);
      this.toolStripMenuItem3.ForeColor = Color.White;
      this.toolStripMenuItem3.Name = "toolStripMenuItem3";
      this.toolStripMenuItem3.Size = new Size(64, 23);
      this.toolStripMenuItem3.Text = "Connect";
      this.toolStripMenuItem3.Click += new EventHandler(this.toolStripMenuItem3_Click);
      this.toolStripTextBox1.BackColor = Color.FromArgb(69, 73, 74);
      this.toolStripTextBox1.BorderStyle = BorderStyle.None;
      this.toolStripTextBox1.Font = new Font("Segoe UI", 9f);
      this.toolStripTextBox1.ForeColor = Color.White;
      this.toolStripTextBox1.Name = "toolStripTextBox1";
      this.toolStripTextBox1.Size = new Size(100, 23);
      this.toolStripTextBox1.Text = "192.168.1.20";
      this.toolStripTextBox1.Click += new EventHandler(this.toolStripTextBox1_Click);
      this.toolStripComboBox1.BackColor = Color.FromArgb(69, 73, 74);
      this.toolStripComboBox1.ForeColor = Color.White;
      this.toolStripComboBox1.Name = "toolStripComboBox1";
      this.toolStripComboBox1.Size = new Size(121, 23);
      this.toolStripComboBox1.Text = "Select Process1";
      this.toolStripComboBox1.DropDown += new EventHandler(this.toolStripComboBox1_DropDown);
      this.toolStripComboBox1.Click += new EventHandler(this.toolStripComboBox1_Click);
      this.toolStripComboBox1.TextChanged += new EventHandler(this.toolStripComboBox1_TextChanged);
      this.AllowDrop = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.ClientSize = new Size(1141, 595);
      this.Controls.Add((Control) this.splitContainer);
      this.Controls.Add((Control) this.toolStrip);
      this.Controls.Add((Control) this.mainMenuStrip);
      this.ForeColor = Color.White;
      this.MainMenuStrip = this.mainMenuStrip;
      this.MinimumSize = new Size(200, 100);
      this.Name = nameof (MainForm);
      this.Text = "ReClass.NET";
      this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
      this.Load += new EventHandler(this.MainForm_Load);
      this.DragDrop += new DragEventHandler(this.MainForm_DragDrop);
      this.DragEnter += new DragEventHandler(this.MainForm_DragEnter);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.EndInit();
      this.splitContainer.ResumeLayout(false);
      this.projectClassContextMenuStrip.ResumeLayout(false);
      this.projectClassesContextMenuStrip.ResumeLayout(false);
      this.projectEnumContextMenuStrip.ResumeLayout(false);
      this.projectEnumsContextMenuStrip.ResumeLayout(false);
      this.selectedNodeContextMenuStrip.ResumeLayout(false);
      this.toolStrip.ResumeLayout(false);
      this.toolStrip.PerformLayout();
      this.mainMenuStrip.ResumeLayout(false);
      this.mainMenuStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
