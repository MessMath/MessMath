using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PracticeGame : UI_Scene
{
    enum Texts
    {
        CoinCount,
    }

    enum Buttons
    {
        SettingBtn,
        AnswerBtn_1,
        AnswerBtn_2,
        AnswerBtn_3,
        AnswerBtn_4,
    }

    enum Images
    {
        BG,
        CoinImage,
        TeacherImage,
    }

    enum GameObjects
    {
        Sample,
        ChooseDifficulty,
        Problem,
        API,
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

        // TODO GameManager에서 플레이어 정보로 Coin개수 가져오기
        GetText((int)Texts.CoinCount).text = Managers.Game.Coin.ToString();
        
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OnClickSettingBtn);
        GetButton((int)Buttons.AnswerBtn_1).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_2).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_3).gameObject.BindEvent(OnClickAnswerBtn);
        GetButton((int)Buttons.AnswerBtn_4).gameObject.BindEvent(OnClickAnswerBtn);

        GetObject((int)GameObjects.Problem).gameObject.SetActive(false);

        return true;
    }

    void OnClickSettingBtn()
    {
        // TODO UI_Setting
        Managers.UI.ShowPopupUI<UI_Setting>();

    }

    void OnClickAnswerBtn()
    {
        GetText((int)Texts.CoinCount).text = Managers.Game.Coin.ToString();
    }

    IEnumerator BlinkTeacherImg(float delayTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);
        int countTime = 0;
        while (countTime < 10)
        {
            if (countTime % 2 == 0)
                GetImage((int)Images.TeacherImage).color = new Color32(255, 255, 255, 90);
            else
                GetImage((int)Images.TeacherImage).color = new Color32(255, 255, 255, 180);

            yield return waitForSeconds;

            countTime++;
        }

        GetImage((int)Images.TeacherImage).color = Color.white;
        yield return null;
    }


}
