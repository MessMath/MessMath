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

        if(characterName != "")
        {
            characterImage.sprite = Resources.Load<Sprite>(picture);
            this.GetComponent<Area>().nameText.text = characterName;
        }
       
        TextRect.GetComponent<TextMeshProUGUI>().text = dialogue;
        BoxRect.sizeDelta = new Vector2(600, BoxRect.sizeDelta.y);

        /*float X = TextRect.sizeDelta.x + 42;
        float Y = TextRect.sizeDelta.y;
        if (Y > 49)
        {
            for (int i = 0; i < 200; i++)
            {
                BoxRect.sizeDelta = new Vector2(X - i * 2, BoxRect.sizeDelta.y);
                Fit(BoxRect);

                if (Y != TextRect.sizeDelta.y) { BoxRect.sizeDelta = new Vector2(X - (i * 2) + 2, Y); break; }
            }
        }
        else BoxRect.sizeDelta = new Vector2(X, Y);*/
        Fit(BoxRect);
        Fit(AreaRect);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
}
