using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TipDatas
{
    [System.Serializable]
    public class TipData
    {
        public string tipText;
    }

    [System.Serializable]
    public class TipInfo
    {
        public List<TipData> tipDataList;
    }
}
