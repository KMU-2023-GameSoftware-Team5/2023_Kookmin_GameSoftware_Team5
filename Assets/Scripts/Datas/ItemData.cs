using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace data
{
    public enum EItemEffect
    {
        Test1, 
        Test2, 
        Test3, 
        None, 
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "data/ItemData", order = 2)]
    public class ItemData: scriptable.CommonStats
    {
        [Header("PixelHumanoidData")]
        // WARNUNG: MUST BE UNIQUE!
        public string itemName = "unique_name";

        public Sprite iconImage;
        public EItemEffect effect;
        public string description;
    }

}
