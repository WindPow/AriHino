using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BooksWardPageStartHandler : MonoBehaviour
{
    [SerializeField] UnityEvent startEvent;
    
    void OnEnable()
    {
        startEvent?.Invoke();
    }
}
