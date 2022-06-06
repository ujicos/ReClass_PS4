// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.InputCorrelatorForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Input;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class InputCorrelatorForm : IconForm
  {
    private static readonly TimeSpan refineInterval = TimeSpan.FromMilliseconds(400.0);
    private readonly ScannerForm scannerForm;
    private readonly RemoteProcess process;
    private readonly KeyboardInput input;
    private InputCorrelatedScanner scanner;
    private bool isScanning;
    private DateTime lastRefineTime;
    private IContainer components;
    private System.Windows.Forms.Timer refineTimer;
    private BannerBox bannerBox;
    private GroupBox settingsGroupBox;
    private HotkeyBox hotkeyBox;
    private ListBox hotkeyListBox;
    private Button removeButton;
    private Button addButton;
    private ScannerForm.ScanValueTypeComboBox valueTypeComboBox;
    private Label label1;
    private Button startStopButton;
    private Label infoLabel;

    public InputCorrelatorForm(ScannerForm scannerForm, RemoteProcess process)
    {
      this.scannerForm = scannerForm;
      this.process = process;
      this.InitializeComponent();
      this.valueTypeComboBox.SetAvailableValues(ScanValueType.Byte, ScanValueType.Short, ScanValueType.Integer, ScanValueType.Long, ScanValueType.Float, ScanValueType.Double);
      this.valueTypeComboBox.SelectedValue = ScanValueType.Integer;
      this.input = new KeyboardInput();
      this.hotkeyBox.Input = this.input;
      this.infoLabel.Text = string.Empty;
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

    private async void InputCorrelatorForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      InputCorrelatorForm inputCorrelatorForm = this;
      inputCorrelatorForm.hotkeyBox.Input = (KeyboardInput) null;
      inputCorrelatorForm.refineTimer.Enabled = false;
      if (inputCorrelatorForm.isScanning)
      {
        e.Cancel = true;
        inputCorrelatorForm.Hide();
        await Task.Delay(TimeSpan.FromSeconds(1.0));
        inputCorrelatorForm.Close();
      }
      else
      {
        inputCorrelatorForm.scanner?.Dispose();
        inputCorrelatorForm.input?.Dispose();
      }
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      KeyboardHotkey keyboardHotkey = this.hotkeyBox.Hotkey.Clone();
      if (keyboardHotkey.IsEmpty)
        return;
      this.hotkeyListBox.Items.Add((object) keyboardHotkey);
      this.hotkeyBox.Clear();
    }

    private void removeButton_Click(object sender, EventArgs e)
    {
      int selectedIndex = this.hotkeyListBox.SelectedIndex;
      if (selectedIndex < 0)
        return;
      this.hotkeyListBox.Items.RemoveAt(selectedIndex);
    }

    private async void startStopButton_Click(object sender, EventArgs e)
    {
      if (this.scanner == null)
      {
        if (this.hotkeyListBox.Items.Count == 0)
        {
          int num = (int) MessageBox.Show("Please add at least one hotkey.", "ReClass.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        this.scanner = new InputCorrelatedScanner(this.process, this.input, this.hotkeyListBox.Items.Cast<KeyboardHotkey>(), this.valueTypeComboBox.SelectedValue);
        this.settingsGroupBox.Enabled = false;
        try
        {
          await this.scanner.Initialize();
          this.startStopButton.Text = "Stop Scan";
          this.refineTimer.Enabled = true;
          return;
        }
        catch (Exception ex)
        {
          Program.ShowException(ex);
        }
      }
      else
      {
        this.refineTimer.Enabled = false;
        this.startStopButton.Text = "Start Scan";
        while (this.isScanning)
          await Task.Delay(TimeSpan.FromSeconds(1.0));
        this.scannerForm.ShowScannerResults((Scanner) this.scanner);
        this.scanner.Dispose();
        this.scanner = (InputCorrelatedScanner) null;
      }
      this.settingsGroupBox.Enabled = true;
    }

    private async void refineTimer_Tick(object sender, EventArgs e)
    {
      if (this.isScanning)
        return;
      this.scanner.CorrelateInput();
      if (!(this.lastRefineTime + InputCorrelatorForm.refineInterval < DateTime.Now))
        return;
      this.isScanning = true;
      try
      {
        await this.scanner.RefineResults(CancellationToken.None, (IProgress<int>) null);
        this.infoLabel.Text = string.Format("Scan Count: {0} Possible Values: {1}", (object) this.scanner.ScanCount, (object) this.scanner.TotalResultCount);
      }
      catch (Exception ex)
      {
        Program.ShowException(ex);
      }
      this.isScanning = false;
      this.lastRefineTime = DateTime.Now;
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
      this.refineTimer = new System.Windows.Forms.Timer(this.components);
      this.bannerBox = new BannerBox();
      this.settingsGroupBox = new GroupBox();
      this.removeButton = new Button();
      this.addButton = new Button();
      this.valueTypeComboBox = new ScannerForm.ScanValueTypeComboBox();
      this.label1 = new Label();
      this.hotkeyListBox = new ListBox();
      this.hotkeyBox = new HotkeyBox();
      this.startStopButton = new Button();
      this.infoLabel = new Label();
      this.bannerBox.BeginInit();
      this.settingsGroupBox.SuspendLayout();
      this.SuspendLayout();
      this.refineTimer.Interval = 50;
      this.refineTimer.Tick += new EventHandler(this.refineTimer_Tick);
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_Canvas_Size;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(301, 48);
      this.bannerBox.TabIndex = 8;
      this.bannerBox.Text = "Scan for values correlated to input.";
      this.bannerBox.Title = "Input Correlator";
      this.settingsGroupBox.Controls.Add((Control) this.removeButton);
      this.settingsGroupBox.Controls.Add((Control) this.addButton);
      this.settingsGroupBox.Controls.Add((Control) this.valueTypeComboBox);
      this.settingsGroupBox.Controls.Add((Control) this.label1);
      this.settingsGroupBox.Controls.Add((Control) this.hotkeyListBox);
      this.settingsGroupBox.Controls.Add((Control) this.hotkeyBox);
      this.settingsGroupBox.Location = new Point(7, 54);
      this.settingsGroupBox.Name = "settingsGroupBox";
      this.settingsGroupBox.Size = new Size(288, 179);
      this.settingsGroupBox.TabIndex = 9;
      this.settingsGroupBox.TabStop = false;
      this.settingsGroupBox.Text = "Settings";
      this.removeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removeButton.Location = new Point(203, 45);
      this.removeButton.Name = "removeButton";
      this.removeButton.Size = new Size(79, 23);
      this.removeButton.TabIndex = 13;
      this.removeButton.Text = "Remove Key";
      this.removeButton.UseVisualStyleBackColor = true;
      this.removeButton.Click += new EventHandler(this.removeButton_Click);
      this.addButton.Location = new Point(6, 45);
      this.addButton.Name = "addButton";
      this.addButton.Size = new Size(58, 23);
      this.addButton.TabIndex = 12;
      this.addButton.Text = "Add Key";
      this.addButton.UseVisualStyleBackColor = true;
      this.addButton.Click += new EventHandler(this.addButton_Click);
      this.valueTypeComboBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.valueTypeComboBox.Location = new Point(66, 149);
      this.valueTypeComboBox.Name = "valueTypeComboBox";
      this.valueTypeComboBox.Size = new Size(216, 21);
      this.valueTypeComboBox.TabIndex = 11;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 152);
      this.label1.Name = "label1";
      this.label1.Size = new Size(64, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "Value Type:";
      this.hotkeyListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.hotkeyListBox.FormattingEnabled = true;
      this.hotkeyListBox.Location = new Point(6, 74);
      this.hotkeyListBox.Name = "hotkeyListBox";
      this.hotkeyListBox.Size = new Size(276, 69);
      this.hotkeyListBox.TabIndex = 11;
      this.hotkeyBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.hotkeyBox.Input = (KeyboardInput) null;
      this.hotkeyBox.Location = new Point(6, 19);
      this.hotkeyBox.Name = "hotkeyBox";
      this.hotkeyBox.Size = new Size(276, 20);
      this.hotkeyBox.TabIndex = 10;
      this.startStopButton.Location = new Point(7, 239);
      this.startStopButton.Name = "startStopButton";
      this.startStopButton.Size = new Size(288, 23);
      this.startStopButton.TabIndex = 13;
      this.startStopButton.Text = "Start Scan";
      this.startStopButton.UseVisualStyleBackColor = true;
      this.startStopButton.Click += new EventHandler(this.startStopButton_Click);
      this.infoLabel.AutoSize = true;
      this.infoLabel.Location = new Point(4, 265);
      this.infoLabel.Name = "infoLabel";
      this.infoLabel.Size = new Size(19, 13);
      this.infoLabel.TabIndex = 11;
      this.infoLabel.Text = "<>";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(301, 287);
      this.Controls.Add((Control) this.infoLabel);
      this.Controls.Add((Control) this.startStopButton);
      this.Controls.Add((Control) this.settingsGroupBox);
      this.Controls.Add((Control) this.bannerBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InputCorrelatorForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Input Correlator";
      this.FormClosing += new FormClosingEventHandler(this.InputCorrelatorForm_FormClosing);
      this.bannerBox.EndInit();
      this.settingsGroupBox.ResumeLayout(false);
      this.settingsGroupBox.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
