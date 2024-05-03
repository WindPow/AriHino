using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class CustomUtageUguiTitle : UtageUguiTitle
{
    [SerializeField] private float fadeOutDuration;

    public override void OnTapStart()
    {
        ScreenEffectManager.Instance.FadeOutSequence(fadeOutDuration, Color.white, () => {
            
            base.OnTapStart(); 
        });
    }
}
