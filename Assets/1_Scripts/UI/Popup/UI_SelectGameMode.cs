using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectGameMode : UI_Popup
{
    enum Texts
    {

    }

    enum Buttons
    {
        PracticeBtn,
        Fight1vs1Btn,
        SeeStoryBtn,
    }

    enum Images
    {
        Panel,
    }

    enum GameObjects
    {

    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetImage((int)Images.Panel).gameObject.BindEvent(OnClickBG);
        GetButton((int)Buttons.PracticeBtn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.PracticeGameScene); });
        GetButton((int)Buttons.Fight1vs1Btn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });
        GetButton((int)Buttons.SeeStoryBtn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.SeeStoryScene); });

        return true;
    }

    void OnClickBG()
    {
        Managers.UI.ClosePopupUI();
    }

}
