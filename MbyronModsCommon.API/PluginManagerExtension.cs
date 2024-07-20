namespace MbyronModsCommon.API;
using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;

public static class PluginManagerExtension {
    public static IEnumerable<PluginManager.PluginInfo> GetPluginsInfoSortByName(this PluginManager _) => Singleton<PluginManager>.instance.GetPluginsInfo().Where(p => p?.userModInstance as IUserMod != null).OrderBy(p => ((IUserMod)p.userModInstance).Name);

    public static bool IsPluginEnabled(string assemblyName) {
        bool found = false;
        PluginManager.instance.GetPluginsInfoSortByName().ForEach(plugin => {
            if (plugin is not null && plugin.userModInstance is IUserMod && plugin.isEnabled) {
                plugin.GetAssemblies().ForEach(a => {
                    if (a.GetName().Name.Equals(assemblyName)) {
                        found = true;
                    }
                });
            }
        });
        return found;
    }

    public static bool IsPluginSubscribed(string assemblyName) {
        bool found = false;
        PluginManager.instance.GetPluginsInfoSortByName().ForEach(plugin => {
            if (plugin is not null && plugin.userModInstance is IUserMod) {
                plugin.GetAssemblies().ForEach(a => {
                    if (a.GetName().Name.Equals(assemblyName)) {
                        found = true;
                    }
                });
            }
        });
        return found;
    }

    public static void GetPluginState(string assemblyName, out bool isSubscribed, out bool isEnabled) {
        isSubscribed = false;
        isEnabled = false;
        foreach (var item in PluginManager.instance.GetPluginsInfoSortByName()) {
            if (item is not null && item.userModInstance is IUserMod) {
                foreach (var assembly in item.GetAssemblies()) {
                    if (assembly.GetName().Name.Equals(assemblyName)) {
                        isSubscribed = true;
                        if (item.isEnabled) {
                            isEnabled = true;
                        }
                    }
                }
            }
        }
    }

    public static bool IsPluginEnabled(ulong id) => PluginManager.instance.GetPluginsInfo().Where(_ => _.isEnabled).Any(_ => _.publishedFileID.AsUInt64 == id);

}
