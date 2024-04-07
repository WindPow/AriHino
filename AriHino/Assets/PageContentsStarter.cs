using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PageContentsStarter : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void OnEnable()
    {
        canvasGroup.alpha = 0;
        FadeCanvasGroup(1, 0.3f).Forget();
    }

    public async UniTask FadeCanvasGroup(float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float normalizedTime = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, normalizedTime);
            await UniTask.Yield();
        }

        canvasGroup.alpha = targetAlpha; // Ensure target alpha is reached exactly
    }
}
