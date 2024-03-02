using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIScaler : MonoBehaviour
{
    [SerializeField] private Vector2 originScale;

    [SerializeField] private Vector2 toScale;

    [SerializeField] private float duration;


    public bool IsScaleChanged { get; private set; }

    public void ScaleTransiton(System.Action onScaleComplete = null){

        transform.DOScale(toScale, duration).OnComplete(() => {
            IsScaleChanged = true;
            onScaleComplete?.Invoke();
        });
    }

    public void ReturnScale(System.Action onReturnComplete = null){

        transform.DOScale(originScale, duration).OnComplete(() => {
            IsScaleChanged = false;
            onReturnComplete?.Invoke();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = originScale;
    }
}
