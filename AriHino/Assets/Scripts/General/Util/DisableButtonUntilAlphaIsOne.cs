using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DisableButtonUntilAlphaIsOne : MonoBehaviour
{
    public CanvasGroup waitForCanvasGroup; // 待機するCanvasGroup

    private Button button; // ボタンコンポーネント

    void Start()
    {
        button = GetComponent<Button>(); // ボタンコンポーネントを取得
        button.interactable = false; // ボタンを無効化
    }

    void Update()
    {
        if (waitForCanvasGroup.alpha >= 1f) // CanvasGroupのalpha値が1になったら
        {
            button.interactable = true; // ボタンを有効化
        }
    }
}