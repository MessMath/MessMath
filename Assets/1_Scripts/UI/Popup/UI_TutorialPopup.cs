using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TutorialDatas;
using MessMathI18n;

public class UI_TutorialPopup : UI_Popup
{
    JsonReader jsonReader;
    List<TutorialData> tutorialData = new List<TutorialData> ();
    int maxCount;
    int count = 0;
    GameObject tutorial;
    List<GameObject> tmp;
    List<GameObject> pages;
    int tutorialPage = 0;
    int index = 0;

    enum Images
    {
       Next,
       Back,
    }

    enum Texts
    {
        TutorialText,
        TutorialText2,
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));

        GetImage((int)Images.Next).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); GetNextTutorialText(); });
        GetImage((int)Images.Back).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); GetPrevTutorialText(); });

        pages = new List<GameObject>();
        tmp = new List<GameObject>();

        tutorial = GetComponent<Canvas>().gameObject.transform.Find("Tutorials").gameObject;
        //tutorialPage = tutorial.GetComponentsInChildren<GameObject>().Length;
        tutorialPage = 6;

        for (int i = 0; i < tutorialPage; i++)
            pages.Add(tutorial.transform.GetChild(i).gameObject);
        Time.timeScale = 0;

        jsonReader = new JsonReader();
        if(LocalizationManager.Get().GetSelectedLanguage() == Language.KOREAN)
        {
            tutorialData = jsonReader.ReadTutorialJson(Application.persistentDataPath + "/" + 3 + "_Tutorial_KOR.json").tutorialDataList;
        }
        else
        {
            tutorialData = jsonReader.ReadTutorialJson(Application.persistentDataPath + "/" + 7 + "_Tutorial_EN.json").tutorialDataList;
        }

        maxCount = tutorialData.Count;

        LoadTutorial();
        return true;
    }

    void nextPopup()
    {
        if (tutorialPage - 1 <= index)
        {
            index = tutorialPage;
            Time.timeScale = 1;
            Managers.UI.ClosePopupUI();
            return;
        }
        pages[index].gameObject.SetActive(false);
        index++;
        Debug.Log(index);
        pages[index].gameObject.SetActive(true);
    }

    void prevPopup()
    {
        if (index <= 0)
        {
            index = 0;
            return;
        }
        pages[index].gameObject.SetActive(false);
        index--;
        Debug.Log(index);
        pages[index].gameObject.SetActive(true);
    }

    void GetNextTutorialText()
    {
        if (count >= maxCount)
        {
            count = maxCount;
            return;
        }
        count++;
        LoadTutorial();
        nextPopup();
    }

    void GetPrevTutorialText()
    {
        if (count <= 0)
        {
            count = 0;
            return;
        }
        count--;
        LoadTutorial();
        prevPopup();
    }

    void LoadTutorial()
    {
        //Managers.TextEffect.SetNormalSpeed();
        //Managers.TextEffect.Typing(tutorialData[count].dialogue, GetText((int)Texts.TutorialText));
        if(count < maxCount - 1)
        {
            GetText((int)Texts.TutorialText).gameObject.SetActive(true);
            if(GetText((int)Texts.TutorialText2).gameObject != null)
                GetText((int)Texts.TutorialText2).gameObject.SetActive(false);
            GetText((int)Texts.TutorialText).text = tutorialData[count].dialogue.Replace("\\n", "\n");
        }
        else if(count == maxCount - 1)
        {
            GetText((int)Texts.TutorialText).gameObject.SetActive(false);
            GetText((int)Texts.TutorialText2).gameObject.SetActive(true);
            GetText((int)Texts.TutorialText2).text = tutorialData[count].dialogue.Replace("\\n", "\n"); ;
        }
     
    }
}
    