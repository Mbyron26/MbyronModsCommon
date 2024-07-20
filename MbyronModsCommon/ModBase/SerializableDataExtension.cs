namespace MbyronModsCommon;
using ICities;

public abstract class ModSerializableDataExtension : SerializableDataExtensionBase {
    public virtual void Created(ISerializableData serializableData) { }
    public virtual void LoadData() { }
    public virtual void SaveData() { }
    public virtual void Released() { }

    public sealed override void OnCreated(ISerializableData serializableData) {
        base.OnCreated(serializableData);
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call serializable data OnCreated");
        Created(serializableData);
    }

    public sealed override void OnLoadData() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call serializable data OnLoadData");
        LoadData();
    }

    public sealed override void OnSaveData() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call serializable data OnSaveData");
        SaveData();
    }

    public sealed override void OnReleased() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call serializable data OnReleased");
        Released();
    }
}