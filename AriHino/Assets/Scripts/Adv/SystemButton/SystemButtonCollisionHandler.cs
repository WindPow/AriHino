using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class SystemButtonCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup targetCanvasGroup; // 透明度を変化させたい別のオブジェクトのCanvasGroup

    private CompositeDisposable disposables;
    private IDisposable fadeInDisposable;
    private IDisposable fadeOutDisposable;
    private bool isFadingIn = false; // フェードイン中かどうかのフラグ
    private bool isFadingOut = false; // フェードアウト中かどうかのフラグ
    private bool isProcessingEnabled = true; // 処理を有効にするかどうかのフラグ

    [SerializeField]
    private float fadeInDuration = 2f; // フェードインの時間
    [SerializeField]
    private float fadeOutDelay = 2f; // マウスが離れてからフェードアウトまでの遅延時間
    [SerializeField]
    private float fadeOutDuration = 2f; // フェードアウトの時間

    private void Start()
    {
        SetAlpha(0f); // 初期の透明度を0に設定

        disposables = new CompositeDisposable();

        // マウスカーソルがColliderに乗った時の処理
        this.OnMouseEnterAsObservable()
            .Subscribe(_ =>
            {
                if (!isFadingIn && !isFadingOut && isProcessingEnabled) // フェード中でない場合かつ処理が有効な場合のみ処理を実行
                {
                    StartFadeIn();
                }
                else if (isFadingOut && isProcessingEnabled) // フェードアウト中の場合かつ処理が有効な場合はキャンセルしてフェードインを開始
                {
                    CancelFadeOut();
                    StartFadeIn();
                }
            })
            .AddTo(disposables);

        // マウスカーソルがColliderから離れた時の処理
        this.OnMouseExitAsObservable()
            .Subscribe(_ =>
            {
                if (!isFadingOut && !isFadingIn && isProcessingEnabled) // フェード中でない場合かつ処理が有効な場合のみ処理を実行
                {
                    fadeOutDisposable = Observable.Timer(TimeSpan.FromSeconds(fadeOutDelay))
                        .Subscribe(__ =>
                        {
                            StartFadeOut();
                        })
                        .AddTo(disposables);
                }
                else if (isFadingIn && isProcessingEnabled) // フェードイン中の場合かつ処理が有効な場合はキャンセルしてフェードアウトを開始
                {
                    CancelFadeIn();
                    StartFadeOut();
                }
            })
            .AddTo(disposables);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

    private void StartFadeIn()
    {
        isFadingIn = true;
        fadeInDisposable = Observable.Timer(TimeSpan.FromSeconds(fadeInDuration))
            .Subscribe(_ =>
            {
                isFadingIn = false;
                fadeInDisposable = null;
            })
            .AddTo(disposables);

        LeanTween.alphaCanvas(targetCanvasGroup, 1f, fadeInDuration)
            .setOnComplete(() =>
            {
                isFadingIn = false;
                fadeInDisposable = null;
            });
    }

    private void StartFadeOut()
    {
        isFadingOut = true;
        fadeOutDisposable = Observable.Timer(TimeSpan.FromSeconds(fadeOutDuration))
            .Subscribe(_ =>
            {
                isFadingOut = false;
                fadeOutDisposable = null;
            })
            .AddTo(disposables);

        LeanTween.alphaCanvas(targetCanvasGroup, 0f, fadeOutDuration)
            .setOnComplete(() =>
            {
                isFadingOut = false;
                fadeOutDisposable = null;
            });
    }

    private void CancelFadeIn()
    {
        if (fadeInDisposable != null)
        {
            fadeInDisposable.Dispose();
            fadeInDisposable = null;
            isFadingIn = false;
        }
    }

    private void CancelFadeOut()
    {
        if (fadeOutDisposable != null)
        {
            fadeOutDisposable.Dispose();
            fadeOutDisposable = null;
            isFadingOut = false;
        }
    }

    private void SetAlpha(float alpha)
    {
        targetCanvasGroup.alpha = alpha;
    }

    // 処理の有効/無効を設定する
    public void SetProcessingEnabled(bool enabled)
    {
        isProcessingEnabled = enabled;
    }
}
