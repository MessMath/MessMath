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
        Main,
        LobbyScene,
        MakeTxtFileScene,
        StoryScene,
        StoryGameScene,
        PracticeGameScene,
        Fight1vs1GameScene,
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
        Unknown,
        
    }

    public enum CurrentStatus 
    { 
        WAITING, 
        DIAGNOSIS, 
        LEARNING 
    }

}
