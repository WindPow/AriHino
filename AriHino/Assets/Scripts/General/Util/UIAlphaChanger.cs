using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAlphaChanger : MonoBehaviour
{
    [SerializeField] private float fromAlpha = 1f;
    [SerializeField] private float toAlpha = 0f;
    [SerializeField] private float duration = 1f;

    private CanvasGroup canvasGroup;

    private bool isAlphaChanged;

    public bool IsAlphaChanged => isAlphaChanged;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ChangeAlpha(System.Action onComplete = null)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = fromAlpha;
            canvasGroup.DOFade(toAlpha, duration).OnComplete(() =>
            {
                isAlphaChanged = true;
                onComplete?.Invoke();
            });
        }
    }

    public void ReturnAlpha(System.Action onComplete = null)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = toAlpha;
            canvasGroup.DOFade(fromAlpha, duration).OnComplete(() =>
            {
                isAlphaChanged = false;
                onComplete?.Invoke();
            });
        }
    }
}
