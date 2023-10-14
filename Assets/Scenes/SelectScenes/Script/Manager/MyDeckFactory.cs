using Assets.PixelHeroes.Scripts.CollectionScripts;
using data;
using battle;
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

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            m_humanoidDataMap =  battle.MyCharacterFactory.Instance().getPixelHumanoidDataMap();
            collection = battle.StaticLoader.Instance().GetCollection();
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
<<<<<<< Updated upstream
        /// <param name="itemName"></param>
        /// <returns>생성한 아이템</returns>
        public EquipItem buildEquipItem(string itemName) {
            EquipItem ret = new EquipItem(getItemData(itemName));
=======
        /// <param name="character">캐릭터 셀렉트 매니저가 가진 캐릭터 정보</param>
        /// <returns>캐릭터셀렉트매니저가 관리할 수 있는 캐릭터 배치 컴포넌트</returns>
        public PlacementCharacter buildPixelHumanoidByPixelCharacter(deck.PixelHumanoid character)
        {
            // string name, Vector3 worldPosition, Transform parent
            if (!m_humanoidDataMap.ContainsKey(character.characterName))
            {
                Debug.LogError("There is no Pixel Humanoid Data. Register it in Static Loader: " + name);
                return null;
            }

            Vector3 worldPosition = character.worldPosition;

            GameObject characterGo = Instantiate(placementCharacterPrefab, Vector3.zero, Quaternion.identity, parent);

            characterGo.transform.position = worldPosition;

            battle.PixelHumanoid go = characterGo.GetComponent<battle.PixelHumanoid>();

            // build
            go.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            go.builder.SpriteLibrary = go.spriteLibrary;
            PixelHumanoidData data = m_humanoidDataMap[character.characterName];
            data.SetOutToBuilder(go.builder);
            go.builder.Rebuild();

            go.Initilize(data);

            PlacementCharacter ret = characterGo.GetComponent<PlacementCharacter>();
            ret.Initialize(character);

            // head name
            GameObject headNameGo = Instantiate(characterHeadNamePrefab, Vector3.zero, Quaternion.identity, characterHeadNameCanvas);

            PlacementCharacterHeadName headName = headNameGo.GetComponent<PlacementCharacterHeadName>();
            headName.Initialize(ret, character.characterNickName);
            ret.headName = headName;

>>>>>>> Stashed changes
            return ret;

        }
    }
}
