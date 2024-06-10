using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BooksCharacterPageView : MonoBehaviour
{
    [SerializeField] private RawImage characterImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private BooksTextView booksTextViewPrefab;
    [SerializeField] private Transform explanationParent;
    [SerializeField] private Transform memoParent;
    [SerializeField] private Transform impressionsParent;

    public BooksCharacterPageViewData PageViewData { get; private set; }

    public void Init(BooksCharacterPageViewData data) {

        if(explanationParent.childCount > 0) explanationParent.DestroyChildren();
        if(memoParent.childCount > 0) memoParent.DestroyChildren();
        if(impressionsParent.childCount > 0) impressionsParent.DestroyChildren();

        PageViewData = data;
        nameText.text = data.CharaName;

        LoadCharacterImage();
        RegistExplanationText();
        RegistMemoText();
        RegistImpressionsText();
    }

    private void LoadCharacterImage() {

        string imagePath = "Texture/Books/Character/" + PageViewData.CharaImageFilePath;
        Texture2D texture = Resources.Load<Texture2D>(imagePath);
        characterImage.texture = texture;
    }

    private void RegistExplanationText() {
        
        foreach (var str in PageViewData.ExplanationStrs) {
            var view = Instantiate(booksTextViewPrefab, explanationParent);
            view.SetData(str);
        }
    }

    private void RegistMemoText() {
        
        foreach (var str in PageViewData.MemoStrs) {
            var view = Instantiate(booksTextViewPrefab, memoParent);
            view.SetData(str);
        }
    }

    private void RegistImpressionsText() {
        
        foreach (var str in PageViewData.ImpressionsStrs) {
            var view = Instantiate(booksTextViewPrefab, impressionsParent);
            view.SetData(str);
        }
    }
}
