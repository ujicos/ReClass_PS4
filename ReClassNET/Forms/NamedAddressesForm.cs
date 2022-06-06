// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.NamedAddressesForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class NamedAddressesForm : IconForm
  {
    private readonly RemoteProcess process;
    private IContainer components;
    private BannerBox bannerBox;
    private PlaceholderTextBox addressTextBox;
    private PlaceholderTextBox nameTextBox;
    private ListBox namedAddressesListBox;
    private IconButton removeAddressIconButton;
    private IconButton addAddressIconButton;

    public NamedAddressesForm(RemoteProcess process)
    {
      this.process = process;
      this.InitializeComponent();
      this.DisplayNamedAddresses();
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

    private void InputTextBox_TextChanged(object sender, EventArgs e)
    {
      this.addAddressIconButton.Enabled = this.IsValidInput();
    }

    private void namedAddressesListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.removeAddressIconButton.Enabled = this.namedAddressesListBox.SelectedIndex != -1;
    }

    private void addAddressIconButton_Click(object sender, EventArgs e)
    {
      if (!this.IsValidInput())
        return;
      this.process.NamedAddresses[this.process.ParseAddress(this.addressTextBox.Text.Trim())] = this.nameTextBox.Text.Trim();
      this.addressTextBox.Text = this.nameTextBox.Text = (string) null;
      this.DisplayNamedAddresses();
    }

    private void removeAddressIconButton_Click(object sender, EventArgs e)
    {
      if (!(this.namedAddressesListBox.SelectedItem is BindingDisplayWrapper<KeyValuePair<IntPtr, string>> selectedItem))
        return;
      this.process.NamedAddresses.Remove(selectedItem.Value.Key);
      this.DisplayNamedAddresses();
    }

    private void DisplayNamedAddresses()
    {
      this.namedAddressesListBox.DataSource = (object) this.process.NamedAddresses.Select<KeyValuePair<IntPtr, string>, BindingDisplayWrapper<KeyValuePair<IntPtr, string>>>((Func<KeyValuePair<IntPtr, string>, BindingDisplayWrapper<KeyValuePair<IntPtr, string>>>) (kv => new BindingDisplayWrapper<KeyValuePair<IntPtr, string>>(kv, (Func<KeyValuePair<IntPtr, string>, string>) (v => "0x" + v.Key.ToString("X016") + ": " + v.Value)))).ToList<BindingDisplayWrapper<KeyValuePair<IntPtr, string>>>();
      this.namedAddressesListBox_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private bool IsValidInput()
    {
      try
      {
        IntPtr address = this.process.ParseAddress(this.addressTextBox.Text.Trim());
        string str = this.nameTextBox.Text.Trim();
        return !address.IsNull() && !string.IsNullOrEmpty(str);
      }
      catch
      {
        return false;
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
      this.bannerBox = new BannerBox();
      this.addressTextBox = new PlaceholderTextBox();
      this.nameTextBox = new PlaceholderTextBox();
      this.namedAddressesListBox = new ListBox();
      this.removeAddressIconButton = new IconButton();
      this.addAddressIconButton = new IconButton();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B16x16_Custom_Type;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(429, 48);
      this.bannerBox.TabIndex = 10;
      this.bannerBox.Text = "Give special memory addresses meaningfull names.";
      this.bannerBox.Title = "Named Addresses";
      this.addressTextBox.Location = new Point(13, 55);
      this.addressTextBox.Name = "addressTextBox";
      this.addressTextBox.PlaceholderText = "Address";
      this.addressTextBox.Size = new Size(154, 20);
      this.addressTextBox.TabIndex = 1;
      this.addressTextBox.TextChanged += new EventHandler(this.InputTextBox_TextChanged);
      this.nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTextBox.Location = new Point(173, 55);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.PlaceholderText = "Name";
      this.nameTextBox.Size = new Size(190, 20);
      this.nameTextBox.TabIndex = 2;
      this.nameTextBox.TextChanged += new EventHandler(this.InputTextBox_TextChanged);
      this.namedAddressesListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.namedAddressesListBox.FormattingEnabled = true;
      this.namedAddressesListBox.Location = new Point(13, 81);
      this.namedAddressesListBox.Name = "namedAddressesListBox";
      this.namedAddressesListBox.Size = new Size(404, 186);
      this.namedAddressesListBox.TabIndex = 0;
      this.namedAddressesListBox.SelectedIndexChanged += new EventHandler(this.namedAddressesListBox_SelectedIndexChanged);
      this.removeAddressIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.removeAddressIconButton.Enabled = false;
      this.removeAddressIconButton.Image = (Image) Resources.B16x16_Button_Remove;
      this.removeAddressIconButton.Location = new Point(394, 54);
      this.removeAddressIconButton.Name = "removeAddressIconButton";
      this.removeAddressIconButton.Pressed = false;
      this.removeAddressIconButton.Selected = false;
      this.removeAddressIconButton.Size = new Size(23, 22);
      this.removeAddressIconButton.TabIndex = 4;
      this.removeAddressIconButton.Click += new EventHandler(this.removeAddressIconButton_Click);
      this.addAddressIconButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.addAddressIconButton.Enabled = false;
      this.addAddressIconButton.Image = (Image) Resources.B16x16_Button_Add;
      this.addAddressIconButton.Location = new Point(369, 54);
      this.addAddressIconButton.Name = "addAddressIconButton";
      this.addAddressIconButton.Pressed = false;
      this.addAddressIconButton.Selected = false;
      this.addAddressIconButton.Size = new Size(23, 22);
      this.addAddressIconButton.TabIndex = 3;
      this.addAddressIconButton.Click += new EventHandler(this.addAddressIconButton_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(429, 279);
      this.Controls.Add((Control) this.addAddressIconButton);
      this.Controls.Add((Control) this.removeAddressIconButton);
      this.Controls.Add((Control) this.namedAddressesListBox);
      this.Controls.Add((Control) this.nameTextBox);
      this.Controls.Add((Control) this.addressTextBox);
      this.Controls.Add((Control) this.bannerBox);
      this.MinimumSize = new Size(445, 317);
      this.Name = nameof (NamedAddressesForm);
      this.Text = "ReClass.NET - Named Addresses";
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
