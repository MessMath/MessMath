using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StoryGame : UI_Scene
{
    enum Texts
    {

    }

    enum Buttons
    {

    }

    enum Images
    {
        BG,
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

        //GetImage((int)Images.Panel).gameObject.BindEvent(OnClickBG);

        return true;
    }

    //void OnClickBG()
    //{
    //    Managers.UI.ClosePopupUI();
    //}

}
