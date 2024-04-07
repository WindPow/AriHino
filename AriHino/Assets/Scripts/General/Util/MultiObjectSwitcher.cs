using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameObjectArray
{
    public GameObject[] array;
}

public class MultiObjectSwitcher : MonoBehaviour
{
    public List<GameObjectArray> objectsToSwitch; // 切り替えるオブジェクトのリスト
    private int currentIndex = 0; // 現在のインデックス

    // 初期化
    private void Start()
    {
        // 最初のオブジェクトのみを表示
        SwitchObject(currentIndex);
    }

    // オブジェクトの切り替え
    public void SwitchObject(int index)
    {
        // インデックスが有効な範囲内であるか確認
        if (index >= 0 && index < objectsToSwitch.Count)
        {
            // 現在のオブジェクトを非表示にする
            foreach (GameObject obj in objectsToSwitch[currentIndex].array)
            {
                obj.SetActive(false);
            }

            // 新しいオブジェクトを表示する
            foreach (GameObject obj in objectsToSwitch[index].array)
            {
                obj.SetActive(true);
            }

            // 現在のインデックスを更新
            currentIndex = index;
        }
        else
        {
            Debug.LogError("Invalid index: " + index);
        }
    }

    public void SetObjectsActive(int index, bool isActive)
{
    // インデックスが有効な範囲内であるか確認
    if (index >= 0 && index < objectsToSwitch.Count)
    {
        // 指定されたインデックスのオブジェクトをアクティブ/非アクティブにする
        foreach (GameObject obj in objectsToSwitch[index].array)
        {
            obj.SetActive(isActive);
        }
    }
    else
    {
        Debug.LogError("Invalid index: " + index);
    }
}
}
