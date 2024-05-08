using UnityEngine;

public static class TransformExtensions
{
    // 子オブジェクトをすべて削除するメソッドの定義
    public static void DestroyChildren(this Transform parent)
    {
        // 子オブジェクトを取得し、ループして削除する
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }
}
