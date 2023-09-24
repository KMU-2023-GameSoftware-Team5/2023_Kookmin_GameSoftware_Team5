using Assets.PixelHeroes.Scripts.CollectionScripts;
using UnityEngine;
using data;

namespace lee
{
    public class StaticLoader : StaticGetter<StaticLoader>
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

        [Header("Reference")]
        [SerializeField] private MyCharacterFactory myCharacterBuilder;

        [Header("Humanoid")]
        [SerializeField] private PixelHumanoidData[] pixeHumanoidDatas;
        public int GetPixelHumanoidCount() { return pixeHumanoidDatas.Length; }
        public PixelHumanoidData GetPixelHumanoidData(int index) 
        {
            if (index >= pixeHumanoidDatas.Length)
            {
                Debug.LogError("invalid index: over the length");
                return null;
            }
            else if (index < 0)
            {
                Debug.LogError("invalid index: minus value");
                return null;
            }
                
            return pixeHumanoidDatas[index]; 
        }


        private void Awake()
        {
            myCharacterBuilder.Initialize(pixeHumanoidDatas);
            BattleManager.Instance().Initialize();
        }
    }
}
