using System.Collections;
using UnityEngine;

public class MoveToInitialPosition : MonoBehaviour
{
    public Vector3 initialOffset; // オブジェクトの初期位置に加算する座標
    public float moveTime = 1f; // 移動にかかる時間
    public CanvasGroup waitForCanvasGroup; // 待機するCanvasGroup

    private Vector3 initialPosition; // オブジェクトの初期位置
    private Vector3 originPosition; // 元の位置

    private void Start()
    {
        originPosition = transform.localPosition;
        initialPosition = transform.localPosition + initialOffset; // オブジェクトの初期位置を設定
        transform.localPosition = initialPosition; // オブジェクトを初期位置に移動

        StartCoroutine(MoveStart());
    }

    private IEnumerator MoveStart()
    {
        if(waitForCanvasGroup) yield return new WaitUntil(() => waitForCanvasGroup.alpha == 1f); // CanvasGroupのalpha値が1になるまで待機する

        float elapsedTime = 0f; // 経過時間

        while (elapsedTime < moveTime)
        {
            transform.localPosition = Vector3.Lerp(initialPosition, originPosition, elapsedTime / moveTime); // 移動
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null; // 1フレーム待機
        }

        transform.localPosition = originPosition; // オブジェクトを初期位置に移動
    }
}