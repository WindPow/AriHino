using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utage;

public class AdvUIHandler : MonoBehaviour
{
    [SerializeField] UIMover booksUiMover;

    [SerializeField] private ObjectActivator booksActivator;

    [SerializeField] private UIBlurHandler uiBlur;

    [SerializeField] private float maxBlurStrength = 2.5f;

    [SerializeField] private UnityEvent[] booksOnCallback;

    [SerializeField] private UnityEvent[] booksOffCallback;

    [SerializeField] private AudioClip booksSe;
    [SerializeField] private SoundPlayMode seSoundMode = SoundPlayMode.Add;

    private bool isWaitInput;

    /// <summary>
    /// Booksの表示ボタン押下時の移動処理
    /// </summary>
    public void OnTapBooksActive() {

        ActivateBooks(booksUiMover.IsMoved);
    }

    public void ActivateBooks(bool isOpen) {

        if(isWaitInput) return;

        if (!isOpen) {
            booksUiMover.MoveTransition(() => isWaitInput = false);
            booksUiMover.gameObject.SetActive(true);
            booksActivator.ActiveChangeObject(true);
            ActiveUiBlur(maxBlurStrength);
            foreach (var callback in booksOnCallback) callback.Invoke();

            BooksManager.Instance.SetIsOpenBooks(true);
        }
        else {
            booksUiMover.ReturnPosition(() => {
                //booksUiMover.gameObject.SetActive(false);
                isWaitInput = false;
                });
            booksActivator.ActiveChangeObject(false);
            ActiveUiBlur(0f);
            foreach (var callback in booksOffCallback) callback.Invoke();

            BooksManager.Instance.SetIsOpenBooks(false);
        }

        SoundManager.GetInstance().PlaySe(booksSe, booksSe.name, seSoundMode);

        isWaitInput = true;
    }

    private void ActiveUiBlur(float value) {

        uiBlur.ChangeMaterialValue(value);
    }
}
