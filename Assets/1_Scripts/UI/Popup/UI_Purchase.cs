using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StoreDatas;
using MessMathI18n;

public class UI_Purchase : UI_Popup
{
    enum GameObjects
    {
        StoryMode,
        OneToOneMode,
    }
    enum Buttons
    {
        CloseButton,
        PurchaseButton,
    }
    enum Texts
    {
        NameTMP,
        ExplanationTMP,
        StoryModeTMP,
        OneToOneTMP,
    }
    enum Images
    {
        ItemImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ClosePopupUI(); });
        return true;
    }

    void OnClickedPurchaseBtn(string name, int price)
    {
        // Sound
        Managers.Sound.Play("PurchaseEff3");

        UI_PurchaseStatus purchaseStatus = Managers.UI.ShowPopupUI<UI_PurchaseStatus>();
        if(Managers.Coin.CheckPurchase(price)) 
        {
            if(purchaseStatus.Init()) purchaseStatus.SetPurchaseStatus(true, GetText((int)Texts.NameTMP).text);
            PlayerPrefs.SetString(name, name);
            //Debug.Log(PlayerPrefs.GetString(name));
            
        }
        else
        {
            if(purchaseStatus.Init()) purchaseStatus.SetPurchaseStatus(false, GetText((int)Texts.NameTMP).text);
        }
    }

    public void SetPopup(bool isGrace, StoreData storeData)
    {
        GetText((int)Texts.NameTMP).text = storeData.name;
        Managers.TextEffect.ApplyTextEffect(storeData.explanation, GetText((int)Texts.ExplanationTMP), 36);
        GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Grace/" + storeData.img, typeof(Sprite)) as Sprite;
        if (storeData.mode == "collection") 
            GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Collections/" + storeData.img, typeof(Sprite)) as Sprite;
        if (storeData.mode == "clothes") 
            GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Clothes/" + storeData.img, typeof(Sprite)) as Sprite;
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(()=>ClosePopupUI());
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(()=>OnClickedPurchaseBtn(storeData.img, storeData.price));
        GetButton((int)Buttons.PurchaseButton).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = storeData.price.ToString();
        if(isGrace)
        {
            switch (storeData.mode)
            {
                case "Both":
                    GetObject((int)GameObjects.StoryMode).SetActive(true);
                    GetObject((int)GameObjects.OneToOneMode).SetActive(true);
                    break;
                case "Story":
                    GetObject((int)GameObjects.StoryMode).SetActive(true);
                    GetObject((int)GameObjects.OneToOneMode).SetActive(false);
                    break;
                case "OneToOne":
                    GetObject((int)GameObjects.StoryMode).SetActive(false);
                    GetObject((int)GameObjects.OneToOneMode).SetActive(true);
                    break;
            }
            GetText((int)Texts.StoryModeTMP).text = I18n.Get(I18nDefine.PURCHASE_STORY_MODE);
            GetText((int)Texts.OneToOneTMP).text = I18n.Get(I18nDefine.PURCHASE_ONE_TO_ONE_MODE);
        }
        else
        {
            GetObject((int)GameObjects.StoryMode).SetActive(false);
            GetObject((int)GameObjects.OneToOneMode).SetActive(false);
        }
        
    }

    public void SetPopup(StoreData storeData)
    {
        GetText((int)Texts.NameTMP).text = storeData.name;
        Managers.TextEffect.ApplyTextEffect(storeData.explanation, GetText((int)Texts.ExplanationTMP), 36);
        GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Grace/" + storeData.img, typeof(Sprite)) as Sprite;
        if (storeData.mode == "collection") GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Collections/" + storeData.img, typeof(Sprite)) as Sprite;
        if (storeData.mode == "clothes") GetImage((int)Images.ItemImage).sprite = Resources.Load("Sprites/Clothes/" + storeData.img, typeof(Sprite)) as Sprite;
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(() => ClosePopupUI());
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(() => OnClickedPurchaseBtn(storeData.img, storeData.price));
        GetButton((int)Buttons.PurchaseButton).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = storeData.price.ToString();
    }
}