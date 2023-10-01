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
        public Dictionary<string, PixelHumanoidData>  m_humanoidDataMap;

        /// <summary>
        /// CreateBuilderControl을 위한 sprite collection
        /// </summary>
        private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }


        [SerializeField] private ItemData[] itemDatas;
        public Dictionary<string, ItemData> itemDataMap;

        /// <summary>
        /// 임시속성 - 캐릭터 배치 오브젝트의 parent canvas
        /// </summary>
        public Transform parent;

        /// <summary>
        /// placement Character(캐릭터 배치 오브젝트) 프리펩
        /// </summary>
        [SerializeField]GameObject placementCharacterPrefap;

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

        /// <summary>
        /// 배치할 캐릭터를 생성하는 메서드
        /// </summary>
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

            GameObject characterGo = Instantiate(placementCharacterPrefap, Vector3.zero, Quaternion.identity, parent);

            characterGo.transform.position = worldPosition;

            lee.PixelHumanoid go = characterGo.GetComponent<lee.PixelHumanoid>();

            // build
            go.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            go.builder.SpriteLibrary = go.spriteLibrary;
            PixelHumanoidData data = m_humanoidDataMap[character.characterName];
            data.SetOutToBuilder(go.builder);
            go.builder.Rebuild();

            go.Initilize(data);

            PlacementCharacter ret = characterGo.GetComponent<PlacementCharacter>();
            ret.Initialize(character);
            return ret;
        }

    }
}
