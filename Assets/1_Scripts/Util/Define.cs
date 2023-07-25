using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{
    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
    }

    public enum Scene
    {
        Unknown,
        Dev,
        MainScene,
        LobbyScene,
        MakeTxtFileScene,
        StoryScene,
        StoryScene2,
        StoryGameScene,
        PracticeGameScene,
        Fight1vs1GameScene,
        TutorialGameScene,
        DiagnosisScene,
        PvpGameScene,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Speech,
        Max,
    }

    public enum Mode
    {
        None,
        DoubleSolve,
    }

    public enum CurrentStatus 
    { 
        WAITING, 
        DIAGNOSIS, 
        LEARNING 
    }

    public const int selectGracePopupTitleText = 5000;
    public const int DiagnosisTeacherText = 10000; // 10020까지 사용 금지 10100
    public const int OneToOneModeSelectGracePopupText = 10100;
    public const int StoryModeSelectGracePopupText = 10101; 
    public const int NextClassText = 10200;
    public const int RightAnswerTeacherTalkText = 10300; // 10303까지 사용
    public const int WrongAnswerTeacherTalkText = 10400; // 10403까지 사용
    public const int TipText = 20000;
}
