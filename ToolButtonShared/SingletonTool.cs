namespace MbyronModsCommon;
using ColossalFramework.UI;
using System;
using UnifiedUI.Helpers;
using UnityEngine;

public abstract class SingletonToolManager<TypeTool, TypeInGameToolButton, TypeMod, TypeConfig> : SingletonTool<TypeTool>, ITool where TypeTool : class, new() where TypeInGameToolButton : ToolButtonBase<TypeConfig> where TypeMod : IMod where TypeConfig : SingletonConfig<TypeConfig>, new() {
    public Action EnableCallback;
    public Action DisableCallback;

    public TypeInGameToolButton InGameToolButton { get; private set; }
    public override bool UUISupport { get; } = PluginManagerExtension.IsPluginEnabled("UnifiedUIMod");
    protected UUICustomButton UUIButton { get; set; }
    protected abstract Texture2D UUIIcon { get; }
    public bool UUIRegistered { get; protected set; }
    protected abstract string Tooltip { get; }
    public bool UUIButtonIsPressed {
        set {
            if (UUIButton is not null) {
                UUIButton.IsPressed = value;
            }
        }
    }
    public override bool IsInit { get; set; }

    public override void Init() {
        Enable();
        IsInit = true;
    }
    public override void DeInit() {
        Disable();
        IsInit = false;
    }

    public override void Enable() {
        if (UUISupport) {
            if (SingletonConfig<TypeConfig>.Instance.ToolButtonPresent == ToolButtonPresent.InGame) {
                AddInGameButton();
            } else if (SingletonItem<TypeConfig>.Instance.ToolButtonPresent == ToolButtonPresent.UUI) {
                RegisterUUI();
            } else if (SingletonItem<TypeConfig>.Instance.ToolButtonPresent == ToolButtonPresent.Both) {
                AddInGameButton();
                RegisterUUI();
            }
        } else {
            EnsurePresent();
            if (SingletonConfig<TypeConfig>.Instance.ToolButtonPresent == ToolButtonPresent.InGame) {
                AddInGameButton();
            }
        }
        EnableCallback?.Invoke();
    }
    public override void Disable() {
        if (UUISupport) {
            LogoutUUI();
        }
        RemoveInGameButton();
        DisableCallback?.Invoke();
    }

    protected virtual void EnsurePresent() {
        if (!UUISupport && (SingletonConfig<TypeConfig>.Instance.ToolButtonPresent != ToolButtonPresent.InGame || SingletonConfig<TypeConfig>.Instance.ToolButtonPresent != ToolButtonPresent.None)) {
            SingletonConfig<TypeConfig>.Instance.ToolButtonPresent = ToolButtonPresent.InGame;
            SingletonMod<TypeMod>.Instance.SaveConfig();
        }
    }

    protected void AddInGameButton() {
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Adding InGame button");
        InGameToolButton = UIView.GetAView().AddUIComponent(typeof(TypeInGameToolButton)) as TypeInGameToolButton;
        InGameToolButton.tooltipBox = UIView.GetAView()?.defaultTooltipBox;
        InGameToolButton.tooltip = Tooltip;
        InGameToolButton.EventToggleChanged += InGameToolButtonToggle;
    }
    protected void RemoveInGameButton() {
        if (InGameToolButton is null) {
            return;
        }
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("Removing InGame button");
        InGameToolButton.EventToggleChanged -= InGameToolButtonToggle;
        InGameToolButton.tooltip = Tooltip;
        InGameToolButton.Destroy();
        InGameToolButton = null;
    }
    protected void RegisterUUI() {
        if (UUIRegistered || !UUISupport) {
            return;
        }
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("UnifiedUI detected, registering UUI");
        UUIButton = UUIHelpers.RegisterCustomButton(AssemblyUtils.CurrentAssemblyName, null, Tooltip, UUIIcon, UUIButtonToggle);
        UUIButton.Button.eventTooltipEnter += (c, e) => c.tooltip = Tooltip;
        UUIButton.IsPressed = false;
        UUIRegistered = true;
    }
    protected void LogoutUUI() {
        if (!UUIRegistered || !UUISupport || UUIButton is null) {
            return;
        }
        Logger.GetLogger(AssemblyUtils.CurrentAssemblyName).Info("UnifiedUI detected, logouting UUI");
        UUIButton.Button.Destroy();
        UUIButton = null;
        UUIRegistered = false;
    }

    protected abstract void UUIButtonToggle(bool isOn);
    protected abstract void InGameToolButtonToggle(bool isOn);

}

public abstract class SingletonTool<T> : SingletonManager<T>, ITool where T : class, new() {
    public abstract bool UUISupport { get; }

    public abstract void Disable();
    public abstract void Enable();
}

public interface ITool {
    bool UUISupport { get; }
    void Enable();
    void Disable();
}

public partial class SingletonConfig<T> where T : SingletonConfig<T>, new() {
    public ToolButtonPresent ToolButtonPresent { get; set; } = ToolButtonPresent.UUI;
    public float ToolButtonPositionX { get; set; }
    public float ToolButtonPositionY { get; set; }
}

public enum ToolButtonPresent {
    None,
    InGame,
    UUI,
    Both,
}