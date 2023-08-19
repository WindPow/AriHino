using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserDataAdvCollectItem
{
    public int CollectItemId { get;}

    public bool IsReaded { get;}

    public UserDataAdvCollectItem(int collectItemId, bool isReaded){
        CollectItemId = collectItemId;
        IsReaded = isReaded;
    }
}
