using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class NotificationManager : MonoBehaviour
{

    private static NotificationManager instance;
    public static NotificationManager Instance {

        get {
            if (instance == null) instance = GameObject.FindObjectOfType<NotificationManager>();

            if (instance == null) {
                GameObject singletonObject = new GameObject(typeof(NotificationManager).Name);
                instance = singletonObject.AddComponent<NotificationManager>();
            }
            return instance;
        }
    }

    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public float displayDuration = 3f;
    public float moveDistance = 50f;
    public float moveDuration = 0.5f;

    private Queue<string> notificationQueue = new Queue<string>();
    private Coroutine displayCoroutine;

    private float originPosY;

    void Awake()
    {
        originPosY = notificationPanel.transform.localPosition.y;
    }

    public void ShowNotification(string message)
    {
        notificationQueue.Enqueue(message);
        if (displayCoroutine == null)
        {
            displayCoroutine = StartCoroutine(DisplayNotification());
        }
    }

    private IEnumerator DisplayNotification()
    {
        while (notificationQueue.Count > 0)
        {
            string message = notificationQueue.Dequeue();
            notificationText.text = message;
            notificationPanel.SetActive(true);

            // 通知パネルを下へ移動させる
            notificationPanel.transform.DOLocalMoveY(moveDistance, moveDuration).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(moveDuration); // 移動アニメーションが完了するのを待つ

            yield return new WaitForSeconds(displayDuration);

            // 通知パネルを元の位置に戻す
            notificationPanel.transform.DOLocalMoveY(originPosY, moveDuration).SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(0.5f); // 移動アニメーションが完了するのを待つ

            notificationPanel.SetActive(false);
        }

        displayCoroutine = null;
    }
}
