using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private TitlePresenter titlePresenter;

    // Start is called before the first frame update
    void Start()
    {
        // プレゼンターを生成＋初期化
        Instantiate<TitlePresenter>(titlePresenter);
        
        TitleModel titleModel = new TitleModel();

        titlePresenter.Initialize(titleModel);

    }
}
