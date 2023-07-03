using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoryData
{
    [System.Serializable]
    public class TalkData
    {
        public string characterName; // 캐릭터	
        public string dialogue; // 대사
        public string sceneEffect; // 장면효과	
        public string soundEffect; // 효과음	
        public float soundEffectDuration; // 효과음지속시간	
        public string backgroundImg; // 배경화면	
        public string txtEffect; // 텍스트효과	
        public string expression; // 표정
    }


    [System.Serializable]
    public class TalkInfo
    {
        public List<TalkData> talkDataList;
    }
}