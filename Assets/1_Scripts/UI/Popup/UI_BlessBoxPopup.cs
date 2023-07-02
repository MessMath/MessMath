using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BlessBoxPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
        SelectedBless,
        SelectedBless1,
        SelectedBless2,
    }

    enum Texts
    {
        ExitBtnText,
        SelectedBlessDescription,
    }

    enum Buttons
    {
        ExitBtn,
    }

    enum Images
    {
        SelectedBless,
    }

    List<UI_BlessItem> _blesses = new List<UI_BlessItem>();
    GameObject selectedObject;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitBtn).gameObject.BindEvent(OnClosePopup);

        RefreshUI();

        return true;
    }

    void RefreshUI()
    {
        _blesses.Clear();
        
        // 프리펩에 보이는 가호 아이콘 정리
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // 플레이저 정보로 채우기
        for (int i = 0; i < 5; i++)
        {
            GameObject gameObject = Managers.UI.MakeSubItem<UI_BlessItem>(GetObject((int)GameObjects.Content).transform).gameObject;
            Utils.FindChild(gameObject, "BlessIconText").GetComponent<TextMeshPro>().text = $"가우스의 가호{i}";
        }

    }

    bool IsBlessSelect = false;
    bool IsBless1Select = false;
    bool IsBless2Select = false;
    void OnClickObject()
    {
        if (!IsBlessSelect)
        {
            selectedObject.transform.parent.SetParent(Utils.FindChild(gameObject, "SelectedBless", true).transform, false);
            selectedObject.transform.position = GetObject((int)GameObjects.SelectedBless).gameObject.transform.position;
            IsBlessSelect = true;

        }
        else if (!IsBless1Select)
        {
            selectedObject.transform.parent.SetParent(Utils.FindChild(gameObject, "SelectedBless1", true).transform, false);
            selectedObject.transform.position = GetObject((int)GameObjects.SelectedBless1).gameObject.transform.position;
            IsBless1Select = true;
        }
        else if (!IsBless2Select)
        {
            selectedObject.transform.parent.SetParent(Utils.FindChild(gameObject, "SelectedBless2", true).transform, false);
            selectedObject.transform.position = GetObject((int)GameObjects.SelectedBless2).gameObject.transform.position;
            IsBless2Select = true;
        }
        else if (selectedObject.transform.parent.parent.name == "SelectedBless")
        {
            GameObject go = GetObject((int)GameObjects.SelectedBless).GetComponentInChildren<UI_BlessItem>().gameObject;
            GameObject goChild = Utils.FindChild(go, "Bless");
            //GetObject((int)GameObjects.SelectedBless).GetComponentInChildren<UI_BlessItem>().gameObject.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            selectedObject.transform.parent.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            goChild.transform.position = go.transform.position;

            IsBlessSelect = false;

        }
        else if(selectedObject.transform.parent.parent.name == "SelectedBless1")
        {
            GameObject go = GetObject((int)GameObjects.SelectedBless1).GetComponentInChildren<UI_BlessItem>().gameObject;
            GameObject goChild = Utils.FindChild(go, "Bless");
            //GetObject((int)GameObjects.SelectedBless1).GetComponentInChildren<UI_BlessItem>().gameObject.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            selectedObject.transform.parent.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            goChild.transform.position = go.transform.position;

            IsBless1Select = false;
        }
        else if (selectedObject.transform.parent.parent.name == "SelectedBless2")
        {
            GameObject go = GetObject((int)GameObjects.SelectedBless2).GetComponentInChildren<UI_BlessItem>().gameObject;
            GameObject goChild = Utils.FindChild(go, "Bless");
            //GetObject((int)GameObjects.SelectedBless2).GetComponentInChildren<UI_BlessItem>().gameObject.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            selectedObject.transform.parent.transform.SetParent(GetObject((int)GameObjects.Content).transform, false);
            goChild.transform.position = go.transform.position;

            IsBless2Select = false;
        }
    }

    void OnPressObject()
    {
        selectedObject.transform.parent = gameObject.transform;
        selectedObject.transform.position = Input.mousePosition;
    }

    void OnPointerUpObject()
    {
        // 놓은 위치가 ContentBox 안인가? SelectedBless 안인가? 체크후 그 오브젝트 위치 변경
        if (selectedObject.GetOrAddComponent<BoxCollider2D>().attachedRigidbody.gameObject.name == "SelectedBless")
        {
            selectedObject.transform.parent = Utils.FindChild(gameObject, "SelectedBless", true).transform;
        }
        else
        {
            selectedObject.transform.parent = Utils.FindChild(gameObject, "Content", true).transform;
        }
    }

    void OnClosePopup()
    {
        // Sound
        // TODO ClosePopupSound

        Managers.UI.ClosePopupUI(this);

    }
}
