// Decompiled with JetBrains decompiler
// Type: ReClassNET.Extensions.RichTextBoxExtension
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReClassNET.Extensions
{
  public static class RichTextBoxExtension
  {
    private const int EmGetrect = 178;
    private const int EmSetrect = 179;

    public static void SetInnerMargin(
      this TextBoxBase textBox,
      int left,
      int top,
      int right,
      int bottom)
    {
      Rectangle formattingRect = textBox.GetFormattingRect();
      Rectangle rect = new Rectangle(left, top, formattingRect.Width - left - right, formattingRect.Height - top - bottom);
      textBox.SetFormattingRect(rect);
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(
      IntPtr hWnd,
      uint msg,
      int wParam,
      ref RichTextBoxExtension.RECT rect);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(
      IntPtr hwnd,
      int wMsg,
      IntPtr wParam,
      ref Rectangle lParam);

    private static void SetFormattingRect(this TextBoxBase textbox, Rectangle rect)
    {
      RichTextBoxExtension.RECT rect1 = new RichTextBoxExtension.RECT(rect);
      RichTextBoxExtension.SendMessage(textbox.Handle, 179U, 0, ref rect1);
    }

    private static Rectangle GetFormattingRect(this TextBoxBase textbox)
    {
      Rectangle lParam = new Rectangle();
      RichTextBoxExtension.SendMessage(textbox.Handle, 178, (IntPtr) 0, ref lParam);
      return lParam;
    }

    private readonly struct RECT
    {
      public readonly int Left;
      public readonly int Top;
      public readonly int Right;
      public readonly int Bottom;

      private RECT(int left, int top, int right, int bottom)
      {
        this.Left = left;
        this.Top = top;
        this.Right = right;
        this.Bottom = bottom;
      }

      public RECT(Rectangle r)
        : this(r.Left, r.Top, r.Right, r.Bottom)
      {
      }
    }
  }
}
