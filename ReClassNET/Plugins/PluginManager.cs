// Decompiled with JetBrains decompiler
// Type: ReClassNET.Plugins.PluginManager
// Assembly: ReClass.NET, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 92B1334E-F9AF-41DF-AFE3-F9686CA00880
// Assembly location: C:\Users\Mariu\Documents\ReClass\PS4\ReClass.NET.exe

using ReClassNET.CodeGenerator;
using ReClassNET.Core;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Native;
using ReClassNET.Nodes;
using ReClassNET.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace ReClassNET.Plugins
{
  internal sealed class PluginManager
  {
    private readonly List<PluginInfo> plugins = new List<PluginInfo>();
    private readonly IPluginHost host;

    public IEnumerable<PluginInfo> Plugins
    {
      get
      {
        return (IEnumerable<PluginInfo>) this.plugins;
      }
    }

    public PluginManager(IPluginHost host)
    {
      this.host = host;
    }

    public void LoadAllPlugins(string path, ILogger logger)
    {
      try
      {
        if (!Directory.Exists(path))
          return;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        this.LoadPlugins((IEnumerable<FileInfo>) directoryInfo.GetFiles("*.dll", SearchOption.AllDirectories), logger, true);
        this.LoadPlugins((IEnumerable<FileInfo>) directoryInfo.GetFiles("*.exe", SearchOption.AllDirectories), logger, true);
        this.LoadPlugins((IEnumerable<FileInfo>) directoryInfo.GetFiles("*.so", SearchOption.AllDirectories), logger, false);
      }
      catch (Exception ex)
      {
        logger.Log(ex);
      }
    }

    private void LoadPlugins(IEnumerable<FileInfo> files, ILogger logger, bool checkProductName)
    {
      foreach (FileInfo file in files)
      {
        FileVersionInfo versionInfo;
        try
        {
          versionInfo = FileVersionInfo.GetVersionInfo(file.FullName);
          if (checkProductName)
          {
            if (versionInfo.ProductName != "ReClass.NET Plugin")
            {
              if (versionInfo.ProductName != "ReClass.NET Native Plugin")
                continue;
            }
          }
        }
        catch
        {
          continue;
        }
        try
        {
          PluginInfo pluginInfo = new PluginInfo(file.FullName, versionInfo);
          if (!pluginInfo.IsNative)
          {
            pluginInfo.Interface = PluginManager.CreatePluginInstance(pluginInfo.FilePath);
            if (pluginInfo.Interface.Initialize(this.host))
            {
              PluginManager.RegisterNodeInfoReaders(pluginInfo);
              PluginManager.RegisterCustomNodeTypes(pluginInfo);
            }
            else
              continue;
          }
          else
          {
            pluginInfo.NativeHandle = PluginManager.CreateNativePluginInstance(pluginInfo.FilePath);
            Program.CoreFunctions.RegisterFunctions(pluginInfo.Name, (ICoreProcessFunctions) new NativeCoreWrapper(pluginInfo.NativeHandle));
          }
          this.plugins.Add(pluginInfo);
        }
        catch (Exception ex)
        {
          logger.Log(ex);
        }
      }
    }

    public void UnloadAllPlugins()
    {
      foreach (PluginInfo plugin in this.plugins)
      {
        if (plugin.Interface != null)
        {
          PluginManager.DeregisterNodeInfoReaders(plugin);
          PluginManager.DeregisterCustomNodeTypes(plugin);
        }
        plugin.Dispose();
      }
      this.plugins.Clear();
    }

    private static Plugin CreatePluginInstance(string filePath)
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(filePath);
      string typeName = withoutExtension + "." + withoutExtension + "Ext";
      if (Activator.CreateInstanceFrom(filePath, typeName).Unwrap() is Plugin plugin)
        return plugin;
      throw new FileLoadException();
    }

    private static IntPtr CreateNativePluginInstance(string filePath)
    {
      IntPtr ptr = NativeMethods.LoadLibrary(filePath);
      if (!ptr.IsNull())
        return ptr;
      throw new FileLoadException("Failed to load native plugin: " + Path.GetFileName(filePath));
    }

    private static void RegisterNodeInfoReaders(PluginInfo pluginInfo)
    {
      IReadOnlyList<INodeInfoReader> nodeInfoReaders = pluginInfo.Interface.GetNodeInfoReaders();
      if (nodeInfoReaders == null || nodeInfoReaders.Count == 0)
        return;
      pluginInfo.NodeInfoReaders = nodeInfoReaders;
      BaseNode.NodeInfoReader.AddRange((IEnumerable<INodeInfoReader>) nodeInfoReaders);
    }

    private static void DeregisterNodeInfoReaders(PluginInfo pluginInfo)
    {
      if (pluginInfo.NodeInfoReaders == null)
        return;
      foreach (INodeInfoReader nodeInfoReader in (IEnumerable<INodeInfoReader>) pluginInfo.NodeInfoReaders)
        BaseNode.NodeInfoReader.Remove(nodeInfoReader);
    }

    private static void RegisterCustomNodeTypes(PluginInfo pluginInfo)
    {
      Plugin.CustomNodeTypes customNodeTypes = pluginInfo.Interface.GetCustomNodeTypes();
      if (customNodeTypes == null)
        return;
      if (customNodeTypes.NodeTypes == null || customNodeTypes.Serializer == null || customNodeTypes.CodeGenerator == null)
        throw new ArgumentException();
      foreach (Type nodeType in (IEnumerable<Type>) customNodeTypes.NodeTypes)
      {
        if (!nodeType.IsSubclassOf(typeof (BaseNode)))
          throw new ArgumentException(string.Format("Type '{0}' is not a valid node.", (object) nodeType));
      }
      pluginInfo.CustomNodeTypes = customNodeTypes;
      NodeTypesBuilder.AddPluginNodeGroup(pluginInfo.Interface, customNodeTypes.NodeTypes);
      CustomNodeSerializer.Add(customNodeTypes.Serializer);
      CppCodeGenerator.Add(customNodeTypes.CodeGenerator);
    }

    private static void DeregisterCustomNodeTypes(PluginInfo pluginInfo)
    {
      if (pluginInfo.CustomNodeTypes == null)
        return;
      NodeTypesBuilder.RemovePluginNodeGroup(pluginInfo.Interface);
      CustomNodeSerializer.Remove(pluginInfo.CustomNodeTypes.Serializer);
      CppCodeGenerator.Remove(pluginInfo.CustomNodeTypes.CodeGenerator);
    }
  }
}
