using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BooksTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetData(string str) {
        text.text = str;
    }
}
