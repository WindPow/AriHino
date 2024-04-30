using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utage;

public class CustomAdvUguiTipsListButton : AdvUguiTipsListButton
{
    public override void Init(TipsInfo tipsInfo)
    {
        tipsTitle.gameObject.SetActive(tipsInfo.IsOpened);
        base.Init(tipsInfo);
    }
}
