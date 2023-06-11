namespace MbyronModsCommon;
using System;
using UnityEngine;

public class ResetModWarningMessageBox : MessageBoxBase {
    public void Init<T>(Action callback) where T : IMod {
        TitleText = $"{ModMainInfo<T>.ModName} {CommonLocalize.Reset}";
        AddLabelInMainPanel(CommonLocalize.ResetModWarning);
        AddButtons(1, 2, CommonLocalize.MessageBox_OK, () => callback()).TextNormalColor = Color.red;
        AddButtons(2, 2, CommonLocalize.Cancel, Close);
    }
}

public class ResetModMessageBox : OneButtonMessageBox {
    public void Init<T>(bool isSucceeded = true) where T : IMod {
        TitleText = $"{ModMainInfo<T>.ModName} {CommonLocalize.Reset}";
        if (isSucceeded) {
            AddLabelInMainPanel(CommonLocalize.ResetModSucceeded);
        } else {
            AddLabelInMainPanel(CommonLocalize.ResetModFailed);
        }
    }
}