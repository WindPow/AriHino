using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するために追加

// マスターデータマネージャクラス
public class MasterDataManager : MonoBehaviour
{
    private static MasterDataManager _instance;
    public static MasterDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MasterDataManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(MasterDataManager).Name);
                    _instance = singletonObject.AddComponent<MasterDataManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    private Dictionary<Type, object> dataDictionary = new Dictionary<Type, object>();

    private void Awake()
    {
        LoadData<MstAdvCollectItemData>("m_collect_item.csv").Forget();

        LoadData<MstCharacterData>("m_character.csv").Forget();
        LoadData<MstBooksData>("m_books.csv").Forget();
        LoadData<MstBooksCharacterPageData>("m_books_character_page.csv").Forget();
        LoadData<MstBooksCharacterExplanationData>("m_books_character_explanation.csv").Forget();
        LoadData<MstBooksCharacterMemoData>("m_books_character_memo.csv").Forget();
        LoadData<MstBooksCharacterImpressionsData>("m_books_character_impressions.csv").Forget();

        LoadData<MstWorldData>("m_world.csv").Forget();
        LoadData<MstBooksWorldPageData>("m_books_world_page.csv").Forget();
        LoadData<MstBooksWorldExplanationData>("m_books_world_explanation.csv").Forget();
        // 他のマスターデータもここで読み込む
    }

    public async UniTask LoadData<T>(string filePath) where T : IMasterData<int>, new()
    {
        Dictionary<int, T> data = await CSVReader.ReadDataAsync<int, T>(filePath);
        dataDictionary[typeof(T)] = data;
    }

    public T GetMasterData<T>(int id) where T : IMasterData<int>
    {
        Type dataType = typeof(T);
        if (dataDictionary.ContainsKey(dataType))
        {
            var data = dataDictionary[dataType] as Dictionary<int, T>;
            if (data.ContainsKey(id))
            {
                return data[id];
            }
            else
            {
                Debug.LogError($"Master data of type {dataType.Name} with ID {id} not found.");
                return default;
            }
        }
        else
        {
            Debug.LogError($"Master data of type {dataType.Name} not loaded.");
            return default;
        }
    }

    public Dictionary<int, T> GetMasterDataDictionary<T>() where T : IMasterData<int>
    {
        Type dataType = typeof(T);
        if (dataDictionary.ContainsKey(dataType))
        {
            return dataDictionary[dataType] as Dictionary<int, T>;
        }
        else
        {
            Debug.LogError($"Master data of type {dataType.Name} not loaded.");
            return null;
        }
    }
}