using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIScaler : MonoBehaviour
{
    [SerializeField] private Vector2 toScale;

    [SerializeField] private float duration;

    private Vector2 originScale;

    public void ScaleTransiton(){

        transform.DOScale(toScale, duration);
    }

    public void ReturnScale(){

        transform.DOScale(originScale, duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        originScale = transform.localScale;
    }
}
