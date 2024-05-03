using UnityEngine;

public static class TransformExtensions
{
    // 子オブジェクトをすべて削除するメソッドの定義
    public static void DestroyChildren(this Transform parent)
    {
        parent.DestroyChildren();
    }
}
