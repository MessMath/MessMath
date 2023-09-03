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
        BGImage,
        UserImage,
        UserBtnImage,
        Pencil,
        PvpImage,
        maine_coon,
        russian_blue,
        siamese,
        long_cat,
        MagicCircle,
        Potion,
        Sun,
        Moon,
        Dog,
        Flame,
        Glacier,
        Liquid,
        Spark,
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
        //PlayAnimation();
        
    }

    async void PlayAnimation()
    {
        if (await Managers.DBManager.GetIsCompletedTutorial(Managers.GoogleSignIn.GetUID()) == true)
            CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Lobby());
    }

    private void Update()
    {

    }

    UI_SelectGracePopup _selectGracePopup = null;
    bool TextOn;
    string[] obtainedMagicCircle = new string[3];
    string[] obtaineBGImage = new string[4];
    List<string> obtainedCollections = new List<string>();

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
        GetImage((int)Images.UserBtnImage).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Info>(); });
        GetButton((int)Buttons.ExerciseBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_PracticeGameScene()); });
        GetButton((int)Buttons.StoreBtn).gameObject.BindEvent(() => { Managers.UI.ShowPopupUI<UI_Store>(); });
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_InventoryPopup>(); });
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_Setting>(); });
        GetButton((int)Buttons.Fight1vs1GameBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.UI.ShowPopupUI<UI_SelectMathMtcfor1vs1>(); });
        GetButton((int)Buttons.PvpBtn).gameObject.BindEvent(() => { CoroutineHandler.StartCoroutine(SceneChangeAnimation_In_Pvp()); });
        GetButton((int)Buttons.PvpBroomstickBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); ButtonTextOnOff(); });
        GetImage((int)Images.MagicCircle).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); RefreshUI(); });

        GetImage((int)Images.maine_coon).gameObject.SetActive(false);
        GetImage((int)Images.russian_blue).gameObject.SetActive(false);
        GetImage((int)Images.siamese).gameObject.SetActive(false);
        GetImage((int)Images.long_cat).gameObject.SetActive(false);
        GetImage((int)Images.MagicCircle).gameObject.SetActive(false);
        GetImage((int)Images.BGImage).gameObject.SetActive(false);
        GetImage((int)Images.Potion).gameObject.SetActive(false);
        GetImage((int)Images.Sun).gameObject.SetActive(false);
        GetImage((int)Images.Moon).gameObject.SetActive(false);
        GetImage((int)Images.Dog).gameObject.SetActive(false);
        GetImage((int)Images.Flame).gameObject.SetActive(false);
        GetImage((int)Images.Glacier).gameObject.SetActive(false);
        GetImage((int)Images.Liquid).gameObject.SetActive(false);
        GetImage((int)Images.Spark).gameObject.SetActive(false);

        RefreshUI();

        CheckStory();
        #endregion
        // Sound
        Managers.Sound.Clear();
        Managers.Sound.Play("LobbyBgm", Define.Sound.Bgm);

        
        CheckTutorial();
        

        return true;
    }

    async void CheckStory()
    {
        if (await Managers.DBManager.GetIsCompletedStory(Managers.GoogleSignIn.GetUID()) == true)
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
            Managers.Sound.Play("ClickBtnEff");
            GetButton((int)Buttons.StoryModeBtn).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });
        }
    }

    async void CheckTutorial()
    {
        var GettingICT = Managers.DBManager.GetIsCompletedTutorial(Managers.GoogleSignIn.GetUID()).GetAwaiter();
        GettingICT.OnCompleted(() => {
            if (GettingICT.GetResult() == true)
                return;
            else
            {
                if(FindObjectOfType<UI_LobbyTutorial>() != null)
                {
                    return;
                }
                Managers.UI.ShowPopupUI<UI_LobbyTutorial>();
            }
        });
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

    async public void RefreshUI()
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

        GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()) + "_full");

        //if (await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()) != "uniform")
        //    GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()) + "_full");
        //else
        //    GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/uniform_full");

        Debug.Log("UI_Lobby RefreshUI");
    }

    async void CheckCollection()
    {
        await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID());
        

        if (await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()) == null) return;

        obtainedCollections = Managers.DBManager.ParseObtanined(await Managers.DBManager.GetObtainedCollections(Managers.GoogleSignIn.GetUID()));

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            // 너 이거 가지고있냐? 그럼 뭐 켜줄게
            if (obtainedCollections[i] == "maine_coon") GetImage((int)Images.maine_coon).gameObject.SetActive(true);
            if (obtainedCollections[i] == "russian_blue") GetImage((int)Images.russian_blue).gameObject.SetActive(true);
            if (obtainedCollections[i] == "siamese") GetImage((int)Images.siamese).gameObject.SetActive(true);
            if (obtainedCollections[i] == "long_cat") GetImage((int)Images.long_cat).gameObject.SetActive(true);
            if (obtainedCollections[i] == "potion") GetImage((int)Images.Potion).gameObject.SetActive(true);
            if (obtainedCollections[i] == "sun") GetImage((int)Images.Sun).gameObject.SetActive(true);
            if (obtainedCollections[i] == "moon") GetImage((int)Images.Moon).gameObject.SetActive(true);
            if (obtainedCollections[i] == "dog") GetImage((int)Images.Dog).gameObject.SetActive(true);
            if (obtainedCollections[i] == "flame") GetImage((int)Images.Flame).gameObject.SetActive(true);
            if (obtainedCollections[i] == "glacier") GetImage((int)Images.Glacier).gameObject.SetActive(true);
            if (obtainedCollections[i] == "liquid") GetImage((int)Images.Liquid).gameObject.SetActive(true);
            if (obtainedCollections[i] == "spark") GetImage((int)Images.Spark).gameObject.SetActive(true);

            // 마법 깃펜이랑 학교는?
            if (obtainedCollections[i] == "magic_quill") GetImage((int)Images.Pencil).sprite = Resources.Load<Sprite>("Sprites/Collections/magic_quill");
            if (obtainedCollections[i] == "castle") { GetImage((int)Images.PvpImage).sprite = Resources.Load<Sprite>("Sprites/Collections/castle"); GetButton((int)Buttons.PvpBtn).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0); }

            // 마법 서클
            if (CheckHaveMagicCircleImage())
            {
                string magicCircleImageSprite = GetRandomMagicCircleSprite();
                if (magicCircleImageSprite == "") return;
                GetImage((int)Images.MagicCircle).gameObject.SetActive(true);
                Debug.Log("Sprites/Collections/" + magicCircleImageSprite);
                GetImage((int)Images.MagicCircle).sprite = Resources.Load<Sprite>("Sprites/Collections/" + magicCircleImageSprite);
            }

            // 배경 이미지
            if (CheckHaveBGImage())
            {
                string bgImageSprite = GetRandomBGImageSprite();
                if (bgImageSprite == "") return;
                GetImage((int)Images.BGImage).gameObject.SetActive(true);
                Debug.Log("Sprites/Collections/" + bgImageSprite);
                GetImage((int)Images.BGImage).sprite = Resources.Load<Sprite>("Sprites/Collections/" + bgImageSprite + "_full");
            }
        }
    }

    #region MagicCircle
    bool CheckHaveMagicCircleImage()
    {
        if (obtainedCollections == null) return false;

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "light_magic_circle") return true;
            if (obtainedCollections[i] == "moon_magic_circle") return true;
            if (obtainedCollections[i] == "old_magic_circle") return true;
        }

        return false;
    }

    string GetRandomMagicCircleSprite()
    {
        if (!CheckHaveMagicCircleImage()) return "";
        int randValue = UnityEngine.Random.Range(0, 2);
            
        for (int i = 0; i < 3; i++)
        {
            obtainedMagicCircle[i] = "";
        }

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "light_magic_circle") obtainedMagicCircle[0] = (obtainedCollections[i]);
            if (obtainedCollections[i] == "moon_magic_circle") obtainedMagicCircle[1] = (obtainedCollections[i]);
            if (obtainedCollections[i] == "old_magic_circle") obtainedMagicCircle[2] = (obtainedCollections[i]);
        }

        if (obtainedMagicCircle[randValue] != "")
        {
            Debug.Log(obtainedMagicCircle[randValue]);
            return obtainedMagicCircle[randValue];
        }
        else 
            return "old_magic_circle";
    }
    #endregion

    #region BGImage
    bool CheckHaveBGImage()
    {
        if (obtainedCollections == null) return false;

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "landscape") return true;
            if (obtainedCollections[i] == "night_landscape") return true;
            if (obtainedCollections[i] == "space_landscape") return true;
            if (obtainedCollections[i] == "dawn_landscape") return true;
        }

        return false;
    }

    string GetRandomBGImageSprite()
    {
        if (!CheckHaveBGImage()) return "";
        int randValue = UnityEngine.Random.Range(0, 4);
        
        for (int i = 0; i < 4; i++)
        {
            obtaineBGImage[i] = "";
        }

        for (int i = 0; i < obtainedCollections.Count; i++)
        {
            if (obtainedCollections[i] == "landscape") obtaineBGImage[0] = (obtainedCollections[i]);
            if (obtainedCollections[i] == "night_landscape") obtaineBGImage[1] = (obtainedCollections[i]);
            if (obtainedCollections[i] == "space_landscape") obtaineBGImage[2] = (obtainedCollections[i]);
            if (obtainedCollections[i] == "dawn_landscape") obtaineBGImage[3] = (obtainedCollections[i]);
        }

        if (obtaineBGImage[randValue] != "")
        {
            Debug.Log(obtaineBGImage[randValue]);
            return obtaineBGImage[randValue];
        }
        else
            return "landscape";
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
