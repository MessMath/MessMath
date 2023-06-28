using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Purchase : UI_Popup
{
    enum Buttons
    {
        CloseButton,
        PurchaseButton,
    }
    enum Texts
    {
        NameTMP,
        ExplanationTMP,
    }
    enum Images
    {
        ItemImage,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(()=>ClosePopupUI());
        return true;
    }

    void OnClickedPurchaseBtn(int price)
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        UI_PurchaseStatus purchaseStatus = Managers.UI.ShowPopupUI<UI_PurchaseStatus>();
        if(Managers.Coin.CheckPurchase(price)) 
        {
            if(purchaseStatus.Init()) purchaseStatus.SetPurchaseStatus(true, GetText((int)Texts.NameTMP).text);
            PlayerPrefs.SetString("GraceOfGauss", "GraceOfGauss");
            Debug.Log(PlayerPrefs.GetString("GraceOfGauss"));
        }
        else
        {
            if(purchaseStatus.Init()) purchaseStatus.SetPurchaseStatus(false, GetText((int)Texts.NameTMP).text);
        }
    }

    public void SetPopup(string name, string explanation, int price, Sprite img)
    {
        GetText((int)Texts.NameTMP).text = name;
        GetText((int)Texts.ExplanationTMP).text = explanation;
        GetImage((int)Images.ItemImage).sprite = img;
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(()=>ClosePopupUI());
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(()=>OnClickedPurchaseBtn(price));
        GetButton((int)Buttons.PurchaseButton).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = price.ToString();
    }
}
