using System.Collections;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFadeIn : MonoBehaviour
{
    public float fadeInTime = 1f; // フェードインにかかる時間
    public Vector3 initialOffset; // オブジェクトの初期位置に加算する座標
    public CanvasGroup[] waitForCanvasGroups; // 待機するCanvasGroupの配列

    private CanvasGroup canvasGroup; // フェードインさせるCanvasGroup
    private Vector3 initialPosition; // オブジェクトの初期位置
    private Vector3 originPosition; // 元の位置
    
    void Start()
    {
        originPosition = transform.localPosition;
        initialPosition = transform.localPosition + initialOffset; // オブジェクトの初期位置を設定
        transform.localPosition = initialPosition; // オブジェクトを初期位置に移動

        canvasGroup = GetComponent<CanvasGroup>(); // CanvasGroupコンポーネントを取得
        StartCoroutine(FadeInCanvasGroup()); // フェードインコルーチンを開始
    }

    IEnumerator FadeInCanvasGroup()
    {
        // 他のオブジェクトのCanvasGroupのalpha値が1になるまで待機
        foreach (CanvasGroup waitForCanvasGroup in waitForCanvasGroups)
        {
            while (waitForCanvasGroup.alpha < 1f) // アルファ値が1になるまで繰り返す
            {
                yield return null; // 1フレーム待機
            }
        }

        canvasGroup.alpha = 0f; // アルファ値を0に設定
        canvasGroup.interactable = false; // インタラクティブを無効化

        float elapsedTime = 0f; // 経過時間

        while (canvasGroup.alpha < 1f) // アルファ値が1になるまで繰り返す
        {
            canvasGroup.alpha += Time.deltaTime / fadeInTime; // 時間で徐々にアルファ値を増加

            transform.localPosition = Vector3.Lerp(initialPosition, originPosition, elapsedTime / fadeInTime); // 移動
            elapsedTime += Time.deltaTime; // 経過時間を加算

            yield return null; // 1フレーム待機
        }

        canvasGroup.interactable = true; // インタラクティブを有効化
        transform.localPosition = originPosition; // オブジェクトを初期位置に移動
    }
}