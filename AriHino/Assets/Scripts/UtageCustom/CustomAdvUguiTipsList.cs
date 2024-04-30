using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;
using UniRx;
using Cysharp.Threading.Tasks;

public class CustomAdvUguiTipsList : AdvUguiTipsList
{
    public override void Close()
    {
        if (this.gameObject.activeSelf)
			{
				//閉じる処理開始処理を呼ぶ
				this.gameObject.SendMessage("OnBeginClose", SendMessageOptions.DontRequireReceiver);

                // Startコルーチンだとオブジェクトが非アクティブの時にバグる
                CoClosing().ToUniTask().Forget();
			}
    }
}
