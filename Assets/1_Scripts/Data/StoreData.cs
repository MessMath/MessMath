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
        public string mode;
        public string bgImage;
        public string img2;
    }

    [System.Serializable]
    public class StoreInfo
    {
        public List<StoreData> storeDataList;
    }
}
