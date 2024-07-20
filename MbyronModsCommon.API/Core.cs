namespace MbyronModsCommon.API;
using ColossalFramework;
using ColossalFramework.Plugins;
using ICities;
using System;
using System.Text;
using UnityEngine;

public static class Core {
    public static uint CalledTime { get; private set; }
    public static void CommonDebug() {
        CalledTime++;
        if (CalledTime > 1) {
            return;
        }
        StringBuilder stringBuilder = new();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(@"------ MbyronModsCommon.API Common Debug ------");
        stringBuilder.AppendLine(@"C# CLR Version: " + Environment.Version);
        stringBuilder.AppendLine(@"Unity Version: " + Application.unityVersion);
        stringBuilder.AppendLine(@"Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        stringBuilder.AppendLine(@"Plugins:");
        foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfoSortByName()) {
            if (info is not null && info.userModInstance is IUserMod modInstance)
                stringBuilder.AppendLine($"{info.name} - {modInstance.Name} " + (info.isEnabled ? @"** Enabled **" : @"** Disabled **"));
        }
        stringBuilder.AppendLine(@"--------------------------------------------------------------");
        Logger.Log(stringBuilder.ToString());
    }
}
