using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIScaler : MonoBehaviour
{
    [SerializeField] private Vector2 originScale;

    [SerializeField] private Vector2 toScale;

    [SerializeField] private float duration;


    public bool isScaleChanged { get; private set; }

    public void ScaleTransiton(){

        transform.DOScale(toScale, duration);
        isScaleChanged = true;
    }

    public void ReturnScale(){

        transform.DOScale(originScale, duration);
        isScaleChanged = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = originScale;
    }
}
