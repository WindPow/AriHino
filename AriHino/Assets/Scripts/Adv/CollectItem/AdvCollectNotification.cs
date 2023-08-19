using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Text;

public class AdvCollectNotification : MonoBehaviour
{
    [SerializeField] private Text getText;
    


    public void Init (int collectItemId){

        // TODO
        //var itemData = null;
        var a = ZString.Format("アーカイブ{0}を入手しました", collectItemId);
        
        
    }
}
