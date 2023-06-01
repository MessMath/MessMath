using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lobby : UI_Scene
{
    enum Images
    {
        BG,
    }

    enum Buttons
    {
        SettingBtn,
        StoryModeBtn,
    }

    enum Texts
    {
        SettingText,
        StoryModeText,
    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetText((int)Texts.SettingText).text = "test Text";
        GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.StoryGameScene); });

        return true;
    }
}
