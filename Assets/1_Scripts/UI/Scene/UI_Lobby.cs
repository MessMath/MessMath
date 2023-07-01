using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Lobby : UI_Scene
{
    enum Images
    {
        BG,
        UserImage,
    }

    enum Buttons
    {
        SettingBtn,
        StoryModeBtn,
        Fight1vs1GameBtn,
        QuestBtn,
        StoreBtn,
        InventoryBtn,
        ExerciseBtn,
    }

    enum Texts
    {
    }

    private void Start()
    {
        Init();
        showTutorial();
    }

    private void Update()
    {
        // Ani
        GetButton((int)Buttons.StoryModeBtn).gameObject.transform.Rotate(new Vector3(0f, 0f, -30f) * Time.deltaTime);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetImage((int)Images.UserImage).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Info>(); });
        GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });
        GetButton((int)Buttons.Fight1vs1GameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });
        GetButton((int)Buttons.ExerciseBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.PracticeGameScene); });
        GetButton((int)Buttons.QuestBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Store>(); });
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Inventory>(); });
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("LobbyBgm", Define.Sound.Bgm);

        return true;
    }
    void showTutorial()
    {
        if (Managers.Game.IsTutorialFinished == false)
            Managers.UI.ShowPopupUI<UI_TutorialPopup>();
    }
}
