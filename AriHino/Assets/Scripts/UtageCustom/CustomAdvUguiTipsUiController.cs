using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class CustomAdvUguiTipsUiController : AdvUguiTipsUiController
{
    public override void OnClickTipsInMainGame(TipsInfo tipsInfo)
    {
        TipsDetail.Open(tipsInfo, MainGame);
    }

    public override void OnClickTipsListInMainGame()
    {
        TipsList.Open(MainGame);
    }
}
