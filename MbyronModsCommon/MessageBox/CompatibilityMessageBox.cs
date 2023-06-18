namespace MbyronModsCommon;
using ColossalFramework;
using MbyronModsCommon.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class CompatibilityMessageBox : MessageBoxBase {
    private string ModName { get; set; } = string.Empty;

    public void Init(string modName, List<ConflictModInfo> conflictModsInfo, Action<MessageBoxBase> disableAction = null, bool first = false) {
        ModName = modName;
        TitleText = $"{modName} {CommonLocalize.OptionPanel_CompatibilityCheck}";
        if (conflictModsInfo is null || !conflictModsInfo.Any()) {
            AddLabelInMainPanel(first ? "The incompatible mods has been unsubscribed, but the game has not been removed normally, it is recommended to restart the game." : CommonLocalize.MessageBox_NormalPrompt);
            AddButton(CommonLocalize.MessageBox_OK, Close);
        } else {
            AddLabelInMainPanel(CommonLocalize.MessageBox_WarningPrompt);
            conflictModsInfo.ForEach(a => AddItem(a));
            AddButton(CommonLocalize.CompatibilityMessageBox_Unsubscribe, () => disableAction(this));
            AddButton(CommonLocalize.Cancel, Close);
        }
    }

    protected AlphaSinglePropertyPanel AddItem(ConflictModInfo mod) {
        var panel = MainPanel.AddUIComponent<AlphaSinglePropertyPanel>();
        panel.Atlas = CustomUIAtlas.MbyronModsAtlas;
        panel.BgSprite = CustomUIAtlas.RoundedRectangle3;
        panel.BgNormalColor = CustomUIColor.CPPrimaryBg;
        panel.Padding = new UnityEngine.RectOffset(10, 10, 10, 10);
        panel.width = MessageBoxParm.ComponentWidth;
        panel.MajorLabelText = mod.name;
        panel.MinorLabelText = (mod.inclusive ? (ModName + " " + CommonLocalize.CompatibilityCheck_SameFunctionality) : CommonLocalize.CompatibilityCheck_Incompatible) + (mod.useInstead.IsNullOrWhiteSpace() ? string.Empty : string.Format(CommonLocalize.CompatibilityCheck_UseInstead, mod.useInstead));
        panel.MinorLabelTextScale = 0.9f;
        panel.StartLayout();
        return panel;
    }
}