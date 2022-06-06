// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.ScannerForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.DataExchange.Scanner;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Properties;
using ReClassNET.UI;
using ReClassNET.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class ScannerForm : IconForm
  {
    private const int MaxVisibleResults = 10000;
    private readonly RemoteProcess process;
    private bool isFirstScan;
    private ReClassNET.MemoryScanner.Scanner scanner;
    private CancellationTokenSource cts;
    private string addressFilePath;
    private IContainer components;
    private BannerBox bannerBox;
    private GroupBox filterGroupBox;
    private DualValueBox dualValueBox;
    private CheckBox isHexCheckBox;
    private ScannerForm.ScanCompareTypeComboBox compareTypeComboBox;
    private Label label1;
    private ScannerForm.ScanValueTypeComboBox valueTypeComboBox;
    private Label label3;
    private GroupBox scanOptionsGroupBox;
    private TextBox fastScanAlignmentTextBox;
    private CheckBox fastScanCheckBox;
    private CheckBox scanCopyOnWriteCheckBox;
    private CheckBox scanExecutableCheckBox;
    private CheckBox scanWritableCheckBox;
    private CheckBox scanMappedCheckBox;
    private CheckBox scanImageCheckBox;
    private CheckBox scanPrivateCheckBox;
    private TextBox stopAddressTextBox;
    private Label label4;
    private TextBox startAddressTextBox;
    private Label label2;
    private FlowLayoutPanel flowLayoutPanel;
    private GroupBox floatingOptionsGroupBox;
    private RadioButton roundTruncateRadioButton;
    private RadioButton roundLooseRadioButton;
    private RadioButton roundStrictRadioButton;
    private GroupBox stringOptionsGroupBox;
    private CheckBox caseSensitiveCheckBox;
    private RadioButton encodingUtf32RadioButton;
    private RadioButton encodingUtf16RadioButton;
    private RadioButton encodingUtf8RadioButton;
    private Button firstScanButton;
    private Button nextScanButton;
    private ProgressBar scanProgressBar;
    private Label resultCountLabel;
    private System.Windows.Forms.Timer updateValuesTimer;
    private MemoryRecordList resultMemoryRecordList;
    private MemoryRecordList addressListMemoryRecordList;
    private ToolStripPanel toolStripPanel;
    private ToolStrip menuToolStrip;
    private ToolStripButton openAddressFileToolStripButton;
    private ToolStripButton saveAddressFileToolStripButton;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripButton clearAddressListToolStripButton;
    private ToolTip infoToolTip;
    private ToolStripButton saveAddressFileAsToolStripButton;
    private ContextMenuStrip resultListContextMenuStrip;
    private ToolStripMenuItem addSelectedResultsToAddressListToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripMenuItem setCurrentClassAddressToolStripMenuItem;
    private ToolStripMenuItem createClassAtAddressToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripMenuItem findOutWhatAccessesThisAddressToolStripMenuItem;
    private ToolStripMenuItem findOutWhatWritesToThisAddressToolStripMenuItem;
    private ToolStripMenuItem removeSelectedRecordsToolStripMenuItem;
    private ToolStripMenuItem changeToolStripMenuItem;
    private ToolStripMenuItem descriptionToolStripMenuItem;
    private ToolStripMenuItem addressToolStripMenuItem;
    private ToolStripMenuItem valueTypeToolStripMenuItem;
    private ToolStripMenuItem valueToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator4;
    private ToolStripMenuItem copyAddressToolStripMenuItem;
    private IconButton undoIconButton;
    private IconButton showInputCorrelatorIconButton;
    private IconButton cancelScanIconButton;

    public ScannerForm(RemoteProcess process)
    {
      this.process = process;
      this.InitializeComponent();
      this.toolStripPanel.Renderer = (ToolStripRenderer) new CustomToolStripProfessionalRenderer(true, false);
      this.menuToolStrip.Renderer = (ToolStripRenderer) new CustomToolStripProfessionalRenderer(false, false);
      this.SetGuiFromSettings(ScanSettings.Default);
      this.OnValueTypeChanged();
      this.Reset();
      this.firstScanButton.Enabled = this.flowLayoutPanel.Enabled = process.IsValid;
      process.ProcessAttached += new RemoteProcessEvent(this.RemoteProcessOnProcessAttached);
      process.ProcessClosing += new RemoteProcessEvent(this.RemoteProcessOnProcessClosing);
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

    private void RemoteProcessOnProcessAttached(RemoteProcess remoteProcess)
    {
      this.firstScanButton.Enabled = this.nextScanButton.Enabled = this.flowLayoutPanel.Enabled = true;
      this.Reset();
      if (!this.addressListMemoryRecordList.Records.Any<MemoryRecord>())
        return;
      if (MessageBox.Show("Keep the current address list?", "Process has changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
      {
        this.addressListMemoryRecordList.Clear();
      }
      else
      {
        foreach (MemoryRecord record in (IEnumerable<MemoryRecord>) this.addressListMemoryRecordList.Records)
        {
          record.ResolveAddress(this.process);
          record.RefreshValue(this.process);
        }
      }
    }

    private void RemoteProcessOnProcessClosing(RemoteProcess remoteProcess)
    {
      this.Reset();
      this.firstScanButton.Enabled = this.nextScanButton.Enabled = this.flowLayoutPanel.Enabled = false;
    }

    private void MemorySearchForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.scanner?.Dispose();
      this.process.ProcessAttached -= new RemoteProcessEvent(this.RemoteProcessOnProcessAttached);
      this.process.ProcessClosing -= new RemoteProcessEvent(this.RemoteProcessOnProcessClosing);
    }

    private void updateValuesTimer_Tick(object sender, EventArgs e)
    {
      this.resultMemoryRecordList.RefreshValues(this.process);
      this.addressListMemoryRecordList.RefreshValues(this.process);
    }

    private void scanTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.OnCompareTypeChanged();
    }

    private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.OnValueTypeChanged();
    }

    private async void firstScanButton_Click(object sender, EventArgs e)
    {
      int num;
      if (num == 0 || this.isFirstScan)
      {
        try
        {
          ScanSettings searchSettings = this.CreateSearchSettings();
          IScanComparer comparer = this.CreateComparer(searchSettings);
          await this.StartFirstScanEx(searchSettings, comparer);
        }
        catch (Exception ex)
        {
          Program.ShowException(ex);
        }
      }
      else
        this.Reset();
    }

    private async void nextScanButton_Click(object sender, EventArgs e)
    {
      ScannerForm scannerForm = this;
      if (!scannerForm.process.IsValid || scannerForm.isFirstScan)
        return;
      scannerForm.firstScanButton.Enabled = false;
      scannerForm.nextScanButton.Enabled = false;
      scannerForm.cancelScanIconButton.Visible = true;
      try
      {
        IScanComparer comparer = scannerForm.CreateComparer(scannerForm.scanner.Settings);
        // ISSUE: reference to a compiler-generated method
        Progress<int> progress = new Progress<int>(new Action<int>(scannerForm.\u003CnextScanButton_Click\u003Eb__18_0));
        scannerForm.cts = new CancellationTokenSource();
        int num = await scannerForm.scanner.Search(comparer, (IProgress<int>) progress, scannerForm.cts.Token) ? 1 : 0;
        scannerForm.ShowScannerResults(scannerForm.scanner);
        scannerForm.undoIconButton.Enabled = scannerForm.scanner.CanUndoLastScan;
      }
      catch (Exception ex)
      {
        Program.ShowException(ex);
      }
      scannerForm.firstScanButton.Enabled = true;
      scannerForm.nextScanButton.Enabled = true;
      scannerForm.cancelScanIconButton.Visible = false;
      scannerForm.scanProgressBar.Value = 0;
    }

    private void cancelScanIconButton_Click(object sender, EventArgs e)
    {
      this.cts?.Cancel();
    }

    private void memorySearchResultControl_ResultDoubleClick(object sender, MemoryRecord record)
    {
      this.addressListMemoryRecordList.Records.Add(record);
    }

    private void openAddressFileToolStripButton_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.CheckFileExists = true;
      openFileDialog1.Filter = "All Scanner Types |*.rcnetscan;*.ct;*.csat|ReClass.NET Scanner File (*.rcnetscan)|*.rcnetscan|Cheat Engine Tables (*.ct)|*.ct|CrySearch Address Tables (*.csat)|*.csat";
      using (OpenFileDialog openFileDialog2 = openFileDialog1)
      {
        if (openFileDialog2.ShowDialog() != DialogResult.OK)
          return;
        IScannerImport scannerImport = (IScannerImport) null;
        string lower = Path.GetExtension(openFileDialog2.FileName)?.ToLower();
        if (!(lower == ".rcnetscan"))
        {
          if (!(lower == ".ct"))
          {
            if (lower == ".csat")
              scannerImport = (IScannerImport) new CrySearchFile();
            else
              Program.Logger.Log(ReClassNET.Logger.LogLevel.Error, "The file '" + openFileDialog2.FileName + "' has an unknown type.");
          }
          else
            scannerImport = (IScannerImport) new CheatEngineFile();
        }
        else
          scannerImport = (IScannerImport) new ReClassScanFile();
        if (scannerImport == null || this.addressListMemoryRecordList.Records.Any<MemoryRecord>() && MessageBox.Show("The address list contains addresses. Do you really want to open the file?", "ReClass.NET Scanner", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        if (scannerImport is ReClassScanFile)
          this.addressFilePath = openFileDialog2.FileName;
        this.addressListMemoryRecordList.SetRecords(scannerImport.Load(openFileDialog2.FileName, Program.Logger).Select<MemoryRecord, MemoryRecord>((Func<MemoryRecord, MemoryRecord>) (r =>
        {
          r.ResolveAddress(this.process);
          r.RefreshValue(this.process);
          return r;
        })));
      }
    }

    private void saveAddressFileToolStripButton_Click(object sender, EventArgs e)
    {
      if (this.addressListMemoryRecordList.Records.None<MemoryRecord>())
        return;
      if (string.IsNullOrEmpty(this.addressFilePath))
        this.saveAsToolStripButton_Click(sender, e);
      else
        new ReClassScanFile().Save((IEnumerable<MemoryRecord>) this.addressListMemoryRecordList.Records, this.addressFilePath, Program.Logger);
    }

    private void saveAsToolStripButton_Click(object sender, EventArgs e)
    {
      if (this.addressListMemoryRecordList.Records.None<MemoryRecord>())
        return;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();
      saveFileDialog1.DefaultExt = ".rcnetscan";
      saveFileDialog1.Filter = "ReClass.NET Scanner File (*.rcnetscan)|*.rcnetscan";
      using (SaveFileDialog saveFileDialog2 = saveFileDialog1)
      {
        if (saveFileDialog2.ShowDialog() != DialogResult.OK)
          return;
        this.addressFilePath = saveFileDialog2.FileName;
        this.saveAddressFileToolStripButton_Click(sender, e);
      }
    }

    private void clearAddressListToolStripButton_Click(object sender, EventArgs e)
    {
      this.addressListMemoryRecordList.Clear();
    }

    private void showInputCorrelatorIconButton_Click(object sender, EventArgs e)
    {
      new InputCorrelatorForm(this, this.process).Show();
    }

    private void resultListContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      ContextMenuStrip contextMenuStrip = (ContextMenuStrip) sender;
      bool flag1 = contextMenuStrip.SourceControl.Parent == this.resultMemoryRecordList;
      this.addSelectedResultsToAddressListToolStripMenuItem.Visible = flag1;
      this.changeToolStripMenuItem.Visible = !flag1;
      this.removeSelectedRecordsToolStripMenuItem.Visible = !flag1;
      bool flag2 = (flag1 ? this.resultMemoryRecordList.SelectedRecords.Count : this.addressListMemoryRecordList.SelectedRecords.Count) > 1;
      for (int index = 3; index < contextMenuStrip.Items.Count; ++index)
        contextMenuStrip.Items[index].Visible = !flag2;
    }

    private static MemoryRecordList GetMemoryRecordListFromMenuItem(object sender)
    {
      return (MemoryRecordList) ((ContextMenuStrip) ((ToolStripItem) sender).Owner).SourceControl.Parent;
    }

    private void addSelectedResultsToAddressListToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (MemoryRecord selectedRecord in (IEnumerable<MemoryRecord>) this.resultMemoryRecordList.SelectedRecords)
        this.addressListMemoryRecordList.Records.Add(selectedRecord);
    }

    private void removeSelectedRecordsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (MemoryRecord selectedRecord in (IEnumerable<MemoryRecord>) this.addressListMemoryRecordList.SelectedRecords)
        this.addressListMemoryRecordList.Records.Remove(selectedRecord);
    }

    private void setCurrentClassAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LinkedWindowFeatures.SetCurrentClassAddress(ScannerForm.GetMemoryRecordListFromMenuItem(sender).SelectedRecord.RealAddress);
    }

    private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LinkedWindowFeatures.CreateClassAtAddress(ScannerForm.GetMemoryRecordListFromMenuItem(sender).SelectedRecord.RealAddress, true);
    }

    private void findOutWhatAccessesThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScannerForm.FindWhatInteractsWithSelectedRecord(ScannerForm.GetMemoryRecordListFromMenuItem(sender).SelectedRecord, false);
    }

    private void findOutWhatWritesToThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ScannerForm.FindWhatInteractsWithSelectedRecord(ScannerForm.GetMemoryRecordListFromMenuItem(sender).SelectedRecord, true);
    }

    private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MemoryRecord selectedRecord = ScannerForm.GetMemoryRecordListFromMenuItem(sender)?.SelectedRecord;
      if (selectedRecord == null)
        return;
      Clipboard.SetText(selectedRecord.RealAddress.ToString("X"));
    }

    private void undoIconButton_Click(object sender, EventArgs e)
    {
      if (this.scanner.CanUndoLastScan)
      {
        this.scanner.UndoLastScan();
        this.ShowScannerResults(this.scanner);
      }
      this.undoIconButton.Enabled = this.scanner.CanUndoLastScan;
    }

    private void SetResultCount(int count)
    {
      this.resultCountLabel.Text = count > 10000 ? string.Format("Found: {0} (only {1} shown)", (object) count, (object) 10000) : string.Format("Found: {0}", (object) count);
    }

    public void ShowScannerResults(ReClassNET.MemoryScanner.Scanner scanner)
    {
      this.SetResultCount(scanner.TotalResultCount);
      this.resultMemoryRecordList.SetRecords(scanner.GetResults().Take<ScanResult>(10000).OrderBy<ScanResult, IntPtr>((Func<ScanResult, IntPtr>) (r => r.Address), (IComparer<IntPtr>) IntPtrComparer.Instance).Select<ScanResult, MemoryRecord>((Func<ScanResult, MemoryRecord>) (r =>
      {
        MemoryRecord memoryRecord = new MemoryRecord(r);
        memoryRecord.ResolveAddress(this.process);
        return memoryRecord;
      })));
    }

    private void OnCompareTypeChanged()
    {
      bool flag1 = true;
      bool flag2 = true;
      bool flag3 = false;
      switch (this.compareTypeComboBox.SelectedValue)
      {
        case ScanCompareType.Between:
        case ScanCompareType.BetweenOrEqual:
          flag3 = true;
          break;
        case ScanCompareType.Unknown:
          flag1 = false;
          flag2 = false;
          break;
      }
      switch (this.valueTypeComboBox.SelectedValue)
      {
        case ScanValueType.Float:
        case ScanValueType.Double:
        case ScanValueType.ArrayOfBytes:
        case ScanValueType.String:
        case ScanValueType.Regex:
          this.isHexCheckBox.Checked = false;
          flag1 = false;
          break;
      }
      this.isHexCheckBox.Enabled = flag1;
      this.dualValueBox.Enabled = flag2;
      this.dualValueBox.ShowSecondInputField = flag3;
    }

    private void OnValueTypeChanged()
    {
      this.SetValidCompareTypes();
      ScanValueType selectedValue = this.valueTypeComboBox.SelectedValue;
      switch (selectedValue)
      {
        case ScanValueType.Byte:
        case ScanValueType.Short:
        case ScanValueType.Integer:
        case ScanValueType.Long:
          this.isHexCheckBox.Enabled = true;
          break;
        case ScanValueType.Float:
        case ScanValueType.Double:
        case ScanValueType.ArrayOfBytes:
        case ScanValueType.String:
        case ScanValueType.Regex:
          this.isHexCheckBox.Checked = false;
          this.isHexCheckBox.Enabled = false;
          break;
      }
      int num = 1;
      if (selectedValue != ScanValueType.Short)
      {
        if ((uint) (selectedValue - 2) <= 3U)
          num = 4;
      }
      else
        num = 2;
      this.fastScanAlignmentTextBox.Text = num.ToString();
      this.floatingOptionsGroupBox.Visible = selectedValue == ScanValueType.Float || selectedValue == ScanValueType.Double;
      this.stringOptionsGroupBox.Visible = selectedValue == ScanValueType.String || selectedValue == ScanValueType.Regex;
    }

    private void SetValidCompareTypes()
    {
      ScanCompareType selectedValue = this.compareTypeComboBox.SelectedValue;
      switch (this.valueTypeComboBox.SelectedValue)
      {
        case ScanValueType.ArrayOfBytes:
        case ScanValueType.String:
        case ScanValueType.Regex:
          this.compareTypeComboBox.SetAvailableValues(ScanCompareType.Equal);
          break;
        default:
          if (this.isFirstScan)
          {
            this.compareTypeComboBox.SetAvailableValuesExclude(ScanCompareType.Changed, ScanCompareType.NotChanged, ScanCompareType.Decreased, ScanCompareType.DecreasedOrEqual, ScanCompareType.Increased, ScanCompareType.IncreasedOrEqual);
            break;
          }
          this.compareTypeComboBox.SetAvailableValuesExclude(ScanCompareType.Unknown);
          break;
      }
      this.compareTypeComboBox.SelectedValue = selectedValue;
    }

    private void Reset()
    {
      this.scanner?.Dispose();
      this.scanner = (ReClassNET.MemoryScanner.Scanner) null;
      this.undoIconButton.Enabled = false;
      this.SetResultCount(0);
      this.resultMemoryRecordList.Clear();
      this.firstScanButton.Enabled = true;
      this.nextScanButton.Enabled = false;
      this.isHexCheckBox.Enabled = true;
      this.valueTypeComboBox.Enabled = true;
      this.OnValueTypeChanged();
      this.floatingOptionsGroupBox.Enabled = true;
      this.stringOptionsGroupBox.Enabled = true;
      this.scanOptionsGroupBox.Enabled = true;
      this.isFirstScan = true;
      this.undoIconButton.Enabled = false;
      this.SetValidCompareTypes();
    }

    public void ExcuteScan(ScanSettings settings, IScanComparer comparer)
    {
      this.Reset();
      this.SetGuiFromSettings(settings);
      this.Invoke((Delegate) (async () => await this.StartFirstScanEx(settings, comparer)));
    }

    private async Task StartFirstScanEx(ScanSettings settings, IScanComparer comparer)
    {
      ScannerForm scannerForm = this;
      if (!scannerForm.process.IsValid)
        return;
      scannerForm.firstScanButton.Enabled = false;
      scannerForm.cancelScanIconButton.Visible = true;
      try
      {
        scannerForm.scanner = new ReClassNET.MemoryScanner.Scanner(scannerForm.process, settings);
        // ISSUE: reference to a compiler-generated method
        Progress<int> progress = new Progress<int>(new Action<int>(scannerForm.\u003CStartFirstScanEx\u003Eb__43_0));
        scannerForm.cts = new CancellationTokenSource();
        int num = await scannerForm.scanner.Search(comparer, (IProgress<int>) progress, scannerForm.cts.Token) ? 1 : 0;
        scannerForm.ShowScannerResults(scannerForm.scanner);
        scannerForm.cancelScanIconButton.Visible = false;
        scannerForm.nextScanButton.Enabled = true;
        scannerForm.valueTypeComboBox.Enabled = false;
        scannerForm.floatingOptionsGroupBox.Enabled = false;
        scannerForm.stringOptionsGroupBox.Enabled = false;
        scannerForm.scanOptionsGroupBox.Enabled = false;
        scannerForm.isFirstScan = false;
        scannerForm.SetValidCompareTypes();
        scannerForm.OnCompareTypeChanged();
      }
      finally
      {
        scannerForm.firstScanButton.Enabled = true;
        scannerForm.scanProgressBar.Value = 0;
      }
    }

    private ScanSettings CreateSearchSettings()
    {
      ScanSettings scanSettings = new ScanSettings();
      scanSettings.ValueType = this.valueTypeComboBox.SelectedValue;
      long result1;
      long.TryParse(this.startAddressTextBox.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result1);
      long result2;
      long.TryParse(this.stopAddressTextBox.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result2);
      scanSettings.StartAddress = (IntPtr) result1;
      scanSettings.StopAddress = (IntPtr) result2;
      scanSettings.EnableFastScan = this.fastScanCheckBox.Checked;
      int result3;
      int.TryParse(this.fastScanAlignmentTextBox.Text, out result3);
      scanSettings.FastScanAlignment = Math.Max(1, result3);
      scanSettings.ScanPrivateMemory = this.scanPrivateCheckBox.Checked;
      scanSettings.ScanImageMemory = this.scanImageCheckBox.Checked;
      scanSettings.ScanMappedMemory = this.scanMappedCheckBox.Checked;
      scanSettings.ScanWritableMemory = CheckStateToSettingState(this.scanWritableCheckBox.CheckState);
      scanSettings.ScanExecutableMemory = CheckStateToSettingState(this.scanExecutableCheckBox.CheckState);
      scanSettings.ScanCopyOnWriteMemory = CheckStateToSettingState(this.scanCopyOnWriteCheckBox.CheckState);
      return scanSettings;

      SettingState CheckStateToSettingState(CheckState state)
      {
        if (state == CheckState.Unchecked)
          return SettingState.No;
        return state == CheckState.Checked ? SettingState.Yes : SettingState.Indeterminate;
      }
    }

    private void SetGuiFromSettings(ScanSettings settings)
    {
      this.valueTypeComboBox.SelectedValue = settings.ValueType;
      this.startAddressTextBox.Text = settings.StartAddress.ToString("X016");
      this.stopAddressTextBox.Text = settings.StopAddress.ToString("X016");
      this.fastScanCheckBox.Checked = settings.EnableFastScan;
      this.fastScanAlignmentTextBox.Text = Math.Max(1, settings.FastScanAlignment).ToString();
      this.scanPrivateCheckBox.Checked = settings.ScanPrivateMemory;
      this.scanImageCheckBox.Checked = settings.ScanImageMemory;
      this.scanMappedCheckBox.Checked = settings.ScanMappedMemory;
      this.scanWritableCheckBox.CheckState = SettingStateToCheckState(settings.ScanWritableMemory);
      this.scanExecutableCheckBox.CheckState = SettingStateToCheckState(settings.ScanExecutableMemory);
      this.scanCopyOnWriteCheckBox.CheckState = SettingStateToCheckState(settings.ScanCopyOnWriteMemory);

      CheckState SettingStateToCheckState(SettingState state)
      {
        if (state == SettingState.Yes)
          return CheckState.Checked;
        return state == SettingState.No ? CheckState.Unchecked : CheckState.Indeterminate;
      }
    }

    private IScanComparer CreateComparer(ScanSettings settings)
    {
      ScanCompareType selectedValue = this.compareTypeComboBox.SelectedValue;
      bool flag = selectedValue == ScanCompareType.Between || selectedValue == ScanCompareType.BetweenOrEqual;
      if (settings.ValueType == ScanValueType.Byte || settings.ValueType == ScanValueType.Short || (settings.ValueType == ScanValueType.Integer || settings.ValueType == ScanValueType.Long))
      {
        NumberStyles style = this.isHexCheckBox.Checked ? NumberStyles.HexNumber : NumberStyles.Integer;
        long result1;
        if (!long.TryParse(this.dualValueBox.Value1, style, (IFormatProvider) null, out result1))
          throw new InvalidInputException(this.dualValueBox.Value1);
        long result2;
        if (!long.TryParse(this.dualValueBox.Value2, style, (IFormatProvider) null, out result2) & flag)
          throw new InvalidInputException(this.dualValueBox.Value2);
        if ((selectedValue == ScanCompareType.Between || selectedValue == ScanCompareType.BetweenOrEqual) && result1 > result2)
          Utils.Swap<long>(ref result1, ref result2);
        switch (settings.ValueType)
        {
          case ScanValueType.Byte:
            return (IScanComparer) new ByteMemoryComparer(selectedValue, (byte) result1, (byte) result2);
          case ScanValueType.Short:
            return (IScanComparer) new ShortMemoryComparer(selectedValue, (short) result1, (short) result2, this.process.BitConverter);
          case ScanValueType.Integer:
            return (IScanComparer) new IntegerMemoryComparer(selectedValue, (int) result1, (int) result2, this.process.BitConverter);
          case ScanValueType.Long:
            return (IScanComparer) new LongMemoryComparer(selectedValue, result1, result2, this.process.BitConverter);
        }
      }
      else if (settings.ValueType == ScanValueType.Float || settings.ValueType == ScanValueType.Double)
      {
        NumberFormatInfo numberFormat1 = NumberFormat.GuessNumberFormat(this.dualValueBox.Value1);
        double result1;
        if (!double.TryParse(this.dualValueBox.Value1, NumberStyles.Float, (IFormatProvider) numberFormat1, out result1))
          throw new InvalidInputException(this.dualValueBox.Value1);
        NumberFormatInfo numberFormat2 = NumberFormat.GuessNumberFormat(this.dualValueBox.Value2);
        double result2;
        if (!double.TryParse(this.dualValueBox.Value2, NumberStyles.Float, (IFormatProvider) numberFormat2, out result2) & flag)
          throw new InvalidInputException(this.dualValueBox.Value2);
        if ((selectedValue == ScanCompareType.Between || selectedValue == ScanCompareType.BetweenOrEqual) && result1 > result2)
          Utils.Swap<double>(ref result1, ref result2);
        int significantDigits = Math.Max(CalculateSignificantDigits(this.dualValueBox.Value1, numberFormat1), CalculateSignificantDigits(this.dualValueBox.Value2, numberFormat2));
        ScanRoundMode roundType = this.roundStrictRadioButton.Checked ? ScanRoundMode.Strict : (this.roundLooseRadioButton.Checked ? ScanRoundMode.Normal : ScanRoundMode.Truncate);
        switch (settings.ValueType)
        {
          case ScanValueType.Float:
            return (IScanComparer) new FloatMemoryComparer(selectedValue, roundType, significantDigits, (float) result1, (float) result2, this.process.BitConverter);
          case ScanValueType.Double:
            return (IScanComparer) new DoubleMemoryComparer(selectedValue, roundType, significantDigits, result1, result2, this.process.BitConverter);
        }
      }
      else
      {
        if (settings.ValueType == ScanValueType.ArrayOfBytes)
          return (IScanComparer) new ArrayOfBytesMemoryComparer(BytePattern.Parse(this.dualValueBox.Value1));
        if (settings.ValueType == ScanValueType.String || settings.ValueType == ScanValueType.Regex)
        {
          if (string.IsNullOrEmpty(this.dualValueBox.Value1))
            throw new InvalidInputException(this.dualValueBox.Value1);
          Encoding encoding = this.encodingUtf8RadioButton.Checked ? Encoding.UTF8 : (this.encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32);
          return settings.ValueType == ScanValueType.String ? (IScanComparer) new StringMemoryComparer(this.dualValueBox.Value1, encoding, this.caseSensitiveCheckBox.Checked) : (IScanComparer) new RegexStringMemoryComparer(this.dualValueBox.Value1, encoding, this.caseSensitiveCheckBox.Checked);
        }
      }
      throw new InvalidOperationException();

      int CalculateSignificantDigits(string input, NumberFormatInfo numberFormat)
      {
        int num1 = 0;
        int num2 = input.IndexOf(numberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
        if (num2 != -1)
          num1 = input.Length - 1 - num2;
        return num1;
      }
    }

    private static void FindWhatInteractsWithSelectedRecord(MemoryRecord record, bool writeOnly)
    {
      int size;
      switch (record.ValueType)
      {
        case ScanValueType.Byte:
          size = 1;
          break;
        case ScanValueType.Short:
          size = 2;
          break;
        case ScanValueType.Integer:
        case ScanValueType.Float:
          size = 4;
          break;
        case ScanValueType.Long:
        case ScanValueType.Double:
          size = 8;
          break;
        case ScanValueType.ArrayOfBytes:
          size = record.ValueLength;
          break;
        case ScanValueType.String:
        case ScanValueType.Regex:
          size = record.ValueLength;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      LinkedWindowFeatures.FindWhatInteractsWithAddress(record.RealAddress, size, writeOnly);
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
      this.bannerBox = new BannerBox();
      this.filterGroupBox = new GroupBox();
      this.valueTypeComboBox = new ScannerForm.ScanValueTypeComboBox();
      this.label3 = new Label();
      this.compareTypeComboBox = new ScannerForm.ScanCompareTypeComboBox();
      this.label1 = new Label();
      this.isHexCheckBox = new CheckBox();
      this.dualValueBox = new DualValueBox();
      this.scanOptionsGroupBox = new GroupBox();
      this.fastScanAlignmentTextBox = new TextBox();
      this.fastScanCheckBox = new CheckBox();
      this.scanCopyOnWriteCheckBox = new CheckBox();
      this.scanExecutableCheckBox = new CheckBox();
      this.scanWritableCheckBox = new CheckBox();
      this.scanMappedCheckBox = new CheckBox();
      this.scanImageCheckBox = new CheckBox();
      this.scanPrivateCheckBox = new CheckBox();
      this.stopAddressTextBox = new TextBox();
      this.label4 = new Label();
      this.startAddressTextBox = new TextBox();
      this.label2 = new Label();
      this.flowLayoutPanel = new FlowLayoutPanel();
      this.floatingOptionsGroupBox = new GroupBox();
      this.roundTruncateRadioButton = new RadioButton();
      this.roundLooseRadioButton = new RadioButton();
      this.roundStrictRadioButton = new RadioButton();
      this.stringOptionsGroupBox = new GroupBox();
      this.caseSensitiveCheckBox = new CheckBox();
      this.encodingUtf32RadioButton = new RadioButton();
      this.encodingUtf16RadioButton = new RadioButton();
      this.encodingUtf8RadioButton = new RadioButton();
      this.firstScanButton = new Button();
      this.nextScanButton = new Button();
      this.scanProgressBar = new ProgressBar();
      this.resultCountLabel = new Label();
      this.updateValuesTimer = new System.Windows.Forms.Timer(this.components);
      this.resultMemoryRecordList = new MemoryRecordList();
      this.resultListContextMenuStrip = new ContextMenuStrip(this.components);
      this.addSelectedResultsToAddressListToolStripMenuItem = new ToolStripMenuItem();
      this.removeSelectedRecordsToolStripMenuItem = new ToolStripMenuItem();
      this.changeToolStripMenuItem = new ToolStripMenuItem();
      this.descriptionToolStripMenuItem = new ToolStripMenuItem();
      this.addressToolStripMenuItem = new ToolStripMenuItem();
      this.valueTypeToolStripMenuItem = new ToolStripMenuItem();
      this.valueToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.setCurrentClassAddressToolStripMenuItem = new ToolStripMenuItem();
      this.createClassAtAddressToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.findOutWhatAccessesThisAddressToolStripMenuItem = new ToolStripMenuItem();
      this.findOutWhatWritesToThisAddressToolStripMenuItem = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.copyAddressToolStripMenuItem = new ToolStripMenuItem();
      this.addressListMemoryRecordList = new MemoryRecordList();
      this.toolStripPanel = new ToolStripPanel();
      this.menuToolStrip = new ToolStrip();
      this.openAddressFileToolStripButton = new ToolStripButton();
      this.saveAddressFileToolStripButton = new ToolStripButton();
      this.saveAddressFileAsToolStripButton = new ToolStripButton();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.clearAddressListToolStripButton = new ToolStripButton();
      this.infoToolTip = new ToolTip(this.components);
      this.undoIconButton = new IconButton();
      this.showInputCorrelatorIconButton = new IconButton();
      this.cancelScanIconButton = new IconButton();
      this.bannerBox.BeginInit();
      this.filterGroupBox.SuspendLayout();
      this.scanOptionsGroupBox.SuspendLayout();
      this.flowLayoutPanel.SuspendLayout();
      this.floatingOptionsGroupBox.SuspendLayout();
      this.stringOptionsGroupBox.SuspendLayout();
      this.resultListContextMenuStrip.SuspendLayout();
      this.toolStripPanel.SuspendLayout();
      this.menuToolStrip.SuspendLayout();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_Eye;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(612, 48);
      this.bannerBox.TabIndex = 7;
      this.bannerBox.Text = "Scan the process memory for specific values.";
      this.bannerBox.Title = "Scanner";
      this.filterGroupBox.Controls.Add((Control) this.valueTypeComboBox);
      this.filterGroupBox.Controls.Add((Control) this.label3);
      this.filterGroupBox.Controls.Add((Control) this.compareTypeComboBox);
      this.filterGroupBox.Controls.Add((Control) this.label1);
      this.filterGroupBox.Controls.Add((Control) this.isHexCheckBox);
      this.filterGroupBox.Controls.Add((Control) this.dualValueBox);
      this.filterGroupBox.Location = new Point(3, 3);
      this.filterGroupBox.Margin = new Padding(3, 3, 3, 1);
      this.filterGroupBox.Name = "filterGroupBox";
      this.filterGroupBox.Size = new Size(308, 103);
      this.filterGroupBox.TabIndex = 8;
      this.filterGroupBox.TabStop = false;
      this.filterGroupBox.Text = "Filter";
      this.valueTypeComboBox.Location = new Point(72, 74);
      this.valueTypeComboBox.Name = "valueTypeComboBox";
      this.valueTypeComboBox.Size = new Size(224, 21);
      this.valueTypeComboBox.TabIndex = 8;
      this.valueTypeComboBox.SelectionChangeCommitted += new EventHandler(this.valueTypeComboBox_SelectionChangeCommitted);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 77);
      this.label3.Name = "label3";
      this.label3.Size = new Size(64, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "Value Type:";
      this.compareTypeComboBox.Location = new Point(72, 50);
      this.compareTypeComboBox.Name = "compareTypeComboBox";
      this.compareTypeComboBox.Size = new Size(224, 21);
      this.compareTypeComboBox.TabIndex = 5;
      this.compareTypeComboBox.SelectionChangeCommitted += new EventHandler(this.scanTypeComboBox_SelectionChangeCommitted);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 53);
      this.label1.Name = "label1";
      this.label1.Size = new Size(62, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Scan Type:";
      this.isHexCheckBox.AutoSize = true;
      this.isHexCheckBox.Location = new Point(6, 28);
      this.isHexCheckBox.Name = "isHexCheckBox";
      this.isHexCheckBox.RightToLeft = RightToLeft.Yes;
      this.isHexCheckBox.Size = new Size(56, 17);
      this.isHexCheckBox.TabIndex = 3;
      this.isHexCheckBox.Text = "Is Hex";
      this.isHexCheckBox.UseVisualStyleBackColor = true;
      this.dualValueBox.Location = new Point(72, 12);
      this.dualValueBox.Name = "dualValueBox";
      this.dualValueBox.ShowSecondInputField = false;
      this.dualValueBox.Size = new Size(224, 34);
      this.dualValueBox.TabIndex = 2;
      this.dualValueBox.Value1 = "0";
      this.dualValueBox.Value2 = "0";
      this.scanOptionsGroupBox.Controls.Add((Control) this.fastScanAlignmentTextBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.fastScanCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanCopyOnWriteCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanExecutableCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanWritableCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanMappedCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanImageCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.scanPrivateCheckBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.stopAddressTextBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.label4);
      this.scanOptionsGroupBox.Controls.Add((Control) this.startAddressTextBox);
      this.scanOptionsGroupBox.Controls.Add((Control) this.label2);
      this.scanOptionsGroupBox.Location = new Point(3, 239);
      this.scanOptionsGroupBox.Margin = new Padding(3, 0, 3, 3);
      this.scanOptionsGroupBox.Name = "scanOptionsGroupBox";
      this.scanOptionsGroupBox.Size = new Size(308, 141);
      this.scanOptionsGroupBox.TabIndex = 9;
      this.scanOptionsGroupBox.TabStop = false;
      this.scanOptionsGroupBox.Text = "Scan Options";
      this.fastScanAlignmentTextBox.Location = new Point(144, 112);
      this.fastScanAlignmentTextBox.Name = "fastScanAlignmentTextBox";
      this.fastScanAlignmentTextBox.Size = new Size(26, 20);
      this.fastScanAlignmentTextBox.TabIndex = 11;
      this.fastScanCheckBox.AutoSize = true;
      this.fastScanCheckBox.Checked = true;
      this.fastScanCheckBox.CheckState = CheckState.Checked;
      this.fastScanCheckBox.Location = new Point(9, 114);
      this.fastScanCheckBox.Name = "fastScanCheckBox";
      this.fastScanCheckBox.Size = new Size(129, 17);
      this.fastScanCheckBox.TabIndex = 10;
      this.fastScanCheckBox.Text = "Fast Scan, Alignment:";
      this.fastScanCheckBox.UseVisualStyleBackColor = true;
      this.scanCopyOnWriteCheckBox.AutoSize = true;
      this.scanCopyOnWriteCheckBox.Location = new Point(189, 91);
      this.scanCopyOnWriteCheckBox.Name = "scanCopyOnWriteCheckBox";
      this.scanCopyOnWriteCheckBox.Size = new Size(95, 17);
      this.scanCopyOnWriteCheckBox.TabIndex = 9;
      this.scanCopyOnWriteCheckBox.Text = "Copy On Write";
      this.scanCopyOnWriteCheckBox.ThreeState = true;
      this.scanCopyOnWriteCheckBox.UseVisualStyleBackColor = true;
      this.scanExecutableCheckBox.AutoSize = true;
      this.scanExecutableCheckBox.Checked = true;
      this.scanExecutableCheckBox.CheckState = CheckState.Indeterminate;
      this.scanExecutableCheckBox.Location = new Point(91, 91);
      this.scanExecutableCheckBox.Name = "scanExecutableCheckBox";
      this.scanExecutableCheckBox.Size = new Size(79, 17);
      this.scanExecutableCheckBox.TabIndex = 8;
      this.scanExecutableCheckBox.Text = "Executable";
      this.scanExecutableCheckBox.ThreeState = true;
      this.scanExecutableCheckBox.UseVisualStyleBackColor = true;
      this.scanWritableCheckBox.AutoSize = true;
      this.scanWritableCheckBox.Checked = true;
      this.scanWritableCheckBox.CheckState = CheckState.Checked;
      this.scanWritableCheckBox.Location = new Point(9, 91);
      this.scanWritableCheckBox.Name = "scanWritableCheckBox";
      this.scanWritableCheckBox.Size = new Size(65, 17);
      this.scanWritableCheckBox.TabIndex = 7;
      this.scanWritableCheckBox.Text = "Writable";
      this.scanWritableCheckBox.ThreeState = true;
      this.scanWritableCheckBox.UseVisualStyleBackColor = true;
      this.scanMappedCheckBox.AutoSize = true;
      this.scanMappedCheckBox.Location = new Point(189, 68);
      this.scanMappedCheckBox.Name = "scanMappedCheckBox";
      this.scanMappedCheckBox.Size = new Size(65, 17);
      this.scanMappedCheckBox.TabIndex = 6;
      this.scanMappedCheckBox.Text = "Mapped";
      this.scanMappedCheckBox.UseVisualStyleBackColor = true;
      this.scanImageCheckBox.AutoSize = true;
      this.scanImageCheckBox.Checked = true;
      this.scanImageCheckBox.CheckState = CheckState.Checked;
      this.scanImageCheckBox.Location = new Point(91, 68);
      this.scanImageCheckBox.Name = "scanImageCheckBox";
      this.scanImageCheckBox.Size = new Size(55, 17);
      this.scanImageCheckBox.TabIndex = 5;
      this.scanImageCheckBox.Text = "Image";
      this.scanImageCheckBox.UseVisualStyleBackColor = true;
      this.scanPrivateCheckBox.AutoSize = true;
      this.scanPrivateCheckBox.Checked = true;
      this.scanPrivateCheckBox.CheckState = CheckState.Checked;
      this.scanPrivateCheckBox.Location = new Point(9, 68);
      this.scanPrivateCheckBox.Name = "scanPrivateCheckBox";
      this.scanPrivateCheckBox.Size = new Size(59, 17);
      this.scanPrivateCheckBox.TabIndex = 4;
      this.scanPrivateCheckBox.Text = "Private";
      this.scanPrivateCheckBox.UseVisualStyleBackColor = true;
      this.stopAddressTextBox.Location = new Point(66, 42);
      this.stopAddressTextBox.Name = "stopAddressTextBox";
      this.stopAddressTextBox.Size = new Size(218, 20);
      this.stopAddressTextBox.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 45);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "Stop:";
      this.startAddressTextBox.Location = new Point(66, 19);
      this.startAddressTextBox.Name = "startAddressTextBox";
      this.startAddressTextBox.Size = new Size(218, 20);
      this.startAddressTextBox.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 22);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Start:";
      this.flowLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel.Controls.Add((Control) this.filterGroupBox);
      this.flowLayoutPanel.Controls.Add((Control) this.floatingOptionsGroupBox);
      this.flowLayoutPanel.Controls.Add((Control) this.stringOptionsGroupBox);
      this.flowLayoutPanel.Controls.Add((Control) this.scanOptionsGroupBox);
      this.flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
      this.flowLayoutPanel.Location = new Point(292, 80);
      this.flowLayoutPanel.Margin = new Padding(0);
      this.flowLayoutPanel.Name = "flowLayoutPanel";
      this.flowLayoutPanel.Size = new Size(317, 314);
      this.flowLayoutPanel.TabIndex = 9;
      this.flowLayoutPanel.WrapContents = false;
      this.floatingOptionsGroupBox.Controls.Add((Control) this.roundTruncateRadioButton);
      this.floatingOptionsGroupBox.Controls.Add((Control) this.roundLooseRadioButton);
      this.floatingOptionsGroupBox.Controls.Add((Control) this.roundStrictRadioButton);
      this.floatingOptionsGroupBox.Location = new Point(3, 107);
      this.floatingOptionsGroupBox.Margin = new Padding(3, 0, 3, 1);
      this.floatingOptionsGroupBox.Name = "floatingOptionsGroupBox";
      this.floatingOptionsGroupBox.Size = new Size(308, 64);
      this.floatingOptionsGroupBox.TabIndex = 9;
      this.floatingOptionsGroupBox.TabStop = false;
      this.floatingOptionsGroupBox.Visible = false;
      this.roundTruncateRadioButton.AutoSize = true;
      this.roundTruncateRadioButton.Location = new Point(72, 42);
      this.roundTruncateRadioButton.Name = "roundTruncateRadioButton";
      this.roundTruncateRadioButton.Size = new Size(68, 17);
      this.roundTruncateRadioButton.TabIndex = 2;
      this.roundTruncateRadioButton.Text = "Truncate";
      this.infoToolTip.SetToolTip((Control) this.roundTruncateRadioButton, "123.45 == 123.99");
      this.roundTruncateRadioButton.UseVisualStyleBackColor = true;
      this.roundLooseRadioButton.AutoSize = true;
      this.roundLooseRadioButton.Checked = true;
      this.roundLooseRadioButton.Location = new Point(72, 26);
      this.roundLooseRadioButton.Name = "roundLooseRadioButton";
      this.roundLooseRadioButton.Size = new Size(103, 17);
      this.roundLooseRadioButton.TabIndex = 1;
      this.roundLooseRadioButton.TabStop = true;
      this.roundLooseRadioButton.Text = "Rounded (loose)";
      this.infoToolTip.SetToolTip((Control) this.roundLooseRadioButton, "123.44 <= x <= 123.46");
      this.roundLooseRadioButton.UseVisualStyleBackColor = true;
      this.roundStrictRadioButton.AutoSize = true;
      this.roundStrictRadioButton.Location = new Point(72, 10);
      this.roundStrictRadioButton.Name = "roundStrictRadioButton";
      this.roundStrictRadioButton.Size = new Size(100, 17);
      this.roundStrictRadioButton.TabIndex = 0;
      this.roundStrictRadioButton.Text = "Rounded (strict)";
      this.infoToolTip.SetToolTip((Control) this.roundStrictRadioButton, "123.45 == 123.454319");
      this.roundStrictRadioButton.UseVisualStyleBackColor = true;
      this.stringOptionsGroupBox.Controls.Add((Control) this.caseSensitiveCheckBox);
      this.stringOptionsGroupBox.Controls.Add((Control) this.encodingUtf32RadioButton);
      this.stringOptionsGroupBox.Controls.Add((Control) this.encodingUtf16RadioButton);
      this.stringOptionsGroupBox.Controls.Add((Control) this.encodingUtf8RadioButton);
      this.stringOptionsGroupBox.Location = new Point(3, 172);
      this.stringOptionsGroupBox.Margin = new Padding(3, 0, 3, 3);
      this.stringOptionsGroupBox.Name = "stringOptionsGroupBox";
      this.stringOptionsGroupBox.Size = new Size(308, 64);
      this.stringOptionsGroupBox.TabIndex = 10;
      this.stringOptionsGroupBox.TabStop = false;
      this.stringOptionsGroupBox.Visible = false;
      this.caseSensitiveCheckBox.AutoSize = true;
      this.caseSensitiveCheckBox.Checked = true;
      this.caseSensitiveCheckBox.CheckState = CheckState.Checked;
      this.caseSensitiveCheckBox.Location = new Point(164, 10);
      this.caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
      this.caseSensitiveCheckBox.Size = new Size(94, 17);
      this.caseSensitiveCheckBox.TabIndex = 3;
      this.caseSensitiveCheckBox.Text = "Case sensitive";
      this.infoToolTip.SetToolTip((Control) this.caseSensitiveCheckBox, "ASD == asd");
      this.caseSensitiveCheckBox.UseVisualStyleBackColor = true;
      this.encodingUtf32RadioButton.AutoSize = true;
      this.encodingUtf32RadioButton.Location = new Point(72, 42);
      this.encodingUtf32RadioButton.Name = "encodingUtf32RadioButton";
      this.encodingUtf32RadioButton.Size = new Size(61, 17);
      this.encodingUtf32RadioButton.TabIndex = 2;
      this.encodingUtf32RadioButton.Text = "UTF-32";
      this.encodingUtf32RadioButton.UseVisualStyleBackColor = true;
      this.encodingUtf16RadioButton.AutoSize = true;
      this.encodingUtf16RadioButton.Location = new Point(72, 26);
      this.encodingUtf16RadioButton.Name = "encodingUtf16RadioButton";
      this.encodingUtf16RadioButton.Size = new Size(61, 17);
      this.encodingUtf16RadioButton.TabIndex = 1;
      this.encodingUtf16RadioButton.Text = "UTF-16";
      this.encodingUtf16RadioButton.UseVisualStyleBackColor = true;
      this.encodingUtf8RadioButton.AutoSize = true;
      this.encodingUtf8RadioButton.Checked = true;
      this.encodingUtf8RadioButton.Location = new Point(72, 10);
      this.encodingUtf8RadioButton.Name = "encodingUtf8RadioButton";
      this.encodingUtf8RadioButton.Size = new Size(55, 17);
      this.encodingUtf8RadioButton.TabIndex = 0;
      this.encodingUtf8RadioButton.TabStop = true;
      this.encodingUtf8RadioButton.Text = "UTF-8";
      this.encodingUtf8RadioButton.UseVisualStyleBackColor = true;
      this.firstScanButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.firstScanButton.Location = new Point(292, 54);
      this.firstScanButton.Name = "firstScanButton";
      this.firstScanButton.Size = new Size(75, 23);
      this.firstScanButton.TabIndex = 11;
      this.firstScanButton.Text = "First Scan";
      this.firstScanButton.UseVisualStyleBackColor = true;
      this.firstScanButton.Click += new EventHandler(this.firstScanButton_Click);
      this.nextScanButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.nextScanButton.Enabled = false;
      this.nextScanButton.Location = new Point(373, 54);
      this.nextScanButton.Name = "nextScanButton";
      this.nextScanButton.Size = new Size(75, 23);
      this.nextScanButton.TabIndex = 12;
      this.nextScanButton.Text = "Next Scan";
      this.nextScanButton.UseVisualStyleBackColor = true;
      this.nextScanButton.Click += new EventHandler(this.nextScanButton_Click);
      this.scanProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.scanProgressBar.Location = new Point(454, 54);
      this.scanProgressBar.Name = "scanProgressBar";
      this.scanProgressBar.Size = new Size(149, 23);
      this.scanProgressBar.TabIndex = 13;
      this.resultCountLabel.AutoSize = true;
      this.resultCountLabel.Location = new Point(8, 59);
      this.resultCountLabel.Name = "resultCountLabel";
      this.resultCountLabel.Size = new Size(19, 13);
      this.resultCountLabel.TabIndex = 15;
      this.resultCountLabel.Text = "<>";
      this.updateValuesTimer.Enabled = true;
      this.updateValuesTimer.Interval = 1000;
      this.updateValuesTimer.Tick += new EventHandler(this.updateValuesTimer_Tick);
      this.resultMemoryRecordList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.resultMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
      this.resultMemoryRecordList.Location = new Point(11, 80);
      this.resultMemoryRecordList.Name = "resultMemoryRecordList";
      this.resultMemoryRecordList.ShowAddressColumn = true;
      this.resultMemoryRecordList.ShowDescriptionColumn = false;
      this.resultMemoryRecordList.ShowPreviousValueColumn = true;
      this.resultMemoryRecordList.ShowValueColumn = true;
      this.resultMemoryRecordList.ShowValueTypeColumn = false;
      this.resultMemoryRecordList.Size = new Size(268, 314);
      this.resultMemoryRecordList.TabIndex = 16;
      this.resultMemoryRecordList.RecordDoubleClick += new MemorySearchResultControlResultDoubleClickEventHandler(this.memorySearchResultControl_ResultDoubleClick);
      this.resultListContextMenuStrip.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.addSelectedResultsToAddressListToolStripMenuItem,
        (ToolStripItem) this.removeSelectedRecordsToolStripMenuItem,
        (ToolStripItem) this.changeToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.setCurrentClassAddressToolStripMenuItem,
        (ToolStripItem) this.createClassAtAddressToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.findOutWhatAccessesThisAddressToolStripMenuItem,
        (ToolStripItem) this.findOutWhatWritesToThisAddressToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.copyAddressToolStripMenuItem
      });
      this.resultListContextMenuStrip.Name = "resultListContextMenuStrip";
      this.resultListContextMenuStrip.Size = new Size(270, 198);
      this.resultListContextMenuStrip.Opening += new CancelEventHandler(this.resultListContextMenuStrip_Opening);
      this.addSelectedResultsToAddressListToolStripMenuItem.Image = (Image) Resources.B16x16_Tree_Expand;
      this.addSelectedResultsToAddressListToolStripMenuItem.Name = "addSelectedResultsToAddressListToolStripMenuItem";
      this.addSelectedResultsToAddressListToolStripMenuItem.Size = new Size(269, 22);
      this.addSelectedResultsToAddressListToolStripMenuItem.Text = "Add selected results to address list";
      this.addSelectedResultsToAddressListToolStripMenuItem.Click += new EventHandler(this.addSelectedResultsToAddressListToolStripMenuItem_Click);
      this.removeSelectedRecordsToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Delete;
      this.removeSelectedRecordsToolStripMenuItem.Name = "removeSelectedRecordsToolStripMenuItem";
      this.removeSelectedRecordsToolStripMenuItem.Size = new Size(269, 22);
      this.removeSelectedRecordsToolStripMenuItem.Text = "Remove selected records";
      this.removeSelectedRecordsToolStripMenuItem.Click += new EventHandler(this.removeSelectedRecordsToolStripMenuItem_Click);
      this.changeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.descriptionToolStripMenuItem,
        (ToolStripItem) this.addressToolStripMenuItem,
        (ToolStripItem) this.valueTypeToolStripMenuItem,
        (ToolStripItem) this.valueToolStripMenuItem
      });
      this.changeToolStripMenuItem.Enabled = false;
      this.changeToolStripMenuItem.Image = (Image) Resources.B16x16_Textfield_Rename;
      this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
      this.changeToolStripMenuItem.Size = new Size(269, 22);
      this.changeToolStripMenuItem.Text = "Change...";
      this.descriptionToolStripMenuItem.Name = "descriptionToolStripMenuItem";
      this.descriptionToolStripMenuItem.Size = new Size(134, 22);
      this.descriptionToolStripMenuItem.Text = "Description";
      this.addressToolStripMenuItem.Name = "addressToolStripMenuItem";
      this.addressToolStripMenuItem.Size = new Size(134, 22);
      this.addressToolStripMenuItem.Text = "Address";
      this.valueTypeToolStripMenuItem.Name = "valueTypeToolStripMenuItem";
      this.valueTypeToolStripMenuItem.Size = new Size(134, 22);
      this.valueTypeToolStripMenuItem.Text = "Value Type";
      this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
      this.valueToolStripMenuItem.Size = new Size(134, 22);
      this.valueToolStripMenuItem.Text = "Value";
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(266, 6);
      this.setCurrentClassAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Exchange_Button;
      this.setCurrentClassAddressToolStripMenuItem.Name = "setCurrentClassAddressToolStripMenuItem";
      this.setCurrentClassAddressToolStripMenuItem.Size = new Size(269, 22);
      this.setCurrentClassAddressToolStripMenuItem.Text = "Set current class address";
      this.setCurrentClassAddressToolStripMenuItem.Click += new EventHandler(this.setCurrentClassAddressToolStripMenuItem_Click);
      this.createClassAtAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Button_Class_Add;
      this.createClassAtAddressToolStripMenuItem.Name = "createClassAtAddressToolStripMenuItem";
      this.createClassAtAddressToolStripMenuItem.Size = new Size(269, 22);
      this.createClassAtAddressToolStripMenuItem.Text = "Create class at address";
      this.createClassAtAddressToolStripMenuItem.Click += new EventHandler(this.createClassAtAddressToolStripMenuItem_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(266, 6);
      this.findOutWhatAccessesThisAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Find_Access;
      this.findOutWhatAccessesThisAddressToolStripMenuItem.Name = "findOutWhatAccessesThisAddressToolStripMenuItem";
      this.findOutWhatAccessesThisAddressToolStripMenuItem.Size = new Size(269, 22);
      this.findOutWhatAccessesThisAddressToolStripMenuItem.Text = "Find out what accesses this address...";
      this.findOutWhatAccessesThisAddressToolStripMenuItem.Click += new EventHandler(this.findOutWhatAccessesThisAddressToolStripMenuItem_Click);
      this.findOutWhatWritesToThisAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Find_Write;
      this.findOutWhatWritesToThisAddressToolStripMenuItem.Name = "findOutWhatWritesToThisAddressToolStripMenuItem";
      this.findOutWhatWritesToThisAddressToolStripMenuItem.Size = new Size(269, 22);
      this.findOutWhatWritesToThisAddressToolStripMenuItem.Text = "Find out what writes to this address...";
      this.findOutWhatWritesToThisAddressToolStripMenuItem.Click += new EventHandler(this.findOutWhatWritesToThisAddressToolStripMenuItem_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(266, 6);
      this.copyAddressToolStripMenuItem.Image = (Image) Resources.B16x16_Page_Copy;
      this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
      this.copyAddressToolStripMenuItem.Size = new Size(269, 22);
      this.copyAddressToolStripMenuItem.Text = "Copy Address";
      this.copyAddressToolStripMenuItem.Click += new EventHandler(this.copyAddressToolStripMenuItem_Click);
      this.addressListMemoryRecordList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.addressListMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
      this.addressListMemoryRecordList.Location = new Point(11, 425);
      this.addressListMemoryRecordList.Name = "addressListMemoryRecordList";
      this.addressListMemoryRecordList.ShowAddressColumn = true;
      this.addressListMemoryRecordList.ShowDescriptionColumn = true;
      this.addressListMemoryRecordList.ShowPreviousValueColumn = false;
      this.addressListMemoryRecordList.ShowValueColumn = true;
      this.addressListMemoryRecordList.ShowValueTypeColumn = true;
      this.addressListMemoryRecordList.Size = new Size(592, 169);
      this.addressListMemoryRecordList.TabIndex = 17;
      this.toolStripPanel.Controls.Add((Control) this.menuToolStrip);
      this.toolStripPanel.Location = new Point(11, 397);
      this.toolStripPanel.Name = "toolStripPanel";
      this.toolStripPanel.Orientation = Orientation.Horizontal;
      this.toolStripPanel.RenderMode = ToolStripRenderMode.Professional;
      this.toolStripPanel.RowMargin = new Padding(0);
      this.toolStripPanel.Size = new Size(128, 25);
      this.menuToolStrip.Dock = DockStyle.None;
      this.menuToolStrip.GripStyle = ToolStripGripStyle.Hidden;
      this.menuToolStrip.Items.AddRange(new ToolStripItem[5]
      {
        (ToolStripItem) this.openAddressFileToolStripButton,
        (ToolStripItem) this.saveAddressFileToolStripButton,
        (ToolStripItem) this.saveAddressFileAsToolStripButton,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.clearAddressListToolStripButton
      });
      this.menuToolStrip.Location = new Point(0, 0);
      this.menuToolStrip.Name = "menuToolStrip";
      this.menuToolStrip.Size = new Size(101, 25);
      this.menuToolStrip.TabIndex = 0;
      this.openAddressFileToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.openAddressFileToolStripButton.Image = (Image) Resources.B16x16_Folder;
      this.openAddressFileToolStripButton.ImageTransparentColor = Color.Magenta;
      this.openAddressFileToolStripButton.Name = "openAddressFileToolStripButton";
      this.openAddressFileToolStripButton.Size = new Size(23, 22);
      this.openAddressFileToolStripButton.ToolTipText = "Open...";
      this.openAddressFileToolStripButton.Click += new EventHandler(this.openAddressFileToolStripButton_Click);
      this.saveAddressFileToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.saveAddressFileToolStripButton.Image = (Image) Resources.B16x16_Save;
      this.saveAddressFileToolStripButton.ImageTransparentColor = Color.Magenta;
      this.saveAddressFileToolStripButton.Name = "saveAddressFileToolStripButton";
      this.saveAddressFileToolStripButton.Size = new Size(23, 22);
      this.saveAddressFileToolStripButton.ToolTipText = "Save";
      this.saveAddressFileToolStripButton.Click += new EventHandler(this.saveAddressFileToolStripButton_Click);
      this.saveAddressFileAsToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.saveAddressFileAsToolStripButton.Image = (Image) Resources.B16x16_Save_As;
      this.saveAddressFileAsToolStripButton.ImageTransparentColor = Color.Magenta;
      this.saveAddressFileAsToolStripButton.Name = "saveAddressFileAsToolStripButton";
      this.saveAddressFileAsToolStripButton.Size = new Size(23, 22);
      this.saveAddressFileAsToolStripButton.ToolTipText = "Save As...";
      this.saveAddressFileAsToolStripButton.Click += new EventHandler(this.saveAsToolStripButton_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(6, 25);
      this.clearAddressListToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
      this.clearAddressListToolStripButton.Image = (Image) Resources.B16x16_Button_Delete;
      this.clearAddressListToolStripButton.ImageTransparentColor = Color.Magenta;
      this.clearAddressListToolStripButton.Name = "clearAddressListToolStripButton";
      this.clearAddressListToolStripButton.Size = new Size(23, 22);
      this.clearAddressListToolStripButton.ToolTipText = "Clear";
      this.clearAddressListToolStripButton.Click += new EventHandler(this.clearAddressListToolStripButton_Click);
      this.infoToolTip.AutomaticDelay = 100;
      this.undoIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.undoIconButton.Image = (Image) Resources.B16x16_Undo;
      this.undoIconButton.Location = new Point(256, 54);
      this.undoIconButton.Name = "undoIconButton";
      this.undoIconButton.Pressed = false;
      this.undoIconButton.Selected = false;
      this.undoIconButton.Size = new Size(23, 22);
      this.undoIconButton.TabIndex = 18;
      this.undoIconButton.Click += new EventHandler(this.undoIconButton_Click);
      this.showInputCorrelatorIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.showInputCorrelatorIconButton.Image = (Image) Resources.B16x16_Canvas_Size;
      this.showInputCorrelatorIconButton.Location = new Point(580, 398);
      this.showInputCorrelatorIconButton.Name = "showInputCorrelatorIconButton";
      this.showInputCorrelatorIconButton.Pressed = false;
      this.showInputCorrelatorIconButton.Selected = false;
      this.showInputCorrelatorIconButton.Size = new Size(23, 22);
      this.showInputCorrelatorIconButton.TabIndex = 19;
      this.showInputCorrelatorIconButton.Click += new EventHandler(this.showInputCorrelatorIconButton_Click);
      this.cancelScanIconButton.Image = (Image) Resources.B16x16_Button_Delete;
      this.cancelScanIconButton.Location = new Point(517, 54);
      this.cancelScanIconButton.Name = "cancelScanIconButton";
      this.cancelScanIconButton.Pressed = false;
      this.cancelScanIconButton.Selected = false;
      this.cancelScanIconButton.Size = new Size(23, 22);
      this.cancelScanIconButton.TabIndex = 21;
      this.cancelScanIconButton.Visible = false;
      this.cancelScanIconButton.Click += new EventHandler(this.cancelScanIconButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(612, 607);
      this.Controls.Add((Control) this.cancelScanIconButton);
      this.Controls.Add((Control) this.showInputCorrelatorIconButton);
      this.Controls.Add((Control) this.undoIconButton);
      this.Controls.Add((Control) this.toolStripPanel);
      this.Controls.Add((Control) this.addressListMemoryRecordList);
      this.Controls.Add((Control) this.resultMemoryRecordList);
      this.Controls.Add((Control) this.resultCountLabel);
      this.Controls.Add((Control) this.scanProgressBar);
      this.Controls.Add((Control) this.nextScanButton);
      this.Controls.Add((Control) this.firstScanButton);
      this.Controls.Add((Control) this.flowLayoutPanel);
      this.Controls.Add((Control) this.bannerBox);
      this.MinimumSize = new Size(628, 622);
      this.Name = nameof (ScannerForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Scanner";
      this.FormClosing += new FormClosingEventHandler(this.MemorySearchForm_FormClosing);
      this.bannerBox.EndInit();
      this.filterGroupBox.ResumeLayout(false);
      this.filterGroupBox.PerformLayout();
      this.scanOptionsGroupBox.ResumeLayout(false);
      this.scanOptionsGroupBox.PerformLayout();
      this.flowLayoutPanel.ResumeLayout(false);
      this.floatingOptionsGroupBox.ResumeLayout(false);
      this.floatingOptionsGroupBox.PerformLayout();
      this.stringOptionsGroupBox.ResumeLayout(false);
      this.stringOptionsGroupBox.PerformLayout();
      this.resultListContextMenuStrip.ResumeLayout(false);
      this.toolStripPanel.ResumeLayout(false);
      this.toolStripPanel.PerformLayout();
      this.menuToolStrip.ResumeLayout(false);
      this.menuToolStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    internal class ScanCompareTypeComboBox : EnumComboBox<ScanCompareType>
    {
    }

    internal class ScanValueTypeComboBox : EnumComboBox<ScanValueType>
    {
    }
  }
}
