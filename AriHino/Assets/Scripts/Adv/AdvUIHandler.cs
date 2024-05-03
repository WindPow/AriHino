using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvUIHandler : MonoBehaviour
{
    [SerializeField] UIMover booksUiMover;

    [SerializeField] private ObjectActivator booksActivator;

    [SerializeField] private UIBlurHandler uiBlur;

    [SerializeField] private float maxBlurStrength = 2.5f;

    private bool isWaitInput;

    /// <summary>
    /// Booksの表示ボタン押下時の移動処理
    /// </summary>
    public void OnTapBooksActive() {

        if(isWaitInput) return;

        if (!booksUiMover.IsMoved) {
            booksUiMover.MoveTransition(() => isWaitInput = false);
            booksUiMover.gameObject.SetActive(true);
            booksActivator.ActiveChangeObject(true);
            ActiveUiBlur(maxBlurStrength);
        }
        else {
            booksUiMover.ReturnPosition(() => {
                booksUiMover.gameObject.SetActive(false);
                isWaitInput = false;
                });
            booksActivator.ActiveChangeObject(false);
            ActiveUiBlur(0f);
        }

        isWaitInput = true;
    }

    private void ActiveUiBlur(float value) {

        uiBlur.ChangeMaterialValue(value);
    }
}
