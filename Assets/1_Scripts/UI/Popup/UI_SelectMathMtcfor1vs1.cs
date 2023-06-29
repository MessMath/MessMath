using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SelectMathMtcfor1vs1 : UI_Popup
{
    enum Texts
    {

    }

    enum Buttons
    {
        Gauss,
        Pythagoras,
        Newton
    }

    enum Images
    {
        Panel,
    }

    enum GameObjects
    {

    }

    private void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));
        BindImage(typeof(Images));

        GetImage((int)Images.Panel).gameObject.BindEvent(OnClickBG);
        // TODO
        // 여기서 정한 수학자가 인게임에서 적으로 등장해야 한다.
        // 버튼을 누르면 다음 팝업(가호 선택 팝업)이 뜨도록.
        GetButton((int)Buttons.Gauss).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.PracticeGameScene);  });
        GetButton((int)Buttons.Pythagoras).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.Fight1vs1GameScene); });
        GetButton((int)Buttons.Newton).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); Managers.Scene.ChangeScene(Define.Scene.StoryScene); });

        return true;
    }

    void OnClickBG()
    {
        // Sound
        Managers.Sound.Play("ClickBtnEff");

        Managers.UI.ClosePopupUI();
    }

}
