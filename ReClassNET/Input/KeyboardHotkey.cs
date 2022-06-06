// Decompiled with JetBrains decompiler
// Type: ReClassNET.Input.KeyboardHotkey
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Input
{
  public class KeyboardHotkey
  {
    private readonly HashSet<System.Windows.Forms.Keys> keys = new HashSet<System.Windows.Forms.Keys>();

    public IEnumerable<System.Windows.Forms.Keys> Keys
    {
      get
      {
        return (IEnumerable<System.Windows.Forms.Keys>) this.keys;
      }
    }

    public bool IsEmpty
    {
      get
      {
        return this.keys.Count == 0;
      }
    }

    public void Clear()
    {
      this.keys.Clear();
    }

    public bool AddKey(System.Windows.Forms.Keys key)
    {
      return this.keys.Add(key);
    }

    public bool Matches(System.Windows.Forms.Keys[] pressedKeys)
    {
      return this.keys.Count != 0 && this.keys.Count <= pressedKeys.Length && this.keys.All<System.Windows.Forms.Keys>(new Func<System.Windows.Forms.Keys, bool>(((Enumerable) pressedKeys).Contains<System.Windows.Forms.Keys>));
    }

    public KeyboardHotkey Clone()
    {
      KeyboardHotkey keyboardHotkey = new KeyboardHotkey();
      foreach (System.Windows.Forms.Keys key in this.Keys)
        keyboardHotkey.AddKey(key);
      return keyboardHotkey;
    }

    public override string ToString()
    {
      return this.keys.Count == 0 ? string.Empty : this.keys.Select<System.Windows.Forms.Keys, string>((Func<System.Windows.Forms.Keys, string>) (k => k.ToString())).Aggregate<string>((Func<string, string, string>) ((s1, s2) => s1 + " + " + s2));
    }
  }
}
