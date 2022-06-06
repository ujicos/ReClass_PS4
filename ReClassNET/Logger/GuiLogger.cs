// Decompiled with JetBrains decompiler
// Type: ReClassNET.Logger.GuiLogger
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Forms;
using System;
using System.Windows.Forms;

namespace ReClassNET.Logger
{
  public class GuiLogger : BaseLogger
  {
    private readonly LogForm form;

    public LogLevel Level { get; set; } = LogLevel.Warning;

    public GuiLogger()
    {
      this.form = new LogForm();
      this.form.FormClosing += (FormClosingEventHandler) ((sender, e) =>
      {
        this.form.Clear();
        this.form.Hide();
        e.Cancel = true;
      });
      this.NewLogEntry += new NewLogEntryEventHandler(this.OnNewLogEntry);
    }

    private void OnNewLogEntry(LogLevel level, string message, Exception ex)
    {
      if (level < this.Level)
        return;
      this.ShowForm();
      this.form.Add(level, message, ex);
    }

    public void ShowForm()
    {
      if (this.form.Visible)
        return;
      this.form.Show();
      this.form.BringToFront();
    }
  }
}
