namespace MbyronModsCommon;
using System.IO;
using System.Xml.Serialization;
using System;
using ColossalFramework.IO;

public partial class SingletonConfig<T> : SingletonItem<T> where T : SingletonConfig<T>, new() {
    public string ModVersion { get; set; } = "0.0.0";
    public LanguageType LocaleType { get; set; } = LanguageType.Default;
    public string LocaleID { get; set; } = string.Empty;
    [XmlIgnore]
    public static string ConfigFilePath => Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");

    public static void Save() {
        try {
            using StreamWriter sw = new(ConfigFilePath);
            XmlSerializer xmlSerializer = new(typeof(T));
            XmlSerializerNamespaces xmlSerializerNamespaces = new();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            xmlSerializer.Serialize(sw, Instance, xmlSerializerNamespaces);
        }
        catch (Exception e) {
            Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Error(e, $"Saving {ConfigFilePath} failed");
        }
    }
    public static bool Load() {
        bool result;
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Start loading mod config data");
        var path = Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");
        try {
            if (File.Exists(path)) {
                using StreamReader sr = new(path);
                XmlSerializer xmlSerializer = new(typeof(T));
                var c = xmlSerializer.Deserialize(sr);
                if (c is not T) {
                    Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Warn($"XML config cannot be deserialized, the target path: {path}");
                    Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Generate mod default data");
                    Instance = new();
                    result = false;
                }
                else {
                    Instance = c as T;
                    result = true;
                    Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Local config exists, loaded the config successfully");
                }
            }
            else {
                Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info($"No local config found, use mod default config");
                Instance = new();
                result = true;
            }
        }
        catch (Exception e) {
            Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Error(e,$"Could't load data from XML");
            Instance = new();
            Save();
            Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info($"Generate mod default data");
            result = false;
        }
        return result;
    }
}