using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GetNicknamePopup : UI_Popup
{
    enum GameObjects
    {
        UserName,
    }

    enum Images
    {
        Image,
    }

    enum Texts
    {
        UserNameText,
        Next,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetText((int)Texts.Next).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); OnClickedNextBtn(); });
        GetText((int)Texts.Next).gameObject.SetActive(false);
        GetImage((int)Images.Image).gameObject.SetActive(false);
        return true;
    }

    private void Update()
    {
        if (GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text != "")
        {
            GetText((int)Texts.Next).gameObject.SetActive(true);
            GetImage((int)Images.Image).gameObject.SetActive(true);
        }
        else
        {
            GetText((int)Texts.Next).gameObject.SetActive(false);
            GetImage((int)Images.Image).gameObject.SetActive(false);
        }
    }

    void OnClickedNextBtn()
    {
        Managers.DBManager.CreateNewUser(GetObject((int)GameObjects.UserName).gameObject.GetComponentInChildren<TMP_InputField>().text);
        Managers.UI.ClosePopupUI(this);
    }

}
