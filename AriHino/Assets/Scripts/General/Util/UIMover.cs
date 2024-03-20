using UnityEngine;
using DG.Tweening;

public class UIMover : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 originPosition;
    [SerializeField] private Vector2 toPosition;
    [SerializeField] private float duration;

    [SerializeField] private bool isStartOn;

    public bool IsMoved { get; private set; }

    void Start()
    {
        rectTransform.anchoredPosition = originPosition;

        if(isStartOn) MoveTransition();
    }

    public void MoveTransition(System.Action onMoveComplete = null)
    {
        rectTransform.DOAnchorPos(toPosition, duration).OnComplete(() =>
        {
            IsMoved = true;
            onMoveComplete?.Invoke(); // コールバックを実行
        });
    }

    public void ReturnPosition(System.Action onReturnComplete = null)
    {
        rectTransform.DOAnchorPos(originPosition, duration).OnComplete(() =>
        {
            IsMoved = false;
            onReturnComplete?.Invoke(); // コールバックを実行
        });
    }
}