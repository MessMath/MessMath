using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PurchaseStatus : UI_Popup
{
    enum Texts
    {
        StatusTMP,
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

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(()=>ClosePopupUI());
        return true;
    }

    public void SetPurchaseStatus(bool isSuccess, string name)
    {
        if(isSuccess)GetText((int)Texts.StatusTMP).text = name + " 구매에 성공 했습니다.\n컬렉션에서 확인할 수 있습니다.";
        else GetText((int)Texts.StatusTMP).text = name + " 구매에 실패 했습니다. 코인이 부족합니다.";
    }

}
