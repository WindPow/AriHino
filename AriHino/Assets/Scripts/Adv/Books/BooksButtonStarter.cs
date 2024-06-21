using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooksButtonStarter : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    void OnEnable()
    {
        rectTransform.rotation = Quaternion.identity;
    }
}
