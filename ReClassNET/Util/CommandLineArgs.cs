// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.CommandLineArgs
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.Collections.Generic;

namespace ReClassNET.Util
{
  public class CommandLineArgs
  {
    private readonly List<string> fileNames = new List<string>();
    private readonly SortedDictionary<string, string> parms = new SortedDictionary<string, string>();

    public string FileName
    {
      get
      {
        return this.fileNames.Count >= 1 ? this.fileNames[0] : (string) null;
      }
    }

    public IEnumerable<string> FileNames
    {
      get
      {
        return (IEnumerable<string>) this.fileNames;
      }
    }

    public IEnumerable<KeyValuePair<string, string>> Parameters
    {
      get
      {
        return (IEnumerable<KeyValuePair<string, string>>) this.parms;
      }
    }

    public CommandLineArgs(string[] args)
    {
      if (args == null)
        return;
      foreach (string str in args)
      {
        if (!string.IsNullOrEmpty(str))
        {
          KeyValuePair<string, string> parameter = CommandLineArgs.GetParameter(str);
          if (parameter.Key.Length == 0)
            this.fileNames.Add(parameter.Value);
          else
            this.parms[parameter.Key] = parameter.Value;
        }
      }
    }

    public string this[string strKey]
    {
      get
      {
        string str;
        return this.parms.TryGetValue(strKey.ToLower(), out str) ? str : (string) null;
      }
    }

    internal static KeyValuePair<string, string> GetParameter(string str)
    {
      if (str.StartsWith("--"))
      {
        str = str.Remove(0, 2);
      }
      else
      {
        if (!str.StartsWith("-"))
          return new KeyValuePair<string, string>(string.Empty, str);
        str = str.Remove(0, 1);
      }
      int val1 = str.IndexOf(':');
      int val2 = str.IndexOf('=');
      if (val1 < 0 && val2 < 0)
        return new KeyValuePair<string, string>(str.ToLower(), string.Empty);
      int length = Math.Min(val1, val2);
      if (length < 0)
        length = val1 < 0 ? val2 : val1;
      return length <= 0 ? new KeyValuePair<string, string>(str.ToLower(), string.Empty) : new KeyValuePair<string, string>(str.Substring(0, length).ToLower(), str.Remove(0, length + 1));
    }
  }
}
