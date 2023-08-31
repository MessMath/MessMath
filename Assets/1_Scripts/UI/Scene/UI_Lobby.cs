using MessMathI18n;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        ExerciseBtn,
        PvpBroomstickBtn,
    }

    enum Texts
    {
        SettingBtnText,
        StoryModeBtnText,
        Fight1vs1GameBtnText,
        StoreBtnText,
        PvpBtnText,
        InventoryBtnText,
        ExerciseBtnText,
        UserBtnText,
        PvpBroomstickBtnText,
    }

    private void Start()
    {
        Init();
        //showTutorial();

        CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
    }

    private void Update()
    {

    }

    UI_SelectGracePopup _selectGracePopup = null;
    bool TextOn;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        TextOn = true;

        #region 바인드
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        #endregion

        #region 연동
        GetImage((int)Images.UserImage).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Info>(); });
        GetButton((int)Buttons.ExerciseBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_PracticeGameScene()); });
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_Store>(); });
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_InventoryPopup>(); });
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });
        GetButton((int)Buttons.Fight1vs1GameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_SelectMathMtcfor1vs1>(); });
        GetButton((int)Buttons.PvpBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Pvp()); });
        GetButton((int)Buttons.PvpBroomstickBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ButtonTextOnOff(); });

        RefreshUI();

        if (PlayerPrefs.HasKey("WatchedStory") && PlayerPrefs.GetInt("WatchedStory") == -2)
        {
            GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() =>
            {
                Managers.Sound.Play("ClickBtnEff");
                _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
                _selectGracePopup._state = UI_SelectGracePopup.State.Story;
            });
        }
        else
        {
            //GetButton((int)Buttons.QuestBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });
        }
        #endregion
        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("LobbyBgm", Define.Sound.Bgm);

        Managers.UI.ShowPopupUI<UI_LobbyTutorial>();

        return true;
    }

    #region 씬변환 애니
    IEnumerator SceneChangeAnimation_In_PracticeGameScene()
    {
        Managers.Sound.Play("ClickBtnEff");

        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.PracticeGameScene, () => { Managers.Scene.ChangeScene(Define.Scene.PracticeGameScene); });

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SceneChangeAnimation_In_Pvp()
    {
        Managers.Sound.Play("ClickBtnEff");

        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(this.transform);
        anim.SetInfo(Define.Scene.PvpMatchingScene, () => { Managers.Scene.ChangeScene(Define.Scene.PvpMatchingScene); });

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator SceneChangeAnimation_In_Lobby()
    {
        // Ani
        UI_LockTouch uI_LockTouch = Managers.UI.ShowPopupUI<UI_LockTouch>();
        SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("Animation/SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
        anim.transform.SetParent(uI_LockTouch.transform);
        anim.SetInfo(Define.Scene.LobbyScene, () => { });

        yield return new WaitForSeconds(0.5f);
        Managers.UI.ClosePopupUI(uI_LockTouch);

    }
    #endregion

    public void RefreshUI()
    {
        GetText((int)Texts.SettingBtnText).text = I18n.Get(I18nDefine.LOBBY_SETTING);
        GetText((int)Texts.StoryModeBtnText).text = I18n.Get(I18nDefine.LOBBY_STORY_GAME);
        GetText((int)Texts.Fight1vs1GameBtnText).text = I18n.Get(I18nDefine.LOBBY_ONE_TO_ONE_GAME);
        GetText((int)Texts.StoreBtnText).text = I18n.Get(I18nDefine.LOBBY_STORE);
        GetText((int)Texts.PvpBtnText).text = I18n.Get(I18nDefine.LOBBY_PVP_GAME);
        GetText((int)Texts.InventoryBtnText).text = I18n.Get(I18nDefine.LOBBY_INVENTORY);
        GetText((int)Texts.ExerciseBtnText).text = I18n.Get(I18nDefine.LOBBY_PRACTICE_GAME);
        GetText((int)Texts.UserBtnText).text = I18n.Get(I18nDefine.LOBBY_STUDENT_ID_CARD);
        GetText((int)Texts.PvpBroomstickBtnText).text = I18n.Get(I18nDefine.LOBBY_HELP_ON);

        if (PlayerPrefs.HasKey("WearClothes"))
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + (PlayerPrefs.GetString("WearClothes")));
        else
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/lobby_Character");

        Debug.Log("UI_Lobby RefreshUI");
    }

    void showTutorial()
    {
        if (PlayerPrefs.GetInt("DoTutorial") != 2)
            Managers.UI.ShowPopupUI<UI_TutorialPopup>();
    }

    void ButtonTextOnOff()
    {
        // Text On 상태일떄
        if (TextOn)
        {
            GetText((int)Texts.SettingBtnText).gameObject.SetActive(false);
            GetText((int)Texts.StoryModeBtnText).gameObject.SetActive(false);
            GetText((int)Texts.Fight1vs1GameBtnText).gameObject.SetActive(false);
            GetText((int)Texts.StoreBtnText).gameObject.SetActive(false);
            GetText((int)Texts.PvpBtnText).gameObject.SetActive(false);
            GetText((int)Texts.InventoryBtnText).gameObject.SetActive(false);
            GetText((int)Texts.ExerciseBtnText).gameObject.SetActive(false);
            GetText((int)Texts.UserBtnText).gameObject.SetActive(false);
            GetText((int)Texts.PvpBroomstickBtnText).text = I18n.Get(I18nDefine.LOBBY_HELP_OFF);
            TextOn = false;
        }
        // Text Off 상태일떄
        else if (!TextOn)
        {
            GetText((int)Texts.SettingBtnText).gameObject.SetActive(true);
            GetText((int)Texts.StoryModeBtnText).gameObject.SetActive(true);
            GetText((int)Texts.Fight1vs1GameBtnText).gameObject.SetActive(true);
            GetText((int)Texts.StoreBtnText).gameObject.SetActive(true);
            GetText((int)Texts.PvpBtnText).gameObject.SetActive(true);
            GetText((int)Texts.InventoryBtnText).gameObject.SetActive(true);
            GetText((int)Texts.ExerciseBtnText).gameObject.SetActive(true);
            GetText((int)Texts.UserBtnText).gameObject.SetActive(true);
            GetText((int)Texts.PvpBroomstickBtnText).text = I18n.Get(I18nDefine.LOBBY_HELP_ON);
            TextOn = true;
        }
    }
}
