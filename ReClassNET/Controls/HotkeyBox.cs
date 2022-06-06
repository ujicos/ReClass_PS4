// Decompiled with JetBrains decompiler
// Type: ReClassNET.Controls.HotkeyBox
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Input;
using ReClassNET.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Controls
{
  [Designer(typeof (HotkeyBoxDesigner))]
  public class HotkeyBox : UserControl
  {
    private IContainer components;
    private Timer timer;
    private TextBox textBox;
    private Button clearButton;

    public KeyboardInput Input { get; set; }

    public KeyboardHotkey Hotkey { get; } = new KeyboardHotkey();

    public HotkeyBox()
    {
      this.InitializeComponent();
      this.DisplayHotkey();
    }

    protected override void SetBoundsCore(
      int x,
      int y,
      int width,
      int height,
      BoundsSpecified specified)
    {
      base.SetBoundsCore(x, y, width, 20, specified);
    }

    private void textBox_Enter(object sender, EventArgs e)
    {
      this.timer.Enabled = true;
    }

    private void textBox_Leave(object sender, EventArgs e)
    {
      this.timer.Enabled = false;
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      if (this.Input == null)
        return;
      Keys[] pressedKeys = this.Input.GetPressedKeys();
      if (pressedKeys.Length == 0)
        return;
      foreach (Keys key in ((IEnumerable<Keys>) pressedKeys).Select<Keys, Keys>((Func<Keys, Keys>) (k => k & Keys.KeyCode)).Where<Keys>((Func<Keys, bool>) (k => (uint) k > 0U)))
        this.Hotkey.AddKey(key);
      this.DisplayHotkey();
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
      this.Clear();
    }

    private void DisplayHotkey()
    {
      this.textBox.Text = this.Hotkey.ToString();
    }

    public void Clear()
    {
      this.Hotkey.Clear();
      this.DisplayHotkey();
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
      this.timer = new Timer(this.components);
      this.textBox = new TextBox();
      this.clearButton = new Button();
      this.SuspendLayout();
      this.timer.Enabled = true;
      this.timer.Interval = 50;
      this.timer.Tick += new EventHandler(this.timer_Tick);
      this.textBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
      this.textBox.Enabled = false;
      this.textBox.Location = new Point(0, 0);
      this.textBox.Name = "textBox";
      this.textBox.Size = new Size(140, 20);
      this.textBox.TabIndex = 0;
      this.textBox.Enter += new EventHandler(this.textBox_Enter);
      this.textBox.Leave += new EventHandler(this.textBox_Leave);
      this.clearButton.Anchor = AnchorStyles.Right;
      this.clearButton.Image = (Image) Resources.B16x16_Button_Delete;
      this.clearButton.Location = new Point(142, 0);
      this.clearButton.Name = "clearButton";
      this.clearButton.Size = new Size(20, 20);
      this.clearButton.TabIndex = 1;
      this.clearButton.UseVisualStyleBackColor = true;
      this.clearButton.Click += new EventHandler(this.clearButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.clearButton);
      this.Controls.Add((Control) this.textBox);
      this.Name = nameof (HotkeyBox);
      this.Size = new Size(162, 20);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
