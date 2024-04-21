using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utage;

public class AdvController : UtageUguiMainGame
{
    protected override void Awake()
    {
        BooksManager.Instance.Init();
        base.Awake();
    }
}
