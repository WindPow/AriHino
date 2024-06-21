using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BooksTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animation dispAnim;

    public void SetData(string str) {
        text.text = str;
    }

    public void PlayDisplayAnim() {
        dispAnim.Play();
    }
}
