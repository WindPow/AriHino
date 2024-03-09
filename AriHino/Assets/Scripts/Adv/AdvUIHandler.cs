using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvUIHandler : MonoBehaviour
{
    [SerializeField] UIMover booksUiMover;

    [SerializeField] private ObjectActivator booksActivator;

    [SerializeField] private UIBlurHandler uiBlur;

    /// <summary>
    /// Booksの表示ボタン押下時の移動処理
    /// </summary>
    public void OnTapBooksActive() {

        if (!booksUiMover.IsMoved) {
            booksUiMover.MoveTransition();
            booksUiMover.gameObject.SetActive(true);
            booksActivator.ActiveChangeObject(true);
            ActiveUiBlur(5f);
        }
        else {
            booksUiMover.ReturnPosition(() => booksUiMover.gameObject.SetActive(false));
            booksActivator.ActiveChangeObject(false);
            ActiveUiBlur(0f);
        }
    }

    private void ActiveUiBlur(float value) {

        uiBlur.ChangeMaterialValue(value);
    }
}
