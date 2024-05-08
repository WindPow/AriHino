using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvCollectItemDetailDialog : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RawImage itemImage;

    private MstCollectItemData mstCollectItemData1;

    public void Init(MstCollectItemData data) {

        mstCollectItemData1 = data;

        titleText.text = data.Name;
        descriptionText.text = data.Description;

        string imagePath = "Texture/CollectItem/" + data.ImagePath;
        Texture2D texture = Resources.Load<Texture2D>(imagePath);
        itemImage.texture = texture;
    }

    public void OnCloseButton() {

        Destroy(this.gameObject);
    }
}
