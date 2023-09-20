using Assets.PixelHeroes.Scripts.CollectionScripts;
using UnityEngine;

namespace lee
{
    public class StaticLoader : StaticGetter<StaticLoader>
    {
        [SerializeField] private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }

        [SerializeField] private GameObject pixelHumanoidPrefap;
        public GameObject GetPixelCharacterPrefap() {  return pixelHumanoidPrefap; }

        [SerializeField] private GameObject defaultArrowPrefap;
        public GameObject GetDefaultArrowPrefab() { return defaultArrowPrefap; }

        [SerializeField] private MyCharacterFactory myCharacterBuilder;
        [SerializeField] private PixelHumanoidData[] pixeHumanoidDatas;

        private void Awake()
        {
            myCharacterBuilder.Initialize(pixeHumanoidDatas);
            BattleManager.Instance().Initialize();
        }
    }
}
