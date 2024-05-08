using UnityEngine;
using System.Collections;

public class ActiveSynchronizer : MonoBehaviour
{
    [SerializeField] private GameObject targetObjecct;
    [SerializeField] private GameObject[] visibleObjects; // 表示用のオブジェクト配列
    [SerializeField] private GameObject[] hiddenObjects;  // 非表示用のオブジェクト配列

    void Start()
    {
        // 開始時に監視を開始
        StartCoroutine(SynchronizeObjects());
    }

    IEnumerator SynchronizeObjects()
    {
        while (true)
        {
            // 監視対象のオブジェクトのアクティブ状態を取得
            bool active = targetObjecct.activeSelf;

            // 表示用のオブジェクトを同期
            foreach (GameObject obj in visibleObjects)
            {
                obj.SetActive(active);
            }

            // 非表示用のオブジェクトを同期
            foreach (GameObject obj in hiddenObjects)
            {
                obj.SetActive(!active);
            }

            // 0.1秒待機して次のフレームへ
            yield return new WaitForSeconds(0.1f);
        }
    }
}
