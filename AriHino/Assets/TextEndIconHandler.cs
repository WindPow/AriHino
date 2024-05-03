using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEndIconHandler : MonoBehaviour
{
    [SerializeField] private Animation animation;
    
    void OnEnable()
    {
        animation.Play();
    }
}
