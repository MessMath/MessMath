using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StoreDatas 
{
    [System.Serializable] 
    public class StoreData
    {
        public string name;
        public string explanation;
        public string img;
        public int price;
    }

    [System.Serializable]
    public class StoreInfo
    {
        public List<StoreData> storeDataList;
    }
}
