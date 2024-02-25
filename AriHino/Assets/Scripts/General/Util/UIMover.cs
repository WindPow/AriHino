using UnityEngine;
using System;
using DG.Tweening;
using UniRx;

public class UIMover : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject; // 移動させたい対象オブジェクト

    [SerializeField]
    private Vector3 moveDirection; // 移動方向

    [SerializeField]
    private float moveAmount = 500f; // 移動量

    [SerializeField]
    private float moveDuration = 1f; // 移動の時間

    private bool isMoving = false; // 移動中かどうかのフラグ
    private bool isInitialPosition = true; // 初期座標かどうかのフラグ
    private Vector3 initialPosition; // 初期位置の保存用

    private Subject<bool> moveStateChangedSubject = new Subject<bool>(); // 移動状態の変化を通知するSubject

    public IObservable<bool> OnMoveStateChanged => moveStateChangedSubject; // 外部に公開するIObservable

    private Tweener moveTween;

    private void Start()
    {
        if (!targetObject) targetObject = transform;

        initialPosition = targetObject.localPosition;

        // 初期座標を通知
        moveStateChangedSubject.OnNext(isInitialPosition);
    }

    public void ToggleMove()
    {

        MoveToTargetPosition();

        // 移動方向を反転する
        moveDirection *= -1f;
    }

    private void MoveToTargetPosition()
    {
        if (isMoving)
        {
            return; // 移動中の場合は処理を中断する
        }

        isMoving = true;
        isInitialPosition = false;

        // 移動状態の変化を通知
        moveStateChangedSubject.OnNext(isInitialPosition);

        Vector3 targetPosition = targetObject.localPosition + moveDirection.normalized * moveAmount;

        // 目標位置まで移動する
        moveTween = targetObject.DOLocalMove(targetPosition, moveDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isMoving = false;
                moveTween = null;
                isInitialPosition = !isInitialPosition;

                // 移動状態の変化を通知
                moveStateChangedSubject.OnNext(isInitialPosition);
            });
    }
}
