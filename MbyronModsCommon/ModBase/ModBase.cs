namespace MbyronModsCommon;
using ICities;
using System;
using System.Collections.Generic;
using System.Globalization;
using ColossalFramework.Globalization;
using ColossalFramework;
using ColossalFramework.UI;

public class ModMainInfo<TypeMod> : SingletonMod<TypeMod> where TypeMod : IMod {
    public static string RawName => Instance.RawName;
    public static string ModName => Instance.ModName;
    public static Version ModVersion => Instance.ModVersion;
    public static BuildVersion VersionType => Instance.VersionType;
}

public abstract class ModBase<TypeMod, TypeConfig> : IMod where TypeMod : ModBase<TypeMod, TypeConfig> where TypeConfig : SingletonConfig<TypeConfig>, new() {
    private CultureInfo modCulture;
    public static ILog Log { get; } = Logger.GetLogger(AssemblyUtils.CurrentAssemblyName);
    public virtual string RawName => AssemblyUtils.CurrentAssemblyName;
    public abstract string ModName { get; }
    public virtual Version ModVersion => AssemblyUtils.CurrentAssemblyVersion;
    public abstract ulong StableID { get; }
    public virtual ulong? BetaID { get; }
    public abstract BuildVersion VersionType { get; }
    public string Name => VersionType switch {
        BuildVersion.BetaDebug or BuildVersion.BetaRelease => ModName + " [BETA] " + ModVersion.GetString(),
        _ => ModName + ' ' + ModVersion.GetString(),
    };
    public virtual string Description => string.Empty;
    public bool IsEnabled { get; private set; }
    public abstract List<ModChangeLog> ChangeLog { get; }
    public virtual List<ConflictModInfo> ConflictMods { get; set; }
    public CultureInfo ModCulture {
        get => modCulture;
        set {
            modCulture = value;
            CommonLocalize.Culture = value;
            SetModCulture(value);
        }
    }
    public bool IsLevelLoaded { get; set; }
    public bool LoadedMode { get; set; }

    public ModBase() {
        Log.Info("Start initializing mod");
        SingletonMod<TypeMod>.Instance = (TypeMod)this;
        if (!LoadConfig()) {
            LoadingManager.instance.m_introLoaded += () => MessageBox.Show<OneButtonMessageBox>().Init(Name, CommonLocalize.XMLWariningMessageBox_Warning);
        }
        LoadLocale();
        LocaleManager.eventLocaleChanged += LoadLocale;
    }

    public virtual void SetModCulture(CultureInfo cultureInfo) { }
    public void OnSettingsUI(UIHelperBase helper) {
        Log.Info("Setting UI");
        SettingsUI(helper);
    }
    protected virtual void SettingsUI(UIHelperBase helper) { }
    public void LoadLocale() {
        try {
            string localeID;
            if (SingletonItem<TypeConfig>.Instance.LocaleType == LanguageType.Default) {
                localeID = GetLocale();
            }
            else {
                localeID = SingletonItem<TypeConfig>.Instance.LocaleID;
                if (localeID.IsNullOrWhiteSpace()) {
                    localeID = GetLocale();
                }
            }
            ModCulture = new CultureInfo(localeID);
            Log.Info($"Change mod locale: {ModCulture.Name}({SingletonItem<TypeConfig>.Instance.LocaleType})");
        }
        catch (Exception e) {
            Log.Error(e, $"Could't change mod locale");
        }
    }
    private string GetLocale() => LocaleManager.exists ? Language.LocaleExtension(LocaleManager.instance.language) : Language.LocaleExtension(new SavedString(Settings.localeID, Settings.gameSettingsFile, DefaultSettings.localeID).value);

    public bool LoadConfig() => SingletonConfig<TypeConfig>.Load();
    public void SaveConfig() => SingletonConfig<TypeConfig>.Save();

    public void OnEnabled() {
        Log.Info("Enabled");
        IsEnabled = true;
        if (UIView.GetAView() != null) {
            CallIntroActions();
        }
        else {
            LoadingManager.instance.m_introLoaded += CallIntroActions;
        }
        Enable();
    }

    public void OnDisabled() {
        Log.Info("Disabled");
        IsEnabled = false;
        Disable();
    }

    private void CallIntroActions() {
        ConflictMods ??= new List<ConflictModInfo>();
        SingletonManager<CompatibilityManager>.Instance.Init(ModName, ConflictMods);
        Log.Info("Call intro actions");
        //API.Core.CommonDebug();
        IntroActions();
    }

    protected virtual void Enable() { }
    protected virtual void Disable() { }
    public virtual void IntroActions() { }

    public void ShowLogMessageBox() {
        var lastVersion = new Version(SingletonConfig<TypeConfig>.Instance.ModVersion);
        if ((VersionType != BuildVersion.StableRelease) && (VersionType != BuildVersion.BetaRelease)) {
            SingletonConfig<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
            return;
        }
        if ((lastVersion.Major == ModVersion.Major) && (lastVersion.Minor == ModVersion.Minor) && (lastVersion.Build == ModVersion.Build)) {
            SingletonConfig<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
            SaveConfig();
            return;
        }
        if (lastVersion < ModVersion) {
            MessageBox.Show<LogMessageBox>().Initialize<TypeMod>(true);
        }
        SingletonConfig<TypeConfig>.Instance.ModVersion = ModVersion.ToString();
        SaveConfig();
    }
}

public struct ModChangeLog {
    public Version ModVersion;
    public DateTime Date;
    public List<LogString> Log;
    public ModChangeLog(Version version, DateTime date, List<LogString> log) {
        ModVersion = version;
        Date = date;
        Log = log;
    }
}

public struct LogString {
    public LogFlag Flag = LogFlag.None;
    public string Content = string.Empty;
    public LogString(LogFlag logFlag, string content) {
        Flag = logFlag;
        Content = content;
    }
}
public enum LogFlag {
    None,
    Added,
    Removed,
    Updated,
    Fixed,
    Optimized,
    Translation,
    Attention,
}

public enum BuildVersion {
    BetaDebug,
    BetaRelease,
    StableDebug,
    StableRelease
}

public interface IMod : IUserMod, IModLoading {
    BuildVersion VersionType { get; }
    string RawName { get; }
    string ModName { get; }
    List<ModChangeLog> ChangeLog { get; }
    Version ModVersion { get; }
    ulong StableID { get; }
    CultureInfo ModCulture { get; set; }

    void SaveConfig();
    bool LoadConfig();
    void LoadLocale();

}

public interface IModLoading {
    bool IsLevelLoaded { get; set; }
    bool LoadedMode { get; set; }
    void ShowLogMessageBox();
}

public static class VersionExtension {
    public static string GetString(this Version version) {
        if (version.Revision > 0)
            return version.ToString(4);
        else if (version.Build > 0)
            return version.ToString(3);
        else
            return version.ToString(2);
    }
}