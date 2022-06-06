// Decompiled with JetBrains decompiler
// Type: ReClassNET.Memory.ProcessInfo
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.Native;
using System;
using System.Drawing;

namespace ReClassNET.Memory
{
  public class ProcessInfo
  {
    private readonly Lazy<Image> icon;

    public IntPtr Id { get; }

    public string Name { get; }

    public string Path { get; }

    public Image Icon
    {
      get
      {
        return this.icon.Value;
      }
    }

    public ProcessInfo(IntPtr id, string name, string path)
    {
      this.Id = id;
      this.Name = name;
      this.Path = path;
      this.icon = new Lazy<Image>((Func<Image>) (() =>
      {
        using (System.Drawing.Icon iconForFile = NativeMethods.GetIconForFile(this.Path))
          return (Image) iconForFile?.ToBitmap();
      }));
    }
  }
}
