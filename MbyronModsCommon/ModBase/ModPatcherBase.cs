namespace MbyronModsCommon;
using CitiesHarmony.API;

public abstract class ModPatcherBase<TypeMod, TypeConfig> : ModBase<TypeMod, TypeConfig> where TypeMod : ModBase<TypeMod, TypeConfig> where TypeConfig : SingletonConfig<TypeConfig>, new() {
    public virtual string HarmonyID => $"Mbyron26.{RawName}";
    public bool IsPatched { get; private set; }
    public HarmonyPatcher Patcher { get; private set; }

    protected override void Enable() {
        PatchAll();
        base.Enable();
    }
    protected override void Disable() => UnpatchAll();

    protected virtual void PatchAll() {
        if (IsPatched) return;
        if (HarmonyHelper.IsHarmonyInstalled) {
            Log.Info("Starting Harmony patches");
            Patcher = new HarmonyPatcher(Log, HarmonyID);
            Patcher.Harmony.PatchAll();
            PatchAction(Patcher);
            IsPatched = true;
            Log.Info("Harmony patches completed");
        }
        else {
            Log.Error("Harmony is not installed correctly");
        }
    }
    protected virtual void UnpatchAll() {
        if (!IsPatched || !HarmonyHelper.IsHarmonyInstalled) 
            return;
        Log.Info("Reverting Harmony patches");
        Patcher.Harmony.UnpatchAll(HarmonyID);
        Patcher = null;
        IsPatched = false;
    }
    protected virtual void PatchAction(HarmonyPatcher harmonyPatcher) { }
    
}