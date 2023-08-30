using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessMathI18n;

public class UI_PurchaseStatus : UI_Popup
{
    enum Texts
    {
        StatusTMP,
        CloseTMP,
    }
    enum Buttons
    {
        CloseButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.CloseTMP).text = I18n.Get(I18nDefine.PURCHASE_CLOSE);
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(()=>ClosePopupUI());
        return true;
    }

    public void SetPurchaseStatus(bool isSuccess, string name)
    {
        if(isSuccess)
            GetText((int)Texts.StatusTMP).text = name + I18n.Get(I18nDefine.PURCHASE_SUCCESS);
        else 
            GetText((int)Texts.StatusTMP).text = name + I18n.Get(I18nDefine.PURCHASE_FAILED);
    }
}
