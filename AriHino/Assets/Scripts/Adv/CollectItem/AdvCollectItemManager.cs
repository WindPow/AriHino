using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// コレクトアイテムを管轄するマネージャー
/// </summary>
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

        // SaveDataから所持しているコレクトアイテムリストを取得
        collectItemModel = new AdvCollectItemModel(SaveDataPackManager.GetAdvCollectDatas());
        collectItemPresenter.Init(collectItemModel);

        Bind();
    }

    private void Bind(){

        collectItemModel.HasCollectItemAddObservable.Subscribe(_ => {
            //TODO セーブデータに登録
        }).AddTo(this);
    }

    public void SetCollectItem(int collectItemId) {

        var mstAdvCollectItem = MasterDataManager.Instance.GetMasterData<MstAdvCollectItemData>(collectItemId);
        collectItemModel.SetCollectItem(mstAdvCollectItem);
    
    }

    public void RemoveCollectItem(int collectItemId) {
        collectItemModel.RemoveCollectItem(collectItemId);
    }

    public void RemoveAllCollectItem() {
        collectItemModel.RemoveAllCollectItem();
    }

}
