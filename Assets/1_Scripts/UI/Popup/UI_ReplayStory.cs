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

    public void AddReplayStory(string characterName, string dialogue, string expression)
    {
        if(!Init()) Init();
        // 발화자에따른 Area생성, 대사랑 발화장 넘겨주기
        string picture = "";
        switch(characterName)
        {
            case "주인공":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "PlayerArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Story/Characters/Expression";
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture, expression);
                break;
            }
            case "가우스":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "OpponentArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Story/Characters/gaus";
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture, expression);
                break;
            }
            case "선생님":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "OpponentArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                picture = "Sprites/Story/Characters/teacher";
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture, expression);
                break;
            }
            case "":
            {
                GameObject item = Managers.UI.MakeSubItem<UI_Area>(GetObject((int)GameObjects.Content).transform, "PositionArea").gameObject; 
                UI_Area area = item.GetOrAddComponent<UI_Area>();
                if(area.Init()) 
                area.SetArea(characterName, dialogue, picture, expression);
                break;
            }
        }

        Fit(contentRect);
    }
    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
}