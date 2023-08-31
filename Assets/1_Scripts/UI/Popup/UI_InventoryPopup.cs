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

    public void RefreshUI()
    {
        // �κ��丮 �˾� �ǻ�
        if (PlayerPrefs.HasKey("WearClothes"))
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/" + (PlayerPrefs.GetString("WearClothes")));
        else
            GetImage((int)Images.UserImage).sprite = Resources.Load<Sprite>("Sprites/Clothes/lobby_Character");

        // ���丮��� ��ȣ �̹���
        if ((PlayerPrefs.GetString($"SelectedGrace0InStory") != "") || (PlayerPrefs.GetString($"SelectedGrace1InStory") != "") || (PlayerPrefs.GetString($"SelectedGrace2InStory") != ""))
            GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_story_grace");
        else
            GetButton((int)Buttons.SelectStoryModeGraceBtn).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Inventory/inven_story_grace_before");

        // �ϴ��ϸ�� ��ȣ �̹���
        // �ϳ��� ��ȣ�� �����Ǿ� �ִٸ� �̹��� ����? -> �ϴ� �̰�
        // 3�� ��� �����ϸ� �̹��� ����?
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

    public void ReplayStoryBtn()
    {
        Managers.Sound.Play("ClickBtnEff");
        Managers.Scene.ChangeScene(Define.Scene.StoryScene);
    }
}
