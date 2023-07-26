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
        StoreBtn,
        PvpBtn,
        InventoryBtn,
        QuestBtn,
        ExerciseBtn,
    }

    enum Texts
    {
    }

    private void Start()
    {
        Init();
        //showTutorial();
    }

    private void Update()
    {
        // Ani
        GetButton((int)Buttons.StoryModeBtn).gameObject.transform.Rotate(new Vector3(0f, 0f, -30f) * Time.deltaTime);
    }

    UI_SelectGracePopup _selectGracePopup = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetImage((int)Images.UserImage).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Info>(); });
        GetButton((int)Buttons.ExerciseBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.PracticeGameScene); });
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Store>(); });
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_InventoryPopup>(); });
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });
        GetButton((int)Buttons.Fight1vs1GameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_SelectMathMtcfor1vs1>(); });

        GetButton((int)Buttons.PvpBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.PvpMatchingScene); });


        if (PlayerPrefs.HasKey("WatchedStory") && PlayerPrefs.GetInt("WatchedStory")==-2) 
        {
            GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() => {
                _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
                _selectGracePopup._state = UI_SelectGracePopup.State.Story;
                 });  
        }
        else
        {
            GetButton((int)Buttons.QuestBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });
        }

        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("LobbyBgm", Define.Sound.Bgm);

        return true;
    }
    void showTutorial()
    {
        if (PlayerPrefs.GetInt("DoTutorial") != 2)
            Managers.UI.ShowPopupUI<UI_TutorialPopup>();
    }
}
