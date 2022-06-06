// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.FoundCodeForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class FoundCodeForm : IconForm
  {
    private volatile bool acceptNewRecords = true;
    private readonly RemoteProcess process;
    private readonly DataTable data;
    private IContainer components;
    private SplitContainer splitContainer;
    private DataGridView foundCodeDataGridView;
    private Button stopButton;
    private DataGridViewTextBoxColumn counterDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn instructionDataGridViewTextBoxColumn;
    private Button closeButton;
    private TextBox infoTextBox;
    private Button createFunctionButton;
    private BannerBox bannerBox;

    public event FoundCodeForm.StopEventHandler Stop;

    public FoundCodeForm(RemoteProcess process, IntPtr address, HardwareBreakpointTrigger trigger)
    {
      this.process = process;
      this.InitializeComponent();
      this.foundCodeDataGridView.AutoGenerateColumns = false;
      this.infoTextBox.Font = new Font(FontFamily.GenericMonospace, this.infoTextBox.Font.Size);
      if (trigger == HardwareBreakpointTrigger.Write)
        this.Text = "Find out what writes to " + address.ToString("X016");
      else
        this.Text = "Find out what accesses " + address.ToString("X016");
      this.bannerBox.Text = this.Text;
      this.data = new DataTable();
      this.data.Columns.Add("counter", typeof (int));
      this.data.Columns.Add("instruction", typeof (string));
      this.data.Columns.Add("info", typeof (FoundCodeForm.FoundCodeInfo));
      this.foundCodeDataGridView.DataSource = (object) this.data;
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      GlobalWindowManager.AddWindow((Form) this);
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
      base.OnFormClosed(e);
      GlobalWindowManager.RemoveWindow((Form) this);
    }

    private void foundCodeDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      FoundCodeForm.FoundCodeInfo selectedInfo = this.GetSelectedInfo();
      if (selectedInfo == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < 5; ++index)
      {
        string str = selectedInfo.Instructions[index].Address.ToString("X016") + " - " + selectedInfo.Instructions[index].Instruction;
        if (index == 2)
          stringBuilder.AppendLine(str + " <<<");
        else
          stringBuilder.AppendLine(str);
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("RAX = " + selectedInfo.DebugInfo.Registers.Rax.ToString("X016"));
      stringBuilder.AppendLine("RBX = " + selectedInfo.DebugInfo.Registers.Rbx.ToString("X016"));
      stringBuilder.AppendLine("RCX = " + selectedInfo.DebugInfo.Registers.Rcx.ToString("X016"));
      stringBuilder.AppendLine("RDX = " + selectedInfo.DebugInfo.Registers.Rdx.ToString("X016"));
      stringBuilder.AppendLine("RDI = " + selectedInfo.DebugInfo.Registers.Rdi.ToString("X016"));
      stringBuilder.AppendLine("RSI = " + selectedInfo.DebugInfo.Registers.Rsi.ToString("X016"));
      stringBuilder.AppendLine("RSP = " + selectedInfo.DebugInfo.Registers.Rsp.ToString("X016"));
      stringBuilder.AppendLine("RBP = " + selectedInfo.DebugInfo.Registers.Rbp.ToString("X016"));
      stringBuilder.AppendLine("RIP = " + selectedInfo.DebugInfo.Registers.Rip.ToString("X016"));
      stringBuilder.AppendLine("R8  = " + selectedInfo.DebugInfo.Registers.R8.ToString("X016"));
      stringBuilder.AppendLine("R9  = " + selectedInfo.DebugInfo.Registers.R9.ToString("X016"));
      stringBuilder.AppendLine("R10 = " + selectedInfo.DebugInfo.Registers.R10.ToString("X016"));
      stringBuilder.AppendLine("R11 = " + selectedInfo.DebugInfo.Registers.R11.ToString("X016"));
      stringBuilder.AppendLine("R12 = " + selectedInfo.DebugInfo.Registers.R12.ToString("X016"));
      stringBuilder.AppendLine("R13 = " + selectedInfo.DebugInfo.Registers.R13.ToString("X016"));
      stringBuilder.AppendLine("R14 = " + selectedInfo.DebugInfo.Registers.R14.ToString("X016"));
      stringBuilder.Append("R15 = " + selectedInfo.DebugInfo.Registers.R15.ToString("X016"));
      this.infoTextBox.Text = stringBuilder.ToString();
    }

    private void FoundCodeForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.StopRecording();
    }

    private void createFunctionButton_Click(object sender, EventArgs e)
    {
      FoundCodeForm.FoundCodeInfo selectedInfo = this.GetSelectedInfo();
      if (selectedInfo == null)
        return;
      IntPtr functionStartAddress = new Disassembler(this.process.CoreFunctions).RemoteGetFunctionStartAddress((IRemoteMemoryReader) this.process, selectedInfo.DebugInfo.ExceptionAddress);
      if (functionStartAddress.IsNull())
      {
        int num = (int) MessageBox.Show("Could not find the start of the function. Aborting.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        ClassNode classAtAddress = LinkedWindowFeatures.CreateClassAtAddress(functionStartAddress, false);
        ReClassNET.Nodes.FunctionNode functionNode = new ReClassNET.Nodes.FunctionNode();
        functionNode.Comment = selectedInfo.Instructions[2].Instruction;
        classAtAddress.AddNode((BaseNode) functionNode);
      }
    }

    private void stopButton_Click(object sender, EventArgs e)
    {
      this.StopRecording();
      this.stopButton.Visible = false;
      this.closeButton.Visible = true;
    }

    private void closeButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private FoundCodeForm.FoundCodeInfo GetSelectedInfo()
    {
      return (this.foundCodeDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault<DataGridViewRow>()?.DataBoundItem is DataRowView dataBoundItem ? dataBoundItem["info"] : (object) null) as FoundCodeForm.FoundCodeInfo;
    }

    private void StopRecording()
    {
      this.acceptNewRecords = false;
      FoundCodeForm.StopEventHandler stop = this.Stop;
      if (stop == null)
        return;
      stop((object) this, EventArgs.Empty);
    }

    public void AddRecord(ExceptionDebugInfo? context)
    {
      if (!context.HasValue || !this.acceptNewRecords)
        return;
      if (this.InvokeRequired)
      {
        this.Invoke((Delegate) (() => this.AddRecord(context)));
      }
      else
      {
        DataRow row1 = this.data.AsEnumerable().FirstOrDefault<DataRow>((Func<DataRow, bool>) (r => r.Field<FoundCodeForm.FoundCodeInfo>("info").DebugInfo.ExceptionAddress == context.Value.ExceptionAddress));
        if (row1 != null)
        {
          row1["counter"] = (object) (row1.Field<int>("counter") + 1);
        }
        else
        {
          Disassembler disassembler = new Disassembler(this.process.CoreFunctions);
          DisassembledInstruction previousInstruction = disassembler.RemoteGetPreviousInstruction((IRemoteMemoryReader) this.process, context.Value.ExceptionAddress);
          if (previousInstruction == null)
            return;
          DisassembledInstruction[] disassembledInstructionArray = new DisassembledInstruction[5];
          disassembledInstructionArray[2] = previousInstruction;
          disassembledInstructionArray[1] = disassembler.RemoteGetPreviousInstruction((IRemoteMemoryReader) this.process, disassembledInstructionArray[2].Address);
          disassembledInstructionArray[0] = disassembler.RemoteGetPreviousInstruction((IRemoteMemoryReader) this.process, disassembledInstructionArray[1].Address);
          int num = 3;
          foreach (DisassembledInstruction disassembledInstruction in (IEnumerable<DisassembledInstruction>) disassembler.RemoteDisassembleCode((IRemoteMemoryReader) this.process, context.Value.ExceptionAddress, 30, 2))
            disassembledInstructionArray[num++] = disassembledInstruction;
          DataRow row2 = this.data.NewRow();
          row2["counter"] = (object) 1;
          row2["instruction"] = (object) previousInstruction.Instruction;
          row2["info"] = (object) new FoundCodeForm.FoundCodeInfo()
          {
            DebugInfo = context.Value,
            Instructions = disassembledInstructionArray
          };
          this.data.Rows.Add(row2);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.splitContainer = new SplitContainer();
      this.foundCodeDataGridView = new DataGridView();
      this.counterDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.instructionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.infoTextBox = new TextBox();
      this.stopButton = new Button();
      this.closeButton = new Button();
      this.createFunctionButton = new Button();
      this.bannerBox = new BannerBox();
      this.splitContainer.BeginInit();
      this.splitContainer.Panel1.SuspendLayout();
      this.splitContainer.Panel2.SuspendLayout();
      this.splitContainer.SuspendLayout();
      ((ISupportInitialize) this.foundCodeDataGridView).BeginInit();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.splitContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer.Location = new Point(0, 49);
      this.splitContainer.Name = "splitContainer";
      this.splitContainer.Orientation = Orientation.Horizontal;
      this.splitContainer.Panel1.Controls.Add((Control) this.foundCodeDataGridView);
      this.splitContainer.Panel2.Controls.Add((Control) this.infoTextBox);
      this.splitContainer.Size = new Size(476, 426);
      this.splitContainer.SplitterDistance = 200;
      this.splitContainer.TabIndex = 0;
      this.foundCodeDataGridView.AllowUserToAddRows = false;
      this.foundCodeDataGridView.AllowUserToDeleteRows = false;
      this.foundCodeDataGridView.AllowUserToResizeRows = false;
      this.foundCodeDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
      this.foundCodeDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.foundCodeDataGridView.Columns.AddRange((DataGridViewColumn) this.counterDataGridViewTextBoxColumn, (DataGridViewColumn) this.instructionDataGridViewTextBoxColumn);
      this.foundCodeDataGridView.Dock = DockStyle.Fill;
      this.foundCodeDataGridView.Location = new Point(0, 0);
      this.foundCodeDataGridView.MultiSelect = false;
      this.foundCodeDataGridView.Name = "foundCodeDataGridView";
      this.foundCodeDataGridView.ReadOnly = true;
      this.foundCodeDataGridView.RowHeadersVisible = false;
      this.foundCodeDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.foundCodeDataGridView.Size = new Size(476, 200);
      this.foundCodeDataGridView.TabIndex = 0;
      this.foundCodeDataGridView.SelectionChanged += new EventHandler(this.foundCodeDataGridView_SelectionChanged);
      this.counterDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.counterDataGridViewTextBoxColumn.DataPropertyName = "counter";
      this.counterDataGridViewTextBoxColumn.HeaderText = "Counter";
      this.counterDataGridViewTextBoxColumn.Name = "counterDataGridViewTextBoxColumn";
      this.counterDataGridViewTextBoxColumn.ReadOnly = true;
      this.counterDataGridViewTextBoxColumn.Width = 69;
      this.instructionDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.instructionDataGridViewTextBoxColumn.DataPropertyName = "instruction";
      this.instructionDataGridViewTextBoxColumn.HeaderText = "Instruction";
      this.instructionDataGridViewTextBoxColumn.Name = "instructionDataGridViewTextBoxColumn";
      this.instructionDataGridViewTextBoxColumn.ReadOnly = true;
      this.infoTextBox.Dock = DockStyle.Fill;
      this.infoTextBox.Location = new Point(0, 0);
      this.infoTextBox.Multiline = true;
      this.infoTextBox.Name = "infoTextBox";
      this.infoTextBox.ReadOnly = true;
      this.infoTextBox.ScrollBars = ScrollBars.Vertical;
      this.infoTextBox.Size = new Size(476, 222);
      this.infoTextBox.TabIndex = 0;
      this.stopButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stopButton.Location = new Point(489, 101);
      this.stopButton.Name = "stopButton";
      this.stopButton.Size = new Size(86, 35);
      this.stopButton.TabIndex = 1;
      this.stopButton.Text = "Stop";
      this.stopButton.UseVisualStyleBackColor = true;
      this.stopButton.Click += new EventHandler(this.stopButton_Click);
      this.closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.closeButton.Location = new Point(489, 101);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new Size(86, 35);
      this.closeButton.TabIndex = 2;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      this.closeButton.Visible = false;
      this.closeButton.Click += new EventHandler(this.closeButton_Click);
      this.createFunctionButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.createFunctionButton.Location = new Point(489, 60);
      this.createFunctionButton.Name = "createFunctionButton";
      this.createFunctionButton.Size = new Size(86, 35);
      this.createFunctionButton.TabIndex = 3;
      this.createFunctionButton.Text = "Create Function Node";
      this.createFunctionButton.UseVisualStyleBackColor = true;
      this.createFunctionButton.Click += new EventHandler(this.createFunctionButton_Click);
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_3D_Glasses;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(587, 48);
      this.bannerBox.TabIndex = 8;
      this.bannerBox.Text = "<>";
      this.bannerBox.Title = "Instruction Finder";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(587, 474);
      this.Controls.Add((Control) this.bannerBox);
      this.Controls.Add((Control) this.createFunctionButton);
      this.Controls.Add((Control) this.closeButton);
      this.Controls.Add((Control) this.stopButton);
      this.Controls.Add((Control) this.splitContainer);
      this.MinimumSize = new Size(603, 464);
      this.Name = nameof (FoundCodeForm);
      this.Text = "<>";
      this.FormClosed += new FormClosedEventHandler(this.FoundCodeForm_FormClosed);
      this.splitContainer.Panel1.ResumeLayout(false);
      this.splitContainer.Panel2.ResumeLayout(false);
      this.splitContainer.Panel2.PerformLayout();
      this.splitContainer.EndInit();
      this.splitContainer.ResumeLayout(false);
      ((ISupportInitialize) this.foundCodeDataGridView).EndInit();
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
    }

    private class FoundCodeInfo
    {
      public ExceptionDebugInfo DebugInfo { get; set; }

      public DisassembledInstruction[] Instructions { get; set; }
    }

    public delegate void StopEventHandler(object sender, EventArgs e);
  }
}
