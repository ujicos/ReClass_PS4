// Decompiled with JetBrains decompiler
// Type: ReClassNET.Forms.SocketListener
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
  public class SocketListener : Form
  {
    private readonly IniFile cfg = new IniFile(Application.StartupPath + "\\ps4info.ini");
    private bool listenerStarted;
    private Task socketListener;
    private IContainer components;
    private RichTextBox richTextBox1;
    private Button button1;
    private Button button2;
    private Button button3;
    private Label label1;

    public SocketListener()
    {
      this.InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (!MainForm.PS4.IsConnected)
        return;
      this.listenerStarted = true;
      Progress<string> progress = new Progress<string>((Action<string>) (s => this.richTextBox1.AppendText(s)));
      this.richTextBox1.AppendText("Starting Listener\n");
      this.socketListener = Task.Factory.StartNew((Action) (() => this.SocketThread((IProgress<string>) progress)), TaskCreationOptions.LongRunning);
    }

    private void SocketThread(IProgress<string> progres)
    {
      TcpListener tcpListener = new TcpListener(IPAddress.Any, 9023);
      tcpListener.Start();
      Thread currentThread;
      TcpClient tcpClient;
      do
      {
        currentThread = Thread.CurrentThread;
        tcpClient = tcpListener.AcceptTcpClient();
        NetworkStream stream = tcpClient.GetStream();
        StreamReader streamReader = new StreamReader((Stream) tcpClient.GetStream());
        StreamWriter streamWriter = new StreamWriter((Stream) tcpClient.GetStream());
        try
        {
          byte[] numArray = new byte[1024];
          stream.Read(numArray, 0, numArray.Length);
          int count = 0;
          foreach (byte num in numArray)
          {
            if (num != (byte) 0)
              ++count;
          }
          string str = Encoding.UTF8.GetString(numArray, 0, count);
          progres.Report(str + "\n");
          streamWriter.Flush();
        }
        catch (Exception ex)
        {
          progres.Report("Something Failed in the socket\n");
        }
      }
      while (this.listenerStarted);
      tcpClient.Close();
      Thread.Sleep(10);
      tcpListener.Stop();
      currentThread.Abort();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      this.listenerStarted = false;
      this.richTextBox1.AppendText("Stopping Listener\n");
    }

    private void button3_Click(object sender, EventArgs e)
    {
      this.richTextBox1.Clear();
    }

    private void SocketListener_Load(object sender, EventArgs e)
    {
    }

    private void SocketListener_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.richTextBox1 = new RichTextBox();
      this.button1 = new Button();
      this.button2 = new Button();
      this.button3 = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.richTextBox1.BackColor = Color.FromArgb(69, 73, 74);
      this.richTextBox1.BorderStyle = BorderStyle.None;
      this.richTextBox1.Dock = DockStyle.Bottom;
      this.richTextBox1.ForeColor = Color.White;
      this.richTextBox1.Location = new Point(0, 67);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new Size(325, 314);
      this.richTextBox1.TabIndex = 0;
      this.richTextBox1.Text = "";
      this.button1.FlatStyle = FlatStyle.Flat;
      this.button1.Location = new Point(12, 12);
      this.button1.Name = "button1";
      this.button1.Size = new Size(95, 23);
      this.button1.TabIndex = 1;
      this.button1.Text = "Start";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new EventHandler(this.button1_Click);
      this.button2.FlatStyle = FlatStyle.Flat;
      this.button2.Location = new Point(113, 12);
      this.button2.Name = "button2";
      this.button2.Size = new Size(95, 23);
      this.button2.TabIndex = 3;
      this.button2.Text = "Stop";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new EventHandler(this.button2_Click);
      this.button3.FlatStyle = FlatStyle.Flat;
      this.button3.Location = new Point(214, 12);
      this.button3.Name = "button3";
      this.button3.Size = new Size(95, 23);
      this.button3.TabIndex = 4;
      this.button3.Text = "Clear";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new EventHandler(this.button3_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 44);
      this.label1.Name = "label1";
      this.label1.Size = new Size(93, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Socket Port: 9023";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(60, 63, 65);
      this.ClientSize = new Size(325, 381);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.button3);
      this.Controls.Add((Control) this.button2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.richTextBox1);
      this.ForeColor = Color.White;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (SocketListener);
      this.Text = "Socket Listener";
      this.FormClosing += new FormClosingEventHandler(this.SocketListener_FormClosing);
      this.Load += new EventHandler(this.SocketListener_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
