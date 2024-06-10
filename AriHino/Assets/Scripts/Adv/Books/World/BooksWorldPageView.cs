using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BooksWorldPageView : MonoBehaviour
{
    [SerializeField] private RawImage WorldImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private BooksTextView booksTextViewPrefab;
    [SerializeField] private Transform explanationParent;

    public BooksWorldPageViewData PageViewData { get; private set; }

    public void Init(BooksWorldPageViewData data) {

        if(explanationParent.childCount > 0) explanationParent.DestroyChildren();
        PageViewData = data;
        nameText.text = data.WorldName;

        LoadWorldImage();
        RegistExplanationText();
    }

    private void LoadWorldImage() {

        string imagePath = "Texture/Books/World/" + PageViewData.WorldImageFilePath;
        Texture2D texture = Resources.Load<Texture2D>(imagePath);
        WorldImage.texture = texture;
    }

    private void RegistExplanationText() {
        
        foreach (var str in PageViewData.ExplanationStrs) {
            var view = Instantiate(booksTextViewPrefab, explanationParent);
            view.SetData(str);
        }
    }
}
