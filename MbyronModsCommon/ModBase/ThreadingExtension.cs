namespace MbyronModsCommon;
using ICities;
using System;

public abstract class ModThreadExtensionBase : ThreadingExtensionBase {
    public virtual void Created(IThreading threading) { }
    public virtual void Released() { }

    protected void AddCallOnceInvoke(bool target, ref bool flag, Action action) {
        if (target) {
            if (!flag) {
                flag = true;
                action.Invoke();
            }
        } else {
            flag = false;
        }
    }

    public sealed override void OnCreated(IThreading threading) {
        base.OnCreated(threading);
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call threading OnCreated");
        Created(threading);
    }

    public sealed override void OnReleased() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Call threading OnReleased");
        Released();
    }
}