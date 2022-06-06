// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.CodeForm
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ColorCode;
using ReClassNET.CodeGenerator;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using ReClassNET.Properties;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class CodeForm : IconForm
  {
    private IContainer components;
    private BannerBox bannerBox;
    private RichTextBox codeRichTextBox;

    public CodeForm(
      ICodeGenerator generator,
      IReadOnlyList<ClassNode> classes,
      IReadOnlyList<EnumDescription> enums,
      ILogger logger)
    {
      this.InitializeComponent();
      this.codeRichTextBox.SetInnerMargin(5, 5, 5, 5);
      string code = generator.GenerateCode(classes, enums, logger);
      StringBuilder sb = new StringBuilder(code.Length * 2);
      using (StringWriter stringWriter = new StringWriter(sb))
        new CodeColorizer().Colorize(code, generator.Language == Language.Cpp ? Languages.Cpp : Languages.CSharp, (IFormatter) new RtfFormatter(), StyleSheets.Default, (TextWriter) stringWriter);
      this.codeRichTextBox.Rtf = sb.ToString();
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

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.bannerBox = new BannerBox();
      this.codeRichTextBox = new RichTextBox();
      this.bannerBox.BeginInit();
      this.SuspendLayout();
      this.bannerBox.Dock = DockStyle.Top;
      this.bannerBox.Icon = (Image) Resources.B32x32_Page_Code;
      this.bannerBox.Location = new Point(0, 0);
      this.bannerBox.Name = "bannerBox";
      this.bannerBox.Size = new Size(629, 48);
      this.bannerBox.TabIndex = 2;
      this.bannerBox.Text = "The classes transformed into source code.";
      this.bannerBox.Title = "Code Generator";
      this.codeRichTextBox.BackColor = Color.WhiteSmoke;
      this.codeRichTextBox.BorderStyle = BorderStyle.None;
      this.codeRichTextBox.Dock = DockStyle.Fill;
      this.codeRichTextBox.ForeColor = Color.White;
      this.codeRichTextBox.Location = new Point(0, 48);
      this.codeRichTextBox.Name = "codeRichTextBox";
      this.codeRichTextBox.Size = new Size(629, 390);
      this.codeRichTextBox.TabIndex = 3;
      this.codeRichTextBox.Text = "";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.ClientSize = new Size(629, 438);
      this.Controls.Add((Control) this.codeRichTextBox);
      this.Controls.Add((Control) this.bannerBox);
      this.ForeColor = Color.White;
      this.MinimumSize = new Size(350, 185);
      this.Name = nameof (CodeForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ReClass.NET - Code Generator";
      this.bannerBox.EndInit();
      this.ResumeLayout(false);
    }
  }
}
