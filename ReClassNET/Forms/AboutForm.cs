// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.AboutForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Controls;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class AboutForm : IconForm
  {
    private IContainer components;
    private BannerBox bannerBox;
    private Label infoLabel;
    private Label platformLabel;
    private Label buildTimeLabel;
    private Label authorLabel;
    private Label homepageLabel;
    private Label platformValueLabel;
    private Label buildTimeValueLabel;
    private Label authorValueLabel;
    private LinkLabel homepageValueLabel;

    public AboutForm()
    {
      this.InitializeComponent();
      this.bannerBox.Icon = (Image) Resources.ReClassNet.ToBitmap();
      this.bannerBox.Title = "ReClass.NET";
      this.bannerBox.Text = "Version: PS4 Port";
      this.platformValueLabel.Text = "x64 PS4 Build";
      this.buildTimeValueLabel.Text = Resources.BuildDate;
      this.authorValueLabel.Text = "MrReeko";
      this.homepageValueLabel.Text = "https://github.com/MrReekoFTWxD";
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

    private void homepageValueLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Process.Start("https://github.com/MrReekoFTWxD");
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
      this.infoLabel = new Label();
      this.platformLabel = new Label();
      this.buildTimeLabel = new Label();
      this.authorLabel = new Label();
      this.homepageLabel = new Label();
      this.platformValueLabel = new Label();
      this.buildTimeValueLabel = new Label();
      this.authorValueLabel = new Label();
      this.homepageValueLabel = new LinkLabel();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) null;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(409, 48);
      this.bannerBox.TabIndex = 0;
      this.bannerBox.Title = "";
      this.infoLabel.AutoSize = true;
      this.infoLabel.Location = new Point(12, 140);
      this.infoLabel.Name = "infoLabel";
      this.infoLabel.Size = new Size(196, 13);
      this.infoLabel.TabIndex = 1;
      this.infoLabel.Text = "This is a port of Reclass.Net to the PS4.";
      this.platformLabel.AutoSize = true;
      this.platformLabel.Location = new Point(12, 60);
      this.platformLabel.Name = "platformLabel";
      this.platformLabel.Size = new Size(51, 13);
      this.platformLabel.TabIndex = 2;
      this.platformLabel.Text = "Platform: ";
      this.buildTimeLabel.AutoSize = true;
      this.buildTimeLabel.Location = new Point(12, 79);
      this.buildTimeLabel.Name = "buildTimeLabel";
      this.buildTimeLabel.Size = new Size(58, 13);
      this.buildTimeLabel.TabIndex = 3;
      this.buildTimeLabel.Text = "Build time: ";
      this.authorLabel.AutoSize = true;
      this.authorLabel.Location = new Point(12, 98);
      this.authorLabel.Name = "authorLabel";
      this.authorLabel.Size = new Size(56, 13);
      this.authorLabel.TabIndex = 4;
      this.authorLabel.Text = "Ported By:";
      this.homepageLabel.AutoSize = true;
      this.homepageLabel.Location = new Point(12, 117);
      this.homepageLabel.Name = "homepageLabel";
      this.homepageLabel.Size = new Size(66, 13);
      this.homepageLabel.TabIndex = 5;
      this.homepageLabel.Text = "Home Page:";
      this.platformValueLabel.AutoSize = true;
      this.platformValueLabel.Location = new Point(84, 60);
      this.platformValueLabel.Name = "platformValueLabel";
      this.platformValueLabel.Size = new Size(19, 13);
      this.platformValueLabel.TabIndex = 6;
      this.platformValueLabel.Text = "<>";
      this.buildTimeValueLabel.AutoSize = true;
      this.buildTimeValueLabel.Location = new Point(84, 79);
      this.buildTimeValueLabel.Name = "buildTimeValueLabel";
      this.buildTimeValueLabel.Size = new Size(19, 13);
      this.buildTimeValueLabel.TabIndex = 7;
      this.buildTimeValueLabel.Text = "<>";
      this.authorValueLabel.AutoSize = true;
      this.authorValueLabel.Location = new Point(84, 98);
      this.authorValueLabel.Name = "authorValueLabel";
      this.authorValueLabel.Size = new Size(19, 13);
      this.authorValueLabel.TabIndex = 8;
      this.authorValueLabel.Text = "<>";
      this.homepageValueLabel.AutoSize = true;
      this.homepageValueLabel.Location = new Point(84, 117);
      this.homepageValueLabel.Name = "homepageValueLabel";
      this.homepageValueLabel.Size = new Size(19, 13);
      this.homepageValueLabel.TabIndex = 9;
      this.homepageValueLabel.TabStop = true;
      this.homepageValueLabel.Text = "<>";
      this.homepageValueLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(this.homepageValueLabel_LinkClicked);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(69, 73, 74);
      this.ClientSize = new Size(409, 168);
      this.Controls.Add((Control) this.homepageValueLabel);
      this.Controls.Add((Control) this.authorValueLabel);
      this.Controls.Add((Control) this.buildTimeValueLabel);
      this.Controls.Add((Control) this.platformValueLabel);
      this.Controls.Add((Control) this.homepageLabel);
      this.Controls.Add((Control) this.authorLabel);
      this.Controls.Add((Control) this.buildTimeLabel);
      this.Controls.Add((Control) this.platformLabel);
      this.Controls.Add((Control) this.infoLabel);
      this.Controls.Add((Control) this.bannerBox);
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Info";
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
