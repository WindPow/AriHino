using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class AdvMenuHandler : UtageUguiMenuButtons
{
    [SerializeField] GameObject menuButton;
    
    [SerializeField] UIScaler menuUiScaler;

    /// <summary>
    /// Menuの閉じるボタン押下時のスケーリング処理
    /// </summary>
    public void OnTapMenuActive() {

        if (!menuUiScaler.isScaleChanged) {
            menuUiScaler.ScaleTransiton();
            menuButton.SetActive(false);
        }
        else {
            menuUiScaler.ReturnScale();
            menuButton.SetActive(true);
        }
    }

}
