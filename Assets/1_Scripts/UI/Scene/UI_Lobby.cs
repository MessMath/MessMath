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
        InfinityGameBtn,
        TestBtn,
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
        GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() => { Managers.Scene.ChangeScene(Define.Scene.MakeTxtFileScene); });
        GetButton((int)Buttons.InfinityGameBtn).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_SelectGameMode>(); ; });
        GetButton((int)Buttons.TestBtn).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_Diagnosis>(); ; });

        return true;
    }
}
