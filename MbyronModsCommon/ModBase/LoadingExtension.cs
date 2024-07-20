namespace MbyronModsCommon;
using ICities;

public abstract class ModLoadingExtension<T> : LoadingExtensionBase where T : IModLoading {
    public virtual void Created(ILoading loading) { }
    public virtual void LevelLoaded(LoadMode mode) { }
    public virtual void LevelUnloading() { }
    public virtual void Released() { }

    public sealed override void OnCreated(ILoading loading) {
        base.OnCreated(loading);
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call loading OnCreated");
        Created(loading);
    }

    public sealed override void OnLevelLoaded(LoadMode mode) {
        SingletonMod<T>.Instance.IsLevelLoaded = true;
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call loading OnLevelLoaded");
        LevelLoaded(mode);
        SingletonMod<T>.Instance.ShowLogMessageBox();
    }

    public sealed override void OnLevelUnloading() {
        SingletonMod<T>.Instance.IsLevelLoaded = false;
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call loading OnLevelUnloading");
        LevelUnloading();
    }

    public sealed override void OnReleased() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call loading OnReleased");
        Released();
    }
}
