using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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

    private Queue<string> notificationQueue = new Queue<string>();
    private Coroutine displayCoroutine;

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

            yield return new WaitForSeconds(displayDuration);

            notificationPanel.SetActive(false);
        }

        displayCoroutine = null;
    }
}
