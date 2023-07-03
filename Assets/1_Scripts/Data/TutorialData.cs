using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialDatas
{
    [System.Serializable]
    public class TutorialData
    {
        public string dialogue;
    }

    [System.Serializable]
    public class TutorialInfo
    {
        public List<TutorialData> tutorialDataList;
    }
}
