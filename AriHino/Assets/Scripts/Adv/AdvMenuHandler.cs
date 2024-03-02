using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class AdvMenuHandler : UtageUguiMenuButtons
{
    [SerializeField] GameObject menuButton;
    
    [SerializeField] UIScaler menuUiScaler;

    [SerializeField] UIMover booksUiMover;

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

    /// <summary>
    /// Booksの表示ボタン押下時の移動処理
    /// </summary>
    public void OnTapBooksActive() {

        if (!booksUiMover.IsMoved) {
            booksUiMover.MoveTransition();
            booksUiMover.gameObject.SetActive(true);
        }
        else {
            booksUiMover.ReturnPosition(() => booksUiMover.gameObject.SetActive(false));
        }
    }

}
