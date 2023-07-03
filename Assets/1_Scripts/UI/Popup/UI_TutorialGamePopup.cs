using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using TutorialDatas;

public class UI_TutorialGamePopup : UI_Popup
{
    JsonReader jsonReader;
    int maxCount;
    int count = -1;
    List<TutorialData> tutorialTalkData = new List<TutorialData>();

    int numOfpages;
    int index;
    Transform tutorials;
    List<Transform> pages;

    enum Buttons
    {
        NextBtn,
    }

    enum Texts
    {
        Text,
    }

    enum GameObjects
    {
        Panel,
        Tutorial1,
        Tutorial2,
        Tutorial3,
        Tutorial4,
        Tutorial5,
        Tutorial6,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.NextBtn).gameObject.BindEvent(() => next());
        GetText((int)Texts.Text).text = "";

        pages= new List<Transform>();
        tutorials = GetComponent<Canvas>().transform.Find("Tutorials");
        numOfpages = System.Enum.GetValues(typeof(GameObjects)).Length - 1;

        Time.timeScale = 0;

        for (int i = 0; i < numOfpages; i++)
        {
            pages.Add(tutorials.GetChild(i));
            Debug.Log(tutorials.GetChild(i));
            pages[i].gameObject.SetActive(false);
        }

        jsonReader = new JsonReader();
        tutorialTalkData = jsonReader.ReadTutorialJson(Application.persistentDataPath + "/" + 3 + "_Tutorial.json").tutorialDataList;
        maxCount = tutorialTalkData.Count;

        pages[0].gameObject.SetActive(true);
        GetText((int)Texts.Text).text = tutorialTalkData[0].dialogue;
        GetText((int)Texts.Text).color = Color.black;
        GetText((int)Texts.Text).fontSize = 50;

        return true;
    }

    void next()
    {
        Managers.Sound.Play("ClickBtnEff");
        if (index >= numOfpages - 1)
        {
            Time.timeScale = 1;
            Managers.UI.ClosePopupUI();
        }

        pages[index].gameObject.SetActive(false);
        index++;
        if (index == numOfpages) return;
        pages[index].gameObject.SetActive(true);
        GetText((int)Texts.Text).text = tutorialTalkData[index].dialogue;
    }
}
