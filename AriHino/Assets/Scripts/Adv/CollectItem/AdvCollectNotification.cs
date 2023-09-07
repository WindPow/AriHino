using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Text;

/// <summary>
/// コレクトアイテム通知表示
/// </summary>
interface IAdvCollectNotification {
    void Init(int collectItemId);

    void Display();
}


public class AdvCollectNotification : MonoBehaviour
{
    [SerializeField] private Text getText;

    public void Init (string collectItemName){

        // TODO
        var str = ZString.Format("アーカイブ「{0}」を入手しました", collectItemName);
        
        getText.text = str;
    }

    public void Display(){

        // TODO
    }
}
