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
    

    private BooksCharacterPageViewData pageViewData;

    public void Init(BooksCharacterPageViewData data) {
        pageViewData = data;
        nameText.text = data.CharaName;

        LoadCharacterImage();
        RegistExplanationText();
        RegistMemoText();
        RegistImpressionsText();
    }

    private void LoadCharacterImage() {

        string imagePath = "Texture/Books/Character/" + pageViewData.CharaImageFilePath;
        Texture2D texture = Resources.Load<Texture2D>(imagePath);
        characterImage.texture = texture;
    }

    private void RegistExplanationText() {
        
        foreach (var str in pageViewData.ExplanationStrs) {
            var view = Instantiate(booksTextViewPrefab, explanationParent);
            view.SetData(str);
        }
    }

    private void RegistMemoText() {
        
        foreach (var str in pageViewData.MemoStrs) {
            var view = Instantiate(booksTextViewPrefab, memoParent);
            view.SetData(str);
        }
    }

    private void RegistImpressionsText() {
        
        foreach (var str in pageViewData.ImpressionsStrs) {
            var view = Instantiate(booksTextViewPrefab, impressionsParent);
            view.SetData(str);
        }
    }
}
