using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するために追加
using System.IO;

// CSVReaderクラス
public class CSVReader
{
    public static async UniTask<Dictionary<TKey, TData>> ReadDataAsync<TKey, TData>(string filePath, char delimiter = ',')
        where TData : IMasterData<TKey>, new()
    {
        TextAsset data = await LoadTextAssetAsync(filePath);
        if (data == null)
        {
            Debug.LogError("CSV file not found at path: " + filePath);
            return null;
        }

        Dictionary<TKey, TData> table = new Dictionary<TKey, TData>();

        string[] lines = data.text.Split('\n');
        string[] headers = lines[0].Trim().Split(delimiter);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(delimiter);
            if (values.Length != headers.Length)
            {
                Debug.LogWarning("Skipping line " + (i + 1) + ". Number of values does not match number of headers.");
                continue;
            }

            TData newData = new TData();
            newData.Initialize(headers, values);
            TKey key = newData.ID;
            table[key] = newData;
        }

        return table;
    }

    // TextAssetを非同期で読み込むメソッド
    private static async UniTask<TextAsset> LoadTextAssetAsync(string filePath)
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>("Master/" + filePath);
        await request.ToUniTask();

        TextAsset textAsset = request.asset as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError("Error loading file at path: " + filePath);
            return null;
        }
        return textAsset;
    }
}

// 統合されたインターフェース
public interface IMasterData<TKey>
{
    TKey ID { get; }
    void Initialize(string[] headers, string[] values);
}