namespace MbyronModsCommon;
using ColossalFramework.Plugins;
using ColossalFramework;
using ICities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using ColossalFramework.PlatformServices;

public class CompatibilityManager : SingletonManager<CompatibilityManager> {
    private string ModName { get; set; } = string.Empty;
    public override bool IsInit { get; set; }
    public List<ConflictModInfo> ConflictMods { get; set; }
    public List<ConflictModInfo> DetectedConflictMods { get; private set; }

    private bool RemoveConflictMods() {
        List<bool> result = new();
        foreach (var item in DetectedConflictMods) {
            result.Add(RemoveConflictMod(item));
        }
        return result.TrueForAll(x => true);
    }

    private bool RemoveConflictMod(ConflictModInfo mod) {
        var flag = PlatformService.workshop.Unsubscribe(new PublishedFileId(mod.fileID));
        if (flag) {
            InternalLogger.Log($"Unsubscribed conflict mod succeeded: {mod.name}");
        } else {
            InternalLogger.Log($"Unsubscribed conflict mod failed: {mod.name}");
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

    public override void DeInit() {
        ConflictMods = null;
        DetectedConflictMods = null;
    }

    public void ShowMessageBox() {
        CheckConflictMods();
        MessageBox.Show<CompatibilityMessageBox>().Init(ModName, DetectedConflictMods, DisableAction);
    }

    private void DisableAction(MessageBoxBase messageBoxBase) {
        MessageBox.Hide(messageBoxBase);
        var result = RemoveConflictMods();
        CheckConflictMods();
        if (result) {
            DetectedConflictMods.Clear();
            MessageBox.Show<CompatibilityMessageBox>().Init(ModName, DetectedConflictMods, first: true);
        } else {
            MessageBox.Show<OneButtonMessageBox>().Init($"{ModName} {CommonLocalize.OptionPanel_CompatibilityCheck}", CommonLocalize.CompatibilityCheckRequestRestart);
        }
    }

    public void Init(string modName, List<ConflictModInfo> conflictMods) {
        ConflictMods = conflictMods;
        ModName = modName;
        DetectedConflictMods = new();
        CheckConflictMods();
        if (DetectedConflictMods.Any()) {
            MessageBox.Show<CompatibilityMessageBox>().Init(ModName, DetectedConflictMods, DisableAction);
        }
    }

    public override void Init() {
        ConflictMods = new List<ConflictModInfo>();
        DetectedConflictMods = new List<ConflictModInfo>();
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