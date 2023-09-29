using Assets.PixelHeroes.Scripts.CollectionScripts;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class MyDeckFactory : MonoBehaviour
    {
        private static MyDeckFactory instance;
        public static MyDeckFactory Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MyDeckFactory>();
            }
            return instance;
        }
        public Dictionary<string, PixelHumanoidData>  m_humanoidDataMap;

        /// <summary>
        /// CreateBuilderControl을 위한 sprite collection
        /// </summary>
        private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }

        [SerializeField] private ItemData[] itemDatas;
        public Dictionary<string, ItemData> itemDataMap;


        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            m_humanoidDataMap =  lee.MyCharacterFactory.Instance().getPixelHumanoidDataMap();
            collection = lee.StaticLoader.Instance().GetCollection();
            itemDataMap = new Dictionary<string, ItemData>();
            foreach (var itemData in itemDatas)
            {
                if (itemData != null)
                    itemDataMap[itemData.itemName] = itemData;
            }

        }

        public PixelHumanoidData getPixelHumanoidData(string characterName)
        {
            return m_humanoidDataMap[characterName];
        }

        public ItemData GetItemData(string itemName) {
            return itemDataMap[itemName];
        }

        /// <summary>
        /// 임시로 픽셀 캐릭터 만드는 메서드
        /// </summary>
        /// <param name="nickname">TODO 파라미터 수정예정</param>
        public PixelCharacter buildPixelCharacter(string nickname) {
            PixelCharacter ret = new PixelHumanoid(nickname);
            return ret;
        }

    }
}
