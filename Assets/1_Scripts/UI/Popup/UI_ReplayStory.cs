using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ReplayStory : UI_Popup
{
    RectTransform contentRect;
    Scrollbar scrollbar;
    enum GameObjects
    {
        Content,
        ScrollBar,
    }
    enum Buttons
    {
        CloseButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        scrollbar = GetObject((int)GameObjects.ScrollBar).GetComponent<Scrollbar>();
        contentRect = GetObject((int)GameObjects.Content).GetComponent<RectTransform>();
        foreach (Transform child in contentRect.transform)
            Managers.Resource.Destroy(child.gameObject);

        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(()=>HidePopupUI());

        return true;
    }

    public void AddReplayStory(string characterName, string dialogue)
    {
        if(!Init()) Init();
        // 발화자에따른 Area생성, 대사랑 발화장 넘겨주기
        string picture = "";
        switch(characterName)
        {
            case "Main character":
            case "주인공":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "PlayerArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Character/Profile/profile_player";
                if(area.Init()) 
                area.SetArea(Managers.UserMng.GetNickname(), dialogue, picture);
                Managers.TextEffect.ReplayTyping(dialogue, area.GetText());
                break;
            }
            case "Gauss":
            case "가우스":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "OpponentArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Character/Profile/profile_gauss";
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture);
                Managers.TextEffect.ReplayTyping(dialogue, area.GetText());
                break;
            }
            case "Principal":
            case "선생님":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "OpponentArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Character/Profile/profile_pricipal";
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture);
                Managers.TextEffect.ReplayTyping(dialogue, area.GetText());
                break;
            }
            case "":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "PositionArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture);
                Managers.TextEffect.ReplayTyping(dialogue, area.GetText());
                break;
            }
        }

        Fit(contentRect);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
}