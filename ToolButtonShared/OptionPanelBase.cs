namespace MbyronModsCommon;

public partial class OptionPanelBase<TypeMod, TypeConfig, TypeOptionPanel> {
    private string[] ToolButtonOptions => new string[] {
        CommonLocalize.OnlyInGame,CommonLocalize.OnlyInUUI,CommonLocalize.Both
    };

    protected virtual void AddToolButtonOptions() {
        OptionPanelHelper.AddGroup(GeneralContainer, CommonLocalize.ToolButton);
        OptionPanelHelper.AddDropDown(CommonLocalize.ToolButtonDisplay, null, ToolButtonOptions, (int)SingletonItem<TypeConfig>.Instance.ToolButtonPresent, 300, 30, ToolButtonDropDownCallBack);
        OptionPanelHelper.Reset();
    }
    protected virtual void ToolButtonDropDownCallBack(int value) {
        SingletonItem<TypeConfig>.Instance.ToolButtonPresent = (ToolButtonPresent)value;
    }
}
