using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StoryGame : UI_Scene
{
    enum Texts
    {
        Calculate_BoardText,
        PrintNumber_Text,
        QuestionNumber_Text,
    }

    enum Buttons
    {
        EqualButton,
    }

    enum Images
    {
        BGIMG,
        WitchImage,
        WitchHPImage_MASK_,
        WitchHPBar,
        WitchHPImage,
        JoyStickHandle,
        Circle,
        heart0,
        heart1,
        heart2,
    }

    enum GameObjects
    {
        DeadLine,
        Wall1,
        Wall2,
        Wall3,
        Wall4,
        JoyStick,
        ArrowController,
        Player,
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

        return true;
    }

    private void Update()
    {
        
    }

}
