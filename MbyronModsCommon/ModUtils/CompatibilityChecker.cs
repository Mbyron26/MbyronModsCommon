namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using ColossalFramework.PlatformServices;

public class CompatibilityChecker<T> : SingletonClass<CompatibilityCheck> where T : IMod {
    public Action<ConflictModInfo, bool> EventConflictModRemoved;
    public Action<bool> EventConflictModsRemoved;

    public List<ConflictModInfo> ConflictMods { get; private set; }
    public List<ConflictModInfo> DetectedConflictMods { get; private set; }

    public bool RemoveConflictMods() {
        if (DetectedConflictMods.Any()) {
            return false;
        }
        List<bool> flags = new();
        foreach (var item in DetectedConflictMods) {
            flags.Add(RemoveConflictMod(item, false));
        }
        var flag = flags.TrueForAll(x => x = true);
        EventConflictModsRemoved?.Invoke(flag);
        return flag;
    }

    public bool RemoveConflictMod(ConflictModInfo mod, bool callEvent) {
        var flag = PlatformService.workshop.Unsubscribe(new PublishedFileId(mod.fileID));
        if (flag) {
            InternalLogger.Log($"Unsubscribed conflict mod succeeded: {mod.name}");
        } else {
            InternalLogger.Log($"Unsubscribed conflict mod failed: {mod.name}");
        }
        if (callEvent) {
            EventConflictModRemoved?.Invoke(mod, flag);
        }
        return flag;
    }

    private void CheckConflictMods() {
        if (ConflictMods is null || !ConflictMods.Any() || DetectedConflictMods is null)
            return;
        DetectedConflictMods.Clear();
        foreach (PluginManager.PluginInfo info in Singleton<PluginManager>.instance.GetPluginsInfoSortByName()) {
            if (info is not null && info.userModInstance is IUserMod) {
                for (int i = 0; i < ConflictMods.Count; i++) {
                    if (info.publishedFileID.AsUInt64 == ConflictMods[i].fileID) {
                        DetectedConflictMods.Add(ConflictMods[i]);
                    }
                }
            }
        }
        if (DetectedConflictMods.Count > 0) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"[{DateTime.Now}] Detected conflict mods:");
            stringBuilder.AppendLine();
            foreach (var item in DetectedConflictMods) {
                stringBuilder.Append($"[{item.name}]\n");
            }
            InternalLogger.Warning(stringBuilder.ToString());
        }
    }

    public void Init() {
        ConflictMods = new List<ConflictModInfo>();
        DetectedConflictMods = new List<ConflictModInfo>();
        InternalLogger.Log("Init compatibility checker");
    }
}

public readonly struct ConflictModInfo {
    public readonly ulong fileID;
    public readonly string name;
    public readonly string useInstead;
    public readonly bool inclusive;

    public ConflictModInfo(ulong modID, string modName, bool isInclusive) {
        fileID = modID;
        name = modName;
        useInstead = null;
        inclusive = isInclusive;
    }
    public ConflictModInfo(ulong modID, string modName, bool isInclusive, string useInstead) {
        fileID = modID;
        name = modName;
        this.useInstead = useInstead;
        inclusive = isInclusive;
    }
}