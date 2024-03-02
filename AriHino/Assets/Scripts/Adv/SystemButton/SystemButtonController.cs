using UnityEngine;
using UniRx;

public class SystemButtonController : MonoBehaviour
{
    [SerializeField]
    private UIMover uiMover;

    [SerializeField]
    //private SystemButtonCollisionHandler buttonCollisionHandler;

    private CompositeDisposable disposables;

    private void Start()
    {
        disposables = new CompositeDisposable();

        // UIMoverのIsMovingが変化した時の処理を購読
        // uiMover.OnMoveStateChanged
        //     .Subscribe(moved =>
        //     {
        //         // SystemButtonCollisionHandlerのenabledを制御
        //         //buttonCollisionHandler.SetProcessingEnabled(moved);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }
}