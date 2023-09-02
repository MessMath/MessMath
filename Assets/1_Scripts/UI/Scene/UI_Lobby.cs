using MessMathI18n;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
    enum Images
    {
        BG,
        UserImage,
        UserBtnImage,
        Pencil,
        PvpImage,
        maine_coon,
        russian_blue,
        siamese,
        long_cat,
        MagicCircle,
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
    string[] obtainedMagicCircle;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        TextOn = true;
        obtainedMagicCircle = new string[3];

        #region 바인드
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        #endregion

        #region 연동
        GetImage((int)Images.UserBtnImage).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Info>(); });
        GetButton((int)Buttons.ExerciseBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_PracticeGameScene()); });
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_Store>(); });
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_InventoryPopup>(); });
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });
        GetButton((int)Buttons.Fight1vs1GameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_SelectMathMtcfor1vs1>(); });
        GetButton((int)Buttons.PvpBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Pvp()); });
        GetButton((int)Buttons.PvpBroomstickBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ButtonTextOnOff(); });

        RefreshUI();

        if (Managers.UserMng.isCompletedStory == true)
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

        if (Managers.UserMng.isCompletedTutorial == false)
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

        CheckCollection();

        if (Managers.UserMng.myClothes != "")
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + Managers.UserMng.myClothes + "_full");
        else
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/uniform_full");

        Debug.Log("UI_Lobby RefreshUI");
    }

    void CheckCollection()
    {
        GetImage((int)Images.maine_coon).gameObject.SetActive(false);
        GetImage((int)Images.russian_blue).gameObject.SetActive(false);
        GetImage((int)Images.siamese).gameObject.SetActive(false);
        GetImage((int)Images.long_cat).gameObject.SetActive(false);
        GetImage((int)Images.MagicCircle).gameObject.SetActive(false);

        if (Managers.UserMng.GetObtainedCollections() == null) return;

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            // 너 이거 가지고있냐? 그럼 뭐 켜줄게
            // 고양이 가지고있냐?
            if (Managers.UserMng.GetObtainedCollections()[i] == "maine_coon") GetImage((int)Images.maine_coon).gameObject.SetActive(true);
            if (Managers.UserMng.GetObtainedCollections()[i] == "russian_blue") GetImage((int)Images.russian_blue).gameObject.SetActive(true);
            if (Managers.UserMng.GetObtainedCollections()[i] == "siamese") GetImage((int)Images.siamese).gameObject.SetActive(true);
            if (Managers.UserMng.GetObtainedCollections()[i] == "long_cat") GetImage((int)Images.long_cat).gameObject.SetActive(true);

            // 마법 깃펜이랑 학교는?
            if (Managers.UserMng.GetObtainedCollections()[i] == "magic_quill") GetImage((int)Images.Pencil).sprite = Resources.Load<Sprite>("Sprites/Collections/magic_quill");
            if (Managers.UserMng.GetObtainedCollections()[i] == "castle") { GetImage((int)Images.PvpImage).sprite = Resources.Load<Sprite>("Sprites/Collections/castle"); GetButton((int)Buttons.PvpBtn).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0); }

            // 마법 서클
            if (CheckHaveMagicCircleImage())
            {
                GetImage((int)Images.MagicCircle).gameObject.SetActive(true);
                GetImage((int)Images.MagicCircle).sprite = Resources.Load<Sprite>("Sprites/Collections/" + GetRandomMagicCircleSprite());
            }
        }
    }

    #region MagicCircle
    bool CheckHaveMagicCircleImage()
    {
        if (Managers.UserMng.UID == null) return false;
        if (Managers.UserMng.GetObtainedCollections() == null) return false;

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "light_magic_circle") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "moon_magic_circle") return true;
            if (Managers.UserMng.GetObtainedCollections()[i] == "old_magic_circle") return true;
        }

        return false;
    }

    string GetRandomMagicCircleSprite()
    {
        if (!CheckHaveMagicCircleImage()) return "";

        for (int i = 0; i < Managers.UserMng.GetObtainedCollections().Count; i++)
        {
            if (Managers.UserMng.GetObtainedCollections()[i] == "light_magic_circle") obtainedMagicCircle[0] = (Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "moon_magic_circle") obtainedMagicCircle[1] = (Managers.UserMng.GetObtainedCollections()[i]);
            if (Managers.UserMng.GetObtainedCollections()[i] == "old_magic_circle") obtainedMagicCircle[2] = (Managers.UserMng.GetObtainedCollections()[i]);
        }

        if (obtainedMagicCircle[UnityEngine.Random.Range(0, 3)] != "")
            return obtainedMagicCircle[UnityEngine.Random.Range(0, 3)];
        else 
            return "old_magic_circle";
    }
    #endregion

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
