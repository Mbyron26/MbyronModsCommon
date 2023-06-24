namespace MbyronModsCommon;
using UnityEngine;

public partial class OptionPanelBase<TypeMod, TypeConfig, TypeOptionPanel> {
    protected virtual void AddToolButtonOptions<T>() where T : class, ITool, new() {
        OptionPanelHelper.AddGroup(GeneralContainer, CommonLocalize.ToolButton);
        var uuiSupport = SingletonTool<T>.Instance.UUISupport;
        var array = uuiSupport ? new string[] { CommonLocalize.None, CommonLocalize.OnlyInGame, CommonLocalize.OnlyInUUI, CommonLocalize.Both } : new string[] { CommonLocalize.None, CommonLocalize.OnlyInGame, };
        if (!uuiSupport) {
            SingletonItem<TypeConfig>.Instance.ToolButtonPresent = (ToolButtonPresent)Mathf.Clamp((int)SingletonItem<TypeConfig>.Instance.ToolButtonPresent, 0, 1);
        }
        OptionPanelHelper.AddDropDown(CommonLocalize.ToolButtonDisplay, null, array, (int)SingletonItem<TypeConfig>.Instance.ToolButtonPresent, 300, 30, ToolButtonDropDownCallBack);
        OptionPanelHelper.Reset();
    }

    protected virtual void ToolButtonDropDownCallBack(int value) =>   SingletonItem<TypeConfig>.Instance.ToolButtonPresent = (ToolButtonPresent)value;
    
}
