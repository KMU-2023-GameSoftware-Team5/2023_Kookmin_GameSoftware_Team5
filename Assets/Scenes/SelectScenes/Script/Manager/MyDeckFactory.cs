using Assets.PixelHeroes.Scripts.CollectionScripts;
using data;
using lee;
using placement;
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

        /// <summary>
        /// 픽셀 캐릭터 모음
        /// </summary>
        public Dictionary<string, PixelHumanoidData>  m_humanoidDataMap;

        /// <summary>
        /// CreateBuilderControl을 위한 sprite collection
        /// </summary>
        private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }


        [SerializeField] private ItemData[] itemDatas;
        public Dictionary<string, ItemData> itemDataMap;

        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터정보 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterInventoryItemPrefeb;

        void Start()
        {
            Initialize();
        }

        public void Initialize()
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

        /// <summary>
        /// 이름으로 캐릭터 정보(PixelHumanoidData) 추출하기
        /// </summary>
        /// <param name="characterName">추출할 캐릭터이름</param>
        /// <returns>캐릭터정보</returns>
        public PixelHumanoidData getPixelHumanoidData(string characterName)
        {
            return m_humanoidDataMap[characterName];
        }

        /// <summary>
        /// 아이템으로 아이템 데이터맵 추출하기
        /// </summary>
        /// <param name="itemName">추출할 아이템이름</param>
        /// <returns>아이템 정보</returns>
        public ItemData getItemData(string itemName) {
            return itemDataMap[itemName];
        }

        /// <summary>
        /// 픽셀캐릭터 생성기
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns>생성한 픽셀 캐릭터</returns>
        public PixelCharacter buildPixelCharacter(string characterName) {
            CommonStats characterStat = new CommonStats();
            characterStat.CopyFrom(getPixelHumanoidData(characterName));
            PixelCharacter ret = new PixelHumanoid(characterName, characterStat);
            return ret;
        }

        /// <summary>
        /// 아이템 생성기
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>생성한 아이템</returns>
        public EquipItem buildEquipItem(string itemName) {
            EquipItem ret = new EquipItem(getItemData(itemName));
            return ret;

        }

        /// <summary>
        /// 플레이어가 현재 보유 중인 캐릭터 정보 UI 생성
        /// </summary>
        /// <param name="character">플레이어의 캐릭터 정보</param>
        /// <param name="targetParent">UI 배치할 transform</param>
        /// <param name="initialize">초기화를 factory에서 할 것인지</param>
        /// <returns></returns>
        public GameObject createCharacterInventoryPrefeb(PixelCharacter character, Transform targetParent, bool initialize=true)
        {
            GameObject go = Instantiate(characterInventoryItemPrefeb, targetParent);
            go.GetComponent<LightCharacterListItem>().Initialize(character);
            return go;
        }

    }
}
