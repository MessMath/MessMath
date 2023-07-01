using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UI_TutorialGamePopup : UI_Popup
{
    int numOfpages;
    int index;
    Transform tutorials;
    List<Transform> pages;

    enum Buttons
    {
        NextBtn,
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
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.NextBtn).gameObject.BindEvent(() => next());

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

        pages[0].gameObject.SetActive(true);

        return true;
    }

    void next()
    {

        if (index >= numOfpages - 1)
        {
            Time.timeScale = 1;
            Managers.UI.ClosePopupUI();
        }

        pages[index].gameObject.SetActive(false);
        index++;
        if (index == numOfpages) return;
        pages[index].gameObject.SetActive(true);
    }
}
