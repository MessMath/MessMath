using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Area : UI_Base
{
    RectTransform AreaRect, BoxRect, TextRect;
    Image characterImage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetArea(string characterName, string dialogue, string picture)
    {
        AreaRect = this.GetComponent<Area>().AreaRect;
        BoxRect = this.GetComponent<Area>().BoxRect;
        TextRect = this.GetComponent<Area>().TextRect;
        characterImage = this.GetComponent<Area>().characterImg;

        /*if(characterName == "주인공")
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>(picture);
            switch(expression)
            {
                case "SOMBER\r":
                    characterImage.sprite = sprites[4];
                    break;
                case "CURIOSITY\r":
                    characterImage.sprite = sprites[6];
                    break;
                case "DISGUSTED\r":
                    characterImage.sprite = sprites[5];
                    break;
                case "SUPRISED\r":
                    characterImage.sprite = sprites[5];
                    break;
                case "":
                    break;
            }
            this.GetComponent<Area>().nameText.text = characterName;
        }
        else if(characterName != "")
        {
            characterImage.sprite = Resources.Load(picture, typeof(Sprite)) as Sprite;
            this.GetComponent<Area>().nameText.text = characterName;
        }*/
        if (characterName != "")
        {
            characterImage.sprite = Resources.Load(picture, typeof(Sprite)) as Sprite;
            this.GetComponent<Area>().nameText.text = characterName;
        }

        TextRect.GetComponent<TextMeshProUGUI>().text = dialogue;
        BoxRect.sizeDelta = new Vector2(600, BoxRect.sizeDelta.y);

        Fit(BoxRect);
        Fit(AreaRect);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    public TextMeshProUGUI GetText()
    {
        return TextRect.GetComponent<TextMeshProUGUI>();
    }
}
