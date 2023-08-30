using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiagnosisDatas
{
    [System.Serializable]
    public class DiagnosisData
    {
        public string dialogue;
    }

    [System.Serializable]
    public class DiagnosisInfo
    {
        public List<DiagnosisData> diagnosisDataList;
    }
}
