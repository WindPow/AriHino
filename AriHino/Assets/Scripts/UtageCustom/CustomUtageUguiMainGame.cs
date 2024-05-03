using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utage;

public class CustomUtageUguiMainGame : UtageUguiMainGame
{
    [SerializeField] private float fadeInDuration = 1;

    protected override void Awake()
    {
        ScreenEffectManager.Instance.FadeInSequence(fadeInDuration);
        BooksManager.Instance.Init();
        base.Awake();
    }
}
