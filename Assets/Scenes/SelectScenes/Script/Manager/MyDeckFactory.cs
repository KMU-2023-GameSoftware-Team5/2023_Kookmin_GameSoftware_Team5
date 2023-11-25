using Assets.PixelHeroes.Scripts.CollectionScripts;
using data;
using battle;
using placement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
                instance.Initialize();
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


        /// <summary>
        /// 캐릭터 설명 모음
        /// </summary>
        public PixelCharacterDescription pixelCharacterDescription;
        public Dictionary<EDefualtAttackType, string> attackTypeDescriptionMap;
        public Dictionary<battle.PixelHumanoid.ESkill, string> skillDescriptionMap;


        [SerializeField] private ItemData[] itemDatas;
        public Dictionary<string, ItemData> itemDataMap;

        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터정보 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterInventoryItemPrefab;

        /// <summary>
        /// 플레이어가 현재 가지고 있는 아이템 정보 프리펩
        /// </summary>
        [SerializeField] GameObject LigthEquipItemPrefab;
        /// <summary>
        /// 플레이어가 현재 가지고 있는 캐릭터정보 프리펩
        /// </summary>
        [SerializeField] GameObject characterCardPrefab;

        [SerializeField] public DetailCanvasManager detailCanvasManager;

        /// <summary>
        /// 캐릭터 강화에 따른 색
        /// </summary>
        public Color[] tierColors;

        public void Initialize()
        {
            characterSpritePool = new Dictionary<string, Sprite>();
            m_humanoidDataMap =  battle.MyCharacterFactory.Instance().getPixelHumanoidDataMap();
            collection = battle.StaticLoader.Instance().GetCollection();
            // 아이템 목록
            itemDataMap = new Dictionary<string, ItemData>();
            foreach (var itemData in itemDatas)
            {
                if (itemData != null)
                    itemDataMap[itemData.itemName] = itemData;
            }

            // 캐릭터 공격타입 설명 목록
            attackTypeDescriptionMap = new Dictionary<EDefualtAttackType, string>();
            foreach (var attackTypeDescriptions  in pixelCharacterDescription.attackTypeDescriptions)
            {
                attackTypeDescriptionMap[attackTypeDescriptions.type] = attackTypeDescriptions.desc;
            }

            // 스킬 설명 목록 
            skillDescriptionMap = new Dictionary<battle.PixelHumanoid.ESkill, string>();
            foreach (var skillDescriptions in pixelCharacterDescription.skillDescriptions)
            {
                skillDescriptionMap[skillDescriptions.type] = skillDescriptions.desc;
            }

            buildSpritePool();
        }

        public GameObject lightCharacterInfo;
        public void createLightCharacterInfo(PixelCharacter character, Transform target)
        {
            GameObject go = Instantiate(lightCharacterInfo, target);
            RectTransform parent = target.gameObject.GetComponent<RectTransform>();
            go.GetComponent<ParentResizer>().initialize(parent);
            go.GetComponent<CharacterIcon>().Initialize(character);
        }

        public Dictionary<string, Sprite> characterSpritePool;
        [SerializeField]GameObject spritePoolMember;
        public UnityEvent<string> nickNameChangeEvent;

        void buildSpritePool()
        {
            foreach(KeyValuePair<string, PixelHumanoidData> character in m_humanoidDataMap)
            {
                GameObject go = Instantiate(spritePoolMember, transform);
                SpritePoolMember tmp = go.GetComponent<SpritePoolMember>();
                characterSpritePool[character.Key] = tmp.buildCharacter(character.Value);
            }
        }

        public Sprite getSprite(string characterName)
        {
            return characterSpritePool[characterName];
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
            PixelHumanoidData pixelHumanoidData = getPixelHumanoidData(characterName);
            PixelCharacter ret = new PixelHumanoid(characterName, pixelHumanoidData);
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
        public GameObject createCharacterInventoryPrefab(PixelCharacter character,Transform targetParent, bool initialize=true, int sortingOrder = 1)
        {
            GameObject go = Instantiate(characterInventoryItemPrefab, targetParent);
            if(initialize)
            {
                LightCharacterListItem tmp = go.GetComponent<LightCharacterListItem>();
                tmp.Initialize(character);
                if(sortingOrder != 1)
                {
                    tmp.setSortingOrder(sortingOrder);
                }
            }
            return go;
        }

        /// <summary>
        /// 플레이어가 현재 보유 중인 캐릭터 정보 UI 생성
        /// </summary>
        /// <param name="character">플레이어의 캐릭터 정보</param>
        /// <param name="targetParent">UI 배치할 transform</param>
        /// <param name="initialize">초기화를 factory에서 할 것인지</param>
        /// <returns></returns>
        public GameObject createCharacterCardPrefab(PixelCharacter character, Transform targetParent, bool initialize = true, int sortingOrder = 1)
        {
            GameObject go = Instantiate(characterCardPrefab, targetParent);
            if (initialize)
            {
                CharacterIcon tmp = go.GetComponent<CharacterIcon>();
                tmp.Initialize(character);
                if (sortingOrder != 1)
                {
                    tmp.setSortingOrder(sortingOrder);
                }
            }
            return go;

        }

        /// <summary>
        /// 플레이어 보유 아이템에 대한 UI 생성
        /// </summary>
        /// <param name="item">아이템 정보</param>
        public void createLightEquipItemUI(EquipItem item, Transform targetParent)
        {
            GameObject newPrefab = Instantiate(LigthEquipItemPrefab, targetParent);
            newPrefab.GetComponent<LightEquipItem>().Initialize(item);
        }


        /// <summary>
        /// 게임 시작할 때 캐릭터 고를 수 있게 해줌
        /// </summary>
        /// <returns></returns>
        public List<List<PixelCharacter>> getNGSelectos()
        {
            List<string> characterNames = new List<string>(m_humanoidDataMap.Keys); //{ "Demon", "Skeleton", "Goblin Archor" };
            System.Random random = new System.Random();

            List<List<PixelCharacter>> ret = new List<List<PixelCharacter>>();
            for(int i = 0; i < 5; i++)
            {
                ret.Add( new List<PixelCharacter>());
                for(int j = 0; j < 5; j++)
                {
                    PixelCharacter character = buildPixelCharacter(characterNames[random.Next(0, characterNames.Count)]);
                    ret[i].Add(character);
                }
            }
            return ret;
        }

        public PixelCharacter buildCharcterByPrice(int price)
        {
            List<string> characterNames = new List<string>(m_humanoidDataMap.Keys);
            PixelCharacter ret = buildPixelCharacter(characterNames[UnityEngine.Random.Range(0, characterNames.Count)]);
            ret.tier = price;
            return ret;
        }

        public EquipItem buildRandomItem()
        {
            List<string> itemNames = new List<string>(itemDataMap.Keys);
            EquipItem ret = buildEquipItem(itemNames[UnityEngine.Random.Range(0, itemNames.Count)]);
            return ret;
        }



        /// <summary>
        /// 사용자에게 전달할 메시지
        /// </summary>
        public InfoMessage infoMessage;

        /// <summary>
        /// 사용자에게 일시적으로 보여줄 메시지 출력
        /// </summary>
        /// <param name="text"></param>
        public void displayInfoMessage(string text)
        {
            infoMessage.displayInfoMessage(text);
        }
    }
}
