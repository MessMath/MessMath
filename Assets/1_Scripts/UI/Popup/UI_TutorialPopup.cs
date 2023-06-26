using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_TutorialPopup : UI_Popup
{
    GameObject tutorial;
    List<Image> tmp;
    List<Image> pages;
    int tutorialPage = 0;
    int index = 0;

    enum Buttons
    {
       NextBtn,
       PrevBtn,
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

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.NextBtn).gameObject.BindEvent(() => nextPopup());
        GetButton((int)Buttons.PrevBtn).gameObject.BindEvent(() => prevPopup());

        pages = new List<Image>();
        tmp = new List<Image>();

        tutorial = GetComponent<Canvas>().gameObject.transform.Find("Tutorials/Page").gameObject;

        tutorialPage = tutorial.GetComponentsInChildren<Image>().Length;

        for (int i = 0; i < tutorialPage; i++)
            tmp.Add(tutorial.GetComponentsInChildren<Image>()[i]);

        pages = Enumerable.Reverse(tmp).ToList();
        return true;
    }

    void nextPopup()
    {
        if (tutorialPage - 1 <= index)
        {
            Managers.Game.IsTutorialFinished = true;
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
