using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestInfo : UI_Popup
{
    enum GameObjects
    {
        UserName,
        UserMessage,
        CustomizingObject1,
        CustomizingObject2,
        CustomizingObject3,
    }

    enum Texts
    {
        UserNameText,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        BG,
        UserImageBG,
        UserImage,

    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        #region CustomizingObject Clear
        GetObject((int)GameObjects.CustomizingObject1).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject2).gameObject.GetComponent<Image>().sprite = null;
        GetObject((int)GameObjects.CustomizingObject3).gameObject.GetComponent<Image>().sprite = null;
        #endregion
        if (Managers.Game.Name != null)
            GetText((int)Texts.UserNameText).text = Managers.Game.Name;
        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(()=> Managers.UI.ClosePopupUI(this));
        GetImage((int)Images.UserImageBG).gameObject.BindEvent(() => OnClickedUserImgBG());

        return true;
    }

    void OnClickedUserImgBG()
    {
        GetText((int)Texts.UserNameText).text = Managers.DBManager.readData(Managers.GoogleSignIn.GetUID(), "nickname");
    }
}
