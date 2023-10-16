﻿using HardHatC2Client.Plugin_BaseClasses;
using HardHatC2Client.Utilities;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace HardHatC2Client.Plugin_Management
{
    public class PluginService
    {
        public static CompositionContainer _container;
        public static AggregateCatalog catalog = new AggregateCatalog();
        public static DirectoryCatalog directoryCatalog; // Reference to DirectoryCatalog
        public static IPluginHub pluginHub;


        public static void RefreshPlugins()
        {
            _container.Dispose();
            directoryCatalog.Refresh();
            _container = new CompositionContainer(catalog);
            pluginHub = _container.GetExportedValue<IPluginHub>();
        }

        public static void InitPlugins()
        {
            char allPlatformPathSeperator = Path.DirectorySeparatorChar;
            string assemblyBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] pathSplit = assemblyBasePath.Split("bin"); // [0] is the main path HardHatC2\HardHatC2Client\ 
            pathSplit[0] = pathSplit[0].Replace("\\", allPlatformPathSeperator.ToString());
            string PluginFolderfilepath = pathSplit[0] + "Plugins";

            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            directoryCatalog = new DirectoryCatalog(PluginFolderfilepath, "*.dll");
            catalog.Catalogs.Add(directoryCatalog);
            _container = new CompositionContainer(catalog);
            pluginHub = _container.GetExportedValue<IPluginHub>();
            Console.WriteLine("Plugins Loaded");
        }

        public static ImplantCommandValidation_Base GetCommandValidationPlugin(string pluginName)
        {
            var taskVal_plugins = pluginHub.ImplantTaskValidation_Plugins;
            var taskVal_plugin = taskVal_plugins.GetPluginEnumerableResult(pluginName);
            var taskVal_base = taskVal_plugin.Value;
            return taskVal_base;
        }

        public static ImplantCreation_Base GetImplantCreationPlugin(string pluginName)
        {
            var creation_plugins = pluginHub.ImplantCreation_Plugins;
            var creation_plugin = creation_plugins.GetPluginEnumerableResult(pluginName);
            var creation_base = creation_plugin.Value;
            return creation_base;
        }
    }
}