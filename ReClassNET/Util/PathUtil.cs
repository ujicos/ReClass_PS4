// Decompiled with JetBrains decompiler
// Type: ReClassNET.Util.PathUtil
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using System;
using System.IO;
using System.Reflection;

namespace ReClassNET.Util
{
  public class PathUtil
  {
    private static readonly Lazy<string> executablePath = new Lazy<string>((Func<string>) (() =>
    {
      string str = (string) null;
      try
      {
        str = Assembly.GetExecutingAssembly().Location;
      }
      catch
      {
      }
      if (string.IsNullOrEmpty(str))
        str = PathUtil.FileUrlToPath(Assembly.GetExecutingAssembly().GetName().CodeBase);
      return str;
    }));
    private static readonly Lazy<string> executableFolderPath = new Lazy<string>((Func<string>) (() => Path.GetDirectoryName(PathUtil.executablePath.Value)));
    private static readonly Lazy<string> temporaryFolderPath = new Lazy<string>(new Func<string>(Path.GetTempPath));
    private static readonly Lazy<string> settingsFolderPath = new Lazy<string>((Func<string>) (() =>
    {
      string folderPath;
      try
      {
        folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      }
      catch (Exception ex)
      {
        folderPath = PathUtil.executableFolderPath.Value;
      }
      string path1;
      try
      {
        path1 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      }
      catch (Exception ex)
      {
        path1 = folderPath;
      }
      return Path.Combine(path1, "ReClass.NET");
    }));
    private static readonly Lazy<string> launcherExecutablePath = new Lazy<string>((Func<string>) (() =>
    {
      string path = Path.Combine(Directory.GetParent(PathUtil.ExecutableFolderPath).FullName, "ReClass.NET_Launcher.exe");
      return File.Exists(path) ? path : (string) null;
    }));

    public static string ExecutablePath
    {
      get
      {
        return PathUtil.executablePath.Value;
      }
    }

    public static string ExecutableFolderPath
    {
      get
      {
        return PathUtil.executableFolderPath.Value;
      }
    }

    public static string TemporaryFolderPath
    {
      get
      {
        return PathUtil.temporaryFolderPath.Value;
      }
    }

    public static string SettingsFolderPath
    {
      get
      {
        return PathUtil.settingsFolderPath.Value;
      }
    }

    public static string LauncherExecutablePath
    {
      get
      {
        return PathUtil.launcherExecutablePath.Value;
      }
    }

    public static string FileUrlToPath(string url)
    {
      if (url.StartsWith("file:///", StringComparison.OrdinalIgnoreCase))
        url = url.Substring(8);
      url = url.Replace('/', Path.DirectorySeparatorChar);
      return url;
    }
  }
}
