using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tierProbabilities", menuName = "shop/shopProbabilities", order = 1)]
public class TierProbabilitiesData : ScriptableObject
{
    [Serializable]
    public struct TierProbabilities
    {
        public int stage;
        public int tier1;
        public int tier2;
        public int tier3;
        public int tier4;
        public int tier5;
    }
    public TierProbabilities[] tierProbabilities;
}
