using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class AdvMenuHandler : UtageUguiMenuButtons
{
    [Header("拡張要素")]

    [SerializeField] GameObject menuButton;
    
    [SerializeField] UIScaler menuUiScaler;

    /// <summary>
    /// Menuの閉じるボタン押下時のスケーリング処理
    /// </summary>
    public void OnTapMenuActive() {

        if (!menuUiScaler.IsScaleChanged) {
            menuUiScaler.ScaleTransiton();
            menuButton.SetActive(false);
        }
        else {
            menuUiScaler.ReturnScale();
            menuButton.SetActive(true);
        }
    }
}
