using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialPopup : UI_Popup
{
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

        GetImage((int)Images.Next).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); nextPopup(); });
        GetImage((int)Images.Back).gameObject.BindEvent(() => { Managers.Sound.Play("ClickBtnEff"); prevPopup(); });

        pages = new List<GameObject>();
        tmp = new List<GameObject>();

        tutorial = GetComponent<Canvas>().gameObject.transform.Find("Tutorials").gameObject;

        //tutorialPage = tutorial.GetComponentsInChildren<GameObject>().Length;
        tutorialPage = 4;

        for (int i = 0; i < tutorialPage; i++)
            pages.Add(tutorial.transform.GetChild(i).gameObject);
        Time.timeScale = 0;

        return true;
    }

    void nextPopup()
    {
        if (tutorialPage - 1 <= index)
        {
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
}
