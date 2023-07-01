using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inventory : UI_Popup
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
        TitleText,
        ExitBtnText,
        BlessBtnText,
        ColectionBtnText,
        SelectedBlessDescription,
    }

    enum Buttons
    {
        ExitBtn,
        BlessBtn,
        BlessBtnSelected,
        ColectionBtn,
        ColectionBtnSelected,
    }

    enum Images
    {
        SelectedBless,
    }

    enum ButtonType
    {
        Bless,
        Colection,
    }

    ButtonType _type = ButtonType.Bless;
    List<UI_BlessItem> _blesses = new List<UI_BlessItem>();
    List<UI_ColectionItem> _colections = new List<UI_ColectionItem>();
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

        GetButton((int)Buttons.BlessBtn).gameObject.BindEvent(() => OnClickButton(ButtonType.Bless));
        GetButton((int)Buttons.BlessBtnSelected).gameObject.BindEvent(() => OnClickButton(ButtonType.Bless));
        GetButton((int)Buttons.ColectionBtn).gameObject.BindEvent(() => OnClickButton(ButtonType.Colection));
        GetButton((int)Buttons.ColectionBtnSelected).gameObject.BindEvent(() => OnClickButton(ButtonType.Colection));

        RefreshUI();

        return true;
    }

    private void Update()
    {
        //foreach (Transform go in Utils.FindChild(gameObject, "Content", true).transform) ;

    }

    void RefreshUI()
    {
        RefreshButton();

        // TODO 한영모드로 변경하면 텍스트 변경해줘야 됨. JSON, XML 정해지지 않아서 모르겠음.
        GetText((int)Texts.TitleText).text = "인벤토리";
        GetText((int)Texts.BlessBtnText).text = "가호";
        GetText((int)Texts.ColectionBtnText).text = "수집품";

        _blesses.Clear();
        _colections.Clear();

        // 프리펩에 보이는 가호 아이콘 정리
        Transform parent = GetObject((int)GameObjects.Content).gameObject.transform;
        foreach (Transform t in parent)
            Managers.Resource.Destroy(t.gameObject);

        // TODO 저장된 정보를 바탕으로 가호 및 수집품 리스트 채워주기. 데베? PlayerPrefs? 자체적인 저장방법? 완전 정해지면 구현해보자.
        if (GetButton((int)Buttons.BlessBtnSelected).gameObject.activeSelf)
            for (int i = 0; i < 5; i++)
            {
                var bless = Managers.UI.MakeSubItem<UI_BlessItem>(parent);
                Utils.FindChild(bless.gameObject, "BlessIconText", true).GetComponent<TextMeshProUGUI>().text = $"가호라구{i}";
                _blesses.Add(bless);

                Utils.FindChild(bless.gameObject, "Bless").BindEvent(() => { selectedObject = Utils.FindChild(bless.gameObject, "Bless"); OnClickObject(); });
            }
        else
            for (int i = 0; i < 5; i++)
            {
                var colection = Managers.UI.MakeSubItem<UI_ColectionItem>(parent);
                Utils.FindChild(colection.gameObject, "ColectionIconText", true).GetComponent<TextMeshProUGUI>().text = $"가우스의 세레스라구{i}";
                _colections.Add(colection);

                //Utils.FindChild(colection.gameObject, "Bless").BindEvent(() => { selectedObject = Utils.FindChild(colection.gameObject, "Bless"); OnClickObject(); });
            }

    }

    void OnClickButton(ButtonType type)
    {
        _type = type;
        RefreshUI();
    }

    void RefreshButton()
    {
        if (_type == ButtonType.Bless)
        {
            GetButton((int)Buttons.BlessBtn).gameObject.SetActive(false);
            GetButton((int)Buttons.BlessBtnSelected).gameObject.SetActive(true);
            GetButton((int)Buttons.ColectionBtn).gameObject.SetActive(true);
            GetButton((int)Buttons.ColectionBtnSelected).gameObject.SetActive(false);
        }
        else if (_type == ButtonType.Colection)
        {
            GetButton((int)Buttons.BlessBtn).gameObject.SetActive(true);
            GetButton((int)Buttons.BlessBtnSelected).gameObject.SetActive(false);
            GetButton((int)Buttons.ColectionBtn).gameObject.SetActive(false);
            GetButton((int)Buttons.ColectionBtnSelected).gameObject.SetActive(true);
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

            Managers.Game.Bl
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
