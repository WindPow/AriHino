using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class FadeContentParameter{
    public float fadeInTime = 1f; // フェードインにかかる時間
    public Vector3 initialOffset; // オブジェクトの初期位置に加算する座標
    public CanvasGroup canvasGroup; // フェードインさせるCanvasGroup

    [NonSerialized] public Vector3 initialPosition; // オブジェクトの初期位置
    [NonSerialized] public Vector3 originPosition; // 元の位置
}

public class CanvasGroupFadeInSequence : MonoBehaviour
{
    public FadeContentParameter[] fadeContentParameters; // フェード要素の配列
    public CanvasGroup[] waitForCanvasGroups; // 待機するCanvasGroupの配列

    void Awake()
    {
        // 要素の初期化（座標）
        foreach (var item in fadeContentParameters)
        {
            item.originPosition = item.canvasGroup.transform.localPosition;
            item.initialPosition = item.originPosition + item.initialOffset;
            item.canvasGroup.transform.localPosition = item.initialPosition;
            item.canvasGroup.alpha = 0;
        }
    }

    void Start()
    {
        StartCoroutine(FadeInCanvasGroups()); // フェードインコルーチンを開始
    }

    IEnumerator FadeInCanvasGroups()
    {
        for (int i = 0; i < fadeContentParameters.Length; i++) // 各CanvasGroupを順番にフェードインさせる
        {
            // 他のオブジェクトのCanvasGroupのalpha値が1になるまで待機
            foreach (CanvasGroup waitForCanvasGroup in waitForCanvasGroups)
            {
                while (waitForCanvasGroup.alpha < 1f) // アルファ値が1になるまで繰り返す
                {
                    yield return null; // 1フレーム待機
                }
            }

            CanvasGroup canvasGroup = fadeContentParameters[i].canvasGroup; // 現在のCanvasGroupを取得
            float fadeInTime = fadeContentParameters[i].fadeInTime; // 現在のCanvasGroupのフェードイン時間を取得
            float elapsedTime = 0f; // 経過時間
            
            canvasGroup.alpha = 0f; // アルファ値を0に設定
            canvasGroup.interactable = false; // インタラクティブを無効化

            while (canvasGroup.alpha < 1f) // アルファ値が1になるまで繰り返す
            {
                canvasGroup.alpha += Time.deltaTime / fadeInTime; // 時間で徐々にアルファ値を増加

                fadeContentParameters[i].canvasGroup.transform.localPosition = Vector3.Lerp(fadeContentParameters[i].initialPosition, fadeContentParameters[i].originPosition, elapsedTime / fadeInTime); // 移動
                elapsedTime += Time.deltaTime; // 経過時間を加算

                yield return null; // 1フレーム待機
            }

            canvasGroup.interactable = true; // インタラクティブを有効化
            fadeContentParameters[i].canvasGroup.transform.localPosition = fadeContentParameters[i].originPosition; // オブジェクトを初期位置に移動
        }
    }
}
