using Assets.PixelHeroes.Scripts.CollectionScripts;
using UnityEngine;
using data;
using System.Collections.Generic;

namespace battle
{
    public class StaticLoader : StaticComponentGetter<StaticLoader>
    {
        [SerializeField] private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }

        [Header("Prefap")]
        [SerializeField] private GameObject pixelHumanoidPrefap;
        public GameObject GetPixelCharacterPrefap() {  return pixelHumanoidPrefap; }

        [SerializeField] private GameObject defaultArrowPrefap;
        public GameObject GetDefaultArrowPrefab() { return defaultArrowPrefap; }

        [SerializeField] private GameObject pixelCharacterHeadBarPrefap;
        public GameObject GetPixelCharacterHeadBarPrefap() { return pixelCharacterHeadBarPrefap; }

        [SerializeField] private GameObject floatingTextPrefap;
        public GameObject GetFlatingTextPrefap() { return floatingTextPrefap; }

        [SerializeField] private GameObject lightningPillar;
        public GameObject GetLightningPillar() {  return lightningPillar; }

        [SerializeField] private GameObject chomp;
        public GameObject GetChomp() { return chomp; }

        [Header("Reference")]
        [SerializeField] private GameObject fireOrbit;
        public GameObject GetFireOrbit() { return fireOrbit; }

        [Header("Humanoid")]
        [SerializeField] private List<PixelHumanoidData> pixelHumanoidDatas = new List<PixelHumanoidData>();
        public int GetPixelHumanoidCount() { return pixelHumanoidDatas.Count; }
        public List<PixelHumanoidData> GetPixelHumanoidDatas() { return pixelHumanoidDatas; }
        public PixelHumanoidData GetPixelHumanoidData(int index) 
        {
            if (index >= pixelHumanoidDatas.Count)
            {
                Debug.LogError("invalid index: over the length");
                return null;
            }
            else if (index < 0)
            {
                Debug.LogError("invalid index: minus value");
                return null;
            }
                
            return pixelHumanoidDatas[index]; 
        }
    }
}
