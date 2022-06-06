// Decompiled with JetBrains decompiler
// Type: ReClassNET.UI.LinkedWindowFeatures
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Debugger;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Nodes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.UI
{
  public class LinkedWindowFeatures
  {
    public static ClassNode CreateClassAtAddress(IntPtr address, bool addDefaultBytes)
    {
      ProjectView projectView = Program.MainForm.ProjectView;
      ClassNode classNode1 = ClassNode.Create();
      classNode1.AddressFormula = address.ToString("X");
      if (addDefaultBytes)
        classNode1.AddBytes(16 * IntPtr.Size);
      ClassNode classNode2 = classNode1;
      projectView.SelectedClass = classNode2;
      return classNode1;
    }

    public static ClassNode CreateDefaultClass()
    {
      IntPtr address = ClassNode.DefaultAddress;
      Module moduleByName = Program.RemoteProcess.GetModuleByName(Program.RemoteProcess.UnderlayingProcess?.Name);
      if (moduleByName != null)
        address = moduleByName.Start;
      return LinkedWindowFeatures.CreateClassAtAddress(address, true);
    }

    public static void SetCurrentClassAddress(IntPtr address)
    {
      ClassNode selectedClass = Program.MainForm.ProjectView.SelectedClass;
      if (selectedClass == null)
        return;
      selectedClass.AddressFormula = address.ToString("X");
    }

    public static void FindWhatInteractsWithAddress(IntPtr address, int size, bool writeOnly)
    {
      RemoteDebugger debugger = Program.RemoteProcess.Debugger;
      if (!debugger.AskUserAndAttachDebugger())
        return;
      if (writeOnly)
        debugger.FindWhatWritesToAddress(address, size);
      else
        debugger.FindWhatAccessesAddress(address, size);
    }

    public static void StartMemoryScan(IScanComparer comparer)
    {
      ScannerForm scannerForm = GlobalWindowManager.Windows.OfType<ScannerForm>().FirstOrDefault<ScannerForm>();
      if (scannerForm != null && MessageBox.Show("Open a new scanner window?", "ReClass.NET", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        scannerForm = (ScannerForm) null;
      if (scannerForm == null)
      {
        scannerForm = new ScannerForm(Program.RemoteProcess);
        scannerForm.Show();
      }
      ScanSettings settings = ScanSettings.Default;
      switch (comparer)
      {
        case ByteMemoryComparer _:
          settings.ValueType = ScanValueType.Byte;
          break;
        case ShortMemoryComparer _:
          settings.ValueType = ScanValueType.Short;
          settings.FastScanAlignment = 2;
          break;
        case IntegerMemoryComparer _:
          settings.ValueType = ScanValueType.Integer;
          settings.FastScanAlignment = 4;
          break;
        case LongMemoryComparer _:
          settings.ValueType = ScanValueType.Long;
          settings.FastScanAlignment = 4;
          break;
        case FloatMemoryComparer _:
          settings.ValueType = ScanValueType.Float;
          settings.FastScanAlignment = 4;
          break;
        case DoubleMemoryComparer _:
          settings.ValueType = ScanValueType.Double;
          settings.FastScanAlignment = 4;
          break;
        case ArrayOfBytesMemoryComparer _:
          settings.ValueType = ScanValueType.ArrayOfBytes;
          break;
        case StringMemoryComparer _:
          settings.ValueType = ScanValueType.String;
          break;
      }
      scannerForm.ExcuteScan(settings, comparer);
    }
  }
}
