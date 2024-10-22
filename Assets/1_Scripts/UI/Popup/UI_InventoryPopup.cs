using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPopup : UI_Popup
{
    UI_SelectGracePopup _selectGracePopup;

    enum Images
    {
        UserImage,
    }

    enum Buttons
    {
        SelectOneToOneModeGraceBtn,
        SelectStoryModeGraceBtn,
        ReplayStoryBtn,
        ExitBtn,
        CostumeBtn,
        CollectionBtn,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(ExitBtn);
        GetButton((int)Buttons.SelectOneToOneModeGraceBtn).gameObject.BindEvent(SelectOneToOneModeGraceBtn);
        GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.BindEvent(SelectStoryModeGraceBtn);
        GetButton((int)Buttons.ReplayStoryBtn).gameObject.BindEvent(ReplayStoryBtn);
        GetButton((int)Buttons.CostumeBtn).gameObject.BindEvent(CostumeBtn);
        GetButton((int)Buttons.CollectionBtn).gameObject.BindEvent(CollectionBtn);

        RefreshUI();

        return true;
    }

    async public void RefreshUI()
    {
        // 인벤토리 팝업 의상
        if (await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()) != "")
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + await Managers.DBManager.GetMyClothes(Managers.GoogleSignIn.GetUID()) + "_full");
        else
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/uniform_full");

        // 스토리모드 가호 이미지
        if ((PlayerPrefs.GetString($"SelectedGrace0InStory") != "") || (PlayerPrefs.GetString($"SelectedGrace1InStory") != "") || (PlayerPrefs.GetString($"SelectedGrace2InStory") != ""))
            GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_story_grace");
        else
            GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_story_grace_before");

        // 일대일모드 가호 이미지
        // 하나라도 가호가 장착되어 있다면 이미지 변경? -> 일단 이거
        // 3개 모두 장착하면 이미지 변경?
        if ((PlayerPrefs.GetString($"SelectedGrace0InOneToOne") != "") || (PlayerPrefs.GetString($"SelectedGrace1InOneToOne") != "") || (PlayerPrefs.GetString($"SelectedGrace2InOneToOne") != ""))
            GetButton((int)Buttons.SelectOneToOneModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_1vs1_grace");
        else
            GetButton((int)Buttons.SelectOneToOneModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_1vs1_grace_before");

    }

    void CostumeBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Managers.UI.ShowPopupUI<UI_ClothesBoxPopup>();
    }

    void CollectionBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Managers.UI.ShowPopupUI<UI_CollectionBoxPopup>();
    }

    public void ExitBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Managers.UI.ClosePopupUI(this);
    }

    public void SelectOneToOneModeGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
        _selectGracePopup._state = UI_SelectGracePopup.State.OneToOne;
    }

    public void SelectStoryModeGraceBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        _selectGracePopup = Managers.UI.ShowPopupUI<UI_SelectGracePopup>();
        _selectGracePopup._state = UI_SelectGracePopup.State.Story;
    }

    async public void ReplayStoryBtn()
    {
        Managers.Sound.Play("ClickBtnEff");

        if (await Managers.DBManager.GetIsCompletedTutorial(Managers.GoogleSignIn.GetUID()) && await Managers.DBManager.GetIsKilledWitch(Managers.GoogleSignIn.GetUID()))
            Managers.Scene.ChangeScene(Define.Scene.EpilogueScene);
        else
            Managers.Scene.ChangeScene(Define.Scene.StoryScene);
    }
}
