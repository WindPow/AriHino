using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;

public class UIBlurHandler : MonoBehaviour
{
    [SerializeField] private Material material;

    [SerializeField] private float duration;

    public void ChangeMaterialValue(float targetValue)
    {
        // DOTweenを使用してmaterialの値を変更するTweenを作成
        material.DOFloat(targetValue, "_SamplingDistance", duration);
    }

    protected void OnDestroy()
    {
        material.SetFloat("_SamplingDistance", 0);
    }

}
