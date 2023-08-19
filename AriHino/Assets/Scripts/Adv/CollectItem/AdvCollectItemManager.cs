using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvCollectItemManager : MonoBehaviour
{
    [SerializeField] private AdvCollectItemPresenter collectItemPresenter;

    private AdvCollectItemModel collectItemModel;

    private static AdvCollectItemManager instance;
    public static AdvCollectItemManager Instance { 

        get {
            if(instance == null) instance = GameObject.FindObjectOfType<AdvCollectItemManager>();

            if(instance == null) {
                GameObject singletonObject = new GameObject(typeof(AdvCollectItemManager).Name);
                instance = singletonObject.AddComponent<AdvCollectItemManager>();
            }
            return instance;
        }
    }

    void Start(){

        Init();
    }

    public void Init(){

        collectItemModel = new AdvCollectItemModel();
        collectItemPresenter.Init(collectItemModel);
    }

    public void SetCollectItem(int collectItemId){

        collectItemModel.SetCollectItem(collectItemId);
    
    }

}
