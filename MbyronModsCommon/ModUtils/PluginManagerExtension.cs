namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using ColossalFramework;
using System.Collections.Generic;
using System.Linq;
using ICities;
using ColossalFramework.UI;
using Epic.OnlineServices.Presence;

public static class PluginManagerExtension {
    public static IEnumerable<PluginManager.PluginInfo> GetPluginsInfoSortByName(this PluginManager pluginManager) => Singleton<PluginManager>.instance.GetPluginsInfo().Where(p => p?.userModInstance as IUserMod != null).OrderBy(p => ((IUserMod)p.userModInstance).Name);

    public static bool FindPlugin(string assemblyName) {
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

}