namespace MbyronModsCommon;
using System.IO;
using System.Xml.Serialization;
using System;
using ColossalFramework.IO;

public partial class SingletonConfig<T> : SingletonItem<T> where T : SingletonConfig<T>, new() {
    public string ModVersion { get; set; } = "0.0.0";
    public LanguageType LocaleType { get; set; } = LanguageType.Default;
    public string LocaleID { get; set; } = string.Empty;
    public bool DebugMode { get; set; }
    [XmlIgnore]
    public static string ConfigFilePath => Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");

    public static void Save() {
        try {
            using StreamWriter sw = new(ConfigFilePath);
            XmlSerializer xmlSerializer = new(typeof(T));
            XmlSerializerNamespaces xmlSerializerNamespaces = new();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);
            xmlSerializer.Serialize(sw, Instance, xmlSerializerNamespaces);
        } catch (Exception e) {
            InternalLogger.Exception($"Saving {ConfigFilePath} failed", e);
        }
    }
    public static bool Load() {
        bool result;
        InternalLogger.Log("Start loading mod config data");
        var path = Path.Combine(DataLocation.localApplicationData, $"{AssemblyUtils.CurrentAssemblyName}Config.xml");
        try {
            if (File.Exists(path)) {
                using StreamReader sr = new(path);
                XmlSerializer xmlSerializer = new(typeof(T));
                var c = xmlSerializer.Deserialize(sr);
                if (c is not T) {
                    InternalLogger.Log($"XML config cannot be deserialized, the target path: {path}");
                    InternalLogger.Log("Generate mod default data");
                    Instance = new();
                    result = false;
                } else {
                    Instance = c as T;
                    result = true;
                    InternalLogger.Log("Local config exists, loaded the config successfully");
                }
            } else {
                InternalLogger.Log($"No local config found, use mod default config");
                Instance = new();
                result = true;
            }
        } catch (Exception e) {
            InternalLogger.Exception($"Could't load data from XML", e);
            Instance = new();
            Save();
            InternalLogger.Log($"Generate mod default data");
            result = false;
        }
        return result;
    }
}