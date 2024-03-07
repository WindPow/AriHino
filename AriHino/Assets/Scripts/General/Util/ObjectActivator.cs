using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public GameObject[] trueObjects; // trueの場合に表示するオブジェクト
    public GameObject[] falseObjects; // falseの場合に表示するオブジェクト

    // bool値に応じてオブジェクトを切り替える
    public void ActiveChangeObject(bool value)
    {
        // trueの場合はtrueObjectsを表示し、falseObjectsを非表示にする
        if (value)
        {
            SetObjectsActive(trueObjects, true);
            SetObjectsActive(falseObjects, false);
        }
        // falseの場合はfalseObjectsを表示し、trueObjectsを非表示にする
        else
        {
            SetObjectsActive(falseObjects, true);
            SetObjectsActive(trueObjects, false);
        }
    }

    // オブジェクトの表示/非表示を設定するヘルパーメソッド
    private void SetObjectsActive(GameObject[] objects, bool value)
    {
        if (objects != null)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    obj.SetActive(value);
                }
            }
        }
    }
}
