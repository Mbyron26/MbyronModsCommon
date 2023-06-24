namespace MbyronModsCommon;
using ICities;

public abstract class ModLoadingExtension<T> : LoadingExtensionBase where T : IModLoading {
    public virtual void Created(ILoading loading) { }
    public virtual void LevelLoaded(LoadMode mode) { }
    public virtual void LevelUnloading() { }
    public virtual void Released() { }

    public sealed override void OnCreated(ILoading loading) {
        base.OnCreated(loading);
        InternalLogger.Log("Call loading OnCreated");
        Created(loading);
    }

    public sealed override void OnLevelLoaded(LoadMode mode) {
        InternalLogger.Log("Call loading OnLevelLoaded");
        LevelLoaded(mode);
        SingletonMod<T>.Instance.IsLevelLoaded = true;
        SingletonMod<T>.Instance.ShowLogMessageBox();
    }

    public sealed override void OnLevelUnloading() {
        InternalLogger.Log("Call loading OnLevelUnloading");
        LevelUnloading();
        SingletonMod<T>.Instance.IsLevelLoaded = false;
    }

    public sealed override void OnReleased() {
        InternalLogger.Log("Call loading OnReleased");
        Released();
    }
}
