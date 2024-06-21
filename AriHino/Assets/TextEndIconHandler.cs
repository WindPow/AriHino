using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEndIconHandler : MonoBehaviour
{
    [SerializeField] private Animation iconAnimation;
    
    void OnEnable()
    {
        iconAnimation.Play();
    }
}
